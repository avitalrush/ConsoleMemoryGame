using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    public class Location
    {
        private readonly int r_Row;
        private readonly int r_Column;

        public Location(int i_Column, int i_Row)
        {
            this.r_Column = i_Column;
            this.r_Row = i_Row;
        }

        public int Column
        {
            get { return r_Column; }
        }

        public int Row
        {
            get { return r_Row; }
        }
    }
}
