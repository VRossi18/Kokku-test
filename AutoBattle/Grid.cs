using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static AutoBattle.Types;

namespace AutoBattle
{
    public class Grid
    {
        public List<GridBox> grids = new List<GridBox>();
        public int xLenght;
        public int yLength;
        //Adding enemy and player positions to the grid so both of the characters are marked on the screen
        public GridBox playerPosition;
        public GridBox enemyPosition;
        public Grid(int Lines, int Columns)
        {
            xLenght = Lines;
            yLength = Columns;
            Console.WriteLine("The battle field has been created\n");
            for (int i = 0; i < Lines; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    GridBox newBox = new GridBox(j, i, false, (Columns * i + j));
                    Console.Write($"{newBox.Index}\n");
                    grids.Add(newBox);
                }
            }
        }

        // prints the matrix that indicates the tiles of the battlefield
        public void drawBattlefield(int Lines, int Columns)
        {
            for (int i = 0; i < Lines; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    //Checking if the enemy or player positions and making sure they are ocupied
                    bool aux = false;
                    if (playerPosition.xIndex == i && playerPosition.yIndex == j || enemyPosition.xIndex == i && enemyPosition.yIndex == j)
                    {
                        aux = true;
                    }
                    //Usnig the constructor and making sure all values are begin filled
                    GridBox currentgrid = new GridBox(i, j, aux, (Columns * i + j));
                    if (currentgrid.ocupied)
                    {
                        Console.Write("[X]\t");
                    }
                    else
                    {
                        Console.Write($"[ ]\t");
                    }
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
            Console.Write(Environment.NewLine + Environment.NewLine);
        }
    }
}
