using Microsoft.EntityFrameworkCore;

namespace CourrencyConversionAPI.Data
{
    public class AppDbContext : DbContext
    {
       // public DbSet<MovieModel> Movies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=./data/App.db");
        }
    }
}
