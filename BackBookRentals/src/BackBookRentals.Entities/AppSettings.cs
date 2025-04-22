namespace BackBookRentals.Entities;

public class AppSettings
{
    public required Jwt Jwt { get; set; }
}

public class Jwt
{
    public required string JWTKey { get; set; }
    public required int LifetimeInSeconds { get; set; }
}