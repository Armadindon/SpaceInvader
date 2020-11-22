using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SpaceInvaders
{
    class Missil : GameObject
    {

        private Sprite sprite = new Sprite(Properties.Resources.shoot1);
        private Vecteur2D position;
        private int missilSpeed = 800;
        int lives = 1;
        Teams team;

        public Missil(Vecteur2D position, Teams team)
        {
            this.position = position;
            this.team = team;
            if (team == Teams.Player) missilSpeed *= -1; //On ira vers le haut
        }

        public Missil(Vecteur2D position, Teams team, int lives) : this(position, team)
        {
            this.lives = lives;
        }

        public override bool collision(GameObject go)
        {
            lives--;
            return true;
        }

        public override void Draw(Game gameInstance, Graphics graphics)
        {
            sprite.Draw().SetResolution(graphics.DpiX, graphics.DpiY);
            graphics.DrawImage(sprite.Draw(), (float) position.X, (float) position.Y);
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
            return lives > 0 && (position.X > 0 && position.X < Game.game.gameSize.Width) && (position.Y > 0 && position.Y < Game.game.gameSize.Height);
        }

        public override bool IsColliding(GameObject go)
        {
            
            if(whichTeam() != go.whichTeam() && getHitbox().intersect(go.getHitbox()))
            {
                return sprite.pixelColliding(go.GetSprite(),position,go.getHitbox().v1).Count != 0;
            }
            return false;
        }

        public override void Update(Game gameInstance, double deltaT)
        {
            position += new Vecteur2D(0, missilSpeed * deltaT);

            foreach (GameObject go in gameInstance.gameObjects)
            {
                checkAndHandleCollision(go);
            }
        }

        private bool checkAndHandleCollision(GameObject go)
        {
            if (IsColliding(go))
            {
                if (go.collision(this))
                {
                    collision(go);
                    return true;
                }
            }
            return false;
        }

        public override Teams whichTeam()
        {
            return team;
        }

        public override void Die()
        {
        }
    }
}