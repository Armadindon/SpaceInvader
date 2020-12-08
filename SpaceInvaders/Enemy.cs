using System.Drawing;
using System.Linq;

namespace SpaceInvaders
{
    /// <summary>
    /// Classe représentant un enemis
    /// Implemente la classe <see cref="SpaceInvaders.GameObject" />
    /// </summary>
    /// <seealso cref="SpaceInvaders.GameObject" />
    class Enemy : GameObject
    {
        /// <summary>
        /// Le sprite de l'ennemi
        /// </summary>
        public Sprite sprite;
        /// <summary>
        /// Position de l'ennemi
        /// </summary>
        public Vecteur2D position;
        /// <summary>
        /// Nombre de vies de l'enemi
        /// </summary>
        public int lives = 1;

        /// <summary>
        /// Crée un nouvel <see cref="Enemy"/>
        /// </summary>
        /// <param name="sprite">Sprite de l'enemi</param>
        /// <param name="position">Position initiale de l'enemi</param>
        /// <param name="lives">Nombre de vies de l'enemi</param>
        public Enemy(Sprite sprite, Vecteur2D position, int lives)
        {
            this.sprite = sprite;
            this.position = position;
            this.lives = lives;
        }

        /// <summary>
        /// Crée un nouvel <see cref="Enemy"/>
        /// </summary>
        /// <param name="sprite">Sprite de l'enemi</param>
        /// <param name="x">Position en abcisse</param>
        /// <param name="y">Position en ordonée</param>
        public Enemy(Sprite sprite, double x, double y) : this(sprite, new Vecteur2D(x, y), 1) { }

        /// <summary>
        /// Crée un nouvel <see cref="Enemy"/>
        /// </summary>
        /// <param name="enemy">L'ennemi utilisé en prototype</param>
        /// <param name="position">Position initiale de l'enemi</param>
        public Enemy(Enemy enemy, Vecteur2D position) : this(new Sprite(enemy.sprite), position, enemy.lives) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="Enemy"/> class.
        /// </summary>
        /// <param name="enemy">L'ennemi utilisé en prototype</param>
        /// <param name="position">Position initiale de l'enemi</param>
        /// <param name="lives">Nombre de vies de l'enemi</param>
        public Enemy(Enemy enemy, Vecteur2D position, int lives) : this(new Sprite(enemy.sprite), position, lives) { }

        /// <summary>
        /// Dessine l'objet
        /// </summary>
        /// <param name="gameInstance">Instance de la partie en cours</param>
        /// <param name="graphics">Graphisme a utiliser</param>
        public override void Draw(Game gameInstance, Graphics graphics)
        {
            sprite.Draw().SetResolution(graphics.DpiX, graphics.DpiY);
            graphics.DrawImage(sprite.Draw(), (float)position.X, (float)position.Y);
        }

        /// <summary>
        /// Determines si l'objet est en vie, si non, il sera retiré par la suite
        /// </summary>
        /// <returns>true si l'objet est en vie, false sinon</returns>
        public override bool IsAlive()
        {
            return lives > 0;
        }

        /// <summary>
        /// Met a jour l'état d'un objet
        /// </summary>
        /// <param name="gameInstance">Instance de la partie</param>
        /// <param name="deltaT">Temps écoulé depuis le dernier appel</param>
        public override void Update(Game gameInstance, double deltaT)
        {
            if (Game.random.Next(0, 100000 - (42 - gameInstance.gameObjects.OfType<Enemy>().Count()) * 1000) < 10)
            {
                Missil missil = new Missil(position + new Vecteur2D(sprite.Draw().Width / 2 - Properties.Resources.shoot1.Width / 2, 0), Teams.Enemy);
                gameInstance.AddNewGameObject(missil);
            }
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
            Bitmap sprite = this.sprite.Draw();
            return new Rectangle(position.X, position.Y, sprite.Width, sprite.Height);
        }

        /// <summary>
        /// Action en cas de collision avec un autre GameObject
        /// </summary>
        /// <param name="go">L'autre GameObject</param>
        /// <returns><c>true</c> si une action a été effectuée, <c>false</c> sinon.</returns>
        public override bool collision(GameObject go)
        {
            sprite.deleteCollidingPixels(go.GetSprite(), position, go.getHitbox().v1);
            lives--;
            return true;
        }

        /// <summary>
        /// Permet d'obtenir le sprite
        /// </summary>
        /// <returns>Sprite.</returns>
        public override Sprite GetSprite()
        {
            return sprite;
        }

        /// <summary>
        /// Action a réaliser en cas de mort
        /// </summary>
        public override void Die()
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(Properties.Resources.explosion);
            if (Game.random.Next(0, 11) < 1) Game.game.AddNewGameObject(new Bonus(new Sprite(Properties.Resources.bonus), position));
            player.Play();
        }
    }
}