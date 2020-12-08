using System;

namespace SpaceInvaders
{
    /// <summary>
    /// Classe utilitaire représentant un Vecteur (peut être utilisé pour représenter une position)
    /// </summary>
    public class Vecteur2D
    {
        /// <summary>
        /// Récupère la position en abcisse
        /// </summary>
        /// <value>la position en abcisse</value>
        public double X { get; private set; }
        /// <summary>
        /// Récupère la position en ordonnée
        /// </summary>
        /// <value>la position en ordonnée</value>
        public double Y { get; private set; }

        /// <summary>
        /// Récupère la norme du vecteur
        /// </summary>
        /// <value>la norme du vecteur</value>
        public double Norme
        {
            get { return Math.Sqrt(X * X + Y * Y); }
        }

        /// <summary>
        /// Crée un nouveau <see cref="Vecteur2D"/>
        /// </summary>
        /// <param name="x">la position en abcisse</param>
        /// <param name="y">la position en ordonnée</param>
        public Vecteur2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Crée un nouveau <see cref="Vecteur2D"/> a la position 0,0
        /// </summary>
        public Vecteur2D() : this(0, 0) { }

        /// <summary>
        /// Crée un nouveau <see cref="Vecteur2D"/> en copiant un autre vecteur
        /// </summary>
        /// <param name="v">Le vecteur a copier</param>
        public Vecteur2D(Vecteur2D v) : this(v.X, v.Y) { }

        /// <summary>
        /// Additionne deux vecteur
        /// </summary>
        /// <param name="v1">Premier vecteur</param>
        /// <param name="v2">Deuxième vecteur</param>
        /// <returns>Résultat de l'addition</returns>
        public static Vecteur2D operator +(Vecteur2D v1, Vecteur2D v2)
        {
            return new Vecteur2D(v1.X + v2.X, v1.Y + v2.Y);
        }

        /// <summary>
        /// Soustrait deux vecteur
        /// </summary>
        /// <param name="v1">Premier vecteur</param>
        /// <param name="v2">Deuxième vecteur</param>
        /// <returns>Résultat de la soustraction</returns>
        public static Vecteur2D operator -(Vecteur2D v1, Vecteur2D v2)
        {
            return new Vecteur2D(v1.X - v2.X, v1.Y - v2.Y);
        }

        /// <summary>
        /// Enlève un vecteur 1,1 au vecteur
        /// </summary>
        /// <param name="v">Le vecteur</param>
        /// <returns>Le résultat</returns>
        public static Vecteur2D operator -(Vecteur2D v)
        {
            return new Vecteur2D(v.X - 1, v.Y - 1);
        }

        /// <summary>
        /// Fait une multiplication scalaire au vecteur
        /// </summary>
        /// <param name="v">Le vecteur</param>
        /// <param name="scalaire">Le coefficient</param>
        /// <returns>Le résultat du scalaire</returns>
        public static Vecteur2D operator *(Vecteur2D v, double scalaire)
        {
            return new Vecteur2D(v.X * scalaire, v.Y * scalaire);
        }

        /// <summary>
        /// Fait une multiplication scalaire au vecteur
        /// </summary>
        /// <param name="scalaire">Le coefficient</param>
        /// <param name="v">Le vecteur</param>
        /// <returns>Le résultat du scalaire</returns>
        public static Vecteur2D operator *(double scalaire, Vecteur2D v)
        {
            return new Vecteur2D(v.X * scalaire, v.Y * scalaire);
        }

        /// <summary>
        /// Fait une division scalaire au vecteur
        /// </summary>
        /// <param name="v">Le vecteur</param>
        /// <param name="scalaire">Le coefficient</param>
        /// <returns>Le résultat du scalaire</returns>
        public static Vecteur2D operator /(Vecteur2D v, double scalaire)
        {
            return new Vecteur2D(v.X / scalaire, v.Y / scalaire);
        }

        /// <summary>
        /// Représente le vecteur sous une chaîne de caractère
        /// </summary>
        /// <returns>A <see cref="System.String" />chaîne de caractère représentant le vecteur</returns>
        public override string ToString()
        {
            return "(" + X + ", " + Y + ")";
        }

    }
}
