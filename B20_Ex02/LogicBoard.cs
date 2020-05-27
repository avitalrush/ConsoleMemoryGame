using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    public class LogicBoard
    {
        // MEMBERS:
        private readonly int r_width, r_height;
        private BoardCell[,] m_Cells;

        // CTOR:
        public LogicBoard(int i_Height, int i_Width)
        {
            this.r_height = i_Height;
            this.r_width = i_Width;

            BoardCell[,] tempCells = new BoardCell[i_Height, i_Width];
            for (int i = 0; i < r_height; i++)
            {
                for (int j = 0; j < r_width; j++)
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
            //m_Cells[i_CardLocationOnBoard.Column, i_CardLocationOnBoard.Row].isHidden = true;
            m_Cells[i_CardLocationOnBoard.Row, i_CardLocationOnBoard.Column].isHidden = true;
        }

        public void RevealCard(Location i_CardLocationOnBoard)
        {
            //Console.WriteLine("Column: {0}, Row: {1}", i_CardLocationOnBoard.Column, i_CardLocationOnBoard.Row);
            m_Cells[i_CardLocationOnBoard.Row, i_CardLocationOnBoard.Column].isHidden = false;
        }
    }
}
