using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BackBookRentals.Entities;

namespace BackBookRentals.Persistence.Configurations;

public class BookConfiguration:IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id_libro")
            .HasDefaultValueSql("NEWID()");

        builder.ToTable(nameof(Book), "Libro");

        builder.Property(x => x.Name).HasMaxLength(100).HasColumnName("nombre");
        builder.Property(x => x.Author).HasMaxLength(100).HasColumnName("autor");
        builder.Property(x => x.Isbn).HasMaxLength(13).HasColumnName("isbn");
        builder.Property(x => x.Status).HasColumnName("estado");

        builder.HasIndex(x => x.Isbn).IsUnique();
    }
}