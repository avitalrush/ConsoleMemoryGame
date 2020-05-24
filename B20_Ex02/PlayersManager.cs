using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    class PlayersManager
    {
        public Player m_Player1;
        public Player m_Player2;
        public Player m_CurrentPlayer;
        public Player m_Winner;
        private bool m_EndedInTie = false;

        public void CreatePlayer(string i_Name, Player.ePlayerType i_PlayerType, bool i_IsItMyTurn)
        {
            if (m_Player1 == null)
            {
                m_Player1 = new Player(i_Name, i_PlayerType, i_IsItMyTurn);
                m_CurrentPlayer = m_Player1;
            }
            else
            {
                m_Player2 = new Player(i_Name, i_PlayerType, i_IsItMyTurn);
            }
        }

        public void SwitchPlayers()
        {
            if(m_CurrentPlayer == m_Player1)
            {
                m_Player1.IsMyTurn = false;
                m_Player2.IsMyTurn = true;
                m_CurrentPlayer = m_Player2;
            }
            else
            {
                m_Player2.IsMyTurn = false;
                m_Player1.IsMyTurn = true;
                m_CurrentPlayer = m_Player1;
            }
        }
        public void GameEndedInTie()
        {
            if (m_Player1.Points == m_Player2.Points)
            {
                m_EndedInTie = true;
            }
        }

        public void GetWinner()
        {
            GameEndedInTie();
            if(!m_EndedInTie)
            {
                if (m_Player1.Points > m_Player2.Points)
                {
                    m_Winner = m_Player1;
                }
                else
                {
                    m_Winner = m_Player2;
                }
            }
            else
            {
                m_Winner = null;
            }
        }
        public Player WhoWonTheGame()
        {
            GetWinner();

            return m_Winner;
        }

        public void GivePointTo()
        {
            m_CurrentPlayer.Points++;
        }
    }
}
