using Microsoft.EntityFrameworkCore;
using Potter.Characters.Domain.Models;
using Potter.Characters.Infra.Mappings;

namespace Potter.Characters.Infra
{
    public class DataContext : DbContext
    {
        // $2a$10$rOgYekVSLM96/Ah7NLXq6enm4sMsRUGI4Rib9h8cMzDMfTXkLYYvi
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
