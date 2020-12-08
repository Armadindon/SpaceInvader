namespace SpaceInvaders
{
    /// <summary>
    /// Controlle les différent niveau au cours du jeu
    /// </summary>
    class LevelController
    {

        /// <summary>
        /// Futur niveau (Ouais le nom de la variable est trompeuse)
        /// </summary>
        private static int currentLevel = 1;


        /// <summary>
        /// Passe au niveau suivant
        /// </summary>
        public static void nextLevel()
        {
            switch (currentLevel)
            {
                case 1:
                    Level1();
                    break;

                case 2:
                    Level2();
                    break;

                case 3:
                    Level3();
                    break;

            }
            currentLevel++;
        }

        /// <summary>
        /// Permet de récupérer le prochain niveau
        /// </summary>
        /// <returns>Numéro du prochain niveau</returns>
        public static int getCurrentLevel()
        {
            return currentLevel;
        }

        /// <summary>
        /// Retourne au premier niveau
        /// </summary>
        public static void reset()
        {
            currentLevel = 1;
        }

        /// <summary>
        /// Permet de savoir si il reste un niveau par la suite
        /// </summary>
        /// <returns><c>true</c> si il reste un niveau, <c>false</c> sinon</returns>
        public static bool haveNextLevel()
        {
            return currentLevel < 4;
        }

        /// <summary>
        /// Permet de générer les enemis du niveau 1
        /// </summary>
        private static void Level1()
        {
            int verticalOffset = 0;
            new LineSchematic(new Enemy(new Sprite(Properties.Resources.ship2, Properties.Resources.ship2bis), 0,0), 3, 3)
                .generateScheme(Game.game, Game.game.gameSize.Width - Game.game.gameSize.Width/4, new Vecteur2D(Game.game.gameSize.Width / 8, Game.game.gameSize.Height / 8));

            verticalOffset += (int) (Properties.Resources.ship2.Height * 1.5);

            new LineSchematic(new Enemy(new Sprite(Properties.Resources.ship3, Properties.Resources.ship3bis), 0, 0), 5, 1)
                .generateScheme(Game.game, Game.game.gameSize.Width - Game.game.gameSize.Width / 4, new Vecteur2D(Game.game.gameSize.Width / 8, Game.game.gameSize.Height / 8 + verticalOffset));

            verticalOffset += (int)(Properties.Resources.ship3.Height * 1.5);

            new LineSchematic(new Enemy(new Sprite(Properties.Resources.ship3, Properties.Resources.ship3bis), 0, 0), 5, 1)
               .generateScheme(Game.game, Game.game.gameSize.Width - Game.game.gameSize.Width / 4, new Vecteur2D(Game.game.gameSize.Width / 8, Game.game.gameSize.Height / 8 + verticalOffset));
        }

        /// <summary>
        /// Permet de générer les enemis du niveau 2
        /// </summary>
        private static void Level2()
        {
            int verticalOffset = 0;
            new LineSchematic(new Enemy(new Sprite(Properties.Resources.ship2, Properties.Resources.ship2bis), 0, 0), 3, 3)
                .generateScheme(Game.game, Game.game.gameSize.Width - Game.game.gameSize.Width / 4, new Vecteur2D(Game.game.gameSize.Width / 8, Game.game.gameSize.Height / 8));

            verticalOffset += (int)(Properties.Resources.ship2.Height * 1.5);

            new LineSchematic(new Enemy(new Sprite(Properties.Resources.ship3, Properties.Resources.ship3bis), 0, 0), 5, 2)
                .generateScheme(Game.game, Game.game.gameSize.Width - Game.game.gameSize.Width / 4, new Vecteur2D(Game.game.gameSize.Width / 8, Game.game.gameSize.Height / 8 + verticalOffset));

            verticalOffset += (int)(Properties.Resources.ship3.Height * 1.5);

            new LineSchematic(new Enemy(new Sprite(Properties.Resources.ship3, Properties.Resources.ship3bis), 0, 0), 5, 2)
               .generateScheme(Game.game, Game.game.gameSize.Width - Game.game.gameSize.Width / 4, new Vecteur2D(Game.game.gameSize.Width / 8, Game.game.gameSize.Height / 8 + verticalOffset));

            verticalOffset += (int)(Properties.Resources.ship3.Height * 1.5);

            new LineSchematic(new Enemy(new Sprite(Properties.Resources.ship6, Properties.Resources.ship6bis), 0, 0), 10, 1)
               .generateScheme(Game.game, Game.game.gameSize.Width - Game.game.gameSize.Width / 4, new Vecteur2D(Game.game.gameSize.Width / 8, Game.game.gameSize.Height / 8 + verticalOffset));
        }

        /// <summary>
        /// Permet de générer les enemis du niveau 3
        /// </summary>
        private static void Level3()
        {
            int verticalOffset = 0;
            new LineSchematic(new Enemy(new Sprite(Properties.Resources.ship4), 0, 0), 1, 10)
                .generateScheme(Game.game, Game.game.gameSize.Width - Game.game.gameSize.Width / 4, new Vecteur2D(Game.game.gameSize.Width / 8, Game.game.gameSize.Height / 8));

            verticalOffset += (int)(Properties.Resources.ship4.Height * 1.5);

            new LineSchematic(new Enemy(new Sprite(Properties.Resources.ship5, Properties.Resources.ship5bis), 0, 0), 5, 3)
                .generateScheme(Game.game, Game.game.gameSize.Width - Game.game.gameSize.Width / 4, new Vecteur2D(Game.game.gameSize.Width / 8, Game.game.gameSize.Height / 8 + verticalOffset));

            verticalOffset += (int)(Properties.Resources.ship5.Height * 1.5);

            new LineSchematic(new Enemy(new Sprite(Properties.Resources.ship5, Properties.Resources.ship5bis), 0, 0), 5, 3)
                .generateScheme(Game.game, Game.game.gameSize.Width - Game.game.gameSize.Width / 4, new Vecteur2D(Game.game.gameSize.Width / 8, Game.game.gameSize.Height / 8 + verticalOffset));

            verticalOffset += (int)(Properties.Resources.ship5.Height * 1.5);

            new LineSchematic(new Enemy(new Sprite(Properties.Resources.ship8, Properties.Resources.ship8bis), 0, 0), 5, 1)
               .generateScheme(Game.game, Game.game.gameSize.Width - Game.game.gameSize.Width / 4, new Vecteur2D(Game.game.gameSize.Width / 8, Game.game.gameSize.Height / 8 + verticalOffset));

            verticalOffset += (int)(Properties.Resources.ship8.Height * 1.5);

            new LineSchematic(new Enemy(new Sprite(Properties.Resources.ship8, Properties.Resources.ship8bis), 0, 0), 5, 1)
               .generateScheme(Game.game, Game.game.gameSize.Width - Game.game.gameSize.Width / 4, new Vecteur2D(Game.game.gameSize.Width / 8, Game.game.gameSize.Height / 8 + verticalOffset));

            verticalOffset += (int)(Properties.Resources.ship8.Height * 1.5);

            new LineSchematic(new Enemy(new Sprite(Properties.Resources.ship8, Properties.Resources.ship8bis), 0, 0), 5, 1)
               .generateScheme(Game.game, Game.game.gameSize.Width - Game.game.gameSize.Width / 4, new Vecteur2D(Game.game.gameSize.Width / 8, Game.game.gameSize.Height / 8 + verticalOffset));
        }
    }
}
