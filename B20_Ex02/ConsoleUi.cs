using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace B20_Ex02
{
    public class ConsoleUi
    {
        private UiBoard m_Board;
        private const string k_HumanOpponent = "0";
        private const string k_ComputerOpponent = "1";
        private const string k_Yes = "Y";
        private const string k_No = "N";
        private const string k_Quit = "Q";
        private const int k_ValidInputSize = 2;
        private const int k_SleepTimeMilliSec = 2000;
        private static readonly List<string> sr_SupportedBoardDimensions = new List<string>() {"4", "5", "6"}; 

        public void SetBoard(UiBoard i_Board)
        {
            m_Board = i_Board;
        }

        public int GetBoardWidth()
        {
            return getDimension("width");
        }

        public int GetBoardHeight()
        {
            return getDimension("height");
        }

        private int getDimension(string i_Dimension)
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
                inputIsValid = validateDimension(dimensionStr);
                if (!inputIsValid)
                {
                    Console.WriteLine(invalidMsg);
                }
            }
            while(!inputIsValid);

            dimensionNum = int.Parse(dimensionStr);

            return dimensionNum;
        }

        private bool validateDimension(string i_Dimension)
        {
            return sr_SupportedBoardDimensions.Contains(i_Dimension);
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

            string enterTypeMsg = string.Format("{0}, please choose your opponent. For Human press {1}, for Computer press {2}", i_FirstPlayerName, k_HumanOpponent, k_ComputerOpponent);
            const string k_ErrorMsg = "Invalid opponent type";
            
            do
            {
                Console.WriteLine(enterTypeMsg);
                opponentTypeStr = Console.ReadLine();
                validType = opponentTypeStr.Equals(k_HumanOpponent) || opponentTypeStr.Equals(k_ComputerOpponent);
                if(!validType)
                {
                    Console.WriteLine(k_ErrorMsg + Environment.NewLine);
                }
            }
            while (!validType);

            Console.WriteLine();
            opponentType = int.Parse(opponentTypeStr);

            return (Player.ePlayerType)opponentType;
        }

        public string GetValidCardChoiceFromPlayer(List<string> i_ValidCardsToChoose, string i_PlayersName)
        {
            string cardLocationStr;
            bool cardCanBeChosen;
            const string k_InvalidCardMsg = "Invalid card choice (card is already shown or location does not exist on board), please try again";

            i_ValidCardsToChoose.Add(k_Quit);
            do
            {
                cardLocationStr = getValidCardLocation(i_PlayersName);
                cardCanBeChosen = checkIfValidCardToChoose(cardLocationStr, i_ValidCardsToChoose);

                if (!cardCanBeChosen)
                {
                    Console.WriteLine(k_InvalidCardMsg);
                }
            }
            while (!cardCanBeChosen);

            return cardLocationStr;
        }

        private string getValidCardLocation(string i_PlayersName)
        {
            string chooseCardMsg = string.Format("{0}, please choose a card: ", i_PlayersName);
            string invalidInputMsg = "Invalid input (input should contain an Uppercase letter and a digit only), please try again";
            string cardChoiceStr;
            bool validInput;

            do
            {
                Console.WriteLine(chooseCardMsg);
                cardChoiceStr = Console.ReadLine();
                validInput = checkIfValidInput(cardChoiceStr);

                if(!validInput)
                {
                    Console.WriteLine(invalidInputMsg);
                }
            }
            while(!validInput);

            return cardChoiceStr;
        }

        private bool checkIfValidCardToChoose(string i_CardChoiceStr, List<string> i_ValidCardsToChoose)
        {
            return i_ValidCardsToChoose.Contains(i_CardChoiceStr);
        }

        private bool checkIfValidInput(string i_InputStr)
        {
            bool validInput;

            if (i_InputStr.Length == k_ValidInputSize)
            {
                char column = i_InputStr[0];
                char row = i_InputStr[1];
                validInput = char.IsUpper(column) && char.IsDigit(row);
            }
            else if (i_InputStr.Equals(k_Quit))
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
            bool validInput;

            do
            {
                Console.WriteLine("Do you want to play another round? For yes press {0}, for No press {1}", k_Yes, k_No);
                inputStr = Console.ReadLine();
                validInput = inputStr.Equals(k_Yes) || inputStr.Equals(k_No) || inputStr.Equals(k_Quit);
                if (!validInput)
                {
                    Console.WriteLine("Invalid input" + Environment.NewLine);
                }
            }
            while(!validInput);

            Console.WriteLine();

            return inputStr.Equals(k_Yes);
        }

        public void PrintBoard(BoardCell[,] i_LogicBoardCells)
        {
            StringBuilder frameRow = new StringBuilder();
            StringBuilder separationRow = new StringBuilder();
            StringBuilder boardRow = new StringBuilder();
            StringBuilder fullBoard = new StringBuilder();

            createFrameRow(ref fullBoard, ref frameRow, m_Board.Width);
            createSeparationRow(ref fullBoard, ref separationRow, m_Board.Width);
            createBoardRows(ref fullBoard, ref boardRow, ref separationRow, i_LogicBoardCells, m_Board.Height, m_Board.Width);

            Console.WriteLine(fullBoard);
        }

        private void createFrameRow(ref StringBuilder i_FullBoard, ref StringBuilder i_FrameRow, int i_Width)
        {
            char column = 'A';

            i_FrameRow.Append("    ");
            for (int i = 1; i <= i_Width; i++)
            {
                i_FrameRow.Append(column++);
                i_FrameRow.Append("   ");
            }

            assembleFullBoard(ref i_FullBoard, ref i_FrameRow);
        }

        private void createSeparationRow(ref StringBuilder i_FullBoard, ref StringBuilder i_SeparationRow, int i_Width)
        {
            i_SeparationRow.Append("  ");
            for (int i = 1; i <= (4 * i_Width) + 1; i++)
            {
                i_SeparationRow.Append('=');
            }

            assembleFullBoard(ref i_FullBoard, ref i_SeparationRow);
        }

        private void createBoardRows(ref StringBuilder i_FullBoard, ref StringBuilder i_BoardRow, ref StringBuilder i_SeparationRow, 
                                     BoardCell[,] i_LogicBoardCells, int i_Height, int i_Width)
        {
            Location cardLocation;

            for (int row = 1; row <= i_Height; row++)
            {
                i_BoardRow.Remove(0, i_BoardRow.Length);
                i_BoardRow.Append(row);
                i_BoardRow.Append(' ');
                i_BoardRow.Append('|');
                for (int column = 1; column <= i_Width; column++)
                {
                    i_BoardRow.Append(' ');
                    if (i_LogicBoardCells[row - 1, column - 1].IsHidden)
                    {
                        i_BoardRow.Append(' ');
                    }
                    else
                    {
                        cardLocation = new Location(column - 1, row - 1);
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
            Console.WriteLine("Thanks for playing!" + Environment.NewLine);
        }

        public void PrintComputerMoveMsg()
        {
            Console.WriteLine("Computer is making a move...");
            System.Threading.Thread.Sleep(k_SleepTimeMilliSec);
        }

        public void PrintPoints(string[] i_PlayersNames, int[] i_PlayersPoints)
        {
            Console.WriteLine("{0} got {1} points", i_PlayersNames[0], i_PlayersPoints[0]);
            Console.WriteLine("{0} got {1} points", i_PlayersNames[1], i_PlayersPoints[1]);
        }
    }
}
