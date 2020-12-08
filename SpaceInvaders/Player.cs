using System.Drawing;
using System.Windows.Forms;

namespace SpaceInvaders
{
    /// <summary>
    /// Classe représentant le joueur
    /// Implémente la classe <see cref="SpaceInvaders.GameObject" />
    /// </summary>
    /// <seealso cref="SpaceInvaders.GameObject" />
    class Player : GameObject
    {

        /// <summary>
        /// Le sprite du joueur
        /// </summary>
        private Sprite sprite;

        /// <summary>
        /// La position actuelle du joueur
        /// </summary>
        private Vecteur2D position;

        /// <summary>
        /// La vitesse de déplacement du joueur
        /// </summary>
        private double PlayerSpeed = 800;

        /// <summary>
        /// Le dernier missile tiré par le joueur (null si aucun)
        /// </summary>
        private Missil activeMissil = null;

        /// <summary>
        /// Le dernier bonus actif
        /// </summary>
        private BonusType activeBonus;

        /// <summary>
        /// Le temps restant pour le bonus
        /// </summary>
        private double remainingTimeBonus = 0;

        /// <summary>
        /// Nombre de vies
        /// </summary>
        private int lives = 3;


        /// <summary>
        /// Crée un nouveau <see cref="Player"/>
        /// </summary>
        /// <param name="position">La position initiale</param>
        public Player(Vecteur2D position)
        {
            sprite = new Sprite(Properties.Resources.ship1);
            this.position = position;
        }

        /// <summary>
        /// Crée un nouveau <see cref="Player"/>
        /// </summary>
        /// <param name="x">la position en abcisse</param>
        /// <param name="y">la position en ordonée</param>
        public Player(double x, double y) : this(new Vecteur2D(x, y)) { }

