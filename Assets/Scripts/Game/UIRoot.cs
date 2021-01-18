using UnityEngine;


namespace Game
{
    /// <summary>
    /// Contains references to different Canvases in the scene.
    /// </summary>
    public class UIRoot : MonoBehaviour
    {

        [SerializeField] private RectTransform defaultCanvas;
        [SerializeField] private RectTransform overlayCanvas;
        [SerializeField] private RectTransform debugCanvas;

        public enum CanvasLayer
        {
            Default,
            Overlay,
            Debug,
        }

        /// <summary>
        /// Returns the Canvas' RectTransform based on the CanvasLayer specified.
        /// </summary>
        public RectTransform GetCanvas(CanvasLayer layer)
        {
            switch (layer)
            {
                case CanvasLayer.Default:
                    return defaultCanvas;
                case CanvasLayer.Overlay:
                    return overlayCanvas;
                case CanvasLayer.Debug:
                    return debugCanvas;
                default:
                    return defaultCanvas;
            }
        }
    }
}
