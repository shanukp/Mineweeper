using System;
using DG.Tweening;
using Game.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Views
{

    /// <summary>
    /// View for the game board screen
    /// </summary>
    public class GameBoardView : DialogViewBase
    {
        public GridLayoutGroup gridContainer;

        public RectTransform gridContainerRectTransform;

        public CellView cellPrefab;

        public CellView[,] gridCellsView;

        #region Public Members
        public event Action BackButtonPressed;

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

            //Initialize(7, 7);

        }

        public void Initialize(uint boardSizeX, uint boardSizeY , Action<uint,uint> OnCellLeftClick , Action<uint,uint> OnCellRightClick)
        {
            Canvas.ForceUpdateCanvases();

            gridCellsView = new CellView[boardSizeX, boardSizeY];

            gridContainer.cellSize = new Vector2((gridContainerRectTransform.sizeDelta.x - 2) / boardSizeY,
                (gridContainerRectTransform.sizeDelta.y - 2)/ boardSizeX);

            gridContainer.constraintCount = (int)boardSizeY;

            for (uint indexX = 0; indexX < boardSizeX; indexX++)
            {
                for (uint indexY = 0; indexY < boardSizeY; indexY++)
                {
                    gridCellsView[indexX, indexY] =
                        ObjectPoolHolder.Instance.GetCellPool().GetObject<CellView>(gridContainerRectTransform.transform);
                    gridCellsView[indexX, indexY].name = $"Cell[{indexX}x{indexY}]";


                    gridCellsView[indexX, indexY].SetCordinates(indexX, indexY);
                    gridCellsView[indexX, indexY].OnRightClicked += OnCellRightClick;
                    gridCellsView[indexX, indexY].OnLeftClicked += OnCellLeftClick;
                }
            }

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

        public void SetCellAsFlagged(uint x,uint y , bool status)
        {
            gridCellsView[x, y].IsFlagged = status;
        }

        public void SetCellContentVisible(uint x, uint y)
        {
            gridCellsView[x, y].IsVisible = true;
        }

        public void SetMineVisible(uint x, uint y)
        {
            gridCellsView[x, y].IsMine = true;
        }

        public void SetNearbyMinesCount(uint x, uint y , int nearbyMines)
        {
            gridCellsView[x, y].SetNearbyMinesCount(nearbyMines);
        }

        public void BackButton()
        {
            BackButtonPressed?.Invoke();
        }


        public void DrawPuzzle()
        {
           
        }

        #endregion
    }
}
