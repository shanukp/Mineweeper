using Game.Core;
using Game.GameStates;
using Game.UI.Models;
using Game.UI.Views;
using System;
using UnityEngine;

namespace Game.UI.Controllers
{

    /// <summary>
    /// Controller for the Game board.
    /// </summary>
    public class GameBoardController : ControllerBase<GameBoardModel, GameBoardView>
    {

        public GameBoardController(GameBoardModel model, GameBoardView view) : base(model, view)
        {
            view.Initialize(7,7, OnCellLeftClicked, OnCellRightClicked);
            view.BackButtonPressed += HandleBackButtonPressed;
        }

        private void HandleBackButtonPressed()
        {
            ServiceLocator.GameStateManager.Pop();

            ServiceLocator.GameStateManager.Add<GameStateHome, GameModeHomeContext>(new GameModeHomeContext()
            { ViewPrefabPath = AssetManifest.HomeScreenPrefabPath,
              DeactivatesPreviousGameMode =true
            });
        }

        private void OnCellLeftClicked(uint x, uint y)
        {
            Open(x, y);
        }

        private void OnCellRightClicked(uint x, uint y)
        {
            ToggleFlagged(x, y);
        }

        public void Open(uint x, uint y)
        {
            UnveilEmptyCells(x, y);

            if (model.IsMineAtBoardCordinates(x, y))
            {
                ServiceLocator.GameStateManager.Add<GameStateGameLost, GameModeGameLostContext>(new GameModeGameLostContext()
                    { ViewPrefabPath = AssetManifest.GameLostOverlayPath,
                      DeactivatesPreviousGameMode = false
                });
            }
            else if (model.IsGameCompleted())
            {
                ServiceLocator.GameStateManager.Add<GameStateGameWon, GameModeGameWonContext>(new GameModeGameWonContext()
                    { ViewPrefabPath = AssetManifest.GameWonOverlayPath,
                      DeactivatesPreviousGameMode = false  
                });
            }
        }

        public void ToggleFlagged(uint x, uint y)
        {
            bool isFlagged =  model.ToggleFlaggedStatus(x,y);
            view.SetCellAsFlagged(x, y, isFlagged);
        }

        private void UnveilEmptyCells(uint x, uint y)
        {
            if (model.IsCellViewed(x, y))
            {
                return;
            }

            model.SetCellAsViewed(x, y, true);

            model.IncrementOpenedCellsCount();

            if (model.GetNearbyMines(x, y) != 0 )
            {
                view.SetCellContentVisible(x, y);
                view.SetNearbyMinesCount(x, y, model.GetNearbyMines(x,y));
                return;
            }
            else if(!model.IsMineAtBoardCordinates(x,y))
            {
                view.SetCellContentVisible(x, y);
            }

            for (uint r = x > 0 ? x - 1 : x; r < x + 2 && r < 7; r++)
            {
                for (uint c = y > 0 ? y - 1 : y; c < y + 2 && c < 7; c++)
                {
                    if (!(r == x && c == y))
                    {
                        UnveilEmptyCells(r, c);
                    }
                }
            }
        }
    }
}
