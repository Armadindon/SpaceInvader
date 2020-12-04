using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SpaceInvaders
{

    class Bonus : GameObject
    {
        private Sprite sprite;
        private Vecteur2D position;
        BonusType type;
        private int lives = 1;
        private int bonusSpeed = 100;

        public Bonus(Sprite sprite, Vecteur2D position)
        {
            this.sprite = sprite;
            this.position = position;
            List<BonusType> types = Enum.GetValues(typeof(BonusType)).Cast<BonusType>().ToList();
            int random = new Random().Next(types.Count);
            type = types[random];
        }

        public override bool collision(GameObject go)
        {
            lives--;
            if (go is Player) ((Player)go).addBonus(type, 6);
            return true;
        }

        public override void Die()
        {
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
            return lives > 0 && (position.X > 0 && position.X < Game.game.gameSize.Width) && (position.Y > 0 && position.Y < Game.game.gameSize.Height);
        }

        public override bool IsColliding(GameObject go)
        {
            return whichTeam() == Teams.Player && go.getHitbox().intersect(getHitbox());
        }

        public override void Update(Game gameInstance, double deltaT)
        {
            position += new Vecteur2D(0, bonusSpeed * deltaT);
            Player p = gameInstance.gameObjects.OfType<Player>().First();
            if (p.IsColliding(this)) collision(p);
        }

        public override Teams whichTeam()
        {
            return Teams.Other;
        }
    }
}
