using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpaceInvaders
{
    class Bunker : GameObject
    {
        private Sprite sprite = new Sprite(Properties.Resources.bunker);
        Vecteur2D position;

        public Bunker(Vecteur2D position)
        {
            this.position = position;
        }

        public Bunker(double x, double y) : this(new Vecteur2D(x, y)) { }

        public override void Draw(Game gameInstance, Graphics graphics)
        {
            throw new NotImplementedException();
        }

        public override Rectangle getHitbox()
        {
            throw new NotImplementedException();
        }

        public override bool IsAlive()
        {
            throw new NotImplementedException();
        }

        public override bool IsColliding(GameObject go)
        {
            throw new NotImplementedException();
        }

        public override void Update(Game gameInstance, double deltaT)
        {
            throw new NotImplementedException();
        }

        public override Teams whichTeam()
        {
            throw new NotImplementedException();
        }
    }
}