using Game.Core;
using Game.GameStates;
using Game.UI.Models;
using Game.UI.Views;

namespace Game.UI.Controllers
{

    /// <summary>
    /// Controller for the HUD.
    /// </summary>
    public class GameLostController : ControllerBase<GameLostModel, GameLostView>
    {

        public GameLostController(GameLostModel model, GameLostView view) : base(model, view)
        {
            view.ContinuePressed += HandleContinueButtonClicked;
        }

        private void HandleContinueButtonClicked()
        {
            ServiceLocator.GameStateManager.PopAll();
            ServiceLocator.GameStateManager.Add<GameStateHome, GameModeHomeContext>(new GameModeHomeContext() { ViewPrefabPath = AssetManifest.HomeScreenPrefabPath });
        }
    }
}
