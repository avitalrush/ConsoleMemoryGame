using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    public class BoardCell
    {
        private bool m_IsHidden;

        public BoardCell()
        {
            m_IsHidden = true;
        }

        public bool isHidden
        {
            get
            {
                return m_IsHidden;
            }

            set
            {
                m_IsHidden = value;
            }
        }
    }
}
