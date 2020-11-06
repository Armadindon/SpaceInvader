using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SpaceInvaders
{
    /**
     *Classe Utilitaire
     *L'intêret et de ne pas donner directement accès aux Bitmap aux GameObjects afin de pouvoir changer le fonctionnement interne si besoin (Animation par exemple)
     */
    public class Sprite
    {
        private Bitmap image;

        public Sprite(Bitmap image)
        {
            this.image = image;
        }

        public Sprite(Sprite sprite) : this(sprite.image) { }

        public Bitmap Draw()
        {
            return image;
        }

        public List<Vecteur2D> pixelColliding(Sprite b, Vecteur2D position, Vecteur2D positionB)
        {
            // TODO : Taille un peu limite, tenter de trouver un moyen de simplifier le code
            List<Vecteur2D> listColliding = new List<Vecteur2D>();
            if (b == null) return listColliding;

            //On détermine le rectangle en collision
            Vecteur2D v1Collision = new Vecteur2D(Math.Max(0, positionB.X - position.X), Math.Max(0, positionB.Y - position.Y));
            Vecteur2D v2Collision = new Vecteur2D(Math.Min(image.Width, positionB.X + b.image.Width - position.X), Math.Min(image.Height, positionB.Y + b.image.Height - position.Y));

            for(int y = (int) v1Collision.Y; y < (int)v2Collision.Y; y++) 
            {
                for (int x = (int)v1Collision.X; x < (int)v2Collision.X; x++)
                {
                    if(isPixelsColliding(x,y,position,positionB, b))
                    {
                        listColliding.Add(new Vecteur2D(x, y));
                    }
                }

            }

            return listColliding;
        }

        private bool isPixelsColliding(int x, int y, Vecteur2D position, Vecteur2D positionB, Sprite b)
        {
            return !image.GetPixel(x, y).Equals(Color.FromArgb(0, 255, 255, 255))
                        && !b.image.GetPixel((int)(position.X - positionB.X + x), (int)(position.Y - positionB.Y + y)).Equals(Color.FromArgb(0, 255, 255, 255));
        }

        public int deleteCollidingPixels(Sprite b, Vecteur2D position, Vecteur2D positionB)
        {
            List < Vecteur2D > collidingPixels = pixelColliding(b, position, positionB);
            foreach (Vecteur2D pixel in collidingPixels)
            {
                image.SetPixel((int)pixel.X, (int)pixel.Y, Color.FromArgb(0,255,255,255));
            }
            return collidingPixels.Count();
        }
    }
}