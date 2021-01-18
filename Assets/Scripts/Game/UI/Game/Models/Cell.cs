
namespace Game
{
    public class Cell
    {
        public uint xCordinate, yCordinate;

        public bool isOpened;

        public bool isFlagged;

        public bool isMine;

        public int nearbyMinesCount;

        public Cell(uint indexX, uint indexY)
        {
            xCordinate = indexX;
            yCordinate = indexY;

            isOpened = false;
            isFlagged = false;
            isMine = false;
            nearbyMinesCount = 0;
        }

        public bool ToggleFlagged()
        {
            isFlagged = !isFlagged;

            return isFlagged;
        }
    }
}