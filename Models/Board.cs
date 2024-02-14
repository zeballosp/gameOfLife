using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;

namespace GameOfLife_A.Models
{
    public class Board
    {
        public int Id { get; set; }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string InternalArray { get; set; }
        [NotMapped]
        public int[][] Array
        {
            get
            {
                return InternalArray.Split(';')
                     .Select(s => s.Split(',')
                                    .Select(token => int.Parse(token))
                                    .ToArray())
                     .ToArray();
            }
            set
            {
                try
                {
                    for (int i = 0; i < Array.GetLength(0); i++)
                    {
                        for (int j = 0; j < Array[0].Length; j++)
                        {
                            InternalArray = InternalArray + Array[i][j] + ",";
                        }
                        //Removes last comma that is automatically added at the end of the row
                        InternalArray = InternalArray.Substring(0, InternalArray.Length - 1);
                        InternalArray = InternalArray + ";";
                    }
                    //Removes last semi colon that is automatically added at the end of the row
                    InternalArray = InternalArray.Substring(0, InternalArray.Length - 1);
                }
                catch (Exception)
                {

                    throw;
                }

            }
        }

        [NotMapped]
        private int ArrayRows
        {
            get
            {
                return Array.GetLength(0);
            }
        }

        [NotMapped]
        private int ArrayColums
        {
            get
            {
                return Array[0].Length;
            }
        }
        public int[][]? FinalState(int[][] initialArray, int statesAway)
        {
            // There are 3 common pattern types: 
            //  - Still lifes: Pattern does not change from one generation to the next
            //  - Oscillators: Pattern returns to their initial state after a finite number of generations
            //  - Spaceships: Pattern translates across the grid
            // Based on this information which one would be the final state?
            // For time's sake I will assume that the final state is one in which the next state is the same as the previous one.
            var final = XAwayState(initialArray, statesAway);
            var finalPlusOne = XAwayState(initialArray, statesAway + 1);
            if (final != finalPlusOne)
            {
                return null;
            }
            return final;
        }

        public int[][]? XAwayState(int[][] initialArray, int statesAway)
        {
            var structArray = initialArray;
            for (int i = 0; i < statesAway; i++)
            {
                var result = xAway(initialArray);
            }
            return structArray;
        }

        public int[][] xAway(int[][] initialArray)
        {
            int[,] nextState = new int[ArrayRows,ArrayColums];

            //var nextState = initialArray.Array;
            int numOfNeighbors;
            int numToEvaluate;

            for (int i = 0; i < ArrayRows; i++)
            {
                for (int j = 0; j < ArrayColums; j++)
                {
                    numToEvaluate = initialArray[i][j];
                    numOfNeighbors = CountNeighbors(i, j, initialArray);
                    if (numToEvaluate == 1)
                    {
                        // Conditions 1
                        // Any live cell with fewer than two live neighbors dies, as if by underpopulation.
                        // Condition 3
                        // Any live cell with more than three live neighbors dies, as if by overpopulation.
                        if (numOfNeighbors < 2 || numOfNeighbors > 3)
                        {
                            // nextState.Array[i][j] = 0;
                            nextState[i,j] = 0;
                        }
                        //Condition 2
                        //Any live cell with two or three live neighbors lives on to the next generation.
                        if (numOfNeighbors == 2 || numOfNeighbors == 3)
                        {
                            nextState[i,j] = 1;
                        }
                    }
                    if (numToEvaluate == 0)
                    {
                        // Condition 4
                        // Any dead cell with exactly three live neighbors becomes a live cell, as if by reproduction.
                        if (numOfNeighbors == 3)
                        {
                            nextState[i,j] = 1;
                        }
                        else
                        {
                            nextState[i,j] = 0;
                        }
                    }
                }
            }

            var result = initialArray;
            for (int i = 0; i < ArrayRows; i++)
            {
                for (int j = 0; j < ArrayColums; j++)
                {
                    result[i][j] = nextState[i, j];
                }
            }
            return result;
        }

        private int CountNeighbors(int x, int y, int[][] mainArray) 
        {
            int neighbordCount;

            List<int> neighbors = new();

            int[] rowpositions = new int[3];
            rowpositions[0] = x - 1;
            rowpositions[1] = x;
            rowpositions[2] = x + 1;

            int[] colpositions = new int[3];
            colpositions[0] = y - 1;
            colpositions[1] = y;
            colpositions[2] = y + 1;

            for (int r = 0; r < 3; r++)
            {
                int rowPosition = rowpositions[r];
                if (rowPosition >= 0 && rowPosition < ArrayRows)
                {
                    for (int c = 0; c < 3; c++)
                    {
                        int columnPosition = colpositions[c];
                        if (columnPosition >= 0 && columnPosition < ArrayColums)
                        {
                            // Avoids to add the number being evaluated as is not a neighbor
                            if (rowPosition == x && columnPosition == y)
                            {
                                continue;
                            }
                            neighbors.Add(mainArray[rowPosition][columnPosition]);
                        }
                    }
                }
            }

            neighbordCount = neighbors.ToArray().Sum();
            return neighbordCount;
        }

    }
}
