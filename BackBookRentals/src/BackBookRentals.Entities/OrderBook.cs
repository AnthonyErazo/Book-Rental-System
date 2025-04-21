using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackBookRentals.Entities;

public class OrderBook: IEntityBase
{
    public virtual Order Order { get; set; } = default!;

    public Guid BookId { get; set; }
    public virtual Book Book { get; set; } = default!;
    public Guid Id { get; set; }
}