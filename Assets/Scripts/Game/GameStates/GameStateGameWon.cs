using System;
using Game.UI.Models;
using Game.UI.Views;
using Game.UI.Controllers;
using System.Threading.Tasks;

namespace Game.GameStates
{
    /// <summary>
    /// Game state for the game won screen
    /// </summary>
    public class GameStateGameWon : GameStateBase<GameModeGameWonContext>
    {
        #region Private Variables
        private GameWonController viewController;
        #endregion

        #region Public Members
        public override void OnAdd()
        {
            CreateView<GameWonView>().ContinueWith(task =>
            {
                viewController = new GameWonController(new GameWonModel(), task.Result);
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
    public class GameModeGameWonContext : GameModeContextBase
    {

    }
}
