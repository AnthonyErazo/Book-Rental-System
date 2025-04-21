using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackBookRentals.Entities;

public class Book : IEntityBase
{
    public string Name { get; set; } = default!;
    public string Author { get; set; } = default!;
    public string Isbn { get; set; } = default!;
    public bool Status { get; set; } = true;

    public virtual ICollection<OrderBook> OrderBooks { get; set; } = new List<OrderBook>();
    public Guid Id { get; set; }
}