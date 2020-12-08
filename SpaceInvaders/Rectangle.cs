using System.Drawing;

namespace SpaceInvaders
{
    /// <summary>
    /// Class utilitaire représentant un rectangle
    /// </summary>
    public class Rectangle
    {
        /// <summary>
        /// Le coin en haut a gauche
        /// </summary>
        public Vecteur2D v1;
        /// <summary>
        /// Le coin en bas a droite
        /// </summary>
        public Vecteur2D v2;

        /// <summary>
        /// Crée un nouveau <see cref="Rectangle"/>
        /// </summary>
        /// <param name="v1">Le coin en haut a gauche</param>
        /// <param name="v2">Le coin en bas a droite</param>
        public Rectangle(Vecteur2D v1, Vecteur2D v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }

        /// <summary>
        /// Crée un nouveau <see cref="Rectangle"/>
        /// </summary>
        /// <param name="x">La position en abcisse du coin en haut a gauche</param>
        /// <param name="y">La position en ordonée du coin en haut a gauche</param>
        /// <param name="width">La longueur du rectangle</param>
        /// <param name="height">La hauteur du rectangle</param>
        public Rectangle(double x, double y, double width, double height) : this(new Vecteur2D(x, y), new Vecteur2D(x + width, y + height)) { }

        /// <summary>
        /// <param name="x">La position en abcisse du coin en haut a gauche</param> en copiant un rectangle
        /// </summary>
        /// <param name="r">Le rectangle a copier</param>
        public Rectangle(Rectangle r) : this(new Vecteur2D(r.v1), new Vecteur2D(r.v2)) { }

        /// <summary>
        /// Permet d'ajouter un déplacement a un rectangle
        /// </summary>
        /// <param name="r">Le rectangle</param>
        /// <param name="v">Le déplacement a effectuer</param>
        /// <returns>The result of the operator.</returns>
        public static Rectangle operator +(Rectangle r, Vecteur2D v)
        {
            return new Rectangle(r.v1 + v, r.v2 + v);
        }

        /// <summary>
        /// Redimensionne en changeant le point en bas a droite
        /// </summary>
        /// <param name="v">Le changement a effectuer</param>
        public void resizeP2(Vecteur2D v)
        {
            this.v2 += v;
        }

        /// <summary>
        /// Redimensionne en changeant le point en haut a gauche
        /// </summary>
        /// <param name="v">Le changement a effectuer</param>
        public void resizeP1(Vecteur2D v)
        {
            this.v1 += v;
        }

        /// <summary>
        /// Verifie si il y a une intersection avec un autre rectangle
        /// </summary>
        /// <param name="r">L'autre rectangle</param>
        /// <returns><c>true</c> si il y a une intersection, <c>false</c> sinon</returns>
        public bool intersect(Rectangle r)
        {
            return !(
                (r.v1.X > v2.X) ||
                (r.v1.Y > v2.Y) ||
                (v1.X > r.v2.X) ||
                (v1.Y > r.v2.Y)
                );
        }

    }
}