using System;
using Game.UI.Models;
using Game.UI.Views;
using Game.UI.Controllers;
using System.Threading.Tasks;

namespace Game.GameStates
{
    /// <summary>
    /// Game mode for the home screen
    /// </summary>
    public class GameStateHome : GameStateBase<GameModeHomeContext>
    {
        #region Private Variables
        private HomeController viewController;
        #endregion

        #region Public Members
        public override void OnAdd()
        {
            CreateView<HomeView>().ContinueWith(task =>
            {
                viewController = new HomeController(new HomeModel(), task.Result);
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
    public class GameModeHomeContext : GameModeContextBase
    {

    }
}
