using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SpaceInvaders
{
    class Missil : GameObject
    {

        private Sprite sprite = new Sprite(Properties.Resources.shoot1);
        private Vecteur2D position;
        private int missilSpeed = 1000;
        Teams team;

        public Missil(Vecteur2D position, Teams team)
        {
            this.position = position;
            this.team = team;
            if (team == Teams.Player) missilSpeed *= -1; //On ira vers le haut
        }

        public override void Draw(Game gameInstance, Graphics graphics)
        {
            sprite.Draw().SetResolution(graphics.DpiX, graphics.DpiY);
            graphics.DrawImage(sprite.Draw(), (float) position.X, (float) position.Y);
        }

        public override bool IsAlive()
        {
            return (position.X > 0 && position.X < Game.game.gameSize.Width) && (position.Y > 0 && position.Y < Game.game.gameSize.Height);
        }

        public override void Update(Game gameInstance, double deltaT)
        {
            position += new Vecteur2D(0, missilSpeed * deltaT);
        }
    }
}