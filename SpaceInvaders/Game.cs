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
    /// <summary>
    /// Représente le jeu et son état
    /// </summary>
    class Game
    {

        #region GameObjects management
        /// <summary>
        /// Tout les entités actuellement dans le jeu
        /// </summary>
        public HashSet<GameObject> gameObjects = new HashSet<GameObject>();

        /// <summary>
        /// Entités a ajouter par la suite
        /// </summary>
        private HashSet<GameObject> pendingNewGameObjects = new HashSet<GameObject>();

        /// <summary>
        /// Prévoit d'ajouter un objet a la liste
        /// Il sera ajouté au prochain appel a update
        /// </summary>
        /// <param name="gameObject">object to add</param>
        public void AddNewGameObject(GameObject gameObject)
        {
            pendingNewGameObjects.Add(gameObject);
        }

        /// <summary>
        /// Enlève les objets morts
        /// </summary>
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

        /// <summary>
        /// L'instance du joueur
        /// </summary>
        public Player player;

        /// <summary>
        /// L'instance du groupe d'ennemis
        /// </summary>
        public EnemyGroup enemyGroup;
        #endregion

        #region game technical elements
        /// <summary>
        /// Taille de la zone de jeu
        /// </summary>
        public Size gameSize;

        /// <summary>
        /// Touches utilisés avant l'appel a update
        /// </summary>
        public HashSet<Keys> keyPressed = new HashSet<Keys>();

        /// <summary>
        /// L'état actuel du jeu
        /// </summary>
        private GameState state = GameState.STARTING;

        /// <summary>
        /// Le temps depuis lequel le jeu est lancé
        /// </summary>
        public double runningTime = 0;

        #endregion

        #region static fields (helpers)

        /// <summary>
        /// Instance du jeu
        /// </summary>
        /// <value>Instance du jeu</value>
        public static Game game { get; private set; }

        /// <summary>
        /// Brush par défaut
        /// </summary>
        private static Brush blackBrush = new SolidBrush(Color.Black);

        /// <summary>
        /// Collection de font custom (une seul dedans)
        /// </summary>
        private static readonly PrivateFontCollection Fonts = new PrivateFontCollection();

        /// <summary>
        /// La police d'écriture par défaut
        /// </summary>
        private static Font defaultFont;

        /// <summary>
        /// La police d'écriture par défaut (mais en petit)
        /// </summary>
        private static Font defaultSmallFont;

        /// <summary>
        /// Objet random instancié pour tout le jeu
        /// </summary>
        public static Random random = new Random();
        #endregion


        #region constructors
        /// <summary>
        /// Permet de créer l'instance de jeu
        /// </summary>
        /// <param name="gameSize">Taille de la zone de jeu</param>
        /// <returns>Instance du jeu</returns>
        public static Game CreateGame(Size gameSize)
        {
            //Init de la font
            int fontLength = Properties.Resources.Pixellari.Length;
            byte[] fontdata = Properties.Resources.Pixellari;
            IntPtr data = Marshal.AllocCoTaskMem(fontLength);
            Marshal.Copy(fontdata, 0, data, fontLength);
            Fonts.AddMemoryFont(data, fontLength);

            defaultFont = new Font(Fonts.Families[0], 24);
            defaultSmallFont = new Font(Fonts.Families[0], 16);

            if (game == null)
            {
                game = new Game(gameSize);
            }
            return game;
        }

        /// <summary>
        /// Permet d'instancier le jeu, en private pour forcer l'utilisation du constructeur
        /// </summary>
        /// <param name="gameSize">Taille de la zone de jeu</param>
        private Game(Size gameSize)
        {
            this.gameSize = gameSize;
        }

        #endregion

        #region methods

        /// <summary>
        /// Elnlèves les clés qui ont été pressé avant le update
        /// Force a ré-appuyer
        /// </summary>
        public void ReleaseKeys()
        {
            keyPressed.Clear();
        }


        /// <summary>
        /// Dessine le jeu quand il est en phase de jeu (justement)
        /// </summary>
        /// <param name="g">L'endroit ou dessiner</param>
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
                //gameObject.getHitbox().Draw(g, Pens.Red); // A décomenter pour voir les hitboxs de tous les objets du jeu
            }
        }

        /// <summary>
        /// Determines l'opacité afin de créer un effet de clignottement
        /// </summary>
        /// <returns>l'opacité entre 0 et 255</returns>
        private int determineOpacity()
        {
            return Math.Abs(((int)(runningTime * 120) % 510) - 255); //Permet de faire un clignottement (Le *120 est présent afin d'accélerer le proccessus)
        }

        /// <summary>
        /// Dessine le jeu quand il est en phase de pause
        /// </summary>
        /// <param name="g">L'endroit ou dessiner</param>
        private void DrawPause(Graphics g)
        {
            SizeF sizeString = g.MeasureString("Jeu en pause", defaultFont);
            g.DrawString("Jeu en pause", defaultFont, blackBrush, (gameSize.Width / 2) - (sizeString.Width / 2), (gameSize.Height / 2) - (sizeString.Height / 2));
            SizeF sizeStringSpace = g.MeasureString("Appuyer sur Echap pour continuer", defaultFont);
            g.DrawString("Appuyer sur Echap pour continuer", defaultFont, new SolidBrush(Color.FromArgb(determineOpacity(), 0, 0, 0)), (gameSize.Width / 2) - (sizeStringSpace.Width / 2), (gameSize.Height / 2) - (sizeStringSpace.Height / 2) + 50f);
        }

        /// <summary>
        /// Dessine le jeu quand avant de passer a un nouveau niveau
        /// </summary>
        /// <param name="g">L'endroit ou dessiner</param>
        private void DrawInterLevel(Graphics g)
        {
            SizeF sizeString = g.MeasureString("Prochain niveau : Niveau " + LevelController.getCurrentLevel(), defaultFont);
            g.DrawString("Prochain niveau : Niveau " + LevelController.getCurrentLevel(), defaultFont, blackBrush, (gameSize.Width / 2) - (sizeString.Width / 2), (gameSize.Height / 2) - (sizeString.Height / 2));
            SizeF sizeStringSpace = g.MeasureString("Appuyer sur Espace pour continuer", defaultFont);
            g.DrawString("Appuyer sur Espace pour continuer", defaultFont, new SolidBrush(Color.FromArgb(determineOpacity(), 0, 0, 0)), (gameSize.Width / 2) - (sizeStringSpace.Width / 2), (gameSize.Height / 2) - (sizeStringSpace.Height / 2) + 50f);
        }

        /// <summary>
        /// Dessine le jeu quand il est perdu
        /// </summary>
        /// <param name="g">L'endroit ou dessiner</param>
        private void DrawLose(Graphics g)
        {
            SizeF sizeString = g.MeasureString("Game Over", defaultFont);
            g.DrawString("Game Over", defaultFont, blackBrush, (gameSize.Width / 2) - (sizeString.Width / 2), (gameSize.Height / 2) - (sizeString.Height / 2));
            SizeF sizeStringSpace = g.MeasureString("Appuyer sur Espace pour relancer", defaultFont);
            g.DrawString("Appuyer sur Espace pour relancer", defaultFont, new SolidBrush(Color.FromArgb(determineOpacity(), 0, 0, 0)), (gameSize.Width / 2) - (sizeStringSpace.Width / 2), (gameSize.Height / 2) - (sizeStringSpace.Height / 2) + 50f);
        }

        /// <summary>
        /// Dessine le jeu quand il est gagné
        /// </summary>
        /// <param name="g">L'endroit ou dessiner</param>
        private void DrawWin(Graphics g)
        {
            SizeF sizeString = g.MeasureString("Vous avez gagne !", defaultFont);
            g.DrawString("Vous avez gagne !", defaultFont, blackBrush, (gameSize.Width / 2) - (sizeString.Width / 2), (gameSize.Height / 2) - (sizeString.Height / 2));
            SizeF sizeStringSpace = g.MeasureString("Appuyer sur Espace pour relancer", defaultFont);
            g.DrawString("Appuyer sur Espace pour relancer", defaultFont, new SolidBrush(Color.FromArgb(determineOpacity(), 0, 0, 0)), (gameSize.Width / 2) - (sizeStringSpace.Width / 2), (gameSize.Height / 2) - (sizeStringSpace.Height / 2) + 50f);
        }

        /// <summary>
        /// Dessine le jeu au lancement (ainsi que le "tuto" du jeu)
        /// </summary>
        /// <param name="g">L'endroit ou dessiner</param>
        private void DrawStarting(Graphics g)
        {
            SizeF sizeString = g.MeasureString("Space Invaders", defaultFont);
            g.DrawString("Space Invaders", defaultFont, blackBrush, (gameSize.Width / 2) - (sizeString.Width / 2), (gameSize.Height / 8) - (sizeString.Height / 2));

            SizeF sizeStringSpace = g.MeasureString("Appuyer sur Espace pour lancer", defaultFont);
            g.DrawString("Appuyer sur Espace pour lancer", defaultFont, new SolidBrush(Color.FromArgb(determineOpacity(), 0, 0, 0)), (gameSize.Width / 2) - (sizeStringSpace.Width / 2), gameSize.Height - sizeStringSpace.Height - 20f);

            //On affiche le tuto
            //On affiche pour les déplacements
            g.DrawImage(Properties.Resources.left_right, gameSize.Width / 16, gameSize.Height / 4, Properties.Resources.left_right.Width / 6, Properties.Resources.left_right.Height / 6);

            SizeF sizeStringMovement = g.MeasureString("Utilisez les flèches du clavier", defaultSmallFont);
            g.DrawString("Utilisez les flèches du clavier", defaultSmallFont, blackBrush, gameSize.Width - sizeStringMovement.Width - 20f, gameSize.Height / 4 + 20f);
            SizeF sizeStringMovement2 = g.MeasureString("pour vous déplacer", defaultSmallFont);
            g.DrawString("pour vous déplacer", defaultSmallFont, blackBrush, gameSize.Width - sizeStringMovement.Width - 20f + sizeStringMovement2.Width / 4, gameSize.Height / 4 + 40f);


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
        /// Dessine le jeu en fonction de son état
        /// </summary>
        /// <param name="g">L'endroit ou dessiner</param>
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

                case GameState.INTER_LEVEL:
                    DrawInterLevel(g);
                    break;
            }
        }

        /// <summary>
        /// Initializes le jeu, en effacant son ancien état
        /// </summary>
        public void initGame()
        {
            //On ajoute le joueur
            this.gameObjects.Clear();
            LevelController.reset();
            this.player = new Player();

            AddNewGameObject(this.player);
            Bunker.generateBunkers(this, 3);
        }


        /// <summary>
        /// Met a jour le status de la partie en fonction de l'état des game objects
        /// </summary>
        private void updateStatus()
        {
            if (gameObjects.OfType<Enemy>().Count() == 0)  // Si il n'y a plus d'ennemi
            {
                //On génère un nouveau EnemyGroup
                this.state = (LevelController.haveNextLevel()) ? GameState.INTER_LEVEL : GameState.WIN;
            }
            if (Game.game.gameObjects.OfType<Player>().Count() == 0) // Si il n'y a plus de joueur
            {
                this.state = GameState.LOSE;
            }
        }

        /// <summary>
        /// Met a jour le jeu quand il tourne
        /// </summary>
        /// <param name="deltaT">Temps passé depuis le dernier appel a update</param>
        private void handleRun(double deltaT)
        {
            // update each game object
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Update(this, deltaT);
            }

            // remove dead objects
            RemoveDeadObjects();
            updateStatus();
            if (keyPressed.Contains(Keys.Escape)) this.state = GameState.PAUSE;
        }

        /// <summary>
        /// Met a jour le jeu quand il est en pause
        /// </summary>
        /// <param name="deltaT">Temps passé depuis le dernier appel a update</param>
        private void handlePause(double deltaT)
        {
            if (keyPressed.Contains(Keys.Escape)) this.state = GameState.RUNNING;
        }


        /// <summary>
        /// Met a jour le jeu quand il se lance
        /// </summary>
        /// <param name="deltaT">Temps passé depuis le dernier appel a update</param>
        private void handleStart(double deltaT)
        {
            if (keyPressed.Contains(Keys.Space))
            {
                this.state = GameState.INTER_LEVEL;
                initGame();
            }
        }

        /// <summary>
        /// Met a jour le jeu quand le joueur a gagné
        /// </summary>
        /// <param name="deltaT">Temps passé depuis le dernier appel a update</param>
        private void handleWin(double deltaT)
        {
            if (keyPressed.Contains(Keys.Space)) this.state = GameState.STARTING;
        }

        /// <summary>
        /// Met a jour le jeu quand le joueur a perdu
        /// </summary>
        /// <param name="deltaT">Temps passé depuis le dernier appel a update</param>
        private void handleLose(double deltaT)
        {
            if (keyPressed.Contains(Keys.Space)) this.state = GameState.STARTING;
        }

        /// <summary>
        /// Met a jour le jeu avant de lancer un nouveau niveau
        /// </summary>
        /// <param name="deltaT">Temps passé depuis le dernier appel a update</param>
        private void handleInterLevel(double deltaT)
        {
            if (keyPressed.Contains(Keys.Space))
            {
                this.state = GameState.RUNNING;
                this.gameObjects.RemoveWhere(g => g is EnemyGroup || g is Bonus); // On remove l'enemyGroup actuel et les bonus qui droppait
                player.addBonus(BonusType.INVINCIBILITY, 0); //On clear le bonus actuel
                this.enemyGroup = new EnemyGroup();
                AddNewGameObject(this.enemyGroup);
            }
        }

        /// <summary>
        /// Met a jour le jeu en fonction de l'état actuel
        /// </summary>
        /// <param name="deltaT">Temps passé depuis le dernier appel a update</param>
        public void Update(double deltaT)
        {
            runningTime += deltaT;

            gameObjects.UnionWith(pendingNewGameObjects);
            pendingNewGameObjects.Clear();

            switch (state)
            {
                case GameState.STARTING:
                    handleStart(deltaT);
                    break;

                case GameState.PAUSE:
                    handlePause(deltaT);
                    break;

                case GameState.RUNNING:
                    handleRun(deltaT);
                    break;

                case GameState.LOSE:
                    handleLose(deltaT);
                    break;

                case GameState.WIN:
                    handleWin(deltaT);
                    break;

                case GameState.INTER_LEVEL:
                    handleInterLevel(deltaT);
                    break;

            }

            ReleaseKeys();
        }

        #endregion
    }
}
