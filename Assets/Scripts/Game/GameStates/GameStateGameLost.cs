using System;
using Game.UI.Models;
using Game.UI.Views;
using Game.UI.Controllers;
using System.Threading.Tasks;

namespace Game.GameStates
{
    /// <summary>
    /// Game state for the game lost screen
    /// </summary>
    public class GameStateGameLost : GameStateBase<GameModeGameLostContext>
    {
        #region Private Variables
        private GameLostController viewController;
        #endregion

        #region Public Members
        public override void OnAdd()
        {
            CreateView<GameLostView>().ContinueWith(task =>
            {
                viewController = new GameLostController(new GameLostModel(), task.Result);
            }, TaskContinuationOptions.ExecuteSynchronously);
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
    public class GameModeGameLostContext : GameModeContextBase
    {

    }
}
