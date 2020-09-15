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
    public class Sprite : Shape
    {
        private Bitmap image;

        public Sprite(Bitmap image)
        {
            this.image = image;
        }

        public Bitmap Draw()
        {
            return image;
        }

        public bool intersect(Shape shape)
        {
            throw new NotImplementedException();
        }
    }
}