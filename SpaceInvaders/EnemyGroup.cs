using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SpaceInvaders
{
    class EnemyGroup : GameObject
    {
        Rectangle hitbox;
        private double xSpeed = 15;
        private double ySpeed = Game.game.gameSize.Height / 16;

        double enemyOffset = 10;
        


        public EnemyGroup()
        {
            //Constructeur par défaut
            Enemy[] scheme = { new Enemy(0, 0), new Enemy(new Sprite(Properties.Resources.ship3), 0, 0), new Enemy(new Sprite(Properties.Resources.ship5), 0, 0), new Enemy(new Sprite(Properties.Resources.ship6), 0, 0), new Enemy(new Sprite(Properties.Resources.ship7), 0, 0), new Enemy(new Sprite(Properties.Resources.ship7), 0, 0), new Enemy(new Sprite(Properties.Resources.ship7), 0, 0) }; 
            for(int i=0; i < 7; i++)
            {
                for(int j=0; j < 6; j++)
                {
                    Game.game.AddNewGameObject(new Enemy(scheme[i], Game.game.gameSize.Width / 8 + (enemyOffset + scheme[0].sprite.Draw().Width) * j, Game.game.gameSize.Height / 16 + (enemyOffset + scheme[0].sprite.Draw().Height) * i));
                }
            }
            hitbox = new Rectangle(Game.game.gameSize.Width / 8, Game.game.gameSize.Height / 16, (enemyOffset + scheme[0].sprite.Draw().Width) * 6 - enemyOffset, (enemyOffset + scheme[0].sprite.Draw().Height) * 7 - enemyOffset);
        }


        public override void Draw(Game gameInstance, Graphics graphics)
        {
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
            foreach (Enemy enemy in gameInstance.gameObjects.OfType<Enemy>()) enemy.position += toAdd;
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

        public override bool collision(GameObject go)
        {
            return false;
        }

        public void resizeHitbox()
        {
            Vecteur2D min = new Vecteur2D(int.MaxValue, int.MaxValue);
            Vecteur2D max = new Vecteur2D();

            foreach(Enemy enemy in Game.game.gameObjects.OfType<Enemy>())
            {
                if (enemy.position.X < min.X) min = new Vecteur2D(enemy.position.X, min.Y);
                if (enemy.position.Y < min.Y) min = new Vecteur2D(min.X, enemy.position.Y);
                if (enemy.position.X + enemy.sprite.Draw().Width > max.X ) max = new Vecteur2D(enemy.position.X + enemy.sprite.Draw().Width, max.Y);
                if (enemy.position.Y + enemy.sprite.Draw().Height > max.Y) max = new Vecteur2D(max.X , enemy.position.Y + enemy.sprite.Draw().Height);

            }
            this.hitbox = new Rectangle(min, max);
        }

        public override Sprite GetSprite()
        {
            return null;
        }
    }
}