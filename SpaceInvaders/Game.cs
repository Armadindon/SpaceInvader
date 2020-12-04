using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SpaceInvaders
{
    class Game
    {

        #region GameObjects management
        /// <summary>
        /// Set of all game objects currently in the game
        /// </summary>
        public HashSet<GameObject> gameObjects = new HashSet<GameObject>();

        /// <summary>
        /// Set of new game objects scheduled for addition to the game
        /// </summary>
        private HashSet<GameObject> pendingNewGameObjects = new HashSet<GameObject>();

        /// <summary>
        /// Schedule a new object for addition in the game.
        /// The new object will be added at the beginning of the next update loop
        /// </summary>
        /// <param name="gameObject">object to add</param>
        public void AddNewGameObject(GameObject gameObject)
        {
            pendingNewGameObjects.Add(gameObject);
        }

        public void RemoveDeadObjects()
        {
            IEnumerable<GameObject> toRemove = gameObjects.Where(gameObject => !gameObject.IsAlive());
            foreach (GameObject g in toRemove)
            {
                g.Die();
            }
            int removed = gameObjects.RemoveWhere(gameObject => !gameObject.IsAlive());
            if (removed > 0) enemyGroup.resizeHitbox();
        }

        public Player player;

        public EnemyGroup enemyGroup;
        #endregion

        #region game technical elements
        /// <summary>
        /// Size of the game area
        /// </summary>
        public Size gameSize;

        /// <summary>
        /// State of the keyboard
        /// </summary>
        public HashSet<Keys> keyPressed = new HashSet<Keys>();

        private GameState state = GameState.RUNNING;

        #endregion

        #region static fields (helpers)

        /// <summary>
        /// Singleton for easy access
        /// </summary>
        public static Game game { get; private set; }

        /// <summary>
        /// A shared black brush
        /// </summary>
        private static Brush blackBrush = new SolidBrush(Color.Black);

        /// <summary>
        /// A shared simple font
        /// </summary>
        private static Font defaultFont = new Font("Times New Roman", 24, FontStyle.Bold, GraphicsUnit.Pixel);

        public static Random random = new Random();
        #endregion


        #region constructors
        /// <summary>
        /// Singleton constructor
        /// </summary>
        /// <param name="gameSize">Size of the game area</param>
        /// 
        /// <returns></returns>
        public static Game CreateGame(Size gameSize)
        {
            if (game == null)
            {
                game = new Game(gameSize);
                game.initGame();
            }
            return game;
        }

        /// <summary>
        /// Private constructor
        /// </summary>
        /// <param name="gameSize">Size of the game area</param>
        private Game(Size gameSize)
        {
            this.gameSize = gameSize;
        }

        #endregion

        #region methods

        /// <summary>
        /// Force a given key to be ignored in following updates until the user
        /// explicitily retype it or the system autofires it again.
        /// </summary>
        /// <param name="key">key to ignore</param>
        public void ReleaseKeys()
        {
            keyPressed.Clear();
        }


        /// <summary>
        /// Draw the whole game
        /// </summary>
        /// <param name="g">Graphics to draw in</param>
        public void Draw(Graphics g)
        {

            switch (state)
            {
                case GameState.PAUSE:
                    g.DrawString("Jeu en pause", defaultFont, blackBrush, 20f, 20f);
                    break;
                case GameState.RUNNING:
                    g.DrawString("Nombre de vies : " + player.getLives(), defaultFont, blackBrush, 20f, 0f);
                    if (player.isBonusActive()) g.DrawString(player.GetBonus().ToString(), defaultFont, blackBrush, gameSize.Width - 250f, 0f);
                    foreach (GameObject gameObject in gameObjects)
                    {
                        gameObject.Draw(this, g);
                        gameObject.getHitbox().Draw(g, Pens.Red);
                    }
                    break;
                case GameState.LOSE:
                    g.DrawString("Vous avez perdu :c", defaultFont, blackBrush, 20f, 20f);
                    break;

                case GameState.WIN:
                    g.DrawString("Vous avez gagné c:", defaultFont, blackBrush, 20f, 20f);
                    break;
            }
        }

        public void initGame()
        {
            //On ajoute le joueur
            this.player = new Player();
            this.enemyGroup = new EnemyGroup();

            AddNewGameObject(this.player);
            AddNewGameObject(this.enemyGroup);
            Bunker.generateBunkers(this, 3);
            state = GameState.RUNNING;
        }

        /// <summary>
        /// Update game
        /// </summary>
        public void Update(double deltaT)
        {
            if (handlePause()) return;

            // add new game objects
            gameObjects.UnionWith(pendingNewGameObjects);
            pendingNewGameObjects.Clear();

            // update each game object
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Update(this, deltaT);
                if (gameObject.GetSprite() != null) gameObject.GetSprite().update(deltaT);
            }

            // remove dead objects
            RemoveDeadObjects();
            updateStatus();
            // release pressed keys
            ReleaseKeys();
        }


        private void updateStatus()
        {
            if (state == GameState.RUNNING)
            {
                if (gameObjects.OfType<Enemy>().Count() == 0)  // Si il n'y a plus d'ennemi
                 {
                    //On génère un nouveau EnemyGroup
                    if (!LevelController.haveNextLevel()) //Si pas de nouveaux enemis n'ont pas été généré = fin de partie
                    {
                        this.state = GameState.WIN;
                    }
                    else
                    {
                        this.enemyGroup = new EnemyGroup();
                        AddNewGameObject(this.enemyGroup);
                    }
                }
                if (Game.game.gameObjects.OfType<Player>().Count() == 0) // Si il n'y a plus de joueur
                {
                    this.state = GameState.LOSE; 
                }
                if (this.state != GameState.RUNNING)
                {
                    this.gameObjects.Clear(); //Si la partie est finie, on nettoie la liste
                }
            }
            else
            {
                if (keyPressed.Contains(Keys.Space)) initGame();
            }

        }

        private bool handlePause()
        {
            if (keyPressed.Contains(Keys.Escape)) state = (state == GameState.PAUSE) ? GameState.RUNNING : GameState.PAUSE;
            if (GameState.PAUSE == state) ReleaseKeys();
            return GameState.PAUSE == state;
        }
        #endregion
    }
}
