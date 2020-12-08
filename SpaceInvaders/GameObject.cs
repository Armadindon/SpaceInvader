using System.Drawing;

namespace SpaceInvaders
{
    /// <summary>
    /// Classe abstraite représentant une entité quelconque en jeu
    /// </summary>
    abstract class GameObject
    {

        public GameObject()
        {
        }

        /// <summary>
        /// Met a jour l'état d'un objet
        /// </summary>
        /// <param name="gameInstance">Instance de la partie</param>
        /// <param name="deltaT">Temps écoulé depuis le dernier appel</param>
        public abstract void Update(Game gameInstance, double deltaT);

        /// <summary>
        /// Dessine l'objet
        /// </summary>
        /// <param name="gameInstance">Instance de la partie en cours</param>
        /// <param name="graphics">Graphisme a utiliser</param>
        public abstract void Draw(Game gameInstance, Graphics graphics);

        /// <summary>
        /// Determines si l'objet est en vie, si non, il sera retiré par la suite
        /// </summary>
        /// <returns>true si l'objet est en vie, false sinon</returns>
        public abstract bool IsAlive();

        /// <summary>
        /// Determine si l'objet est en collision avec un autre gameObject
        /// </summary>
        /// <param name="go">L'autre GameObject</param>
        /// <returns><c>true</c> si les objets sont en collision, sinon <c>false</c>.</returns>
        public abstract bool IsColliding(GameObject go);
        /// <summary>
        /// Renvoie la team de l'objet
        /// </summary>
        /// <returns>La team associé a l'objet</returns>
        public abstract Teams whichTeam();
        /// <summary>
        /// Permet de récuperer la hitbox de l'objet
        /// </summary>
        /// <returns>Rectangle de la hitbox</returns>
        public abstract Rectangle getHitbox();
        /// <summary>
        /// Action en cas de collision avec un autre GameObject
        /// </summary>
        /// <param name="go">L'autre GameObject</param>
        /// <returns><c>true</c> si une action a été effectuée, <c>false</c> sinon.</returns>
        public abstract bool collision(GameObject go);
        /// <summary>
        /// Permet d'obtenir le sprite
        /// </summary>
        /// <returns>Sprite.</returns>
        public abstract Sprite GetSprite();
        /// <summary>
        /// Action a réaliser en cas de mort
        /// </summary>
        public abstract void Die();

    }
}
