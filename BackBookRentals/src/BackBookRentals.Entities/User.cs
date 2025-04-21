using System.ComponentModel.DataAnnotations;

namespace BackBookRentals.Entities;

public class User: IEntityBase
{

    public required string UserName { get; set; }
    public required string Password { get; set; }
    public virtual ICollection<UserToken> Tokens { get; set; } = new HashSet<UserToken>();
    public Guid Id { get; set; }
}