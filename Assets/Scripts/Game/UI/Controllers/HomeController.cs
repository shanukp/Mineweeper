using Game.Core;
using Game.GameStates;
using Game.UI.Models;
using Game.UI.Views;

namespace Game.UI.Controllers
{

    /// <summary>
    /// Controller for the HUD.
    /// </summary>
    public class HomeController : ControllerBase<HomeModel, HomeView>
    {

        public HomeController(HomeModel model, HomeView view) : base(model, view)
        {
            view.PlayPressed += HandlePlayButtonPressed;
        }

        private void HandlePlayButtonPressed()
        {
            ServiceLocator.GameStateManager.Pop();
            ServiceLocator.GameStateManager.Add<GameStateBoard, GameModeBoardContext>(new GameModeBoardContext() { ViewPrefabPath = AssetManifest.GameBoardScreenPrefabPath }) ;
        }
    }
}
