using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class CellView : MonoBehaviour, IPointerClickHandler
    {
        public RectTransform contentRect;
        public Button cellButton;
        public Text cellValue;
        public Image flagContent;
        public Image bombImage;

        private bool _isMine;
        private uint _nearbyMines;

        private uint _xCordinate;

        private uint _yCordinate;

        public Action<uint,uint> OnRightClicked;

        public Action<uint,uint> OnLeftClicked;


        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                OnRightClicked?.Invoke(_xCordinate,_yCordinate);
            }
            else if(eventData.button == PointerEventData.InputButton.Left)
            {
                OnLeftClicked?.Invoke(_xCordinate, _yCordinate);
            }
        }

        public bool IsVisible
        {
            set
            {
                contentRect.gameObject.SetActive(value);
                cellValue.text = "X";
            }
        }

        public bool IsFlagged
        {
            set
            {
                flagContent.gameObject.SetActive(value);
            }
        }

        public bool IsMine
        {
            set
            {
                bombImage.gameObject.SetActive(value);
                cellValue.gameObject.SetActive(!value);
            }
        }

        public void SetCordinates(uint _xCordinate , uint yCordinate)
        {
            this._xCordinate = _xCordinate;
            this._yCordinate = yCordinate;
        }

        public void Intialize(Action<uint,uint> OnLeftClick , Action<uint,uint> OnRightClick)
        {
            this.OnLeftClicked += OnLeftClick;
            this.OnRightClicked += OnRightClick;
        }

        public void SetNearbyMinesCount(int nearbyMines)
        {
            cellValue.text = nearbyMines.ToString();
        }
    }
}