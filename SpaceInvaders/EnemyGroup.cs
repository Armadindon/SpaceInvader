using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SpaceInvaders
{
    class EnemyGroup : GameObject
    {
        Rectangle hitbox;
        private double xSpeed = 20;
        private double ySpeed = Game.game.gameSize.Height / 16;

        public EnemyGroup()
        {
            //Constructeur par défaut
            LevelController.nextLevel();
        }


        public override void Draw(Game gameInstance, Graphics graphics)
        {
        }

        public override bool IsAlive()
        {
            return Game.game.gameObjects.OfType<Enemy>().Count() != 0;
        }

        public override void Update(Game gameInstance, double deltaT)
        {
            if (hitbox == null) resizeHitbox();
            handleCollisions(gameInstance.gameObjects);
            Vecteur2D toAdd = determineToAdd(gameInstance, deltaT);
            if (hitbox.v2.X == gameInstance.gameSize.Width || hitbox.v1.X == 0)
            {
                hitbox += toAdd;
                xSpeed *= -1;
            }
            else
            {
                hitbox += toAdd;
            }
            foreach (Enemy enemy in gameInstance.gameObjects.OfType<Enemy>()) enemy.position += toAdd;
        }

        private void handleCollisions(HashSet<GameObject> objects)
        {
            foreach (GameObject gameObject in objects.Where((go) => { return go is Player || go is Bunker; }))
            {
                if (gameObject.IsColliding(this))
                {
                    gameObject.collision(this);
                }
            }
        }

        private Vecteur2D determineToAdd(Game gameInstance, double deltaT)
        {
            Vecteur2D toAdd;
            if (hitbox.v2.X == gameInstance.gameSize.Width || hitbox.v1.X == 0)
            {
                toAdd = new Vecteur2D((xSpeed < 0) ? 1 : -1, ySpeed); // Gère la fin de l'écran
                xSpeed *= 1.25;
            }
            else if (hitbox.v1.X + xSpeed * deltaT < 0)
            {
                toAdd = new Vecteur2D(-hitbox.v1.X, 0);
            }
            else if (hitbox.v2.X + xSpeed * deltaT > Game.game.gameSize.Width)
            {
                toAdd = new Vecteur2D(gameInstance.gameSize.Width - hitbox.v2.X, 0);
            }
            else
            {
                toAdd = new Vecteur2D(deltaT * xSpeed, 0);
            }

            return toAdd;
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
            return hitbox;
        }

        public override bool collision(GameObject go)
        {
            return false;
        }

        public void resizeHitbox()
        {
            Vecteur2D min = new Vecteur2D(int.MaxValue, int.MaxValue);
            Vecteur2D max = new Vecteur2D();

            foreach (Enemy enemy in Game.game.gameObjects.OfType<Enemy>())
            {
                if (enemy.position.X < min.X) min = new Vecteur2D(enemy.position.X, min.Y);
                if (enemy.position.Y < min.Y) min = new Vecteur2D(min.X, enemy.position.Y);
                if (enemy.position.X + enemy.sprite.Draw().Width > max.X) max = new Vecteur2D(enemy.position.X + enemy.sprite.Draw().Width, max.Y);
                if (enemy.position.Y + enemy.sprite.Draw().Height > max.Y) max = new Vecteur2D(max.X, enemy.position.Y + enemy.sprite.Draw().Height);

            }
            this.hitbox = new Rectangle(min, max);
        }

        public override Sprite GetSprite()
        {
            return null;
        }

        public override void Die()
        {
        }
    }
}