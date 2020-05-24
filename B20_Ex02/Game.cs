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
            InitializePlayers();
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

            InitializeBoards();
            printBoard();   // Game's method, who is a mediator --> asks Logic for his LogicBoard and sends it to UI for printing

            while (validMovesLeft && !m_QuitGame)
            {
                currentPlayer = m_Logic.getCurrentPlayer();
                currentMove = initMove(currentPlayer);

                for (int i = 0; i < 2 && !m_QuitGame; i++)       // this is a move for one player, it has 2 parts (2 cards)
                {
                    validMoves = m_Logic.getValidMovesList();                                 // gets validMovesList, creates it from logicBoard + concatenates "Q" string

                    //////// here we need cases in case currentPlayer.type == human --> continue like this, 

                    moveStr = m_Ui.getValidMoveFromUser(validMoves);                   // get move from user and checks validMoves for validity

                    if (moveStr.Equals("Q"))                                           // if user input is Q --> quit
                    {
                        m_QuitGame = true;
                    }
                    else                                                                    // else - user input is surly VALID and we can continue
                    {
                        makeValidMove(moveStr);         //change the name of the method because it's confusing, here it's only half of the move
                    }
                }

                if(!m_QuitGame)
                {
                    calculateMoveResult(currentMove);
                    validMovesLeft = m_Logic.checkIfValidMovesLeft();
                }
            }

            printGameResult();

            m_AnotherRound = m_Ui.askUserForAnotherRound();
        }

        void InitializeBoards()
        {
            int width, height;
            bool validBoardSize = true;
            const string k_invalidMsg = "Invalid Board Size (odd number of cells)";

            do
            {
                width = m_Ui.getBoardWidth();
                height = m_Ui.getBoardHeight();
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

        bool validateBoardSize(int i_width, int i_height)
        {
            int numOfCells = i_width * i_height;
            return numOfCells % 2 == 0;
        }

        void InitializeLogicBoard(int i_Width, int i_Height)
        {
            LogicBoard board = new LogicBoard(i_Width, i_Height);
            m_Logic.SetBoard(board);
            //m_Logic.SetBoard(i_Width, i_Height);
        }

        void InitializeUIBoard(int i_Width, int i_Height)
        {
            UIBoard board = new UIBoard(i_Width, i_Height);
            //m_Ui.SetBoard(i_Width, i_Height);
            shuffelValuesIntoBoard(ref board);
            m_Ui.SetBoard(board);
        }

        void shuffelValuesIntoBoard(ref UIBoard io_Board)
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

        void calculateMoveResult(Move i_Move)
        {
            bool matchingCards = checkIfMatchingCards(i_Move);

            if (!matchingCards)
            {
                System.Threading.Thread.Sleep(2000);
                m_Logic.undoMove(i_Move);
                m_Logic.switchPlayers();
                clearAndPrintBoard();
            }
            else
            {
                m_Logic.givePointToCurrentPlayer();
            }
        }

        bool checkIfMatchingCards(Move i_Move)
        {
            Location locationOfFirstCard = i_Move.getLocationOfFirstCard();
            Location locationOfSecondCard = i_Move.getLocationOfSecondCard();

            char firstValue = m_Ui.getCardValue(locationOfFirstCard);
            char secondValue = m_Ui.getCardValue(locationOfSecondCard);

            return firstValue.Equals(secondValue);
        }
    }
}
