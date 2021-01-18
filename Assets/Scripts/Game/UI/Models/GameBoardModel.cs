using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Models
{
    /// <summary>
    /// Model for the game board screen.
    /// </summary>
    public class GameBoardModel : ModelBase
    {
        private Board _board;

        private int openedCells;

        #region Public Members

        public GameBoardModel()
        {
            CreateNewBoard();
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            _board.CreateGrid(7,7);
            _board.PopulateGrid(7, 7, 3);
            _board.DebugLog(7, 7);
        }

        private void CreateNewBoard()
        {
            _board = new Board();
        }

        public bool IsMineAtBoardCordinates(uint x , uint y )
        {
           return _board.IsMinePresentAt(x, y);
        }

        public bool IsCellViewed(uint x, uint y)
        {
            return _board.IsCellOpened(x, y);
        }

        public void SetCellAsViewed(uint x, uint y, bool value)
        {
             _board.SetCellAsClicked(x, y , true);
        }

        public bool ToggleFlaggedStatus(uint x, uint y)
        {
            return _board.ToggleFlagged(x,y);
        }

        public int GetNearbyMines(uint x, uint y)
        {
            return _board.GetNeabyMinesCount(x, y);
        }

        public void IncrementOpenedCellsCount()
        {
            openedCells++;
        }

        public bool IsGameCompleted()
        {
            return openedCells + 3 >= 49;
        }

        #endregion
    }

}
