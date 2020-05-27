using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    public class Player
    {
        private string m_PlayerName;
        private int m_Points;
        private bool m_IsMyTurn;
        private ePlayerType m_PlayerType;

        public Player(string i_Name, ePlayerType i_Type, bool i_IsPlayersTurn)
        {
            this.m_PlayerName = i_Name;
            this.m_Points = 0;
            this.m_IsMyTurn = i_IsPlayersTurn;
            this.m_PlayerType = i_Type;
        }

        public enum ePlayerType
        {
            Human,
            Computer
        }

        public string Name
        {
            get { return m_PlayerName; }

            set { m_PlayerName = value; }
        }

        public int Points
        {
            get { return m_Points; }
            
            set { m_Points = value; }
        }

        public ePlayerType PlayerType
        {
            get { return m_PlayerType; }

            set { m_PlayerType = value; }
        }

        public bool IsMyTurn
        {
            get { return m_IsMyTurn; }

            set { m_IsMyTurn = value; }
        }
    }
}
