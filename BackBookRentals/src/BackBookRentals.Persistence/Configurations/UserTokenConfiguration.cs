using BackBookRentals.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BackBookRentals.Persistence.Configurations;

public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        builder.ToTable(nameof(UserToken), "Usuario_Token");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id_token")
            .HasDefaultValueSql("NEWID()");

        builder.Property(x => x.UserId)
            .HasColumnName("id_user");

        builder.Property(x => x.TokenId)
            .HasColumnName("token_id")
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(x => x.TokenId).IsUnique();
    }
}