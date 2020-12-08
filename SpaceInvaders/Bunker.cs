using System.Drawing;

namespace SpaceInvaders
{
    /// <summary>
    /// Représente un bunker en jeu, dernier rempart avant l'envahisseur
    /// Implémente le <see cref="SpaceInvaders.GameObject" />
    /// </summary>
    /// <seealso cref="SpaceInvaders.GameObject" />
    class Bunker : GameObject
    {
        /// <summary>
        /// Sprite du Bunker (a qui il peut manquer quelque pixels)
        /// </summary>
        private Sprite sprite = new Sprite(Properties.Resources.bunker);
        /// <summary>
        /// La position du Bunker
        /// </summary>
        Vecteur2D position;
        /// <summary>
        /// Si le bunker est en vie (Toujours vrai sauf en cas de collisions)
        /// </summary>
        private bool alive = true;

        /// <summary>
        /// Génère une rangée de bunkers
        /// </summary>
        /// <param name="instance">Instance de la partie</param>
        /// <param name="number">Nombre de bunker a générer</param>
        public static void generateBunkers(Game instance, int number)
        {
            Bitmap sprite = Properties.Resources.bunker;
            Bitmap playerSprite = Properties.Resources.ship1;
            double spacing = 50;
            double totalSize = spacing * (number - 1) + sprite.Width * number;

            for (int i = 0; i < number; i++)
            {
                instance.AddNewGameObject(new Bunker(instance.gameSize.Width / 4 + i * (sprite.Width + spacing), instance.gameSize.Height - (playerSprite.Height * 3 + spacing + sprite.Height)));
            }
        }

        /// <summary>
        /// Crée un nouveau <see cref="Bunker"/>.
        /// </summary>
        /// <param name="position">Position du nouveau Bunker</param>
        public Bunker(Vecteur2D position)
        {
            this.position = position;
        }

        /// <summary>
        /// Crée un nouveau <see cref="Bunker"/>.
        /// </summary>
        /// <param name="x">Position en abcisse</param>
        /// <param name="y">Position en ordonée</param>
        public Bunker(double x, double y) : this(new Vecteur2D(x, y)) { }

        /// <summary>
        /// Action en cas de collision avec un autre GameObject
        /// </summary>
        /// <param name="go">L'autre GameObject</param>
        /// <returns><c>true</c> si une action a été effectuée, <c>false</c> sinon.</returns>
        public override bool collision(GameObject go)
        {
            if (go is EnemyGroup)
            {
                alive = false;
            }
            else sprite.deleteCollidingPixels(go.GetSprite(), position, go.getHitbox().v1);
            return true;
        }

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
        /// Permet de récuperer la hitbox de l'objet
        /// </summary>
        /// <returns>Rectangle de la hitbox</returns>
        public override Rectangle getHitbox()
        {
            Bitmap sprite = this.sprite.Draw();
            return new Rectangle(position.X, position.Y, sprite.Width, sprite.Height);
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
        /// Determines si l'objet est en vie, si non, il sera retiré par la suite
        /// </summary>
        /// <returns>true si l'objet est en vie, false sinon</returns>
        public override bool IsAlive()
        {
            return alive;
        }

        /// <summary>
        /// Determine si l'objet est en collision avec un autre gameObject
        /// </summary>
        /// <param name="go">L'autre GameObject</param>
        /// <returns><c>true</c> si les objets sont en collision, sinon <c>false</c>.</returns>
        public override bool IsColliding(GameObject go)
        {
            if (whichTeam() != go.whichTeam() && getHitbox().intersect(go.getHitbox()))
            {
                if (go is EnemyGroup) return true;
                return sprite.pixelColliding(go.GetSprite(), position, go.getHitbox().v1).Count != 0;
            }
            return false;
        }

        /// <summary>
        /// Met a jour l'état d'un objet
        /// </summary>
        /// <param name="gameInstance">Instance de la partie</param>
        /// <param name="deltaT">Temps écoulé depuis le dernier appel</param>
        public override void Update(Game gameInstance, double deltaT)
        {
            //Ne fait rien
        }

        /// <summary>
        /// Renvoie la team de l'objet
        /// </summary>
        /// <returns>La team associé a l'objet</returns>
        public override Teams whichTeam()
        {
            return Teams.Props;
        }

        /// <summary>
        /// Action a réaliser en cas de mort
        /// </summary>
        public override void Die()
        {
        }
    }
}