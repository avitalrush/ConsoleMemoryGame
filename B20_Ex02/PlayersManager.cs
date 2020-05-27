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

        private void gameEndedInTie()
        {
            if (m_Player1.Points == m_Player2.Points)
            {
                m_EndedInTie = true;
            }
        }

        public Player WhoWonTheGame()
        {
            gameEndedInTie();
            if (!m_EndedInTie)
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

            return m_Winner;
        }

        public void GivePointToCurrentPlayer()
        {
            m_CurrentPlayer.Points++;
        }

        public void SwitchTurnToMainPlayer()
        {
            m_Player1.IsMyTurn = true;
            m_Player2.IsMyTurn = false;
        }

        public string[] GetPlayersNames()
        {
            string[] playersNames = new string[2];

            playersNames[0] = m_Player1.Name;
            playersNames[1] = m_Player2.Name;

            return playersNames;
        }

        public int[] GetPlayersPoints()
        {
            int[] playersPoints = new int[2];

            playersPoints[0] = m_Player1.Points;
            playersPoints[1] = m_Player2.Points;

            return playersPoints;
        }
    }
}
