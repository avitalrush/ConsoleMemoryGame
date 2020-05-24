using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    class Logic
    {
        private LogicBoard m_Board;
        private PlayersManager m_AllPlayers;
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
