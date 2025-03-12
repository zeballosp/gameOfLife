using Xunit;
using GameOfLife_A.Services;
using System.Reflection;

namespace GameOfLife_A.Tests
{
   public class GameOfLifeServiceTests
   {
      private readonly GameOfLifeService _service;

      public GameOfLifeServiceTests()
      {
         _service = new GameOfLifeService();
      }

      [Fact]
      public void FinalState_ReturnsFinalState_WhenStable()
      {
         var initial = new int[][]
         {
                new int[] { 0, 1, 0, 1, 0 },
                new int[] { 1, 0, 1, 1, 0 },
                new int[] { 0, 0, 1, 0, 1 },
                new int[] { 1, 1, 0, 0, 1 },
                new int[] { 0, 1, 0, 1, 0 }
         };

         var result = _service.FinalState(initial, 6);

         var expected = new int[][]
         {
                new int[] { 0, 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 0, 0 }
         };

         Assert.NotNull(result);
         Assert.True(AreArraysEqual(expected, result));
      }

      [Fact]
      public void FinalState_ReturnsNull_WhenNotStable()
      {
         var initial = new int[][]
         {
                new int[] { 0, 1, 0, 1, 0 },
                new int[] { 1, 0, 1, 0, 1 },
                new int[] { 0, 1, 0, 1, 0 },
                new int[] { 1, 0, 1, 0, 1 },
                new int[] { 0, 1, 0, 1, 0 }
         };

         var result = _service.FinalState(initial, 1);

         Assert.Null(result);
      }

      [Fact]
      public void XAwayState_ReturnsCorrectState_AfterSpecifiedSteps()
      {
         var initial = new int[][]
         {
                new int[] { 0, 1, 0, 1, 0 },
                new int[] { 1, 0, 1, 1, 0 },
                new int[] { 0, 0, 1, 0, 1 },
                new int[] { 1, 1, 0, 0, 1 },
                new int[] { 0, 1, 0, 1, 0 }
         };

         var result = _service.XAwayState(initial, 3);

         var expected = new int[][]
         {
                new int[] { 0, 0, 0, 0, 0 },
                new int[] { 0, 1, 0, 1, 0 },
                new int[] { 0, 1, 0, 1, 0 },
                new int[] { 0, 1, 0, 0, 0 },
                new int[] { 0, 0, 0, 0, 0 }
         };

         Assert.True(AreArraysEqual(expected, result));
      }

      [Fact]
      public void NextState_CalculatesCorrectNextState()
      {
         var initial = new int[][]
         {
                new int[] { 0, 1, 0, 1, 0 },
                new int[] { 1, 0, 1, 1, 0 },
                new int[] { 0, 0, 1, 0, 1 },
                new int[] { 1, 1, 0, 0, 1 },
                new int[] { 0, 1, 0, 1, 0 }
         };

         var result = _service.NextState(initial);

         var expected = new int[][]
         {
                new int[] { 0, 1, 0, 1, 0 },
                new int[] { 0, 0, 0, 0, 1 },
                new int[] { 1, 0, 1, 0, 1 },
                new int[] { 1, 1, 0, 0, 1 },
                new int[] { 1, 1, 1, 0, 0 }
         };

         Assert.True(AreArraysEqual(expected, result));
      }

      [Fact]
      public void CountNeighbors_ReturnsCorrectCount()
      {
         var grid = new int[][]
         {
                new int[] { 1, 1, 0, 0, 0 },
                new int[] { 0, 1, 0, 1, 0 },
                new int[] { 1, 0, 0, 0, 1 },
                new int[] { 0, 1, 0, 1, 0 },
                new int[] { 0, 0, 1, 0, 0 }
         };

         var position0_0 = _service.CountNeighbors(0, 0, grid);
         var position0_2 = _service.CountNeighbors(0, 2, grid);
         var position0_4 = _service.CountNeighbors(0, 4, grid);
         var position2_0 = _service.CountNeighbors(2, 0, grid);
         var position2_1 = _service.CountNeighbors(2, 1, grid);
         var position4_0 = _service.CountNeighbors(4, 0, grid);
         var position4_1 = _service.CountNeighbors(4, 1, grid);
         var position4_4 = _service.CountNeighbors(4, 4, grid);

         Assert.Equal(2, position0_0);
         Assert.Equal(3, position0_2);
         Assert.Equal(1, position0_4);
         Assert.Equal(2, position2_0);
         Assert.Equal(3, position2_1);
         Assert.Equal(1, position4_0);
         Assert.Equal(2, position4_1);
         Assert.Equal(1, position4_4);
      }

      [Fact]
      public void AreArraysEqual_ReturnsTrue_ForIdenticalArrays()
      {
         var array1 = new int[][]
         {
                new int[] { 1, 0, 1, 0, 1 },
                new int[] { 0, 1, 0, 1, 0 },
                new int[] { 1, 0, 1, 0, 1 },
                new int[] { 0, 1, 0, 1, 0 },
                new int[] { 1, 0, 1, 0, 1 }
         };

         var array2 = (int[][])array1.Clone();

         var areArraysEqualMethod = typeof(GameOfLifeService)
             .GetMethod("AreArraysEqual", BindingFlags.NonPublic | BindingFlags.Instance);

         var result = (bool)areArraysEqualMethod.Invoke(_service, new object[] { array1, array2 });

         Assert.True(result);
      }

      [Fact]
      public void AreArraysEqual_ReturnsFalse_ForDifferentArrays()
      {
         var array1 = new int[][]
         {
                new int[] { 1, 0, 1, 0, 1 },
                new int[] { 0, 1, 0, 1, 0 },
                new int[] { 1, 0, 1, 0, 1 },
                new int[] { 0, 1, 0, 1, 0 },
                new int[] { 1, 0, 1, 0, 1 }
         };

         var array2 = new int[][]
         {
                new int[] { 0, 1, 0, 1, 0 },
                new int[] { 1, 0, 1, 0, 1 },
                new int[] { 0, 1, 0, 1, 0 },
                new int[] { 1, 0, 1, 0, 1 },
                new int[] { 0, 1, 0, 1, 0 }
         };

         var areArraysEqualMethod = typeof(GameOfLifeService)
             .GetMethod("AreArraysEqual", BindingFlags.NonPublic | BindingFlags.Instance);

         var result = (bool)areArraysEqualMethod.Invoke(_service, new object[] { array1, array2 });

         Assert.False(result);
      }

      private bool AreArraysEqual(int[][] a, int[][] b) => a.Zip(b, (x, y) => x.SequenceEqual(y)).All(equal => equal);
   }
}
