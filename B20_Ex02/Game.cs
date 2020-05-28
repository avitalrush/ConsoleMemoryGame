using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    public class Game
    {
        private bool m_AnotherRound;
        private bool m_QuitGame;
        private Logic m_Logic;
        private ConsoleUi m_ConsoleUi;

        public Game()
        {
            m_AnotherRound = true;
            m_QuitGame = false;
            m_Logic = new Logic();
            m_ConsoleUi = new ConsoleUi();
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
            List<string> validCardsToChoose;
            const int k_NumOfCardsToChoose = 2;

            initializeBoards();
            clearAndPrintBoard();
            while(validMovesLeft && !m_QuitGame)
            {
                currentPlayer = m_Logic.GetCurrentPlayer();
                currentMove = initializeMove(currentPlayer);

                for(int i = 0; i < k_NumOfCardsToChoose && !m_QuitGame; i++)
                {
                    validCardsToChoose = m_Logic.GetValidCardsList();
                    if (currentPlayer.PlayerType == Player.ePlayerType.Human)
                    {
                        makePlayerCardChoice(validCardsToChoose, currentPlayer.Name, ref currentMove);
                    }
                    else
                    {
                        makeComputersCardChoice(validCardsToChoose, ref currentMove);
                    }
                }

                if (!m_QuitGame)
                {
                    calculateMoveResult(currentMove);
                    validMovesLeft = m_Logic.CheckIfValidMovesLeft();
                }
            }

            if(!m_QuitGame)
            {
                printGameResult();
                m_AnotherRound = m_ConsoleUi.AskUserForAnotherRound();
                if(!m_AnotherRound)
                {
                    m_ConsoleUi.PrintGoodbyeMsg();
                }
                else
                {
                    m_Logic.SwitchTurnToMainPlayer(); 
                }
            }
            else
            {
                m_ConsoleUi.PrintGoodbyeMsg();
            }
        }

        private void initializeBoards()
        {
            int height, width;
            bool validBoardSize = true;
            const string k_invalidMsg = "Invalid Board Size (odd number of cells), please try again";

            do
            {
                height = m_ConsoleUi.GetBoardHeight();
                width = m_ConsoleUi.GetBoardWidth();
                Console.WriteLine();
                validBoardSize = validateBoardSize(height, width);
                if(!validBoardSize)
                {
                    Console.WriteLine(k_invalidMsg);
                }
            }
            while(!validBoardSize);

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
            m_Logic.SetBoard(i_Height, i_Width);
        }

        private void initializeUiBoard(int i_Height, int i_Width)
        {
            UiBoard board = new UiBoard(i_Height, i_Width);
            board.ShuffleCards();
            m_ConsoleUi.SetBoard(board);
        }

        private void calculateMoveResult(Move i_Move)
        {
            bool matchingCards = checkIfMatchingCards(i_Move);
            bool validMovesLeft;

            if (!matchingCards)
            {
                Console.WriteLine("Cards are not Matching");
                System.Threading.Thread.Sleep(2000);
                m_Logic.UndoMove(i_Move);
                m_Logic.SwitchPlayers();
                clearAndPrintBoard();
            }
            else
            {
                m_Logic.GivePointToCurrentPlayer();
                validMovesLeft = m_Logic.CheckIfValidMovesLeft();
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

            char firstCardValue = m_ConsoleUi.GetCardValue(firstCardLocation);
            char secondCardValue = m_ConsoleUi.GetCardValue(secondCardLocation);

            return firstCardValue.Equals(secondCardValue);
        }

        private void printGameResult()
        {
            Player winner = m_Logic.GetWinner();
            string[] playersNames = m_Logic.GetPlayersNames();
            int[] playersPoints = m_Logic.GetPlayersPoints();

            if (winner == null)
            {
                m_ConsoleUi.PrintTieMsg();
            }
            else
            {
                m_ConsoleUi.PrintWinnerMsg(winner.Name);
            }

            m_ConsoleUi.PrintPoints(playersNames, playersPoints);
        }

        private void initializePlayers()
        {
            string player1Name, player2Name;
            Player.ePlayerType player2Type;
            bool isPlayersTurn = true;

            player1Name = m_ConsoleUi.GetPlayerName("Player No. 1");
            m_Logic.AddPlayer(player1Name, Player.ePlayerType.Human, isPlayersTurn);

            player2Type = m_ConsoleUi.GetOpponentType(player1Name);
            if(player2Type == Player.ePlayerType.Computer)
            {
                player2Name = "Computer";
            }
            else
            {
                player2Name = m_ConsoleUi.GetPlayerName("Player No. 2");
            }

            m_Logic.AddPlayer(player2Name, player2Type, !isPlayersTurn);
        }

        private Move initializeMove(Player i_CurrentPlayer)
        {
            Move currentMove = new Move(i_CurrentPlayer);

            return currentMove;
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
            BoardCell[,] logicBoard = m_Logic.Board.Cells;
            m_ConsoleUi.PrintBoard(logicBoard);
        }

        private void revealCardInLocation(string i_LocationStr, ref Move i_CurrentMove)
        {
            string locationDigitsStr = convertLettersToDigits(i_LocationStr);
            Location cardLocation = m_Logic.GetLocationFromStr(locationDigitsStr);

            m_Logic.RevealCard(cardLocation);
            i_CurrentMove.SetLocation(cardLocation);
            clearAndPrintBoard();
        }

        private string convertLettersToDigits(string i_LocationStr)
        {
            char xCord = i_LocationStr[0];
            char yCord = i_LocationStr[1];
            int xCordNum = (int)(xCord - 'A') + 1;

            return string.Format("{0}{1}", xCordNum, yCord);
        }

        private string getValidCardChoiceFromComputer(List<string> i_ValidCards)
        {
            List<string> validCardsToChoose = i_ValidCards;
            validCardsToChoose.Remove("Q");
            Random numberGenerator = new Random();
            int chosenRandomIndex = numberGenerator.Next(validCardsToChoose.Count - 1);

            return validCardsToChoose[chosenRandomIndex];
        }

        private void makePlayerCardChoice(List<string> i_ValidCards, string i_PlayerName, ref Move i_CurrentMove)
        {
            string cardChoiceStr = m_ConsoleUi.GetValidCardChoiceFromPlayer(i_ValidCards, i_PlayerName);

            if(cardChoiceStr.Equals("Q"))
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

            m_ConsoleUi.PrintComputerMoveMsg();
            revealCardInLocation(cardChoiceStr, ref i_CurrentMove);
        }
    }
}
