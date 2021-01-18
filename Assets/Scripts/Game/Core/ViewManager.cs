using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Game.UI.Views;
using Game.Utility;

namespace Game.Core
{
    /// <summary>
    /// Handles creating new views.
    /// </summary>
    public class ViewManager
    {

        private readonly AssetManager assetManager;
        private UIRoot uiRoot;


        public ViewManager(AssetManager assetManager)
        {
            this.assetManager = assetManager;

            uiRoot = GameObject.FindObjectOfType<UIRoot>();
            if (uiRoot == null)
            {
                Debug.LogError("Unable to find UIRoot");
            }
        }

        /// <summary>
        /// Instantiates a new view GameObject from a prefab reference.
        /// The returned GameObject is disabled.
        /// </summary>
        public T CreateView<T>(T prefab, Transform parent = null) where T : ViewBase
        {
            var view = GameObject.Instantiate<T>(prefab);
            view.gameObject.name = prefab.gameObject.name;
            view.SetActive(false);
            view.transform.SetParent(parent != null ? parent : uiRoot.GetCanvas(UIRoot.CanvasLayer.Default), false);
            view.transform.localScale = prefab.transform.localScale;
            return view;
        }

        /// <summary>
        /// Instantiates a new view GameObject from a prefab reference.
        /// The returned GameObject is disabled.
        /// </summary>
        public T CreateView<T>(T prefab, UIRoot.CanvasLayer canvasLayer) where T : ViewBase
        {
            return CreateView<T>(prefab, uiRoot.GetCanvas(canvasLayer));
        }

        /// <summary>
        /// Instantiates a new view GameObject from a path.
        /// The returned GameObject is disabled.
        /// </summary>
        public Task<T> CreateView<T>(string prefabPath, Transform parent = null) where T : ViewBase
        {
            return assetManager.LoadAssetAsync<T>(prefabPath).ContinueWith(task =>
            {
                return CreateView(task.Result, parent);
            }, TaskContinuationOptions.ExecuteSynchronously);
        }

        /// <summary>
        /// Instantiates a new view GameObject from a path.
        /// The returned GameObject is disabled.
        /// </summary>
        public Task<T> CreateView<T>(string prefabPath, UIRoot.CanvasLayer canvasLayer) where T : ViewBase
        {
            return CreateView<T>(prefabPath, uiRoot.GetCanvas(canvasLayer));
        }

    }

}

