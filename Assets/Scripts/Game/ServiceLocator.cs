using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public static class ServiceLocator
    {

        public static ViewManager ViewManager { get; set; }

        public static AssetManager AssetManager { get; set; }

        public static GameStateManager GameStateManager { get; set; }

        public static CoroutineManager CoroutineManager { get; set; }

        //public static 

        public static void Initialize()
        {
            GameStateManager = new GameStateManager();
            AssetManager = new AssetManager();
            ViewManager = new ViewManager(AssetManager);

            GameObject coroutineManagerGameObject = new GameObject("CoroutineManager");
            CoroutineManager = coroutineManagerGameObject.AddComponent<CoroutineManager>();
        }
    }
}
