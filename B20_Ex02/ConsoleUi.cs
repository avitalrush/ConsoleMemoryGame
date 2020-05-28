using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace B20_Ex02
{
    public class ConsoleUi
    {
        private UiBoard m_Board;

        public void SetBoard(UiBoard i_Board)
        {
            m_Board = i_Board;
        }

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
            string enterInputMsg = string.Format("Please enter the board's {0} (between 4-6):", i_Dimension);
            string invalidMsg = string.Format("Invalid {0}. Please enter the board's {0} (between 4-6):", i_Dimension);
            bool inputIsValid = true;
            string dimensionStr;
            int dimensionNum;

            Console.WriteLine(enterInputMsg);
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
            string msg = string.Format("Hello {0}, what is your name?", i_NumberOfPlayer);

            Console.WriteLine(msg);

            return Console.ReadLine();
        }

        public Player.ePlayerType GetOpponentType(string i_FirstPlayerName)
        {
            int opponentType;
            string opponentTypeStr;
            bool validType = false;
            const string k_HumenOp = "0";
            const string k_ComputerOp = "1";
            string enterTypeMsg = string.Format("{0}, please choose your opponent. For Human press {1}, for Computer press {2}", i_FirstPlayerName, k_HumenOp, k_ComputerOp);
            string errorMsg = "Invalid opponent type";
            
            do
            {
                Console.WriteLine(enterTypeMsg);
                opponentTypeStr = Console.ReadLine();
                validType = opponentTypeStr.Equals(k_HumenOp) || opponentTypeStr.Equals(k_ComputerOp);
                if(!validType)
                {
                    Console.WriteLine(errorMsg);
                    Console.WriteLine();
                }
            }
            while (!validType);

            Console.WriteLine();
            opponentType = int.Parse(opponentTypeStr);

            return (Player.ePlayerType)opponentType;
        }

        public string GetValidCardChoiceFromPlayer(List<string> i_ValidCardsToChoose, string i_PlayersName)
        {
            string cardChoiceStr;
            bool validCardChoice = false;
            bool validInput = false;
            string chooseCardMsg = string.Format("{0}, please choose a card: ", i_PlayersName);
            string invalidCardMsg = "Invalid card choice (card is already shown or location does not exist on board). Please choose another card:";
            string invalidInputMsg = "Invalid input (input should contain an Uppercase letter and a digit only). Please choose another card:";

            Console.WriteLine(chooseCardMsg);
            cardChoiceStr = Console.ReadLine();

            do
            {
                validInput = checkIfValidInput(cardChoiceStr);
                if(!validInput)
                {
                    Console.WriteLine(invalidInputMsg);
                    cardChoiceStr = Console.ReadLine();
                }
                else
                {
                    validCardChoice = checkIfValidCardChoice(cardChoiceStr, i_ValidCardsToChoose);
                    if (!validCardChoice)
                    {
                        Console.WriteLine(invalidCardMsg);
                        cardChoiceStr = Console.ReadLine();
                    }
                }
            }
            while (!validInput || !validCardChoice);

            return cardChoiceStr;
        }

        private bool checkIfValidCardChoice(string i_CardChoiceStr, List<string> i_ValidCardsToChoose)
        {
            return i_ValidCardsToChoose.Contains(i_CardChoiceStr);
        }

        private bool checkIfValidInput(string i_InputStr)
        {
            bool validInput;

            if (i_InputStr.Length == 2)
            {
                char column = i_InputStr[0];
                char row = i_InputStr[1];
                validInput = char.IsUpper(column) && char.IsDigit(row);
            }
            else if (i_InputStr.Equals("Q"))
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
            string inputStr;
            bool validInput = false;
            const string k_Yes = "Y";
            const string k_No = "N";

            do
            {
                Console.WriteLine("Do you want to play another round? For yes press {0}, for No press {1}", k_Yes, k_No);
                inputStr = Console.ReadLine();
                validInput = inputStr.Equals(k_Yes) || inputStr.Equals(k_No);
                if (!validInput)
                {
                    Console.WriteLine("Invalid input");
                    Console.WriteLine();
                }
            }
            while(!validInput);
            Console.WriteLine();

            return inputStr.Equals(k_Yes);
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
                    if (i_LogicBoardCells[i - 1, j - 1].IsHidden)
                    {
                        i_BoardRow.Append(' ');
                    }
                    else
                    {
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

        public void PrintComputerMoveMsg()
        {
            Console.WriteLine("Computer is making a move...");
            System.Threading.Thread.Sleep(2000);
        }

        public void PrintPoints(string[] i_PlayersNames, int[] i_PlayersPoints)
        {
            Console.WriteLine("{0} got {1} points", i_PlayersNames[0], i_PlayersPoints[0]);
            Console.WriteLine("{0} got {1} points", i_PlayersNames[1], i_PlayersPoints[1]);
        }
    }
}
