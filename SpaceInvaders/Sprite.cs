using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SpaceInvaders
{

    /// <summary>
    /// Classe reprensentant un sprite d'un objet, peut potentiellement être animé
    /// </summary>
    public class Sprite
    {
        /// <summary>
        /// Les images ou l'image du sprite
        /// </summary>
        private Bitmap[] image;

        /// <summary>
        /// Crée un nouveau <see cref="Sprite"/>
        /// </summary>
        /// <param name="images">Les images composant le sprite</param>
        public Sprite(params Bitmap[] images)
        {
            this.image = new Bitmap[images.Length];
            for (int i = 0; i < images.Length; i++)
            {
                this.image[i] = (Bitmap)images[i].Clone(); //Pour éviter qu'un changement sur un bitmap affecte les autres
            }
        }

        /// <summary>
        /// Crée un nouveau <see cref="Sprite"/> en copiant un autre sprite
        /// </summary>
        /// <param name="sprite">Le sprite a copier</param>
        public Sprite(Sprite sprite) : this(sprite.image) { }

        /// <summary>
        /// Donne l'image a afficher
        /// </summary>
        /// <returns>Image a dessiner</returns>
        public Bitmap Draw()
        {
            return image[(int)(Game.game.runningTime) % image.Length];
        }

        /// <summary>
        /// Determine les pixels qui sont en collisions entre deux sprites
        /// </summary>
        /// <param name="b">L'autre sprite</param>
        /// <param name="position">La position de ce sprite</param>
        /// <param name="positionB">La position de l'autre sprite</param>
        /// <returns>La liste des pixels en collisions (représentés par des vecteurs)</returns>
        public List<Vecteur2D> pixelColliding(Sprite b, Vecteur2D position, Vecteur2D positionB)
        {
            List<Vecteur2D> listColliding = new List<Vecteur2D>();
            if (b == null) return listColliding;

            //On détermine le rectangle en collision
            Vecteur2D v1Collision = new Vecteur2D(Math.Max(0, positionB.X - position.X), Math.Max(0, positionB.Y - position.Y));
            Vecteur2D v2Collision = new Vecteur2D(Math.Min(Draw().Width, positionB.X + b.Draw().Width - position.X), Math.Min(Draw().Height, positionB.Y + b.Draw().Height - position.Y));

            for (int y = (int)v1Collision.Y; y < (int)v2Collision.Y; y++)
            {
                for (int x = (int)v1Collision.X; x < (int)v2Collision.X; x++)
                {
                    if (isPixelsColliding(x, y, position, positionB, b))
                    {
                        listColliding.Add(new Vecteur2D(x, y));
                    }
                }

            }

            return listColliding;
        }

        /// <summary>
        /// Determines si les pixels sont en collision
        /// </summary>
        /// <param name="x">Position en abcisse du pixel</param>
        /// <param name="y">Position en ordonnée du pixel</param>
        /// <param name="position">La position du sprite</param>
        /// <param name="positionB">La position de l'autre sprite</param>
        /// <param name="b">L'autre sprite</param>
        /// <returns><c>true</c> si les pixels sont bien en collisions, sinon, <c>false</c>.</returns>
        private bool isPixelsColliding(int x, int y, Vecteur2D position, Vecteur2D positionB, Sprite b)
        {
            return !Draw().GetPixel(x, y).Equals(Color.FromArgb(0, 255, 255, 255))
                        && !b.Draw().GetPixel((int)(position.X - positionB.X + x), (int)(position.Y - positionB.Y + y)).Equals(Color.FromArgb(0, 255, 255, 255));
        }

        /// <summary>
        /// Supprime les pixels qui sont en collisions
        /// </summary>
        /// <param name="b">L'autre sprite</param>
        /// <param name="position">La position du sprite</param>
        /// <param name="positionB">La position de l'autre sprite</param>
        /// <returns>nombre de pixels supprimées</returns>
        public int deleteCollidingPixels(Sprite b, Vecteur2D position, Vecteur2D positionB)
        {
            List<Vecteur2D> collidingPixels = pixelColliding(b, position, positionB);
            foreach (Vecteur2D pixel in collidingPixels)
            {
                Draw().SetPixel((int)pixel.X, (int)pixel.Y, Color.FromArgb(0, 255, 255, 255));
            }
            return collidingPixels.Count();
        }
    }
}