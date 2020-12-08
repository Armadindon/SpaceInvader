using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SpaceInvaders
{

    /// <summary>
    /// Classe Bonus.
    /// Représente l'objet de jeu Bonus, récupérable après avoir tué un enemi
    /// Implemente le <see cref="SpaceInvaders.GameObject" />
    /// </summary>
    /// <seealso cref="SpaceInvaders.GameObject" />
    class Bonus : GameObject
    {
        /// <summary>
        /// Durée d'un bonus
        /// </summary>
        public static double BONUS_TIME = 6d;

        /// <summary>
        /// Sprite du bonus
        /// </summary>
        private Sprite sprite;
        /// <summary>
        /// Position actuelle du Bonus
        /// </summary>
        private Vecteur2D position;
        /// <summary>
        /// Type de Bonus
        /// </summary>
        BonusType type;
        /// <summary>
        /// Nombre de vies du bonus
        /// </summary>
        private int lives = 1;
        /// <summary>
        /// Vitesse de chute du bonus
        /// </summary>
        private int bonusSpeed = 100;

        /// <summary>
        /// Crée un nouveau <see cref="Bonus"/>.
        /// </summary>
        /// <param name="sprite">Sprite du bonus</param>
        /// <param name="position">Position initiale du Bonus</param>
        public Bonus(Sprite sprite, Vecteur2D position)
        {
            this.sprite = sprite;
            this.position = position;
            List<BonusType> types = Enum.GetValues(typeof(BonusType)).Cast<BonusType>().ToList();
            int random = new Random().Next(types.Count);
            type = types[random];
        }

        /// <summary>
        /// Action en cas de collision avec un autre GameObject
        /// </summary>
        /// <param name="go">L'autre GameObject</param>
        /// <returns><c>true</c> si une action a été effectuée, <c>false</c> sinon.</returns>
        public override bool collision(GameObject go)
        {
            lives--;
            if (go is Player) ((Player)go).addBonus(type, BONUS_TIME);
            return true;
        }

        /// <summary>
        /// Action a réaliser en cas de mort
        /// </summary>
        public override void Die()
        {
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
            return whichTeam() == Teams.Player && go.getHitbox().intersect(getHitbox());
        }

        /// <summary>
        /// Met a jour l'état d'un objet
        /// </summary>
        /// <param name="gameInstance">Instance de la partie</param>
        /// <param name="deltaT">Temps écoulé depuis le dernier appel</param>
        public override void Update(Game gameInstance, double deltaT)
        {
            position += new Vecteur2D(0, bonusSpeed * deltaT);
            Player p = gameInstance.gameObjects.OfType<Player>().First();
            if (p.IsColliding(this)) collision(p);
        }

        /// <summary>
        /// Renvoie la team de l'objet
        /// </summary>
        /// <returns>La team associé a l'objet</returns>
        public override Teams whichTeam()
        {
            return Teams.Other;
        }

        /// <summary>
        /// Conversions du type de Bonus en chaîne de caractère (Et en français s'il vous plait)
        /// </summary>
        /// <param name="bonus">Le type de bonus</param>
        /// <returns>Chaine associé au type de bonus</returns>
        public static String convertToString(BonusType bonus)
        {
            switch (bonus)
            {
                case BonusType.INVINCIBILITY:
                    return "Invincibilite";

                case BonusType.MULTIPLE_SHOT:
                    return "Multi Tir";

                case BonusType.SUPER_MISSIL:
                    return "Super Missile";
            }
            return "";
        }
    }
}
