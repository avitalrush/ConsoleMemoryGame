using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    public class PlayersManager
    {
        public Player m_Player1;
        public Player m_Player2;
        private Player m_CurrentPlayer;
        private Player m_Winner;
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
            if (m_CurrentPlayer == m_Player1)
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

        public Player GetCurrentPlayer
        {
            get { return m_CurrentPlayer; }

            set { m_CurrentPlayer = value; }
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
