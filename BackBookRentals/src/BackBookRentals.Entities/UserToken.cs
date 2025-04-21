using System.ComponentModel.DataAnnotations.Schema;

namespace BackBookRentals.Entities;

public class UserToken:IEntityBase
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = default!;
    public string TokenId { get; set; } = default!;
}