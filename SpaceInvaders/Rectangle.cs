using System.Drawing;

namespace SpaceInvaders
{
    public class Rectangle
    {
        public Vecteur2D v1;
        public Vecteur2D v2;

        public Rectangle(Vecteur2D v1, Vecteur2D v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }

        public Rectangle(double x, double y, double width, double height) : this(new Vecteur2D(x, y), new Vecteur2D(x + width, y + height)) { }

        public Rectangle(Rectangle r) : this(new Vecteur2D(r.v1), new Vecteur2D(r.v2)) { }

        public static Rectangle operator +(Rectangle r, Vecteur2D v)
        {
            return new Rectangle(r.v1 + v, r.v2 + v);
        }

        public void resizeP2(Vecteur2D v)
        {
            this.v2 += v;
        }

        public void resizeP1(Vecteur2D v)
        {
            this.v1 += v;
        }

        public bool intersect(Rectangle r)
        {
            return !(
                (r.v1.X > v2.X) ||
                (r.v1.Y > v2.Y) ||
                (v1.X > r.v2.X) ||
                (v1.Y > r.v2.Y)
                );
        }

        public bool contains(Vecteur2D v)
        {
            return intersect(new Rectangle(v.X, v.Y, 0, 0));
        }

        public Vecteur2D getOffsetV1(Rectangle r)
        {
            return v1 - r.v1;
        }

        public Vecteur2D getOffsetV2(Rectangle r)
        {
            return v2 - r.v2;
        }

        public void Draw(Graphics g, Pen pen)
        {
            g.DrawRectangle(pen, (float)v1.X, (float)v1.Y, (float)(v2.X - v1.X), (float)(v2.Y - v1.Y));
        }
    }
}