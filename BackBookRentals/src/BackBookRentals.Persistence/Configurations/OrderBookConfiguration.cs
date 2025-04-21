using BackBookRentals.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackBookRentals.Persistence.Configurations;

public class OrderBookConfiguration: IEntityTypeConfiguration<OrderBook>
{
    public void Configure(EntityTypeBuilder<OrderBook> builder)
    {
        builder.HasKey(x => new { x.Id, x.BookId });

        builder.ToTable(nameof(OrderBook), "Pedidos_Libros");

        builder.HasOne(x => x.Order)
            .WithMany(o => o.OrderBooks)
            .HasForeignKey(x => x.Id)
            .HasConstraintName("FK_Pedido_Libro_Pedido");

        builder.HasOne(x => x.Book)
            .WithMany(b => b.OrderBooks)
            .HasForeignKey(x => x.BookId)
            .HasConstraintName("FK_Pedido_Libro_Libro");
    }
}