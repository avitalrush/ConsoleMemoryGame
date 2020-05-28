using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    public class LogicBoard
    {
        private readonly int r_Height, r_Width;
        private BoardCell[,] m_Cells;

        public LogicBoard(int i_Height, int i_Width)
        {
            this.r_Height = i_Height;
            this.r_Width = i_Width;

            m_Cells = new BoardCell[i_Height, i_Width];

            for (int rowIndex = 0; rowIndex < r_Height; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < r_Width; columnIndex++)
                {
                    m_Cells[rowIndex, columnIndex] = new BoardCell();
                }
            }
        }

        public int Width
        {
            get
            {
                return r_Width;
            }
        }

        public int Height
        {
            get
            {
                return r_Height;
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
            m_Cells[i_CardLocationOnBoard.Row, i_CardLocationOnBoard.Column].IsHidden = true;
        }

        public void RevealCard(Location i_CardLocationOnBoard)
        {
            m_Cells[i_CardLocationOnBoard.Row, i_CardLocationOnBoard.Column].IsHidden = false;
        }
    }
}
