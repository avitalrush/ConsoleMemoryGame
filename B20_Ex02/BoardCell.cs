using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    public class BoardCell
    {
        // MEMBERS:
        private Location m_cellLocation;
        private bool m_isHidden;

        // CTOR:
        public BoardCell()
        {
            m_cellLocation = new Location();
            m_isHidden = false;
        }

        // METHODS:

        public bool isHidden
        {
            get
            {
                return m_isHidden;
            }
            set
            {
                m_isHidden = value;
            }
        }
    }
}
