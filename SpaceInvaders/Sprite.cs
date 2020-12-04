using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SpaceInvaders
{
    /**
     *Classe Utilitaire
     *L'intêret et de ne pas donner directement accès aux Bitmap aux GameObjects afin de pouvoir changer le fonctionnement interne si besoin (Animation par exemple)
     */
    public class Sprite
    {
        private Bitmap[] image;
        private double aliveTime = 0;

        public Sprite(params Bitmap[] images)
        {
            this.image = new Bitmap[images.Length];
            for (int i = 0; i < images.Length; i++)
            {
                this.image[i] = (Bitmap)images[i].Clone(); //Pour éviter qu'un changement sur un bitmap affecte les autres
            }
        }

        public Sprite(Sprite sprite) : this(sprite.image) { }

        public Bitmap Draw()
        {
            return image[(int)(aliveTime) % image.Length];
        }

        public List<Vecteur2D> pixelColliding(Sprite b, Vecteur2D position, Vecteur2D positionB)
        {
            // TODO : Taille un peu limite, tenter de trouver un moyen de simplifier le code
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

        public void update(double deltaT)
        {
            aliveTime += deltaT;
        }

        private bool isPixelsColliding(int x, int y, Vecteur2D position, Vecteur2D positionB, Sprite b)
        {
            return !Draw().GetPixel(x, y).Equals(Color.FromArgb(0, 255, 255, 255))
                        && !b.Draw().GetPixel((int)(position.X - positionB.X + x), (int)(position.Y - positionB.Y + y)).Equals(Color.FromArgb(0, 255, 255, 255));
        }

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