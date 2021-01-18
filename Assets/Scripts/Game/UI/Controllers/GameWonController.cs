using Game.Core;
using Game.GameStates;
using Game.UI.Models;
using Game.UI.Views;

namespace Game.UI.Controllers
{

    /// <summary>
    /// Controller for the HUD.
    /// </summary>
    public class GameWonController : ControllerBase<GameWonModel, GameWonView>
    {
        public GameWonController(GameWonModel model, GameWonView view) : base(model, view)
        {
            view.ContinuePressed += HandleContinueButtonClicked;
        }

        private void HandleContinueButtonClicked()
        {
            ServiceLocator.GameStateManager.PopAll();
            ServiceLocator.GameStateManager.Add<GameStateHome, GameModeHomeContext>(new GameModeHomeContext() { ViewPrefabPath = AssetManifest.HomeScreenPrefabPath }) ;
        }
    }
}
