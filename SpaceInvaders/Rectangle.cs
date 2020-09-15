using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceInvaders
{
    public class Rectangle : Shape
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


        public bool intersect(Shape shape)
        {
            throw new NotImplementedException();
        }
    }
}