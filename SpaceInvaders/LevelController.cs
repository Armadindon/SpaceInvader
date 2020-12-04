using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceInvaders
{
    class LevelController
    {

        private static int currentLevel = 1;


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

        public static bool haveNextLevel()
        {
            return currentLevel < 4;
        }

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
