using BackBookRentals.Dto.Exception;
using BackBookRentals.Dto.Request;
using BackBookRentals.Dto.Response;
using BackBookRentals.Entities;
using BackBookRentals.Repositories.Abstractions;
using BackBookRentals.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace BackBookRentals.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository userRepository;
    private readonly IUserTokenRepository tokenRepository;
    private readonly ILogger<UserService> logger;
    private readonly IOptions<AppSettings> options;

    public UserService(
        IUserRepository userRepository, 
        IUserTokenRepository tokenRepository, 
        ILogger<UserService> logger,
        IOptions<AppSettings> options)
    {
        this.userRepository = userRepository;
        this.tokenRepository = tokenRepository;
        this.logger = logger;
        this.options = options;
    }

    public async Task<BaseGenericResponse> RegisterAsync(AuthRequestDto request)
    {
        try
        {
            var user = new User
            {
                UserName = request.UserName,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            var id = await userRepository.AddAsync(user);

            return new BaseGenericResponse
            {
                Success=true,
                Message="Usuario registrado"

            };
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException?.Message.Contains("username", StringComparison.OrdinalIgnoreCase) == true ||
                ex.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true)
            {
                throw new ResponseException("Ya existe otro usuario con ese username.", HttpStatusCode.BadRequest);
            }

            logger.LogError(ex, "Error al actualizar cliente");
            throw;
        }
    }

    public async Task<GenericResponse<LoginResponseDto>> LoginAsync(AuthRequestDto request)
    {
        var user = await userRepository.GetByUserNameAsync(request.UserName)
                   ?? throw new ResponseException("Credenciales inválidas", HttpStatusCode.BadRequest);

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            throw new ResponseException("Credenciales inválidas", HttpStatusCode.BadRequest);

        var token= await GenerateTokenAsync(user);

        return new GenericResponse<LoginResponseDto>
        {
            Success = true,
            Message = "Login exitoso",
            Data = token
        };
    }

    public async Task<BaseGenericResponse> LogoutAsync(Guid userId, string jti)
    {
        await tokenRepository.InvalidateTokenAsync(jti);

        return new BaseGenericResponse
        {
            Success = true,
            Message = "Sesión cerrada correctamente"
        };
    }

    public async Task<BaseGenericResponse> LogoutAllAsync(Guid userId)
    {
        await tokenRepository.InvalidateAllTokensAsync(userId);

        return new BaseGenericResponse
        {
            Success = true,
            Message = "Todas las sesiones fueron cerradas correctamente"
        };
    }



    private async Task<LoginResponseDto> GenerateTokenAsync(User user)
    {
        var jti = Guid.NewGuid().ToString();
        var dateNow = DateTime.UtcNow;
        var expiration = dateNow.AddSeconds(options.Value.Jwt.LifetimeInSeconds);

        var claims = new List<Claim>()
        {
            new Claim("userId", user.Id.ToString()),
            new Claim("tokenId", jti)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.Jwt.JWTKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var securityToken = new JwtSecurityToken(
            issuer: null, 
            audience: null, 
            claims: claims,
            signingCredentials: credentials, 
            expires: expiration);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);

        var token = new UserToken
        {
            UserId = user.Id,
            TokenId = jti
        };
        await tokenRepository.AddAsync(token);

        return new LoginResponseDto
        {
            Token = tokenString,
            ExpirationDate = expiration
        };
    }
}