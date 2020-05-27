using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace B20_Ex02
{
    public class UIBoard
    {
        // MEMBERS:
        private readonly int r_width, r_height;
        private Card[,] m_Cards;

        // CTOR:
        public UIBoard(int i_height, int i_width)
        {
            this.r_height = i_height;
            this.r_width = i_width;
            m_Cards = new Card[i_height, i_width];
        }

        // METHODS:
        // // get / set METHODS
        public int Width
        {
            get
            {
                return r_width;
            }
        }

        public int Height
        {
            get
            {
                return r_height;
            }
        }

        public char GetCardValue(Location i_LocationOnBoard)
        {
            return m_Cards[i_LocationOnBoard.Row, i_LocationOnBoard.Column].CardValue;
        }

        public void ShuffleCards()
        {
            int numOfUniqueValues = (Width * Height) / 2;

            char[] boardValues = new char[numOfUniqueValues];

            // if numOfUniqueValues = 4 , i need the letters A B C D
            List<char> boardLetters = getBoardLetters(numOfUniqueValues);

            List<char> shuffledLetters = getShuffledLetters(boardLetters);

            initializeBoardCards(shuffledLetters);
        }

        private void initializeBoardCards(List<char> i_BoardLetters)
        {
            int indexOfLetter = 0;
            for (int rowIndex = 0; rowIndex < Height; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < Width; columnIndex++)
                {
                    m_Cards[rowIndex, columnIndex] = new Card(i_BoardLetters[indexOfLetter++]);
                }
            }
        }

        private List<char> getShuffledLetters(List<char> i_OriginalLettersList)
        {
            int numOfLetters = i_OriginalLettersList.Count;
            List<char> shuffledLettersList = new List<char>();
            Random numberGenerator = new Random();

            for(int index = 0; index < numOfLetters; index++)
            {
                int chosenRandomIndex = numberGenerator.Next(i_OriginalLettersList.Count - 1);
                //Console.WriteLine("Random: {0} Setting value {1} in index {2}", chosenRandomIndex, i_OriginalLettersList[chosenRandomIndex], index);
                shuffledLettersList.Add(i_OriginalLettersList[chosenRandomIndex]); //[index] = i_OriginalLettersList[chosenRandomIndex];
                i_OriginalLettersList.RemoveAt(chosenRandomIndex);
            }

            return shuffledLettersList;
        }

        private List<char> getBoardLetters(int i_NumOfLetters)
        {
            string allLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string chosenLetters = allLetters.Substring(0, i_NumOfLetters);

            string boardLetters = chosenLetters + chosenLetters;

            return new List<char>(boardLetters.ToCharArray());
        }
    }
}
