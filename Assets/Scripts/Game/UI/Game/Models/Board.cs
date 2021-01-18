using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = System.Random;

namespace Game
{
    public class Board 
    {
        private Cell[,] _grid;

        public Board()
        {
        }

        #region Grid

        public void CreateGrid(uint sizeX, uint sizeY)
        {
            _grid = new Cell[sizeX, sizeY];

            for (uint indexX = 0; indexX < sizeX; indexX++)
            {
                for (uint indexY = 0; indexY < sizeY; indexY++)
                {
                    _grid[indexX, indexY] = new Cell(indexX, indexY);
                }
            }
        }

        public void PopulateGrid(uint x, uint y, uint settingsMinesCount)
        {
            // Placing Mines
            Random random = new Random();

            uint placed = settingsMinesCount;

            while (placed > 0)
            {
                uint r = (uint)random.Next((int)x);
                uint c = (uint)random.Next((int)y);

                if (!_grid[r, c].isMine &&
                    (r > x + 1 || r < x - 1) &&
                    (c > y + 1 || c < y - 1))
                {
                    _grid[r, c].isMine = true;

                    UpdateMineCountForNeighbouringCells(r, c, x, y);

                    placed--;
                }
            }
        }

        private void UpdateMineCountForNeighbouringCells(uint x, uint y, uint sizeX, uint sizeY)
        {
            for (uint r = x > 0 ? x - 1 : x; r < x + 2 && r < sizeX; r++)
            {
                for (uint c = y > 0 ? y - 1 : y; c < y + 2 && c < sizeY; c++)
                {
                    if (!(r == x && c == y) && !_grid[r, c].isMine)
                    {
                        _grid[r, c].nearbyMinesCount++;
                    }
                }
            }
        }

        public void Reset(uint sizeX, uint sizeY)
        {
            // Clearing out the User Grid
            for (uint r = 0; r < sizeX; ++r)
            {
                for (uint c = 0; c < sizeY; ++c)
                {
                    _grid[r, c].isFlagged = false;
                    _grid[r, c].isOpened = false;
                    _grid[r, c].nearbyMinesCount = 0;
                }
            }
        }

        public void DebugLog(uint sizeX, uint sizeY)
        {
            StringBuilder builder = new StringBuilder();

            for (uint indexX = 0; indexX < sizeX; indexX++)
            {
                builder.Append("\n");

                for (uint indexY = 0; indexY < sizeY; indexY++)
                {
                    builder.Append(_grid[indexX, indexY].isMine ? "M," : _grid[indexX, indexY].nearbyMinesCount + ",");
                }
            }

            Debug.LogError(builder.ToString());
        }

        #endregion


        #region Cells

        public bool ToggleFlagged(uint x, uint y)
        {
            return _grid[x, y].ToggleFlagged();
        }

        public int GetNeabyMinesCount(uint x, uint y)
        {
            return _grid[x, y].nearbyMinesCount;
        }

        public bool IsMinePresentAt(uint x, uint y)
        {
            return _grid[x, y].isMine;
        }

        public bool IsCellOpened(uint x, uint y)
        {
            return _grid[x, y].isOpened;
        }

        public void SetCellAsClicked(uint x, uint y, bool value)
        {
            _grid[x, y].isOpened = value;
        }
        #endregion
    }
}