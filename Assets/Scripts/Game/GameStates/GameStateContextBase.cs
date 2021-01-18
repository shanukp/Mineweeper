namespace Game.GameStates
{

    /// <summary>
    /// Base class for context that is passed into GameModes.
    /// </summary>
    public class GameModeContextBase
    {

        /// <summary>
        /// The asset path to the view prefab.
        /// </summary>
        public string ViewPrefabPath;

        /// <summary>
        /// Should adding this GameMode onto the stack deactivate
        /// the previous GameMode.
        /// </summary>
        public bool DeactivatesPreviousGameMode = true;
    }

}