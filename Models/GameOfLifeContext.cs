using Microsoft.EntityFrameworkCore;

namespace GameOfLife_A.Models
{
    public class GameOfLifeContext : DbContext
    {
        public GameOfLifeContext(DbContextOptions<GameOfLifeContext> options) : base(options)
        {
        }

        public DbSet<Board>? Boards { get; set; }
    }
}
