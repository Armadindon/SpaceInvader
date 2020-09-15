using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SpaceInvaders
{
    class EnemyGroup : GameObject
    {
        Rectangle hitbox = new Rectangle(Game.game.gameSize.Width / 4, Game.game.gameSize.Height / 8, Game.game.gameSize.Width / 2, Game.game.gameSize.Height / 4);
        private int xSpeed = 50;
        private int ySpeed = Game.game.gameSize.Height / 16;


        public override void Draw(Game gameInstance, Graphics graphics)
        {
            //En cas de débug
            graphics.DrawRectangle(Pens.Red, (float)hitbox.v1.X, (float)hitbox.v1.Y, (float) (hitbox.v2.X - hitbox.v1.X), (float) (hitbox.v2.Y - hitbox.v1.Y));
        }

        public override bool IsAlive()
        {
            return true;
        }

        public override void Update(Game gameInstance, double deltaT)
        {
            if( hitbox.v2.X == gameInstance.gameSize.Width || hitbox.v1.X == 0)
            {
                hitbox += new Vecteur2D((xSpeed < 0)? 1 : -1 , ySpeed); ;
                xSpeed *= -1;
            }
            else if (hitbox.v1.X + xSpeed * deltaT < 0)
            {
                hitbox += new Vecteur2D(-hitbox.v1.X, 0);
            }
            else if (hitbox.v2.X + xSpeed * deltaT > Game.game.gameSize.Width)
            {
                hitbox += new Vecteur2D(gameInstance.gameSize.Width - hitbox.v2.X ,0);
            }
            else
            {
                hitbox += new Vecteur2D(deltaT * xSpeed, 0);
            }

        }
    }
}