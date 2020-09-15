﻿using System;
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

        public Player(Vecteur2D position)
        {
            sprite = new Sprite(SpaceInvaders.Properties.Resources.ship1);
            this.position = position;
        }

        public Player(double x, double y): this(new Vecteur2D(x, y)) { }

        public override void Draw(Game gameInstance, Graphics graphics)
        {
            sprite.Draw().SetResolution(graphics.DpiX, graphics.DpiY);
            graphics.DrawImage(sprite.Draw(), (float) position.X, (float) position.Y);
            //Draw Hitbox
            graphics.DrawRectangle(Pens.Red, (float)position.X, (float)position.Y, sprite.Draw().Width, sprite.Draw().Height);
            //Draw Player Camp Hitbox
            graphics.DrawRectangle(Pens.Red, 0  , (float)(gameInstance.gameSize.Width * 0.75), gameInstance.gameSize.Width, (float)(gameInstance.gameSize.Height * 0.25));

        }

        public override bool IsAlive()
        {
            return true;
        }

        public override void Update(Game gameInstance, double deltaT)
        {
            if (activeMissil != null && !activeMissil.IsAlive()) this.activeMissil = null;
            if (Game.game.keyPressed.Contains(Keys.Right))
            {
                if (position.X + PlayerSpeed * deltaT > Game.game.gameSize.Width - sprite.Draw().Width) position = new Vecteur2D(Game.game.gameSize.Width - sprite.Draw().Width, position.Y);
                else position += new Vecteur2D(PlayerSpeed * deltaT, 0);
            }
            if (Game.game.keyPressed.Contains(Keys.Left))
            {
                if (position.X + -PlayerSpeed * deltaT < 0) position = new Vecteur2D(0, position.Y);
                else position += new Vecteur2D(-PlayerSpeed * deltaT, 0);
            }
            if (Game.game.keyPressed.Contains(Keys.Space) && activeMissil == null)
            {
                //TODO : Faire en sorte que la classe Missil rajoute sa taille de sprite de manière statique
                this.activeMissil = new Missil(position + new Vecteur2D(sprite.Draw().Width / 2 - Properties.Resources.shoot1.Width / 2, 0), Teams.Player);
                gameInstance.AddNewGameObject(this.activeMissil);
            }

        }
    }
}