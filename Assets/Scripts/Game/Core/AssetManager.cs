using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Game.Utility;
using Game.Extensions;
using System.Collections;
using System.Threading;

namespace Game.Core
{
    /// <summary>
    /// Handles loading assets.
    /// </summary>
    public class AssetManager
    {
        /// <summary>
        /// Loads an asset asynchronously from the Resources directory.
        /// </summary>
        public async Task<T> LoadAssetAsync<T>(string assetPath) where T : Object
        {
            ResourceRequest loadRequest = Resources.LoadAsync<T>(assetPath);

           while(!loadRequest.isDone)
           {
                await Task.Delay(25);
           }

            var asset = loadRequest.asset as T;

            if (asset == null)
            {
                var message = string.Format("Unable to find asset {0} of type {1}", assetPath, typeof(T).ToString());
                throw new UnityException(message);
            }
            return asset;
        }


        /// <summary>
        /// Loads an asset immediately from the Resources directory.
        /// </summary>
        public T LoadAsset<T>(string assetPath) where T : Object
        {
            return Resources.Load<T>(assetPath);
        }

        /// <summary>
        /// Loads all assets contained in a folder in the Resources directory
        /// </summary>
        public T[] LoadAllAssets<T>(string assetPath) where T : Object
        {
            return Resources.LoadAll<T>(assetPath);
        }
    }
}
