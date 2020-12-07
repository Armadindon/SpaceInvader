using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
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

        private GameState state = GameState.STARTING;

        public double runningTime = 0;

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


        private static readonly PrivateFontCollection Fonts = new PrivateFontCollection();

        /// <summary>
        /// A shared simple font
        /// </summary>
        private static Font defaultFont;

        private static Font defaultSmallFont;

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
            //Init de la font
            int fontLength = Properties.Resources.Pixellari.Length;
            byte[] fontdata = Properties.Resources.Pixellari;
            IntPtr data = Marshal.AllocCoTaskMem(fontLength);
            Marshal.Copy(fontdata, 0, data, fontLength);
            Fonts.AddMemoryFont(data, fontLength);

            defaultFont = new Font(Fonts.Families[0],24);
            defaultSmallFont = new Font(Fonts.Families[0], 16);

            if (game == null)
            {
                game = new Game(gameSize);
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


        private void DrawRunning(Graphics g)
        {
            g.DrawString("Nombre de vies : " + player.getLives(), defaultFont, blackBrush, 20f, 0f);
            //if (player.isBonusActive()) g.DrawString(player.GetBonus().ToString(), defaultFont, blackBrush, gameSize.Width - 250f, 0f);

            if (player.isBonusActive())
            {
                g.FillRectangle(blackBrush, gameSize.Width - 210f, 0f, 200f * (float)(player.getRemainingTimeBonus() / Bonus.BONUS_TIME), 25f);
                SizeF size = g.MeasureString(Bonus.convertToString(player.GetBonus()), defaultSmallFont);
                g.DrawString(Bonus.convertToString(player.GetBonus()), defaultSmallFont, new SolidBrush(Color.White), gameSize.Width - 105f - size.Width / 2, 0f);
            }
            

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Draw(this, g);
                //gameObject.getHitbox().Draw(g, Pens.Red);
            }
        }

        private int determineOpacity()
        {
            return Math.Abs(((int) (runningTime*120)%510)-255);
        }

        private void DrawPause(Graphics g)
        {
            SizeF sizeString = g.MeasureString("Jeu en pause", defaultFont);
            g.DrawString("Jeu en pause", defaultFont, blackBrush, (gameSize.Width/ 2) - (sizeString.Width / 2) , (gameSize.Height / 2) - (sizeString.Height / 2));
            SizeF sizeStringSpace = g.MeasureString("Appuyer sur Espace pour continuer", defaultFont);
            g.DrawString("Appuyer sur Espace pour continuer", defaultFont, new SolidBrush(Color.FromArgb(determineOpacity(),0,0,0)), (gameSize.Width / 2) - (sizeStringSpace.Width / 2), (gameSize.Height / 2) - (sizeStringSpace.Height / 2) + 50f);
        }

        private void DrawLose(Graphics g)
        {
            SizeF sizeString = g.MeasureString("Game Over", defaultFont);
            g.DrawString("Game Over", defaultFont, blackBrush, (gameSize.Width / 2) - (sizeString.Width / 2), (gameSize.Height / 2) - (sizeString.Height / 2));
            SizeF sizeStringSpace = g.MeasureString("Appuyer sur Espace pour relancer", defaultFont);
            g.DrawString("Appuyer sur Espace pour relancer", defaultFont, new SolidBrush(Color.FromArgb(determineOpacity(), 0, 0, 0)), (gameSize.Width / 2) - (sizeStringSpace.Width / 2), (gameSize.Height / 2) - (sizeStringSpace.Height / 2) + 50f);
        }

        private void DrawWin(Graphics g)
        {
            SizeF sizeString = g.MeasureString("Vous avez gagne !", defaultFont);
            g.DrawString("Vous avez gagne !", defaultFont, blackBrush, (gameSize.Width / 2) - (sizeString.Width / 2), (gameSize.Height / 2) - (sizeString.Height / 2));
            SizeF sizeStringSpace = g.MeasureString("Appuyer sur Espace pour relancer", defaultFont);
            g.DrawString("Appuyer sur Espace pour relancer", defaultFont, new SolidBrush(Color.FromArgb(determineOpacity(), 0, 0, 0)), (gameSize.Width / 2) - (sizeStringSpace.Width / 2), (gameSize.Height / 2) - (sizeStringSpace.Height / 2) + 50f);
        }

        private void DrawStarting(Graphics g)
        {
            SizeF sizeString = g.MeasureString("Space Invaders", defaultFont);
            g.DrawString("Space Invaders", defaultFont, blackBrush, (gameSize.Width / 2) - (sizeString.Width / 2), (gameSize.Height / 8) - (sizeString.Height / 2));

            SizeF sizeStringSpace = g.MeasureString("Appuyer sur Espace pour lancer", defaultFont);
            g.DrawString("Appuyer sur Espace pour lancer", defaultFont, new SolidBrush(Color.FromArgb(determineOpacity(), 0, 0, 0)), (gameSize.Width / 2) - (sizeStringSpace.Width / 2), gameSize.Height - sizeStringSpace.Height - 20f);

            //On affiche le tuto
            //On affiche pour les déplacements
            g.DrawImage(Properties.Resources.left_right, gameSize.Width / 16, gameSize.Height / 4, Properties.Resources.left_right.Width / 6, Properties.Resources.left_right.Height/6);

            SizeF sizeStringMovement = g.MeasureString("Utilisez les flèches du clavier", defaultSmallFont);
            g.DrawString("Utilisez les flèches du clavier", defaultSmallFont, blackBrush, gameSize.Width - sizeStringMovement.Width - 20f, gameSize.Height / 4 + 20f);
            SizeF sizeStringMovement2 = g.MeasureString("pour vous déplacer", defaultSmallFont);
            g.DrawString("pour vous déplacer", defaultSmallFont, blackBrush, gameSize.Width - sizeStringMovement.Width - 20f + sizeStringMovement2.Width/4, gameSize.Height / 4 + 40f);


            //On affiche pour la pause
            g.DrawImage(Properties.Resources.escape, gameSize.Width / 16, (int)(gameSize.Height / 2.25), Properties.Resources.escape.Width / 6, Properties.Resources.escape.Height / 6);
            SizeF sizeStringEscape = g.MeasureString("Utilisez la touche echap", defaultSmallFont);
            g.DrawString("Utilisez la touche echap", defaultSmallFont, blackBrush, gameSize.Width - sizeStringEscape.Width - 200f, (int)(gameSize.Height / 2.25) + 20f);
            SizeF sizeStringEscape2 = g.MeasureString("pour vous déplacer", defaultSmallFont);
            g.DrawString("pour vous déplacer", defaultSmallFont, blackBrush, gameSize.Width - sizeStringEscape.Width - 200f + sizeStringEscape2.Width / 8, (int)(gameSize.Height / 2.25) + 40f);

            //On affiche pour le tir
            SizeF sizeStringShot = g.MeasureString("Et utilisez la touche espace pour tirer !", defaultSmallFont);
            g.DrawString("Et utilisez la touche espace pour tirer !", defaultSmallFont, blackBrush, (gameSize.Width / 2) - (sizeStringShot.Width / 2), (float)(gameSize.Height / 1.35));

        }

        /// <summary>
        /// Draw the whole game
        /// </summary>
        /// <param name="g">Graphics to draw in</param>
        public void Draw(Graphics g)
        {

            switch (state)
            {
                case GameState.STARTING:
                    DrawStarting(g);
                    break;

                case GameState.PAUSE:
                    DrawPause(g);
                    break;

                case GameState.RUNNING:
                    DrawRunning(g);
                    break;

                case GameState.LOSE:
                    DrawLose(g);
                    break;

                case GameState.WIN:
                    DrawWin(g);
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
            runningTime += deltaT;
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
