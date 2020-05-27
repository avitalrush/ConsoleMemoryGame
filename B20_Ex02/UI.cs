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

        // METHODS:
        public void SetBoard(UIBoard i_Board)
        {
            m_Board = i_Board;
            //shuffleValuesIntoBoard(ref m_Board);
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
            string enterInputmsg = string.Format("Please enter the board's {0} (between 4-6):", i_Dimension);
            string invalidMsg = string.Format("Invalid {0}. Please enter the board's {0} (between 4-6):", i_Dimension);
            bool inputIsValid = true;
            string dimensionStr;
            int dimensionNum;

            Console.WriteLine(enterInputmsg);
            do
            {
                dimensionStr = Console.ReadLine();
                inputIsValid = ValidateDimension(dimensionStr);

                if (!inputIsValid)
                {
                    Console.WriteLine(invalidMsg);
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

        public int GetOpponentType(string i_PlayerOneName)
        {
            int typeChosen;
            string input;
            bool validType = false;
            string msg = string.Format("{0}, please choose your opponent. for Human press 0, for Computer press 1: ", i_PlayerOneName);
            string errorMsg = string.Format("Invalid key. for Human press 0, for Computer press 1: ");

            Console.WriteLine(msg);
            input = Console.ReadLine();
            do
            {
                if (input == "0" || input == "1")
                {
                    validType = true;
                }
                else
                {
                    Console.WriteLine(errorMsg);
                    input = Console.ReadLine();
                }
            }
            while (!validType);
            typeChosen = int.Parse(input);

            return typeChosen;
        }

        public string GetValidCardChoiceFromPlayer(List<string> i_ValidCardsToChoose, string i_PlayersName)
        {
            string userCardChoiceStr;
            bool validCardChoice = false;
            bool validInput = false;
            string chooseCardMsg = string.Format("{0}, please choose a card: ", i_PlayersName);
            string invalidCardMsg = string.Format("Invalid card choice (card is already shown or location does not exist on board). Please choose another card: ");
            string invalidInputMsg = string.Format("Invalid input (input should contain an Uppercase letter and a digit only). Please choose another card:");

            Console.WriteLine(chooseCardMsg);
            userCardChoiceStr = Console.ReadLine();

            do
            {
                validInput = CheckValidityOfInput(userCardChoiceStr);
                if(!validInput)
                {
                    Console.WriteLine(invalidInputMsg);
                    userCardChoiceStr = Console.ReadLine();
                }

                else
                {
                    validCardChoice = CheckValidityOfCardChoice(userCardChoiceStr, i_ValidCardsToChoose);
                    if (!validCardChoice)
                    {
                        Console.WriteLine(invalidCardMsg);
                        userCardChoiceStr = Console.ReadLine();
                    }
                }
            }
            while (!validInput || !validCardChoice);

            return userCardChoiceStr;
        }

        private bool CheckValidityOfCardChoice(string i_UserChoiceStr, List<string> i_ValidCardsToChoose)
        {
            //bool validMove;
            //validMove = i_ValidMoves.Exists(x => x == i_UserMoveStr);
            //return validMove;

            return i_ValidCardsToChoose.Contains(i_UserChoiceStr);
        }

        private bool CheckValidityOfInput(string i_UserChoiceStr)
        {
            bool validInput;
            if (i_UserChoiceStr.Length == 2)
            {
                char column = i_UserChoiceStr[0];
                char row = i_UserChoiceStr[1];
                validInput = char.IsUpper(column) && char.IsDigit(row);
            }
            else if (i_UserChoiceStr.Equals("Q"))
            {
                validInput = true;
            }
            else
            {
                validInput = false;
            }

            return validInput;
        }

        public char GetCardValue(Location i_LocationOnBoard)
        {
            return m_Board.GetCardValue(i_LocationOnBoard);
        }

        public bool AskUserForAnotherRound()
        {
            char userDesicion;
            bool playAgain;
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

            if(userDesicion == 'Y')
            {
                playAgain = true;
            }

            else
            {
                playAgain = false;
            }

            return playAgain;
        }

        public void PrintBoard(BoardCell[,] i_LogicBoardCells)
        {
            StringBuilder frameRow = new StringBuilder();
            StringBuilder seperationRow = new StringBuilder();
            StringBuilder boardRow = new StringBuilder();
            StringBuilder fullBoard = new StringBuilder();

            createFrameRow(ref fullBoard, ref frameRow, m_Board.Width);
            createSeperationRow(ref fullBoard, ref seperationRow, m_Board.Width);
            createBoardRows(ref fullBoard, ref boardRow, ref seperationRow, i_LogicBoardCells, m_Board.Height, m_Board.Width);

            Console.WriteLine(fullBoard);
        }

        private void createFrameRow(ref StringBuilder i_FullBoard, ref StringBuilder i_FrameRow, int i_Width)
        {
            char column = 'A';
            int i;

            i_FrameRow.Append("    ");
            for (i = 1; i <= i_Width; i++)
            {
                i_FrameRow.Append(column++);
                i_FrameRow.Append("   ");
            }

            assembleFullBoard(ref i_FullBoard, ref i_FrameRow);
        }

        private void createSeperationRow(ref StringBuilder i_FullBoard, ref StringBuilder i_SeparationRow, int i_Width)
        {
            int i;

            i_SeparationRow.Append("  ");
            for (i = 1; i <= (4 * i_Width) + 1; i++)
            {
                i_SeparationRow.Append('=');
            }

            assembleFullBoard(ref i_FullBoard, ref i_SeparationRow);
        }

        private void createBoardRows(ref StringBuilder i_FullBoard, ref StringBuilder i_BoardRow, ref StringBuilder i_SeparationRow, 
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
                        //cardLocation = new Location(i - 1, j - 1);
                        cardLocation = new Location(j - 1, i - 1);
                        i_BoardRow.Append(m_Board.GetCardValue(cardLocation));
                    }

                    i_BoardRow.Append(' ');
                    i_BoardRow.Append('|');
                }

                assembleFullBoard(ref i_FullBoard, ref i_BoardRow);
                assembleFullBoard(ref i_FullBoard, ref i_SeparationRow);
            }
        }

        private void assembleFullBoard(ref StringBuilder i_FullBoard, ref StringBuilder i_RowToBeAppended)
        {
            i_FullBoard.AppendLine(i_RowToBeAppended.ToString());
        }

        public void PrintWinnerMsg(string i_WinnerPlayer)
        {
            Console.WriteLine("{0} is the winner!", i_WinnerPlayer);
        }

        public void PrintTieMsg()
        {
            Console.WriteLine("It's a tie!");
        }

        public void PrintGoodbyeMsg()
        {
            Console.WriteLine("Thanks for playing!");
        }

        public void PrintPoints(string[] i_PlayersNames, int[] i_PlayersPoints)
        {
            Console.WriteLine("{0} got {1} points", i_PlayersNames[0], i_PlayersPoints[0]);
            Console.WriteLine("{0} got {1} points", i_PlayersNames[1], i_PlayersPoints[1]);
        }
    }
}
