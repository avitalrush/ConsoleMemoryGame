using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    class UIBoard
    {
        // MEMBERS:
        private readonly int r_width, r_height;
        private Card[,] m_Cards;

        // CTOR:
        public UIBoard(int i_width, int i_height)
        {
            this.r_width = i_width;
            this.r_height = i_height;

            // initialize Cards matrix
            for (int i = 0; i < r_width; i++)
            {
                for (int j = 0; j < r_height; j++)
                {
                    m_Cards[i, j] = ();
                }
            }
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

        public char getCardValue(Location i_LocationOnBoard)
        {
            return m_Cards[i_LocationOnBoard.Column, i_LocationOnBoard.Row].CardValue;

        }
    }
}
