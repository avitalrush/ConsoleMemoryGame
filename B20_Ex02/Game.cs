using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    public class Game
    {
        private const string k_QuitMoveString = "Q";
        private const int k_SleepTimeMiliSec = 2000;
        private readonly Logic r_Logic;
        private readonly ConsoleUi r_ConsoleUi;
        private bool m_AnotherRound;
        private bool m_QuitGame;

        public Game()
        {
            m_AnotherRound = true;
            m_QuitGame = false;
            r_Logic = new Logic();
            r_ConsoleUi = new ConsoleUi();
        }

        public void Start()
        {
            initializePlayers();
            while(m_AnotherRound && !m_QuitGame)
            {
                run();
            }
        }

        private void run()
        {
            Player currentPlayer;
            Move currentMove;
            bool validMovesLeft = true;

            r_Logic.ResetPlayersScore();
            initializeBoards();
            clearAndPrintBoard();
            while (validMovesLeft && !m_QuitGame)
            {
                currentPlayer = r_Logic.GetCurrentPlayer();
                currentMove = makeMove(currentPlayer);
                if (!m_QuitGame)
                {
                    calculateMoveResult(currentMove);
                    validMovesLeft = r_Logic.CheckIfValidMovesLeft();
                }
            }

            if (!m_QuitGame)
            {
                printGameResult();
                askUserForAnotherRound();
            }
            else
            {
                r_ConsoleUi.PrintGoodbyeMsg();
            }
        }

        private void askUserForAnotherRound()
        {
            m_AnotherRound = r_ConsoleUi.AskUserForAnotherRound();

            if (!m_AnotherRound)
            {
                r_ConsoleUi.PrintGoodbyeMsg();
            }
            else
            {
                r_Logic.SwitchTurnToMainPlayer();
            }
        }

        private Move makeMove(Player i_CurrentPlayer)
        {
            List<string> validCardsToChoose;    
            const int k_NumOfCardsToChoose = 2;
            Move currentMove = new Move(i_CurrentPlayer);

            for (int i = 0; i < k_NumOfCardsToChoose && !m_QuitGame; i++)
            {
                validCardsToChoose = r_Logic.GetValidCardsList();
                if (i_CurrentPlayer.PlayerType == Player.ePlayerType.Human)
                {
                    makePlayerCardChoice(validCardsToChoose, i_CurrentPlayer.Name, ref currentMove);
                }
                else
                {
                    makeComputersCardChoice(validCardsToChoose, ref currentMove);
                }
            }

            return currentMove;
        }

        private void initializeBoards()
        {
            int height, width;
            bool validBoardSize = true;
            const string k_InvalidMsg = "Invalid Board Size (odd number of cells), please try again";

            do
            {
                height = r_ConsoleUi.GetBoardHeight();
                width = r_ConsoleUi.GetBoardWidth();
                Console.WriteLine();
                validBoardSize = validateBoardSize(height, width);
                if(!validBoardSize)
                {
                    Console.WriteLine(k_InvalidMsg);
                }
            }
            while (!validBoardSize);

            initializeLogicBoard(height, width);
            initializeUiBoard(height, width);
        }

        private bool validateBoardSize(int i_Height, int i_Width)
        {
            int numOfCells = i_Height * i_Width;
            return numOfCells % 2 == 0;
        }

        private void initializeLogicBoard(int i_Height, int i_Width)
        {
            r_Logic.SetBoard(i_Height, i_Width);
        }

        private void initializeUiBoard(int i_Height, int i_Width)
        {
            UiBoard board = new UiBoard(i_Height, i_Width);

            board.ShuffleCards();
            r_ConsoleUi.SetBoard(board);
        }

        private void calculateMoveResult(Move i_Move)
        {
            bool matchingCards = checkIfMatchingCards(i_Move);
            bool validMovesLeft;
            const string k_NoMatchMsg = "Cards are not matching!";

            if (!matchingCards)
            {
                Console.WriteLine(k_NoMatchMsg);
                System.Threading.Thread.Sleep(k_SleepTimeMiliSec);
                r_Logic.UndoMove(i_Move);
                r_Logic.SwitchPlayers();
                clearAndPrintBoard();
            }
            else
            {
                r_Logic.GivePointToCurrentPlayer();
                validMovesLeft = r_Logic.CheckIfValidMovesLeft();
                if(validMovesLeft)
                {
                    Console.WriteLine("It's a Match! {0}, you get another turn", i_Move.GetPlayer().Name);
                }
            }
        }

        private bool checkIfMatchingCards(Move i_Move)
        {
            Location firstCardLocation = i_Move.GetLocationOfFirstCard();
            Location secondCardLocation = i_Move.GetLocationOfSecondCard();
            char firstCardValue = r_ConsoleUi.GetCardValue(firstCardLocation);
            char secondCardValue = r_ConsoleUi.GetCardValue(secondCardLocation);

            return firstCardValue.Equals(secondCardValue);
        }

        private void printGameResult()
        {
            Player winner = r_Logic.GetWinner();
            string[] playersNames = r_Logic.GetPlayersNames();
            int[] playersPoints = r_Logic.GetPlayersPoints();

            if (winner == null)
            {
                r_ConsoleUi.PrintTieMsg();
            }
            else
            {
                r_ConsoleUi.PrintWinnerMsg(winner.Name);
            }

            r_ConsoleUi.PrintPoints(playersNames, playersPoints);
        }

        private void initializePlayers()
        {
            string player1Name, player2Name;
            Player.ePlayerType player2Type;
            const bool v_IsPlayersTurn = true;

            player1Name = r_ConsoleUi.GetPlayerName("Player No. 1");
            r_Logic.AddPlayer(player1Name, Player.ePlayerType.Human, v_IsPlayersTurn);

            player2Type = r_ConsoleUi.GetOpponentType(player1Name);
            if(player2Type == Player.ePlayerType.Computer)
            {
                player2Name = "Computer";
            }
            else
            {
                player2Name = r_ConsoleUi.GetPlayerName("Player No. 2");
                Console.WriteLine();
            }

            r_Logic.AddPlayer(player2Name, player2Type, !v_IsPlayersTurn);
        }

        private void clearAndPrintBoard()
        {
            clearBoard();
            printBoard();
        }

        private void clearBoard()
        {
            Ex02.ConsoleUtils.Screen.Clear();
        }

        private void printBoard()
        {
            BoardCell[,] logicBoard = r_Logic.Board.Cells;
            r_ConsoleUi.PrintBoard(logicBoard);
        }

        private void revealCardInLocation(string i_LocationStr, ref Move i_CurrentMove)
        {
            string locationDigitsStr = convertLettersToDigits(i_LocationStr);
            Location cardLocation = r_Logic.GetLocationFromStr(locationDigitsStr);

            r_Logic.RevealCard(cardLocation);
            i_CurrentMove.SetLocation(cardLocation);
            clearAndPrintBoard();
        }

        private string convertLettersToDigits(string i_LocationStr)
        {
            char xCord = i_LocationStr[0];
            char yCord = i_LocationStr[1];
            int xCordNum = (xCord - 'A') + 1;

            return string.Format("{0}{1}", xCordNum, yCord);
        }

        private string getValidCardChoiceFromComputer(List<string> i_ValidCardsToChoose)
        {
            Random numberGenerator = new Random();
            int chosenRandomIndex = numberGenerator.Next(i_ValidCardsToChoose.Count - 1);

            return i_ValidCardsToChoose[chosenRandomIndex];
        }

        private void makePlayerCardChoice(List<string> i_ValidCards, string i_PlayerName, ref Move i_CurrentMove)
        {
            string cardChoiceStr = r_ConsoleUi.GetValidCardChoiceFromPlayer(i_ValidCards, i_PlayerName);

            if(cardChoiceStr.Equals(k_QuitMoveString))
            {
                m_QuitGame = true;
            }
            else
            {
                revealCardInLocation(cardChoiceStr, ref i_CurrentMove);
            }
        }

        private void makeComputersCardChoice(List<string> i_ValidCards, ref Move i_CurrentMove)
        {
            string cardChoiceStr = getValidCardChoiceFromComputer(i_ValidCards);

            r_ConsoleUi.PrintComputerMoveMsg();
            revealCardInLocation(cardChoiceStr, ref i_CurrentMove);
        }
    }
}
