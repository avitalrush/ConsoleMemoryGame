using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace B20_Ex02
{
    public class UI
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

        public int GetBoardWidth()
        {
            return GetDimension("width");
        }

        public int GetBoardHeight()
        {
            return GetDimension("height");
        }

        public int GetDimension(string i_Dimension)
        {
            string msg = string.Format("Please enter the board's {0} (between 4-6):", i_Dimension);
            string invalidMsg = string.Format("Invalid {0}. Please enter the board's {0} (between 4-6):", i_Dimension);
            bool inputIsValid = true;
            string dimensionStr;
            int dimensionNum;

            do
            {
                Console.WriteLine(msg);
                dimensionStr = Console.ReadLine();
                inputIsValid = ValidateDimension(dimensionStr);

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

        public bool ValidateDimension(string i_Dimension)
        {
            return i_Dimension == "4" || i_Dimension == "5" || i_Dimension == "6";
        }

        public string GetPlayerName(string i_NumberOfPlayer)
        {
            string msg = string.Format("Hello {0}, what is your name? ", i_NumberOfPlayer);
            Console.WriteLine(msg);
            return Console.ReadLine();
        }

        public Player.ePlayerType GetOpponentType(string i_PlayerOneName)
        {
            string typeChosen;
            bool validType = false;
            string msg = string.Format("{0}, please choose your opponent. for Human press 0, for Computer press 1: ", i_PlayerOneName);
            string errorMsg = string.Format("Invalid key. for Human press 0, for Computer press 1: ");

            Console.WriteLine(msg);
            typeChosen = Console.ReadLine();
            do
            {
                if(typeChosen == "0" || typeChosen == "1")
                {
                    validType = true;
                }
                else
                {
                    Console.WriteLine(errorMsg);
                    typeChosen = Console.ReadLine();
                }
            }
            while (!validType);

            return (Player.ePlayerType)Convert.ToInt32(typeChosen);
        }

        public char GetCardValue(Location i_LocationOnBoard)
        {
            return m_Board.GetCardValue(i_LocationOnBoard);
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

        public char AskUserForAnotherRound()
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

        public string GetValidMoveFromUser()
        {
            string userMoveStr;
            Console.WriteLine("Choose a card : ");
            userMoveStr = Console.ReadLine();

            return userMoveStr;
        }

        public void PrintBoard(BoardCell[,] i_LogicBoardCells)
        {
            StringBuilder frameRow = new StringBuilder();
            StringBuilder seperationRow = new StringBuilder();
            StringBuilder boardRow = new StringBuilder();
            StringBuilder fullBoard = new StringBuilder();

            CreateFrameRow(ref fullBoard, ref frameRow, m_Board.Width);
            CreateSeperationRow(ref fullBoard, ref seperationRow, m_Board.Width);
            CreateBoardRows(ref fullBoard, ref boardRow, ref seperationRow, i_LogicBoardCells, m_Board.Height, m_Board.Width);

            Console.WriteLine(fullBoard);
        }

        public void CreateFrameRow(ref StringBuilder i_FullBoard, ref StringBuilder i_FrameRow, int i_Width)
        {
            char column = 'A';
            int i;

            i_FrameRow.Append("   ");
            for (i = 1; i <= i_Width; i++)
            {
                i_FrameRow.Append(column++);
                i_FrameRow.Append("   ");
            }

            AssembleFullBoard(ref i_FullBoard, ref i_FrameRow);
        }

        public void CreateSeperationRow(ref StringBuilder i_FullBoard, ref StringBuilder i_SeparationRow, int i_Width)
        {
            int i;

            i_SeparationRow.Append("  ");
            for (i = 1; i <= (4 * i_Width) + 1; i++)
            {
                i_SeparationRow.Append('=');
            }

            AssembleFullBoard(ref i_FullBoard, ref i_SeparationRow);
        }

        public void CreateBoardRows(ref StringBuilder i_FullBoard, ref StringBuilder i_BoardRow, ref StringBuilder i_SeparationRow, 
                                    BoardCell[,] i_LogicBoardCells, int i_Height, int i_Width)
        {
            int i, j;
            Location cardLocation;

            for (i = 1; i <= i_Height; i++)
            {
                i_BoardRow.Remove(0, i_BoardRow.Length);
                i_BoardRow.Append(i);
                i_BoardRow.Append(' ');
                i_BoardRow.Append('|');
                for (j = 1; j <= i_Width; j++)
                {
                    i_BoardRow.Append(' ');
                    if (i_LogicBoardCells[i - 1, j - 1].isHidden)
                    {
                        i_BoardRow.Append(' ');
                    }
                    else
                    {
                        cardLocation = new Location(i - 1, j - 1);
                        i_BoardRow.Append(m_Board.GetCardValue(cardLocation));
                    }

                    i_BoardRow.Append(' ');
                    i_BoardRow.Append('|');
                }

                AssembleFullBoard(ref i_FullBoard, ref i_BoardRow);
                AssembleFullBoard(ref i_FullBoard, ref i_SeparationRow);
            }
        }

        public void AssembleFullBoard(ref StringBuilder i_FullBoard, ref StringBuilder i_RowToBeAppended)
        {
            i_FullBoard.AppendLine(i_RowToBeAppended.ToString());
        }
    }
}
