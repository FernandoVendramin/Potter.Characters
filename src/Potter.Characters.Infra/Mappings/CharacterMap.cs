using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Potter.Characters.Domain.Models;

namespace Potter.Characters.Infra.Mappings
{
    public class CharacterMap : IEntityTypeConfiguration<Character>
    {
        public void Configure(EntityTypeBuilder<Character> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(c => c.Name)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(c => c.Role)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.School)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(c => c.House)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.Patronus)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
