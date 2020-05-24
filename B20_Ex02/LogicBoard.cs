using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    class LogicBoard
    {
        // MEMBERS:
        private readonly int r_width, r_height;
        private BoardCell[,] m_Cells;

        // CTOR:
        public LogicBoard(int i_width, int i_height)
        {
            this.r_width = i_width;
            this.r_height = i_height;

            BoardCell[,] tempCells = new BoardCell[i_width, i_height];
            for (int i = 0; i < r_width; i++)
            {
                for (int j = 0; j < r_height; j++)
                {
                    tempCells[i, j] = new BoardCell();
                }
            }
            m_Cells = tempCells;
        }

        // METHODS:
        // // get / set METHODS
        public int Width
        {
            get
            {
                return r_width;
            }
        }

        public int Height
        {
            get
            {
                return r_height;
            }
        }

        public BoardCell[,] Cells
        {
            get
            {
                return m_Cells;
            }
        }

        public void HideCard(Location i_CardLocationOnBoard)
        {
            m_Cells[i_CardLocationOnBoard.Column, i_CardLocationOnBoard.Row].isHidden = true;
        }
    }
}
