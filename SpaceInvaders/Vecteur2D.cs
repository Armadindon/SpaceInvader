using System;

namespace SpaceInvaders
{
    public class Vecteur2D
    {
        public double X { get; private set; }
        public double Y { get; private set; }

        public double Norme
        {
            get { return Math.Sqrt(X * X + Y * Y); }
        }

        public Vecteur2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Vecteur2D() : this(0, 0) { }

        public Vecteur2D(Vecteur2D v) : this(v.X, v.Y) { }

        public static Vecteur2D operator +(Vecteur2D v1, Vecteur2D v2)
        {
            return new Vecteur2D(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vecteur2D operator -(Vecteur2D v1, Vecteur2D v2)
        {
            return new Vecteur2D(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vecteur2D operator -(Vecteur2D v)
        {
            return new Vecteur2D(v.X - 1, v.Y - 1);
        }

        public static Vecteur2D operator *(Vecteur2D v, double scalaire)
        {
            return new Vecteur2D(v.X * scalaire, v.Y * scalaire);
        }

        public static Vecteur2D operator *(double scalaire, Vecteur2D v)
        {
            return new Vecteur2D(v.X * scalaire, v.Y * scalaire);
        }

        public static Vecteur2D operator /(Vecteur2D v, double scalaire)
        {
            return new Vecteur2D(v.X / scalaire, v.Y / scalaire);
        }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ")";
        }

    }
}
