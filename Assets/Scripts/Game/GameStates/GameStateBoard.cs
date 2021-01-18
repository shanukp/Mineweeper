using System;
using Game.UI.Models;
using Game.UI.Views;
using Game.UI.Controllers;


namespace Game.GameStates
{

    /// <summary>
    /// Game state for game board screen
    /// </summary>
    public class GameStateBoard : GameStateBase<GameModeBoardContext>
    {
        #region Private Variables
        private GameBoardController viewController;
        #endregion

        #region Public Members
        public override void OnAdd()
        {
            CreateView<GameBoardView>().ContinueWith(task =>
            {
                viewController = new GameBoardController(new GameBoardModel(), task.Result);
            }, System.Threading.Tasks.TaskContinuationOptions.ExecuteSynchronously);
        }

        public override void OnActivate(Action activated = null)
        {
            base.OnActivate(activated);
        }

        public override void OnDeactivate(Action deactivated = null)
        {
            base.OnDeactivate(deactivated);
        }

        public override void OnRemove()
        {
            base.OnRemove();
            viewController?.Dispose();
        }
        #endregion
    }

    /// <inheritdoc/>
    public class GameModeBoardContext : GameModeContextBase
    {
        public bool isStarter;
    }
}
