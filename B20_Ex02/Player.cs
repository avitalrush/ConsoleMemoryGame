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
        private Enum m_PlayerType;

        public Player(string i_Name,Enum i_Type,bool i_IsItMyTurn)
        {
            m_PlayerName = i_Name;
            m_Points = 0;
            m_PlayerType = i_Type;
            m_IsMyTurn = i_IsItMyTurn;
        }

        public int Points
        {
            get { return m_Points; }
            set { m_Points = value; }
        }

        public bool IsMyTurn
        {
            get { return m_IsMyTurn; }
            set { m_IsMyTurn = value; }
        }
    }
}
