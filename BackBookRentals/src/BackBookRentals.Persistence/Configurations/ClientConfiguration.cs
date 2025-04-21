using BackBookRentals.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackBookRentals.Persistence.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id_cliente")
            .HasDefaultValueSql("NEWID()");

        builder.ToTable(nameof(Client), "Cliente");

        builder.Property(x => x.Names).HasMaxLength(100).HasColumnName("nombres");
        builder.Property(x => x.LastNames).HasMaxLength(100).HasColumnName("apellidos");
        builder.Property(x => x.Dni).HasMaxLength(8).HasColumnName("dni");
        builder.Property(x => x.Age).HasColumnName("edad");

        builder.HasIndex(x => x.Dni).IsUnique();
    }
}