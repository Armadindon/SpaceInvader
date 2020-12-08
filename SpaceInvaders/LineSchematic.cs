namespace SpaceInvaders
{
    /// <summary>
    /// Représente une ligne d'ennemi a générer
    /// </summary>
    class LineSchematic
    {

        /// <summary>
        /// Nombre de vie des enemis dans la ligne
        /// </summary>
        int lives;
        /// <summary>
        /// Nombre d'enemis dans la ligne
        /// </summary>
        int count;
        /// <summary>
        /// Enemis "type"
        /// </summary>
        Enemy scheme;

        /// <summary>
        /// Crée un nouveau <see cref="LineSchematic"/>
        /// </summary>
        /// <param name="enemy">Le schema</param>
        /// <param name="count">Le nombre d'enemis</param>
        /// <param name="lives">La vie de chaque enemi</param>
        public LineSchematic(Enemy enemy, int count, int lives)
        {
            this.lives = lives;
            this.scheme = enemy;
            this.count = count;
        }

        /// <summary>
        /// Genère les enemis du schema
        /// </summary>
        /// <param name="instance">Instance de la partie</param>
        /// <param name="sizeLine">Taille de la ligne</param>
        /// <param name="start">Le coin en haut a gauche de la ligne</param>
        public void generateScheme(Game instance, int sizeLine, Vecteur2D start)
        {
            int offset = (sizeLine - count * scheme.GetSprite().Draw().Width) / (count +1);
            start += new Vecteur2D(offset, 0);
            for (int i = 0; i < count; i++)
            {
                instance.AddNewGameObject(new Enemy(scheme, start, lives));
                start += new Vecteur2D(offset + scheme.GetSprite().Draw().Width, 0);
            }
        }
    }
}
