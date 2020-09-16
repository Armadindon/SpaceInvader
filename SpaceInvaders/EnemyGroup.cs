using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SpaceInvaders
{
    class EnemyGroup : GameObject
    {
        public static EnemyGroup enemyGroup;

        Rectangle hitbox;
        private double xSpeed = 15;
        private double ySpeed = Game.game.gameSize.Height / 16;

        List<List<Enemy>> enemies = new List<List<Enemy>>();
        double enemyOffset = 10;
        

        public static EnemyGroup createEnemyGroup()
        {
            if (EnemyGroup.enemyGroup != null) return EnemyGroup.enemyGroup;
            EnemyGroup.enemyGroup = new EnemyGroup();
            return EnemyGroup.enemyGroup;
        }

        private EnemyGroup()
        {
            //Constructeur par défaut
            Enemy[] scheme = { new Enemy(0, 0), new Enemy(new Sprite(Properties.Resources.ship3), 0, 0), new Enemy(new Sprite(Properties.Resources.ship5), 0, 0), new Enemy(new Sprite(Properties.Resources.ship6), 0, 0), new Enemy(new Sprite(Properties.Resources.ship7), 0, 0), new Enemy(new Sprite(Properties.Resources.ship7), 0, 0), new Enemy(new Sprite(Properties.Resources.ship7), 0, 0) }; 
            for(int i=0; i < 7; i++)
            {
                enemies.Add(new List<Enemy>());
                for(int j=0; j < 6; j++)
                {
                    enemies[i].Add(new Enemy(scheme[i], Game.game.gameSize.Width / 8 + (enemyOffset + scheme[0].sprite.Draw().Width) * j, Game.game.gameSize.Height / 16 + (enemyOffset + scheme[0].sprite.Draw().Height) * i));
                    Game.game.AddNewGameObject(enemies[i][j]);
                }
            }
            hitbox = new Rectangle(Game.game.gameSize.Width / 8, Game.game.gameSize.Height / 16, (enemyOffset + scheme[0].sprite.Draw().Width) * 6 - enemyOffset, (enemyOffset + scheme[0].sprite.Draw().Height) * 7 - enemyOffset);
        }


        public override void Draw(Game gameInstance, Graphics graphics)
        {
            //graphics.DrawRectangle(Pens.Red, (float)hitbox.v1.X, (float)hitbox.v1.Y, (float) (hitbox.v2.X - hitbox.v1.X), (float) (hitbox.v2.Y - hitbox.v1.Y));
        }

        public override bool IsAlive()
        {
            return true;
        }

        public override void Update(Game gameInstance, double deltaT)
        {
            Vecteur2D toAdd;
            if( hitbox.v2.X == gameInstance.gameSize.Width || hitbox.v1.X == 0)
            {
                toAdd = new Vecteur2D((xSpeed < 0) ? 1 : -1, ySpeed);
                hitbox += toAdd;
                xSpeed *= -1;
            }
            else if (hitbox.v1.X + xSpeed * deltaT < 0)
            {
                toAdd = new Vecteur2D(-hitbox.v1.X, 0);
                hitbox += toAdd;
            }
            else if (hitbox.v2.X + xSpeed * deltaT > Game.game.gameSize.Width)
            {
                toAdd = new Vecteur2D(gameInstance.gameSize.Width - hitbox.v2.X, 0);
                hitbox += toAdd;
            }
            else
            {
                toAdd = new Vecteur2D(deltaT * xSpeed, 0);
                hitbox += toAdd;
            }
            foreach (List<Enemy> enemies in this.enemies)
                foreach (Enemy enemy in enemies)
                    enemy.position += toAdd;
        }

        public override bool IsColliding(GameObject go)
        {
            return whichTeam() != go.whichTeam() && getHitbox().intersect(go.getHitbox());
        }

        public override Teams whichTeam()
        {
            return Teams.Enemy;
        }

        public override Rectangle getHitbox()
        {
            return hitbox;
        }
    }
}