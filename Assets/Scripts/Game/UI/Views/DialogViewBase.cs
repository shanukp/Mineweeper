using System;
using UnityEngine;
using Game.Utility;
using GameEngine.UI.Extensions;
using DG.Tweening;


namespace Game.UI.Views
{

    /// <summary>
    /// Base class for dialog views.
    /// </summary>
    public abstract class DialogViewBase : ViewBase
    {
        public CanvasGroup CanvasGroup
        {
            get
            {
                if (canvasGroup == null)
                {
                    canvasGroup = this.GetComponent<CanvasGroup>();
                }
                if (canvasGroup == null)
                {
                    canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
                }
                return canvasGroup;
            }
        }

        private CanvasGroup canvasGroup;

        protected const float TweenDuration = 0.5f;

        private bool isOpen = false;

        [SerializeField] public bool animateOpen = true;
        [SerializeField] public bool animateClose = true;


        /// <summary>
        /// Opens this dialog.
        /// </summary>
        public virtual void Open(Action Opened = null)
        {
            if (this.gameObject.activeInHierarchy)
            {
                return;
            }
            this.SetActive(true);

            if (animateOpen)
            {
                this.transform.position += this.RectTransform.AlignEdges(transform.parent as RectTransform, RectTransform.Edge.Top, RectTransform.Edge.Bottom);
                this.transform.DOMove(this.transform.position + this.RectTransform.AlignEdges(transform.parent as RectTransform, RectTransform.Edge.Top), TweenDuration).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    isOpen = true;
                    if (Opened != null)
                    {
                        Opened();
                    }
                });
            }
            else
            {
                isOpen = true;
                if (Opened != null)
                {
                    Opened();
                }
            }
        }

        /// <summary>
        /// Closes this dialog.
        /// </summary>
        public virtual void Close(Action Closed = null)
        {
            if (!this.gameObject.activeInHierarchy)
            {
                return;
            }

            if (!isOpen)
            {
                this.SetActive(false);
                if (Closed != null)
                {
                    Closed();
                }
            }

            if (animateClose)
            {
                var newPosition = this.transform.position + this.RectTransform.AlignEdges(transform.parent as RectTransform, RectTransform.Edge.Top, RectTransform.Edge.Bottom);
                this.transform.DOMove(newPosition, TweenDuration).SetEase(Ease.InBack).OnComplete(() =>
                {
                    this.SetActive(false);
                    isOpen = false;
                    if (Closed != null)
                    {
                        Closed();
                    }
                });
            }
            else
            {
                this.SetActive(false);
                isOpen = false;
                if (Closed != null)
                {
                    Closed();
                }
            }
        }
    }
}
