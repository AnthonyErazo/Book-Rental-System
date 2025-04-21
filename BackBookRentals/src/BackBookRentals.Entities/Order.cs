using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackBookRentals.Entities;

public class Order: IEntityBase
{
    public DateTime RegisterTime { get; set; } = DateTime.UtcNow;
    public bool Status { get; set; } = false;

    public Guid ClientId { get; set; }
    public virtual Client Client { get; set; } = default!;

    public virtual ICollection<OrderBook> OrderBooks { get; set; } = new List<OrderBook>();
    public Guid Id { get; set; }
}