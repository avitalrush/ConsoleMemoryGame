using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    class Location
    {
        private int column;
        private int row;

        public Location(int i_Column, int i_Row)
        {
            this.column = i_Column;
            this.row = i_Row;
        }
        public int Column
        {
            get
            {
                return column;
            }
        }
        public int Row
        {
            get
            {
                return row;
            }
        }
    }
}
