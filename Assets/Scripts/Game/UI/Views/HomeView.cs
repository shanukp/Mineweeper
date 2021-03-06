﻿using System;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using DG.Tweening;

namespace Game.UI.Views
{


    /// <summary>
    /// View for the home screen 
    /// </summary>
    public class HomeView : DialogViewBase
    {
        public event Action PlayPressed;

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
        
       
        public void PressPlayButton()
        {
            PlayPressed?.Invoke();
        }
    }
}
