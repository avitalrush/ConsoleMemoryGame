using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace B20_Ex02
{
    class UI
    {
        private UIBoard uiBoard;
        private char AskUserForAnotherRound()
        {
            char userDesicion;
            Console.WriteLine("Do you want to play another round? Y for Yes, N for No : ");
            userDesicion = char.Parse(Console.ReadLine());
            do
            {
                if(!(userDesicion == 'Y' || userDesicion == 'N'))
                {
                    Console.WriteLine("Please enter only Y for Yes, N for No : ");
                    userDesicion = char.Parse(Console.ReadLine());
                }
            }
            while(!(userDesicion == 'Y' || userDesicion == 'N'));

            return userDesicion;
        }

        private string getValidMoveFromUser()
        {
            string userMoveStr;
            Console.WriteLine("Choose a card : ");
            userMoveStr = Console.ReadLine();

            return userMoveStr;
        }

        private void printBoard(BoardCell[,] i_LogicBoardCells)
        {
            char column = 'A';
            int i = 0, j = 0;

            StringBuilder firstLine = new StringBuilder();
            StringBuilder seperationRow = new StringBuilder();
            StringBuilder boardRow = new StringBuilder();
            StringBuilder fullBoard = new StringBuilder();

            firstLine.Append("    ");
            for (i = 1; i <= uiBoard.Height; i++)
            {
                firstLine.Append(column + i);
                firstLine.Append("  ");
            }

            fullBoard.AppendLine(firstLine.ToString());

            seperationRow.Append("   ");
            for (i = 1; i <= 4*uiBoard.Width+1; i++)
            {
                seperationRow.Append('=');
            }

            fullBoard.AppendLine(seperationRow.ToString());


            for (i = 1; i <= uiBoard.Height; i++)
            {
                boardRow.Remove(0, 4 * uiBoard.Width + 2);
                boardRow.Append(i);
                boardRow.Append(' ');
                boardRow.Append(|);
                for (j = 1; j < uiBoard.Width; j++)
                {
                    boardRow.Append(' ');
                    if(i_LogicBoardCells[i - 1, j - 1].m_IsHidden)
                    {
                        boardRow.Append(' ');
                    }
                    else
                    {
                        boardRow.Append(uiBoard.m_Cards[i - 1, j - 1]);
                    }

                }
                fullBoard.AppendLine(boardRow.ToString());
                fullBoard.AppendLine(seperationRow.ToString());
            }
        }
    }
}
