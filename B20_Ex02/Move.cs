using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    public class Move
    {
        private Location m_FirstCardLocation;
        private Location m_SecondCardLocation;
        private readonly Player r_Player;

        public Move(Player i_Player)
        {
            this.r_Player = i_Player;
        }

        public void SetLocation(Location i_CardLocation)
        {
            if(m_FirstCardLocation == null)
            {
                m_FirstCardLocation = i_CardLocation;
            }
            else
            {
                m_SecondCardLocation = i_CardLocation;
            }
        }

        public Location GetLocationOfFirstCard()
        {
            return m_FirstCardLocation;
        }

        public Location GetLocationOfSecondCard()
        {
            return m_SecondCardLocation;
        }

        public Player GetPlayer()
        {
            return r_Player;
        }
    }
}
