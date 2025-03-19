using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameOfLife_A.Models
{
   public class Board
   {
      public int Id { get; set; }

      [EditorBrowsable(EditorBrowsableState.Never)]
      public string InternalArray { get; set; } = "";

      [NotMapped]
      public int[][] Array
      {
         get
         {
            return InternalArray
                .Split(';')
                .Select(row => row.Split(',')
                                  .Select(cell => int.Parse(cell))
                                  .ToArray())
                .ToArray();
         }
         set
         {
            InternalArray = string.Join(";",
                value.Select(row => string.Join(",", row))
            );
         }
      }

      [NotMapped]
      public int ArrayRows => Array.Length;

      [NotMapped]
      public int ArrayColumns => Array.Length > 0 ? Array[0].Length : 0;
   }
}
