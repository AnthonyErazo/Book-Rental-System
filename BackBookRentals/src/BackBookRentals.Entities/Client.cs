namespace BackBookRentals.Entities;

public class Client: IEntityBase
{
    public string Names { get; set; } = default!;
    public string LastNames { get; set; } = default!;
    public string Dni { get; set; } = default!;
    public int Age { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public Guid Id { get; set; }
}