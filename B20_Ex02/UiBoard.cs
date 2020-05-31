using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace B20_Ex02
{
    public class UiBoard
    {
        private readonly int r_Width, r_Height;
        private readonly Card[,] r_Cards;

        public UiBoard(int i_Height, int i_Width)
        {
            this.r_Height = i_Height;
            this.r_Width = i_Width;
            r_Cards = new Card[i_Height, i_Width];
        }

        public int Width
        {
            get { return r_Width; }
        }

        public int Height
        {
            get { return r_Height; }
        }

        public char GetCardValue(Location i_CardsLocationOnBoard)
        {
            return r_Cards[i_CardsLocationOnBoard.Row, i_CardsLocationOnBoard.Column].CardValue;
        }

        public void ShuffleCards()
        {
            int numOfUniqueValues = (Height * Width) / 2;
            char[] boardValues = new char[numOfUniqueValues];

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
                    r_Cards[rowIndex, columnIndex] = new Card(i_BoardLetters[indexOfLetter++]);
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
                shuffledLettersList.Add(i_OriginalLettersList[chosenRandomIndex]);
                i_OriginalLettersList.RemoveAt(chosenRandomIndex);
            }

            return shuffledLettersList;
        }

        private List<char> getBoardLetters(int i_NumOfLetters)
        {
            const string k_AllLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string chosenLetters = k_AllLetters.Substring(0, i_NumOfLetters);
            string boardLetters = string.Concat(chosenLetters, chosenLetters);
            List<char> boardLettersList = new List<char>(boardLetters.ToCharArray());

            return boardLettersList;
        }
    }
}
