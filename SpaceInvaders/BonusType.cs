namespace SpaceInvaders
{
    /// <summary>
    /// Représente les différents types de bonus possible
    /// </summary>
    public enum BonusType
    {
        /// <summary>
        /// Invencibilité : Le joueur deviens insensibles aux tirs ennemis pendant la durée du bonus
        /// </summary>
        INVINCIBILITY,
        /// <summary>
        /// Super Missile : Missile plus puissant, pouvant perforer les ennemis et/ou leur faire plus de dégats pendant la durée du bonus
        /// </summary>
        SUPER_MISSIL,
        /// <summary>
        /// Tir Multiple : Le joueur tire 3 missiles côte a côte pendant la durée du bonus
        /// </summary>
        MULTIPLE_SHOT,
        /// <summary>
        /// +1 up : Comme dans mario
        /// </summary>
        ONE_LIFE
    }
}