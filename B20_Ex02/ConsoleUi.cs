using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace B20_Ex02
{
    public class ConsoleUi
    {
        private const string k_HumanOpponent = "0";
        private const string k_ComputerOpponent = "1";
        private const string k_Yes = "Y";
        private const string k_No = "N";
        private const string k_Quit = "Q";
        private const int k_ValidInputSize = 2;
        private const int k_SleepTimeMilliSec = 2000;
        private static readonly List<string> sr_SupportedBoardDimensions = new List<string>() { "4", "5", "6" };
        private UiBoard m_Board;

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
            StringBuilder fullBoard = new StringBuilder();

            createFrameRow(ref fullBoard);
            createBoardRows(ref fullBoard, i_LogicBoardCells);
            Console.WriteLine(fullBoard);
        }

        private StringBuilder createSeparationRow()
        {
            StringBuilder separationRow = new StringBuilder();
            int boardWidth = m_Board.Width;

            separationRow.Append("  ");
            for (int i = 1; i <= (4 * boardWidth) + 1; i++)
            {
                separationRow.Append('=');
            }

            return separationRow;
        }

        private void createFrameRow(ref StringBuilder i_FullBoard)
        {
            StringBuilder frameRow = new StringBuilder();
            StringBuilder separationRow = createSeparationRow();
            char column = 'A';
            int boardWidth = m_Board.Width;

            frameRow.Append("    ");
            for (int i = 1; i <= boardWidth; i++)
            {
                frameRow.Append(column++);
                frameRow.Append("   ");
            }

            assembleFullBoard(ref i_FullBoard, frameRow);
            assembleFullBoard(ref i_FullBoard, separationRow);
        }

        private void createBoardRows(ref StringBuilder i_FullBoard, BoardCell[,] i_LogicBoardCells)
        {
            StringBuilder boardRow = new StringBuilder();
            StringBuilder separationRow = createSeparationRow();
            Location cardLocation;
            int boardHeight = m_Board.Height;
            int boardWidth = m_Board.Width;
            
            for (int row = 1; row <= boardHeight; row++)
            {
                boardRow.Remove(0, boardRow.Length);
                boardRow.Append(row);
                boardRow.Append(' ');
                boardRow.Append('|');
                for (int column = 1; column <= boardWidth; column++)
                {
                    boardRow.Append(' ');
                    if (i_LogicBoardCells[row - 1, column - 1].IsHidden)
                    {
                        boardRow.Append(' ');
                    }
                    else
                    {
                        cardLocation = new Location(column - 1, row - 1);
                        boardRow.Append(m_Board.GetCardValue(cardLocation));
                    }

                    boardRow.Append(' ');
                    boardRow.Append('|');
                }

                assembleFullBoard(ref i_FullBoard, boardRow);
                assembleFullBoard(ref i_FullBoard, separationRow);
            }
        }

        private void assembleFullBoard(ref StringBuilder i_FullBoard, StringBuilder i_RowToBeAppended)
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
