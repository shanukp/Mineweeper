using UnityEngine;

namespace Game.UI.Views
{
    /// <summary>
    /// Base class for all views.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public abstract class ViewBase : MonoBehaviour
    {
        public RectTransform RectTransform
        {
            get
            {
                if (rectTransform == null)
                {
                    rectTransform = this.GetComponent<RectTransform>();
                }
                return rectTransform;
            }
        }

        private RectTransform rectTransform;

        /// <summary>
        /// Sets the GameObject active or inactive.
        /// </summary>
        public void SetActive(bool active)
        {
            if (this != null && this.gameObject != null)
            {
                this.gameObject.SetActive(active);
            }
        }

        /// <summary>
        /// Destroy this GameObject.
        /// </summary>
        public virtual void Destroy()
        {
            if (this != null && this.gameObject != null)
            {
                GameObject.Destroy(this.gameObject);
            }
        }

    }

}