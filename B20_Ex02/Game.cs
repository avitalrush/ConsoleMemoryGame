using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    class Game
    {
        // MEMBERS:
        //private const int k_NumOfPlayers = 2;
        private bool m_AnotherRound = true;
        private bool m_QuitGame = false;
        private Logic m_Logic;
        private UI m_Ui;

        // CTOR:
        public Game()
        {
            m_Logic = new Logic();
            m_Ui = new UI();
        }

        // METHODS:
        public void Start()
        {
            initializePlayers();
            while (m_AnotherRound && !m_QuitGame)
            {
                Run();
            }
        }

        public void Run()
        {
            Player currentPlayer;
            Player winner;
            Move currentMove;
            Card currentCard;
            //LogicBoard logicBoard;
            //UIBoard uiBoard;
            bool validMovesLeft = true;
            List<string> validMoves;
            string moveStr;

            /////////////////////////////////////////////////////////////

            initializeBoards();
            printBoard();   // Game's method, who is a mediator --> asks Logic for his LogicBoard and sends it to UI for printing

            while (validMovesLeft && !m_QuitGame)
            {
                currentPlayer = m_Logic.GetCurrentPlayer();
                currentMove = initMove(currentPlayer);

                for (int i = 0; i < 2 && !m_QuitGame; i++)       // this is a move for one player, it has 2 parts (2 cards)
                {
                    validMoves = m_Logic.GetValidMovesList();                                 // gets validMovesList, creates it from logicBoard + concatenates "Q" string

                    //////// here we need cases in case currentPlayer.type == human --> continue like this, 

                    moveStr = m_Ui.GetValidMoveFromUser(validMoves);                   // get move from user and checks validMoves for validity

                    if (moveStr.Equals("Q"))                                           // if user input is Q --> quit
                    {
                        m_QuitGame = true;
                    }
                    else                                                                    // else - user input is surly VALID and we can continue
                    {
                        makeValidMove(moveStr,ref currentMove);         //change the name of the method because it's confusing, here it's only half of the move
                    }
                }

                if(!m_QuitGame)
                {
                    calculateMoveResult(currentMove);
                    validMovesLeft = m_Logic.CheckIfValidMovesLeft();
                }
            }

            printGameResult();

            m_AnotherRound = m_Ui.askUserForAnotherRound();
        }

        private void initializeBoards()
        {
            int width, height;
            bool validBoardSize = true;
            const string k_invalidMsg = "Invalid Board Size (odd number of cells)";

            do
            {
                width = m_Ui.GetBoardWidth();
                height = m_Ui.GetBoardHeight();
                validBoardSize = validateBoardSize(width, height);

                if(!validBoardSize)
                {
                    Console.WriteLine(k_invalidMsg);
                }
            }
            while(!validBoardSize);
            
            InitializeLogicBoard(width, height);
            InitializeUIBoard(width, height);
        }

        private bool validateBoardSize(int i_width, int i_height)
        {
            int numOfCells = i_width * i_height;
            return numOfCells % 2 == 0;
        }

        private void InitializeLogicBoard(int i_Width, int i_Height)
        {
            LogicBoard board = new LogicBoard(i_Width, i_Height);
            m_Logic.SetBoard(board);
            //m_Logic.SetBoard(i_Width, i_Height);
        }

        private void InitializeUIBoard(int i_Width, int i_Height)
        {
            UIBoard board = new UIBoard(i_Width, i_Height);
            //m_Ui.SetBoard(i_Width, i_Height);
            shuffelValuesIntoBoard(ref board);
            m_Ui.SetBoard(board);
        }

        private void shuffelValuesIntoBoard(ref UIBoard io_Board)
        {
            int width = io_Board.Width;
            int height = io_Board.Height;
            int numOfUniqueValues = (width * height) / 2;
            char[] boardValues = new char[numOfUniqueValues];


            // if numOfUniqueValues = 4 , i need the letters A B C D
            for (int i = 0; i < numOfUniqueValues; i++)
            {
                int temp = 65 + i;
                boardValues[numOfUniqueValues] = '0' + temp;
            }
        }

        private void calculateMoveResult(Move i_Move)
        {
            bool matchingCards = checkIfMatchingCards(i_Move);

            if (!matchingCards)
            {
                System.Threading.Thread.Sleep(2000);
                m_Logic.UndoMove(i_Move);
                m_Logic.SwitchPlayers();
                clearAndPrintBoard();
            }
            else
            {
                m_Logic.GivePointToCurrentPlayer();
            }
        }

        private bool checkIfMatchingCards(Move i_Move)
        {
            Location locationOfFirstCard = i_Move.GetLocationOfFirstCard();
            Location locationOfSecondCard = i_Move.GetLocationOfSecondCard();

            char firstValue = m_Ui.GetCardValue(locationOfFirstCard);
            char secondValue = m_Ui.GetCardValue(locationOfSecondCard);

            return firstValue.Equals(secondValue);
        }

        private void printGameResult()
        {
            Player winnerPlayer = m_Logic.GetWinner(); //להשתמש בשדה לוג'יק שאביטל יצרה
            if (!m_QuitGame)
            {
                UI.printWinnerMessage(winnerPlayer);
            }
            else
            {
                UI.printGoodByeMessage();
            }
        }

        private void initializePlayers()
        {
            string player1Name, player2Name;
            Player.ePlayerType player2Type;
            player1Name = m_Ui.GetPlayerName("Player no. 1");
            m_Logic.AddPlayer(player1Name, "Human", true);
            player2Type = m_Ui.GetOpponentType(player1Name);
            if(player2Type == "Computer")
            {
                player2Name = "Computer";
            }
            else
            {
                player2Name = m_Ui.GetPlayerName("Player no. 1");
            }
            m_Logic.AddPlayer(player2Name, player2Type, false);
        }

        private Move initMove(Player i_CurrentPlayer)
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
            m_Ui.PrintBoard(logicBoard);
        }

        private void makeValidMove(string i_PlayerMoveStr, ref Move currentMove)
        {
            Location cellLocation = m_Logic.GetCellLocation(i_PlayerMoveStr);
            m_Logic.RevealCard(cellLocation);
            currentMove.SetLocation(cellLocation);
            clearAndPrintBoard();
        }
}
