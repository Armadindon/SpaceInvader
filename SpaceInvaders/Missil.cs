using System.Drawing;

namespace SpaceInvaders
{
    /// <summary>
    /// Classe représentant un missile
    /// Implemente la classe <see cref="SpaceInvaders.GameObject" />
    /// </summary>
    /// <seealso cref="SpaceInvaders.GameObject" />
    class Missil : GameObject
    {

        /// <summary>
        /// Le sprite du missile
        /// </summary>
        private Sprite sprite = new Sprite(Properties.Resources.shoot1);
        /// <summary>
        /// La position actuelle du misssile
        /// </summary>
        private Vecteur2D position;
        /// <summary>
        /// La vitesse du missile
        /// </summary>
        private int missilSpeed = 800;
        /// <summary>
        /// Nombre de vies du missile
        /// </summary>
        int lives = 1;
        /// <summary>
        /// L'équipe du missile (qui l'a tiré)
        /// </summary>
        Teams team;

        /// <summary>
        /// Initialise un nouveau <see cref="Missil"/>
        /// </summary>
        /// <param name="position">Position initiale</param>
        /// <param name="team">Equipe du missile</param>
        public Missil(Vecteur2D position, Teams team)
        {
            this.position = position;
            this.team = team;
            if (team == Teams.Player) missilSpeed *= -1; //On ira vers le haut
        }

        /// <summary>
        /// Initialise un nouveau <see cref="Missil"/>
        /// </summary>
        /// <param name="position">Position initiale</param>
        /// <param name="team">Equipe du missile</param>
        /// <param name="lives">Nombre de vies du missile</param>
        public Missil(Vecteur2D position, Teams team, int lives) : this(position, team)
        {
            this.lives = lives;
        }

        /// <summary>
        /// Action en cas de collision avec un autre GameObject
        /// </summary>
        /// <param name="go">L'autre GameObject</param>
        /// <returns><c>true</c> si une action a été effectuée, <c>false</c> sinon.</returns>
        public override bool collision(GameObject go)
        {
            lives--;
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
            return lives > 0 && (position.X > 0 && position.X < Game.game.gameSize.Width) && (position.Y > 0 && position.Y < Game.game.gameSize.Height);
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
            position += new Vecteur2D(0, missilSpeed * deltaT);

            foreach (GameObject go in gameInstance.gameObjects)
            {
                checkAndHandleCollision(go);
            }
        }

        /// <summary>
        /// Vérifie si il y a une collision, si oui, fait l'action nécéssaire
        /// </summary>
        /// <param name="go">l'autre gameObject</param>
        private void checkAndHandleCollision(GameObject go)
        {
            if (IsColliding(go))
            {
                if (go.collision(this))
                {
                    collision(go);
                }
            }
        }

        /// <summary>
        /// Renvoie la team de l'objet
        /// </summary>
        /// <returns>La team associé a l'objet</returns>
        public override Teams whichTeam()
        {
            return team;
        }

        /// <summary>
        /// Action a réaliser en cas de mort
        /// </summary>
        public override void Die()
        {
        }
    }
}