using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    public class Logic
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

        public Player GetCurrentPlayer()
        {
            return m_AllPlayers.GetCurrentPlayer;
        }

        public List<string> GetValidMovesList()
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
                        strIndex = CreateStringIndex(i, j);
                        validMoves.Add(strIndex);
                    }
                }
            }

            validMoves.Add("Q");

            return validMoves;
        }

        public string CreateStringIndex(int i_column, int i_row)
        {
            // method gets i=E, j=4 --> returns "E4"

            string strColumn = i_column.ToString();
            string strRow = i_row.ToString();
            string strIndex = string.Concat(strColumn, strRow);
            return strIndex;
        }

        public bool CheckIfValidMovesLeft()
        {
            List<string> validMoves = GetValidMovesList();
            return validMoves.Count == 0;
        }

        public void UndoMove(Move i_Move)
        {
            Location locationOfFirstCard = i_Move.GetLocationOfFirstCard();
            Location locationOfSecondCard = i_Move.GetLocationOfSecondCard();

            m_Board.HideCard(locationOfFirstCard);
            m_Board.HideCard(locationOfSecondCard);
        }

        public void SwitchPlayers()
        {
            m_AllPlayers.SwitchPlayers();
        }

        public void GivePointToCurrentPlayer()
        {
            m_AllPlayers.GivePointToCurrentPlayer();
        }

        public Player GetWinner()
        {
            return m_AllPlayers.WhoWonTheGame();
        }

        public void AddPlayer(string i_Name, Player.ePlayerType i_PlayerType, bool i_IsItMyTurn)
        {
            m_AllPlayers.CreatePlayer(i_Name, i_PlayerType, i_IsItMyTurn);
        }

        public void RevealCard(Location i_CellLocation)
        {
            m_Board.RevealCard(i_CellLocation);
        }

        public Location GetCellLocation(string i_PlayerMoveStr)
        {
            Location cellLocation = new Location(i_PlayerMoveStr[0], i_PlayerMoveStr[1]);
            return cellLocation;
        }
    }
}
