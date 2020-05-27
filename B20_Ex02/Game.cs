using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    public class Game
    {
        // MEMBERS:
        //private const int k_NumOfPlayers = 2;     // Think how to name this const, instead of using the number 2 in the for loop, we should use some const. also - should it be a const member or can it be a const variable inside 'run' method?
        private bool m_AnotherRound = true;
        private bool m_QuitGame = false;
        private Logic m_Logic;                      // Suggestion: make member read_only
        private UI m_Ui;                            // Suggestion: make member read_only

        // CTOR:
        public Game()                               // Should we initialize here also the members m_AnotherRound and m_QuitGame ?
        {
            m_Logic = new Logic();
            m_Ui = new UI();
        }

        // METHODS:
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

            initializeBoards();
            printBoard();                                                            // Game asks Logic for his LogicBoard and sends it to UI for printing

            while(validMovesLeft && !m_QuitGame)
            {
                currentPlayer = m_Logic.GetCurrentPlayer();
                currentMove = initMove(currentPlayer);

                for(int i = 0; i < 2 && !m_QuitGame; i++)                           // This is a move for one player & it has 2 parts (2 cards) --> THINK OF A CONST NAME TO REPLACE 2
                {
                    validCardsToChoose = m_Logic.GetValidMovesList();               // Creates validMovesList from logicBoard + concatenates "Q" string

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
                    //Console.WriteLine("Valid moves left? {0}",validMovesLeft);
                }
            }

            if(!m_QuitGame)      // if the user didn't quit the game but the game ended normally
            {
                printGameResult();
                m_AnotherRound = m_Ui.AskUserForAnotherRound();
                if(!m_AnotherRound) // if the player doesn't want another round
                {
                    m_Ui.PrintGoodbyeMsg();
                }
                else
                {
                    m_Logic.SwitchTurnToMainPlayer(); 
                }
            }
            else // the game ended because the player quited
            {
                m_Ui.PrintGoodbyeMsg();
            }
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

            initializeLogicBoard(height, width);
            initializeUiBoard(height, width);
        }

        private bool validateBoardSize(int i_width, int i_height)
        {
            int numOfCells = i_width * i_height;
            return numOfCells % 2 == 0;
        }

        private void initializeLogicBoard(int i_Height, int i_Width)
        {
            m_Logic.SetBoard(i_Height, i_Width);
        }

        private void initializeUiBoard(int i_Height, int i_Width)
        {
            UIBoard board = new UIBoard(i_Height, i_Width);
            board.ShuffleCards();
            m_Ui.SetBoard(board);
        }

        private void calculateMoveResult(Move i_Move)
        {
            bool matchingCards = checkIfMatchingCards(i_Move);

            if(!matchingCards)
            {
                Console.WriteLine("Cards are not Matching :(");
                System.Threading.Thread.Sleep(2000);
                m_Logic.UndoMove(i_Move);
                m_Logic.SwitchPlayers();
                clearAndPrintBoard();
            }
            else
            {
                m_Logic.GivePointToCurrentPlayer();
                bool validMovesLeft = m_Logic.CheckIfValidMovesLeft();
                if(validMovesLeft)
                {
                    Console.WriteLine("It's a Match! {0}, yot get another turn", i_Move.GetPlayer().Name);
                }
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
            // if there's a winner in the game -
            Player winner = m_Logic.GetWinner();
            m_Ui.PrintWinnerMsg(winner.Name);

            // also, print points state -
            string[] playersNames = m_Logic.GetPlayersNames();
            int[] playersPoints = m_Logic.GetPlayersPoints();
            m_Ui.PrintPoints(playersNames, playersPoints);
        }

        private void initializePlayers()
        {
            string player1Name, player2Name;
            int player2TypeInt;
            Player.ePlayerType player2Type;

            player1Name = m_Ui.GetPlayerName("Player no. 1");
            m_Logic.AddPlayer(player1Name, Player.ePlayerType.Human, true);
            player2TypeInt = m_Ui.GetOpponentType(player1Name);
            player2Type = (Player.ePlayerType)player2TypeInt;

            if (player2TypeInt == 1)
            {
                player2Name = "Computer";
            }
            else
            {
                player2Name = m_Ui.GetPlayerName("Player no. 2");
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

        private void makeValidCardReveal(string i_PlayerMoveStr, ref Move i_CurrentMove)
        {
            // method recieved i_PlayerMoveStr = A3
            // in order to use it as board location we need to convert it to 13 (Column 1, Row 3)

            string digitsLocation = convertLettersToDigits(i_PlayerMoveStr);
            Location cellLocation = m_Logic.GetCellLocation(digitsLocation);

            m_Logic.RevealCard(cellLocation);
            i_CurrentMove.SetLocation(cellLocation);
            clearAndPrintBoard();
        }

        private string convertLettersToDigits(string i_strLocation)
        {
            char xCord = i_strLocation[0];
            char yCord = i_strLocation[1];
            int xCordNum = (int)(xCord - 'A')+1;        // convert 'A' to 1

            //Console.Write("Converted Value: {0}{1}", xCordNum, yCord);
            return String.Format("{0}{1}", xCordNum,yCord);
        }

        private string getValidCardChoiceFromComputer(List<string> i_ValidMoves)
        {
            // can the computer choose Q ? probably not. so inside GetValidMoveFromComputer(validMoves) we need to remove the Q frm the valid list move
            // need to add AI for the computer

            List<string> computerValidMoves = i_ValidMoves;
            computerValidMoves.Remove("Q");

            Random numberGenerator = new Random();
            int chosenRandomIndex = numberGenerator.Next(computerValidMoves.Count - 1);

            return computerValidMoves[chosenRandomIndex];
        }

        private void makePlayerCardChoice(List<string> i_ValidCards, string i_PlayerName, ref Move i_CurrentMove)
        {
            string currentCardChoiceStr = m_Ui.GetValidCardChoiceFromPlayer(i_ValidCards, i_PlayerName);        // Get move from user and checks validMoves for validity, should we change the variable name to userMoveStr ?

            if(currentCardChoiceStr.Equals("Q"))                        // If user input is Q --> want to quit the game
            {
                m_QuitGame = true;
            }
            else                                                        // Else - the user's input is surly VALID (UI checked it) and we can continue
            {
                makeValidCardReveal(currentCardChoiceStr, ref i_CurrentMove);            // Change the name of the method, here it's only HALF of the move. maybe chooseValidCard ?
            }

        }

        private void makeComputersCardChoice(List<string> i_ValidCards, ref Move i_CurrentMove)
        {
            string currentCardChoiceStr = getValidCardChoiceFromComputer(i_ValidCards);

            Console.WriteLine("Computer is making it's move...");
            System.Threading.Thread.Sleep(2000);

            makeValidCardReveal(currentCardChoiceStr, ref i_CurrentMove);
        }
    }
}
