using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    public class Location // Shaked defined this class as public to solve problem when UiBoard tried to create object of this class
    {
        private int m_Row;
        private int m_Column;

        public Location(int i_Column, int i_Row)
        {
            this.m_Column = i_Column;
            this.m_Row = i_Row;
        }

        public int Column
        {
            get
            {
                return m_Column;
            }
        }

        public int Row
        {
            get
            {
                return m_Row;
            }
        }
    }
}
