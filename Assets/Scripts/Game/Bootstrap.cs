using System;
using System.Collections.Generic;
using System.Linq;
using Game.Core;
using Game.GameStates;
using Game.Utility;
using UnityEngine;


namespace Game
{

    /// <summary>
    /// Initialization point of the game.
    /// Constructs an instance of SocialEngine based off configuration and specified settings.
    /// Game specific logic mostly lives in SocialEngineGame.
    /// </summary>
    public class Bootstrap : MonoBehaviour
    {

        private void Awake()
        {
            ServiceLocator.Initialize();
        }

        private void Start()
        {
            IntializeObjectPools();
            StartGame();
        }


        /// <summary>
        /// Configures and launches game
        /// </summary>
        private void StartGame()
        {
            ServiceLocator.GameStateManager.Add<GameStateHome, GameModeHomeContext>(new GameModeHomeContext() { ViewPrefabPath = AssetManifest.HomeScreenPrefabPath }) ;
        }

        private void IntializeObjectPools()
        {
            ObjectPoolHolder.Instance.CreateCellPool();
        }
    }
}
