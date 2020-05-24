using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    class Logic
    {
        // MEMBERS:
        private LogicBoard m_Board;
        private PlayersManager m_AllPlayers;

        // CTOR:
        public Logic()
        {
            
        }

        // METHODS:

        public LogicBoard Board
        {
            get
            {
                return m_Board;
            }
        }

        public void SetBoard(LogicBoard i_Board)
        {
            m_Board = i_Board;
        }

        public Player getCurrentPlayer()
        {
            return m_AllPlayers.getCurrentPlayer();
        }

        public List<string> getValidMovesList()
        {
            // method creates and returns validMovesList + concatenates "Q" string

            List<string> validMoves = new List<string>();
            BoardCell[,] tempCells = m_Board.Cells;
            int width = m_Board.Width;
            int height = m_Board.Height;
            string strIndex;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if(tempCells[i, j].isHidden)
                    {
                        strIndex = createStringIndex(i, j);
                        validMoves.Add(strIndex);
                    }
                }
            }
            validMoves.Add("Q");

            return validMoves;
        }

        private string createStringIndex(int i_column, int i_row)
        {
            // method gets i=E, j=4 --> returns "E4"

            string strColumn = i_column.ToString();
            string strRow = i_row.ToString();
            string strIndex = string.Concat(strColumn, strRow);
            return strIndex;
        }

        public bool checkIfValidMovesLeft()
        {
            List<string> validMoves = getValidMovesList();
            return validMoves.Count == 0;
        }

        public void undoMove(Move i_Move)
        {
            Location locationOfFirstCard = i_Move.getLocationOfFirstCard();
            Location locationOfSecondCard = i_Move.getLocationOfSecondCard();

            m_Board.HideCard(locationOfFirstCard);
            m_Board.HideCard(locationOfSecondCard);
        }

        public void switchPlayers()
        {
            m_AllPlayers.switchPlayers();
        }

        public void givePointToCurrentPlayer()
        {
            m_AllPlayers.givePointToCurrentPlayer();

        public Player GetWinner()
        {
            return m_AllPlayers.WhoWonTheGame();
        }

        public void AddPlayer(string i_Name, Player.ePlayerType i_PlayerType, bool i_IsItMyTurn)
        {
            m_AllPlayers.CreatePlayer(i_Name, i_PlayerType, i_IsItMyTurn);
        }

        public void givePoint()
        {
            m_AllPlayers.GivePointTo();
        }

        public Location getCellLocation(string i_PlayerMoveStr)
        {
            return m_Board.revealCardInCell(i_PlayerMoveStr);
        }
    }
}
