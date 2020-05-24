using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    class Player
    {
        private string m_PlayerName;
        private int m_Points;
        private bool m_IsMyTurn;
        private ePlayerType m_PlayerType;

        public Player(string i_Name,ePlayerType i_Type,bool i_IsItMyTurn)
        {
            this.m_PlayerName = i_Name;
            this.m_Points = 0;
            this.m_PlayerType = i_Type;
            this.m_IsMyTurn = i_IsItMyTurn;
        }
        public enum ePlayerType
        {
            Human,
            Computer
        };
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
