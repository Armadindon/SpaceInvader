namespace SpaceInvaders
{
    /// <summary>
    /// Représente les différents états possible du jeu
    /// </summary>
    public enum GameState
    {
        /// <summary>
        /// Le jeu se lance
        /// </summary>
        STARTING,
        /// <summary>
        /// Le jeu est perdu (Padpo)
        /// </summary>
        LOSE,
        /// <summary>
        /// Le jeu est gagné (Lachance)
        /// </summary>
        WIN,
        /// <summary>
        /// On est entre deux niveaux
        /// </summary>
        INTER_LEVEL,
        /// <summary>
        /// Le jeu est dans la boucle de jeu principale
        /// </summary>
        RUNNING,
        /// <summary>
        /// Le jeu est en pause
        /// </summary>
        PAUSE
    }
}