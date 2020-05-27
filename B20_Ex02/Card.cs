using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    public class Card
    {
        private readonly char r_Value;

        public Card(char i_Value)
        {
            r_Value = i_Value;
        }

        public char CardValue
        {
            get
            {
                return r_Value;
            }
        }
    }
}
