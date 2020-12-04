using System.Drawing;
using System.Linq;

namespace SpaceInvaders
{
    class Enemy : GameObject
    {
        public Sprite sprite;
        public Vecteur2D position;
        public int lives = 1;

        public Enemy(Sprite sprite, Vecteur2D position, int lives)
        {
            this.sprite = sprite;
            this.position = position;
            this.lives = lives;
        }

        public Enemy(Sprite sprite, double x, double y) : this(sprite, new Vecteur2D(x, y), 1) { }

        public Enemy(Enemy enemy, Vecteur2D position) : this(new Sprite(enemy.sprite), position, enemy.lives) { }
        public Enemy(Enemy enemy, Vecteur2D position, int lives) : this(new Sprite(enemy.sprite), position, lives) { }

        public Enemy(Enemy enemy, double x, double y) : this(enemy, new Vecteur2D(x,y)) { }

        public Enemy(Vecteur2D position) : this(new Sprite(Properties.Resources.ship2), position, 1) { }

        public Enemy(double x, double y) : this(new Sprite(Properties.Resources.ship2), new Vecteur2D(x, y), 1) { }

        public override void Draw(Game gameInstance, Graphics graphics)
        {
            sprite.Draw().SetResolution(graphics.DpiX, graphics.DpiY);
            graphics.DrawImage(sprite.Draw(), (float)position.X, (float)position.Y);
        }

        public override bool IsAlive()
        {
            return lives > 0;
        }

        public override void Update(Game gameInstance, double deltaT)
        {
            if (Game.random.Next(0, 100000 - (42 - gameInstance.gameObjects.OfType<Enemy>().Count()) * 1000) < 10)
            {
                Missil missil = new Missil(position + new Vecteur2D(sprite.Draw().Width / 2 - Properties.Resources.shoot1.Width / 2, 0), Teams.Enemy);
                gameInstance.AddNewGameObject(missil);
            }
        }

        public override bool IsColliding(GameObject go)
        {
            return whichTeam() != go.whichTeam() && getHitbox().intersect(go.getHitbox());
        }

        public override Teams whichTeam()
        {
            return Teams.Enemy;
        }

        public override Rectangle getHitbox()
        {
            Bitmap sprite = this.sprite.Draw();
            return new Rectangle(position.X, position.Y, sprite.Width, sprite.Height);
        }

        public override bool collision(GameObject go)
        {
            sprite.deleteCollidingPixels(go.GetSprite(), position, go.getHitbox().v1);
            lives--;
            return true;
        }

        public override Sprite GetSprite()
        {
            return sprite;
        }

        public override void Die()
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(Properties.Resources.explosion);
            if (Game.random.Next(0, 11) < 1) Game.game.AddNewGameObject(new Bonus(new Sprite(Properties.Resources.bonus), position));
            player.Play();
        }
    }
}