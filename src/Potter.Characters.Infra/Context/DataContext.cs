using Microsoft.EntityFrameworkCore;
using Potter.Characters.Domain.Models;
using Potter.Characters.Infra.Mappings;

namespace Potter.Characters.Infra.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Character> Character { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CharacterMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}
