using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    class Game
    {
        private bool anotherRound = true;
        private bool quitGame = false;
        private Move newMove;
        private Logic m_Logic;
        

        public void Start()
        {
            InitializePlayers();

            while(anotherRound)
            {
                Run();
            }
        }

        void Run()
        {
            Player currentPlayer;
            Move currentMove;
            string currentCellStr;
            List<string> validMoves;
            Card currentCard;
            bool validMovesLeft;
            Player winner;

            InitializeBoards();         // and print init logic board

            while(validMovesLeft && !quitGame)
            {
                currentPlayer = Logic.getCurrentPlayer();
                currentMove = initMove(currentPlayer);

                for(int i = 0; i < 2 && !quitGame; i++)
                {
                    validMoves = Logic.getValidMovesList();                                 // gets validMovesList, creates it from logicBoard + concatenates "Q" string
                    currentCellStr = UI.getValidMoveFromUser(validMoves);                   // get move from user and checks validMoves for validity

                    if (currentCellStr.Equals("Q"))                                         // if user input is Q --> quit
                    {
                        quitGame = true;
                    }
                    else                                                                    // else - user input is surly VALID and we can continue
                    {
                        makeValidMove(currentCellStr);
                        // ^^^^^^^^^
                        //Location cellLocation = Logic.revealCardInCell(currentCellStr);     // reveal the card and get his location on board
                        //currentMove.setCardLocation(cellLocation);                          // update card's location in move
                        //clearAndPrintBoard();                                               // game's method, is the mediator between logic and UI boards
                    }
                }

                if(!quitGame)
                {
                    calculateMoveResult(currentMove);
                    /*
                     ^^^^^^^^^^
                    if (!currentMove.matchingCards())                                           // launch Move's method matchingCards who checks in UIBoard according to locations
                    {
                        //sleep(2);
                        Logic.undoMove(currentMove);
                        Logic.switchPlayer(currentPlayer);
                        clearAndPrintBoard();
                    }
                    else
                    {
                        Logic.givePoint(currentPlayer);
                    }
                    */

                    validMovesLeft = Logic.checkIfValidMovesLeft();
                }
               
            }

            
            playAnotherRound = UI.askUserForAnotherRound();
            if(!playAnotherRound)
            {
                anotherRound = false;
            }
        }

        private void printGameResult()
        {
            Player winnerPlayer = newLogic.GetWinner(); //להשתמש בשדה לוג'יק שאביטל יצרה
            if (!quitGame)
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
            string player1Name, player2Name, player2Type;
            player1Name = UI.getPlayerName();
            newLogic.addPlayer(player1Name, "Human", true);
            player2Type = UI.getOpponentType();
            newLogic.addPlayer(player2Name, player2Type, false);
        }

        private void initMove(Player i_CurrentPlayer)
        {
            newMove=new Move(i_CurrentPlayer);
        }

        public void clearAndPrintBoard()
        {
            clearBoard();
            printBoard();
        }

        public void clearBoard()
        {
            Ex02.ConsoleUtils.Screen.Clear();
        }

        public void printBoard()
        {
            BoardCell[,] logicBoard = m_Logic.m_Board.Cells();
            UI.printBoard(logicBoard);
        }

        private void MakeValidMove(string i_PlayerMoveStr)
        {
            Location cellLocation = m_Logic.getCellLocation(i_PlayerMoveStr);
            newMove.SetLocation(cellLocation);
            clearAndPrintBoard();
        }
}
