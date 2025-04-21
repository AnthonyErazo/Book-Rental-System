namespace BackBookRentals.Entities;

public class AppSettings
{
    public Jwt Jwt { get; set; }
}

public class Jwt
{
    public string JWTKey { get; set; }
    public int LifetimeInSeconds { get; set; }
}