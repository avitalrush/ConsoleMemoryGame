using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    public class Logic
    {
        private LogicBoard m_Board;
        private PlayersManager m_AllPlayers;

        public Logic()
        {
            m_AllPlayers = new PlayersManager();
        }

        public LogicBoard Board
        {
            get
            {
                return m_Board;
            }
        }

        public void SetBoard(int i_Height, int i_Width)
        {
            m_Board = new LogicBoard(i_Height, i_Width);
        }

        public Player GetCurrentPlayer()
        {
            return m_AllPlayers.GetCurrentPlayer;
        }

        public List<string> GetValidCardsList()
        {
            List<string> validCards = new List<string>();
            BoardCell[,] boardCells = m_Board.Cells;
            int height = m_Board.Height;
            int width = m_Board.Width;
            string cellLocationStr;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if(boardCells[i, j].isHidden)
                    {
                        cellLocationStr = CreateStrLocation(i, j);
                        validCards.Add(cellLocationStr);
                    }
                }
            }

            validCards.Add("Q");

            return validCards;
        }

        public string CreateStrLocation(int i_Row, int i_Column)
        {
            int intRow = i_Row + 1;
            int intColumn = 'A' + i_Column;
            char charColumn = Convert.ToChar(intColumn);

            return string.Format("{0}{1}", charColumn, intRow);
        }

        public bool CheckIfValidMovesLeft()
        {
            return GetValidCardsList().Count > 1;
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
            return m_AllPlayers.GetWinner();
        }

        public void AddPlayer(string i_Name, Player.ePlayerType i_PlayerType, bool i_IsPlayersTurn)
        {
            m_AllPlayers.CreatePlayer(i_Name, i_PlayerType, i_IsPlayersTurn);
        }

        public void RevealCard(Location i_CellLocation)
        {
            m_Board.RevealCard(i_CellLocation);
        }

        public Location GetLocationFromStr(string i_LocationStr)
        {
            Location cellLocation = new Location(i_LocationStr[0]- '0' - 1, i_LocationStr[1]- '0' - 1);

            return cellLocation;
        }

        public void SwitchTurnToMainPlayer()
        {
            m_AllPlayers.SwitchTurnToMainPlayer();
        }

        public string[] GetPlayersNames()
        {
            return m_AllPlayers.GetPlayersNames();
        }

        public int[] GetPlayersPoints()
        {
            return m_AllPlayers.GetPlayersPoints();
        }
    }
}
