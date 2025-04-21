using BackBookRentals.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackBookRentals.Persistence.Configurations;

public class OrderConfiguration: IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id_pedido")
            .HasDefaultValueSql("NEWID()");

        builder.ToTable(nameof(Order), "Pedido");

        builder.Property(x => x.RegisterTime)
            .HasColumnName("fecha_pedido")
            .HasDefaultValueSql("GETDATE()");

        builder.Property(x => x.Status).HasColumnName("estado");

        builder.HasOne(x => x.Client)
            .WithMany(c => c.Orders)
            .HasForeignKey(x => x.ClientId)
            .HasConstraintName("FK_Pedido_Cliente");
    }
}