using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    public class Location // Shaked defined this class as public to solve problem when UIBoard tried to create object of this class
    {
        private int column;
        private int row;

        public Location()
        {
            
        }

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
