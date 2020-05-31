using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    public class Logic
    {
        private readonly PlayersManager r_AllPlayers;
        private LogicBoard m_Board;

        public Logic()
        {
            r_AllPlayers = new PlayersManager();
        }

        public LogicBoard Board
        {
            get { return m_Board; }
        }

        public void SetBoard(int i_Height, int i_Width)
        {
            m_Board = new LogicBoard(i_Height, i_Width);
        }

        public Player GetCurrentPlayer()
        {
            return r_AllPlayers.GetCurrentPlayer;
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
                    if (boardCells[i, j].IsHidden)
                    {
                        cellLocationStr = CreateStrLocation(i, j);
                        validCards.Add(cellLocationStr);
                    }
                }
            }

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
            r_AllPlayers.SwitchPlayers();
        }

        public void GivePointToCurrentPlayer()
        {
            r_AllPlayers.GivePointToCurrentPlayer();
        }

        public Player GetWinner()
        {
            return r_AllPlayers.GetWinner();
        }

        public void AddPlayer(string i_Name, Player.ePlayerType i_PlayerType, bool i_IsPlayersTurn)
        {
            r_AllPlayers.CreatePlayer(i_Name, i_PlayerType, i_IsPlayersTurn);
        }

        public void RevealCard(Location i_CellLocation)
        {
            m_Board.RevealCard(i_CellLocation);
        }

        public Location GetLocationFromStr(string i_LocationStr)
        {
            Location cellLocation = new Location(getIndexFromLetter(i_LocationStr[0]), getIndexFromLetter(i_LocationStr[1]));

            return cellLocation;
        }

        private int getIndexFromLetter(char i_Letter)
        {
            return i_Letter - '0' - 1;
        }

        public void SwitchTurnToMainPlayer()
        {
            r_AllPlayers.SwitchTurnToMainPlayer();
        }

        public string[] GetPlayersNames()
        {
            return r_AllPlayers.GetPlayersNames();
        }

        public int[] GetPlayersPoints()
        {
            return r_AllPlayers.GetPlayersPoints();
        }

        public void ResetPlayersScore()
        {
            r_AllPlayers.ResetPlayersScore();
        }
    }
}
