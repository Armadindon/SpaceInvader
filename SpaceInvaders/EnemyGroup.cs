using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SpaceInvaders
{
    /// <summary>
    /// Représente le groupe d'énnemis
    /// Implémente la classe <see cref="SpaceInvaders.GameObject" />
    /// </summary>
    /// <seealso cref="SpaceInvaders.GameObject" />
    class EnemyGroup : GameObject
    {
        /// <summary>
        /// La hitbox du groupe
        /// </summary>
        Rectangle hitbox;
        /// <summary>
        /// La vitesse du groupe en abcisse
        /// </summary>
        private double xSpeed = 20;
        /// <summary>
        /// La vitesse du groupe en ordonnée
        /// </summary>
        private double ySpeed = Game.game.gameSize.Height / 16;

        /// <summary>
        /// Crée un nouveau bloc <see cref="EnemyGroup"/> et le remplit.
        /// </summary>
        public EnemyGroup()
        {
            //Constructeur par défaut
            LevelController.nextLevel(); //Passe au niveau suivant
        }


        /// <summary>
        /// Dessine l'objet
        /// </summary>
        /// <param name="gameInstance">Instance de la partie en cours</param>
        /// <param name="graphics">Graphisme a utiliser</param>
        public override void Draw(Game gameInstance, Graphics graphics)
        {
        }

        /// <summary>
        /// Determines si l'objet est en vie, si non, il sera retiré par la suite
        /// </summary>
        /// <returns>true si l'objet est en vie, false sinon</returns>
        public override bool IsAlive()
        {
            return Game.game.gameObjects.OfType<Enemy>().Count() != 0;
        }

        /// <summary>
        /// Met a jour l'état d'un objet
        /// </summary>
        /// <param name="gameInstance">Instance de la partie</param>
        /// <param name="deltaT">Temps écoulé depuis le dernier appel</param>
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

        /// <summary>
        /// Gère les collisions des enemis
        /// </summary>
        /// <param name="objects">Les objets a tester</param>
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

        /// <summary>
        /// Determines to le déplacement a ajouter au groupe d'ennemi
        /// </summary>
        /// <param name="gameInstance">L'instance de partie</param>
        /// <param name="deltaT">Le temps depuis le dernier appel</param>
        /// <returns>Vecteur a ajouter</returns>
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

        /// <summary>
        /// Determine si l'objet est en collision avec un autre gameObject
        /// </summary>
        /// <param name="go">L'autre GameObject</param>
        /// <returns><c>true</c> si les objets sont en collision, sinon <c>false</c>.</returns>
        public override bool IsColliding(GameObject go)
        {
            return whichTeam() != go.whichTeam() && getHitbox().intersect(go.getHitbox());
        }

        /// <summary>
        /// Renvoie la team de l'objet
        /// </summary>
        /// <returns>La team associé a l'objet</returns>
        public override Teams whichTeam()
        {
            return Teams.Enemy;
        }

        /// <summary>
        /// Permet de récuperer la hitbox de l'objet
        /// </summary>
        /// <returns>Rectangle de la hitbox</returns>
        public override Rectangle getHitbox()
        {
            return hitbox;
        }

        /// <summary>
        /// Action en cas de collision avec un autre GameObject
        /// </summary>
        /// <param name="go">L'autre GameObject</param>
        /// <returns><c>true</c> si une action a été effectuée, <c>false</c> sinon.</returns>
        public override bool collision(GameObject go)
        {
            return false;
        }

        /// <summary>
        /// Redimensionne la hitbox en fonction des enemis à l'intérieur
        /// </summary>
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

        /// <summary>
        /// Permet d'obtenir le sprite
        /// </summary>
        /// <returns>Sprite.</returns>
        public override Sprite GetSprite()
        {
            return null;
        }

        /// <summary>
        /// Action a réaliser en cas de mort
        /// </summary>
        public override void Die()
        {
        }
    }
}