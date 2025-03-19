namespace GameOfLife_A.Services
{
   /// <summary>
   /// Service that handles the state evolution of Conway's Game of Life.
   /// </summary>
   public class GameOfLifeService
   {
      /// <summary>
      /// Calculates the final state of the Game of Life after a specified number of steps.
      /// Returns null if the state does not stabilize.
      /// </summary>
      /// <param name="initialArray">The initial state of the game grid.</param>
      /// <param name="statesAway">The number of steps to simulate.</param>
      /// <returns>
      /// The final state of the grid if it stabilizes; otherwise, null.
      /// </returns>
      public int[][] FinalState(int[][] initialArray, int statesAway)
      {
         var final = XAwayState(initialArray, statesAway);
         var finalPlusOne = XAwayState(initialArray, statesAway + 1);

         if (!AreArraysEqual(final, finalPlusOne))
         {
            return null;
         }

         return final;
      }

      /// <summary>
      /// Calculates the state of the grid after a specified number of steps.
      /// </summary>
      /// <param name="initialArray">The initial state of the game grid.</param>
      /// <param name="statesAway">The number of steps to simulate.</param>
      /// <returns>The state of the grid after the specified steps.</returns>
      public int[][] XAwayState(int[][] initialArray, int statesAway)
      {
         var state = initialArray;

         for (int i = 0; i < statesAway; i++)
         {
            state = NextState(state);
         }

         return state;
      }

      /// <summary>
      /// Computes the next state of the grid based on the current state.
      /// </summary>
      /// <param name="initialArray">The current state of the grid.</param>
      /// <returns>The next state of the grid.</returns>
      public int[][] NextState(int[][] initialArray)
      {
         int rows = initialArray.Length;
         int cols = initialArray[0].Length;
         int[][] nextState = new int[rows][];

         //TODO: This can be optimized by using a single array and calculating the position of the cell
         for (int i = 0; i < rows; i++)
         {
            nextState[i] = new int[cols];
            for (int j = 0; j < cols; j++)
            {
               int neighbors = CountNeighbors(i, j, initialArray);
               int cell = initialArray[i][j];
               //Any live cell with fewer than two live neighbors dies(underpopulation).
               //Any live cell with more than three live neighbors dies(overpopulation).
               //Any live cell with two or three live neighbors survives to the next generation.
               //This 3 rules can be simplified to: if the number of neighboors is exactly 3 or the number of neighboors
               //is 2 and the cell is alive,the cell will be alive in the next state
               nextState[i][j] = (cell == 1 && (neighbors == 2 || neighbors == 3)) ||
                                 // Dead cell becomes alive if it has exactly 3 neighbors
                                 (cell == 0 && neighbors == 3) ? 1 : 0;
            }
         }

         return nextState;
      }

      /// <summary>
      /// Counts the number of live neighbors for a cell at the specified position.
      /// </summary>
      /// <param name="rowPosition">Row index of the cell.</param>
      /// <param name="colPosition">Column index of the cell.</param>
      /// <param name="grid">The game grid.</param>
      /// <returns>The number of live neighbors.</returns>
      public int CountNeighbors(int rowPosition, int colPosition, int[][] grid)
      {
         int rows = grid.Length;
         int cols = grid[0].Length;
         int count = 0;

         // 8 possible directions

         //-1 to count neighbors one row above
         //0 to count neighbors in the same row
         //1 to count neighbors one row below
         int[] rowPositionCalculator = { -1, -1, -1, 0, 0, 1, 1, 1 };
         // -1 to count neighbors one column to the left
         // 0 to count neighbors in the same column
         // 1 to count neighbors one column to the right
         int[] colPositionCalculator = { -1, 0, 1, -1, 1, -1, 0, 1 };
         //With this positions we can calculate the 8 possible directions

         for (int i = 0; i < 8; i++)
         {
            int nextRowPosition = rowPosition + rowPositionCalculator[i];
            int nextColPosition = colPosition + colPositionCalculator[i];

            //This solution does not use wrapping.
            //If the cell is in the border of the grid, it will not count the neighbors that are outside the grid
            if (nextRowPosition >= 0 && nextRowPosition < rows && nextColPosition >= 0 && 
               nextColPosition < cols && grid[nextRowPosition][nextColPosition] == 1)
            {
               count++;
            }
         }

         return count;
      }

      /// <summary>
      /// Compares two 2D arrays to check if they are equal.
      /// </summary>
      /// <param name="a">First array to compare.</param>
      /// <param name="b">Second array to compare.</param>
      /// <returns>True if both arrays are equal; otherwise, false.</returns>
      private bool AreArraysEqual(int[][] a, int[][] b)
      {
         if (a.Length != b.Length) return false;

         for (int i = 0; i < a.Length; i++)
         {
            if (!a[i].SequenceEqual(b[i])) return false;
         }

         return true;
      }
   }
}