        /// <summary>
        /// Crée un nouveau <see cref="Player"/> au centre
        /// </summary>
        public Player() : this(0, 0)
        {
            Bitmap playerSprite = Properties.Resources.ship1;
            Vecteur2D pos = new Vecteur2D((Game.game.gameSize.Width / 2) - (playerSprite.Width), Game.game.gameSize.Height - (2 * playerSprite.Height));
            this.position = pos;
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
        /// Determines si l'objet est en vie, si non, il sera retiré par la suite
        /// </summary>
        /// <returns>true si l'objet est en vie, false sinon</returns>
        public override bool IsAlive()
        {
            return lives > 0;
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
        /// Récupère le nombre de vie restante au joueur
        /// </summary>
        /// <returns>Le nombre de vie restantes</returns>
        public int getLives()
        {
            return lives;
        }

        /// <summary>
        /// Met a jour l'état d'un objet
        /// </summary>
        /// <param name="gameInstance">Instance de la partie</param>
        /// <param name="deltaT">Temps écoulé depuis le dernier appel</param>
        public override void Update(Game gameInstance, double deltaT)
        {
            if (activeMissil != null && !activeMissil.IsAlive()) this.activeMissil = null;
            if (Game.game.keyPressed.Contains(Keys.Right) || Game.game.keyPressed.Contains(Keys.Left))
            {
                handleOutOfBound(deltaT);
                position += determineMove(deltaT);
            }
            if (Game.game.keyPressed.Contains(Keys.Space) && activeMissil == null)
            {
                fireMissil(gameInstance);
            }
            if (remainingTimeBonus > 0) //Si un bonus est actif
            {
                remainingTimeBonus -= deltaT;
            }
        }

        /// <summary>
        /// Tire un missile
        /// </summary>
        /// <param name="gameInstance">L'instance de la partie</param>
        private void fireMissil(Game gameInstance)
        {
            if (remainingTimeBonus > 0 && activeBonus == BonusType.SUPER_MISSIL) this.activeMissil = new Missil(position + new Vecteur2D(sprite.Draw().Width / 2 - Properties.Resources.shoot1.Width / 2, 0), Teams.Player, 3);
            else this.activeMissil = new Missil(position + new Vecteur2D(sprite.Draw().Width / 2 - Properties.Resources.shoot1.Width / 2, 0), Teams.Player);

            if (remainingTimeBonus > 0 && activeBonus == BonusType.MULTIPLE_SHOT)
            {
                //On ajoute deux autres missiles
                gameInstance.AddNewGameObject(new Missil(position + new Vecteur2D(sprite.Draw().Width / 2 - Properties.Resources.shoot1.Width / 2 - 20f, 0), Teams.Player));
                gameInstance.AddNewGameObject(new Missil(position + new Vecteur2D(sprite.Draw().Width / 2 - Properties.Resources.shoot1.Width / 2 + 20f, 0), Teams.Player));
            }
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(Properties.Resources.laser);
            player.Play();
            gameInstance.AddNewGameObject(this.activeMissil);
        }

        /// <summary>
        /// Determine le mouvement a effectuer
        /// </summary>
        /// <param name="deltaT">Temps depuis le dernier update</param>
        /// <returns>Vecteur a ajouter</returns>
        private Vecteur2D determineMove(double deltaT)
        {
            if (Game.game.keyPressed.Contains(Keys.Right))
            {
                return new Vecteur2D(PlayerSpeed * deltaT, 0);
            }
            return new Vecteur2D(-PlayerSpeed * deltaT, 0);
        }

        /// <summary>
        /// Gère les cas ou il va y avoir une sortie d'écran
        /// </summary>
        /// <param name="deltaT">The delta t.</param>
        private void handleOutOfBound(double deltaT)
        {
            if (position.X + PlayerSpeed * deltaT > Game.game.gameSize.Width - sprite.Draw().Width)
            {
                position = new Vecteur2D(Game.game.gameSize.Width - sprite.Draw().Width, position.Y);
            }
            else if (position.X + -PlayerSpeed * deltaT < 0)
            {
                position = new Vecteur2D(0, position.Y);
            }
        }

        /// <summary>
        /// Renvoie la team de l'objet
        /// </summary>
        /// <returns>La team associé a l'objet</returns>
        public override Teams whichTeam()
        {
            return Teams.Player;
        }

        /// <summary>
        /// Action en cas de collision avec un autre GameObject
        /// </summary>
        /// <param name="go">L'autre GameObject</param>
        /// <returns><c>true</c> si une action a été effectuée, <c>false</c> sinon.</returns>
        public override bool collision(GameObject go)
        {
            if (go is EnemyGroup) lives -= int.MaxValue;
            else
            {
                sprite.deleteCollidingPixels(go.GetSprite(), position, go.getHitbox().v1);
                if (remainingTimeBonus <= 0 || activeBonus != BonusType.INVINCIBILITY) lives--;
            }
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
        }

        /// <summary>
        /// Ajoute un bonus
        /// </summary>
        /// <param name="type">Type de bonus</param>
        /// <param name="time">Temps du bonus</param>
        public void addBonus(BonusType type, double time)
        {
            if (type == BonusType.ONE_LIFE)
            {
                lives++;
                return;
            }
            this.activeBonus = type;
            this.remainingTimeBonus = time;
        }

        /// <summary>
        /// Determine si le bonus est actif
        /// </summary>
        /// <returns><c>true</c> si le bonus est actif, sinon, <c>false</c>.</returns>
        public bool isBonusActive()
        {
            return remainingTimeBonus > 0;
        }

        /// <summary>
        /// Obtient le type du dernier bonus
        /// </summary>
        /// <returns>Type du bonus</returns>
        public BonusType GetBonus()
        {
            return activeBonus;
        }

        /// <summary>
        /// Permet d'obtenir le type du dernier bonus
        /// </summary>
        /// <returns>Temps restant au bonus</returns>
        public double getRemainingTimeBonus()
        {
            return remainingTimeBonus;
        }
    }
}