using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SpaceInvaders
{
    public class Rectangle
    {
        public Vecteur2D v1;
        public Vecteur2D v2;

        Rectangle(Vecteur2D v1, Vecteur2D v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }

        public Rectangle(double x, double y, double width, double height) : this(new Vecteur2D(x, y), new Vecteur2D(x + width, y + height)){ }


        public static Rectangle operator+ (Rectangle r, Vecteur2D v)
        {
            return new Rectangle(r.v1 + v, r.v2 + v);
        }

        public bool intersect(Rectangle r)
        {
            return !((r.v1.X > v2.X) || (r.v1.Y > v2.Y) || (v1.X > r.v2.X) || (v1.Y > r.v2.X)); 
        }

        public void Draw(Graphics g ,Pen pen)
        {
            g.DrawRectangle(pen, (float) v1.X, (float) v1.Y, (float)( v2.X - v1.X), (float)( v2.Y - v1.Y));
        }
    }
}