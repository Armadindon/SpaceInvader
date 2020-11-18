using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Configuration;

namespace SpaceInvaders
{
    class Player : GameObject
    {

        private Sprite sprite;

        private Vecteur2D position;

        private double PlayerSpeed = 800;

        private Missil activeMissil = null;

        private int lives = 500;


        public Player(Vecteur2D position)
        {
            sprite = new Sprite(Properties.Resources.ship1);
            this.position = position;
        }

        public Player(double x, double y): this(new Vecteur2D(x, y)) { }

        public Player() : this(0,0) 
        {
            Bitmap playerSprite = Properties.Resources.ship1;
            Vecteur2D pos = new Vecteur2D((Game.game.gameSize.Width / 2) - (playerSprite.Width), Game.game.gameSize.Height - (2 * playerSprite.Height));
            this.position = pos;
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

        public override bool IsAlive()
        {
            return lives > 0;
        }

        public override bool IsColliding(GameObject go)
        {
            return  whichTeam() != go.whichTeam() && getHitbox().intersect(go.getHitbox());
        }

        public int getLives()
        {
            return lives;
        }

        public override void Update(Game gameInstance, double deltaT)
        {
            if (activeMissil != null && !activeMissil.IsAlive()) this.activeMissil = null;
            if (Game.game.keyPressed.Contains(Keys.Right) || Game.game.keyPressed.Contains(Keys.Left))
            {
                handleOutOfBound(deltaT);
                position += determineMove(deltaT);
            }
            if (Game.game.keyPressed.Contains(Keys.Space) && activeMissil == null)
            {
                fireMissil(gameInstance);
            }
        }

        private void fireMissil(Game gameInstance)
        {
            this.activeMissil = new Missil(position + new Vecteur2D(sprite.Draw().Width / 2 - Properties.Resources.shoot1.Width / 2, 0), Teams.Player);
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(Properties.Resources.laser);
            player.Play();
            gameInstance.AddNewGameObject(this.activeMissil);
        }

        private Vecteur2D determineMove( double deltaT)
        {
            if (Game.game.keyPressed.Contains(Keys.Right))
            {
                return new Vecteur2D(PlayerSpeed * deltaT, 0);
            }
            return new Vecteur2D(-PlayerSpeed * deltaT, 0);
        }

        private void handleOutOfBound(double deltaT)
        {
            if (position.X + PlayerSpeed * deltaT > Game.game.gameSize.Width - sprite.Draw().Width)
            {
                position = new Vecteur2D(Game.game.gameSize.Width - sprite.Draw().Width, position.Y);
            }
            else if (position.X + -PlayerSpeed * deltaT < 0)
            {
                position = new Vecteur2D(0, position.Y);
            }
        }

        public override Teams whichTeam()
        {
            return Teams.Player;
        }

        public override bool collision(GameObject go)
        {
            if (go is EnemyGroup) lives -= int.MaxValue;
            else
            {
                sprite.deleteCollidingPixels(go.GetSprite(), position, go.getHitbox().v1);
                lives--;
            }
            return true;
        }

        public override Sprite GetSprite()
        {
            return sprite;
        }

        public override void Die()
        {
        }
    }
}