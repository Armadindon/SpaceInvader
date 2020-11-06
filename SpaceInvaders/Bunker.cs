using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpaceInvaders
{
    class Bunker : GameObject
    {
        private Sprite sprite = new Sprite(Properties.Resources.bunker);
        Vecteur2D position;
        private bool alive =true;

        public static void generateBunkers(Game instance, int number)
        {
            Bitmap sprite = Properties.Resources.bunker;
            Bitmap playerSprite = Properties.Resources.ship1;
            double spacing = 50;
            double totalSize = spacing * (number-1) + sprite.Width * number;

            for(int i = 0; i < number; i++)
            {
                instance.AddNewGameObject(new Bunker(instance.gameSize.Width / 4 + i * (sprite.Width + spacing), instance.gameSize.Height - (playerSprite.Height*3 + spacing + sprite.Height)));
            }
        }

        public Bunker(Vecteur2D position)
        {
            this.position = position;
        }

        public Bunker(double x, double y) : this(new Vecteur2D(x, y)) { }

        public override bool collision(GameObject go)
        {
            if (go is EnemyGroup)
            {
                alive = false;
            }
            else sprite.deleteCollidingPixels(go.GetSprite(), position, go.getHitbox().v1);
            return true;
        }

        public override void Draw(Game gameInstance, Graphics graphics)
        {
            sprite.Draw().SetResolution(graphics.DpiX, graphics.DpiY);
            graphics.DrawImage(sprite.Draw(), (float)position.X, (float)position.Y);
        }

        public override Rectangle getHitbox()
        {
            Bitmap sprite = this.sprite.Draw();
            return new Rectangle(position.X, position.Y, sprite.Width, sprite.Height);
        }

        public override Sprite GetSprite()
        {
            return sprite;
        }

        public override bool IsAlive()
        {
            return alive;
        }

        public override bool IsColliding(GameObject go)
        {
            if (whichTeam() != go.whichTeam() && getHitbox().intersect(go.getHitbox()))
            {
                if (go is EnemyGroup) return true;
                return sprite.pixelColliding(go.GetSprite(), position, go.getHitbox().v1).Count != 0;
            }
            return false;
        }

        public override void Update(Game gameInstance, double deltaT)
        {
            //Ne fait rien
        }

        public override Teams whichTeam()
        {
            return Teams.Props;
        }
    }
}