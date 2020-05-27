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
            m_AllPlayers = new PlayersManager();
        }

        // METHODS:
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
            // method creates and returns validMovesList + concatenates "Q" string

            List<string> validMoves = new List<string>();
            BoardCell[,] tempCells = m_Board.Cells;
            int width = m_Board.Width;
            int height = m_Board.Height;
            string strIndex;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
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

        public string CreateStringIndex(int i_Row, int i_Column)
        {
            // method gets i=5, j=4 --> returns "E4"

            int intRow = i_Row + 1;
            int intColumn = 'A' + i_Column;
            char charColumn = Convert.ToChar(intColumn);

            //Console.WriteLine("strIndex: {0}", String.Format("{0}{1}", charColumn, strRow));
            return String.Format("{0}{1}", charColumn, intRow);
        }

        public bool CheckIfValidMovesLeft()
        {
            // if there's only one item in validMovesList --> it can be only 'Q' --> the game ends
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

        public Location GetCellLocation(string i_StrLocation)
        {
            Location cellLocation = new Location(i_StrLocation[0]-'0' - 1, i_StrLocation[1]-'0' - 1);
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
