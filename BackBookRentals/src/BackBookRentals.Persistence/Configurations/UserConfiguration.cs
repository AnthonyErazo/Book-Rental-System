using BackBookRentals.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackBookRentals.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id_usuario")
            .HasDefaultValueSql("NEWID()");

        builder.ToTable(nameof(User), "Usuario");

        builder.Property(x => x.UserName)
            .HasColumnName("user_name")
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(x => x.Password)
            .HasColumnName("user_pass")
            .IsRequired()
            .HasMaxLength(60);
        builder.HasIndex(x => x.UserName).IsUnique();

        builder.HasMany(u => u.Tokens)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}