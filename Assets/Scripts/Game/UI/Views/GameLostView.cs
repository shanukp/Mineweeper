using System;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using DG.Tweening;


namespace Game.UI.Views
{


    /// <summary>
    /// View for the game lost overlay
    /// </summary>
    public class GameLostView : DialogViewBase
    {
        public event Action ContinuePressed;

        public override void Open(Action Opened = null)
        {
            this.SetActive(true);

            this.CanvasGroup.alpha = 0;
            this.CanvasGroup.DOFade(1f, TweenDuration).OnComplete(() =>
            {
                if (Opened != null)
                {
                    Opened();
                }
            });
        }

        public override void Close(Action Closed = null)
        {
            this.CanvasGroup.DOFade(0f, TweenDuration / 2f).OnComplete(() =>
            {
                this.SetActive(false);
                if (Closed != null)
                {
                    Closed();
                }
            });
        }

        public void HandleContinuePressed()
        {
            ContinuePressed?.Invoke();
        }
    }
}
