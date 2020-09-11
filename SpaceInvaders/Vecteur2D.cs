using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceInvaders
{
    class Vecteur2D
    {
        double x;
        double y;

        public double Norme
        {
            get { return Math.Sqrt(x * x + y * y); }
        }

        public Vecteur2D(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public Vecteur2D() : this(0, 0) { }

        public static Vecteur2D operator+ (Vecteur2D v1, Vecteur2D v2)
        {
            return new Vecteur2D(v1.x + v2.x, v1.y + v2.y);
        }

        public static Vecteur2D operator- (Vecteur2D v1, Vecteur2D v2)
        {
            return new Vecteur2D(v1.x - v2.x, v1.y - v2.y);
        }

        public static Vecteur2D operator -(Vecteur2D v)
        {
            return new Vecteur2D(v.x - 1, v.y - 1);
        }

        public static Vecteur2D operator* (Vecteur2D v, double scalaire)
        {
            return new Vecteur2D(v.x * scalaire, v.y * scalaire);
        }

        public static Vecteur2D operator* (double scalaire, Vecteur2D v)
        {
            return new Vecteur2D(v.x * scalaire, v.y * scalaire);
        }

        public static Vecteur2D operator/ (Vecteur2D v, double scalaire)
        {
            return new Vecteur2D(v.x / scalaire, v.y / scalaire);
        }

    }
}
