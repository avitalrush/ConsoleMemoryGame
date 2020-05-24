using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace B20_Ex02
{

    class UI
    {
        // MEMBERS:
        private UIBoard m_Board;

        // CTOR:
        public UI()
        {
            
        }

        // METHODS:
        public void SetBoard(int i_Width, int i_Height)
        {
            m_Board = new UIBoard(i_Width, i_Height);
            shuffelValuesIntoBoard(ref m_Board);
        }

        // // 'get input from user' METHODS:

        public int getBoardWidth()
        {
            return getDimension("width");
        }

        public int getBoardHeight()
        {
            return getDimension("height");
        }

        private int getDimension(string dimension)
        {
            string msg = string.Format("Please enter the board's {0} (between 4-6):", dimension);
            string invalidMsg = string.Format("Invalid {0}. Please enter the board's {0} (between 4-6):", dimension);
            bool inputIsValid = true;
            string dimensionStr;
            int dimensionNum;

            do
            {
                Console.WriteLine(msg);
                dimensionStr = Console.ReadLine();
                inputIsValid = validateDimension(dimensionStr);

                if (!inputIsValid)
                {
                    Console.WriteLine(invalidMsg);
                    string input = Console.ReadLine();
                }
            }
            while(!inputIsValid);

            dimensionNum = int.Parse(dimensionStr);
            return dimensionNum;
        }

        bool validateDimension(string dimension)
        {
            return (dimension == "4" || dimension == "5" || dimension == "6");
        }

        public char getCardValue(Location i_LocationOnBoard)
        {
            return m_Board.getCardValue(i_LocationOnBoard);
        }

        /*
         int getNumOfRows()
        {
            string inputStr;
            int numOfRows;
            bool inputIsValid;

            do
            {
                Console.WriteLine("Please enter the board's height (a number between 4 and 6): ");
                inputStr = Console.ReadLine();

                inputIsValid = validateNumOfRows(inputStr);
                if (!inputIsValid)
                {
                    Console.WriteLine("Invalid input. Please enter a valid input: ");
                }
            }
            while (!inputIsValid);

            numOfRows = int.Parse(inputStr);
            return numOfRows;
        }

        void getBoardDimensions(ref int[] arr)
        {
            string inputStr;
            bool inputIsValid;

            do
            {
                Console.WriteLine("Please enter the board's height (a number between 4 and 6): ");
                inputStr = Console.ReadLine();

                inputIsValid = validateNumOfRows(inputStr);
                if (!inputIsValid)
                {
                    Console.WriteLine("Invalid input. Please enter a valid input: ");
                }
            }
            while (!inputIsValid);
            int numOfRows;
            numOfRows = int.Parse(inputStr);
            return numOfRows;
        }
         */

        private char AskUserForAnotherRound()
        {
            char userDesicion;
            Console.WriteLine("Do you want to play another round? Y for Yes, N for No : ");
            userDesicion = char.Parse(Console.ReadLine());
            do
            {
                if(!(userDesicion == 'Y' || userDesicion == 'N'))
                {
                    Console.WriteLine("Please enter only Y for Yes, N for No : ");
                    userDesicion = char.Parse(Console.ReadLine());
                }
            }
            while(!(userDesicion == 'Y' || userDesicion == 'N'));

            return userDesicion;
        }

        private string getValidMoveFromUser()
        {
            string userMoveStr;
            Console.WriteLine("Choose a card : ");
            userMoveStr = Console.ReadLine();

            return userMoveStr;
        }

        private void printBoard(BoardCell[,] i_LogicBoardCells)
        {
            char column = 'A';
            int i = 0, j = 0;

            StringBuilder firstLine = new StringBuilder();
            StringBuilder seperationRow = new StringBuilder();
            StringBuilder boardRow = new StringBuilder();
            StringBuilder fullBoard = new StringBuilder();

            firstLine.Append("    ");
            for (i = 1; i <=m_Board.Height; i++)
            {
                firstLine.Append(column + i);
                firstLine.Append("  ");
            }

            fullBoard.AppendLine(firstLine.ToString());

            seperationRow.Append("   ");
            for (i = 1; i <= 4*m_Board.Width+1; i++)
            {
                seperationRow.Append('=');
            }

            fullBoard.AppendLine(seperationRow.ToString());


            for (i = 1; i <= m_Board.Height; i++)
            {
                boardRow.Remove(0, 4 * m_Board.Width + 2);
                boardRow.Append(i);
                boardRow.Append(' ');
                boardRow.Append(|);
                for (j = 1; j < m_Board.Width; j++)
                {
                    boardRow.Append(' ');
                    if(i_LogicBoardCells[i - 1, j - 1].m_IsHidden)
                    {
                        boardRow.Append(' ');
                    }
                    else
                    {
                        boardRow.Append(m_Board.m_Cards[i - 1, j - 1]);
                    }

                }
                fullBoard.AppendLine(boardRow.ToString());
                fullBoard.AppendLine(seperationRow.ToString());
            }
        }
    }
}
