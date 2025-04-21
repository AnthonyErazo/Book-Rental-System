using BackBookRentals.Api.Filters;
using BackBookRentals.Dto.Response;
using BackBookRentals.Persistence;
using BackBookRentals.Repositories.Abstractions;
using BackBookRentals.Repositories.Implementations;
using BackBookRentals.Services.Abstractions;
using BackBookRentals.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using BackBookRentals.Services.Profiles;
using System.Text.Json;
using BackBookRentals.Entities;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using HealthChecks.UI.Client;
using BackBookRentals.Dto.Exception;
using BackBookRentals.Api.Providers;

var builder = WebApplication.CreateBuilder(args);

// Configurar el entorno
var environment = builder.Environment;
builder.Configuration
    .SetBasePath(environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.Configure<AppSettings>(builder.Configuration);

var corsConfiguration = "BackBookRentalsCors";
builder.Services.AddCors(setup =>
{
    setup.AddPolicy(corsConfiguration, policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseLazyLoadingProxies()
        .UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"));
});

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<BookProfile>();
    config.AddProfile<ClientProfile>();
    config.AddProfile<OrderProfile>();
});

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserTokenRepository, UserTokenRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddScoped<IClientsService, ClientService>();
builder.Services.AddScoped<IBooksService, BookService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Book Rentals API",
        Version = "v1",
        Description = "API para gestionar el alquiler de libros en una biblioteca.",
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme.  
                        Enter your token below.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.OperationFilter<AuthEndpointsExampleFilter>();
    c.OperationFilter<AuthorizeCheckOperationFilter>();
    c.OperationFilter<ClientEndpointsExampleFilter>();
    c.OperationFilter<BookEndpointsExampleFilter>();
    c.OperationFilter<OrderEndpointsExampleFilter>();

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var firstError = context.ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .FirstOrDefault() ?? "Error de validación.";

            return new BadRequestObjectResult(new BaseGenericResponse
            {
                Success = false,
                Message = firstError
            });
        };
    });
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHealthChecks()
    .AddCheck("selfcheck", () => HealthCheckResult.Healthy())
    .AddDbContextCheck<ApplicationDbContext>();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:JWTKey"] ??
                                     throw new InvalidOperationException("JWT key not configured"));
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>()
                .CreateLogger("JWT");

            var userTokenRepo = context.HttpContext.RequestServices.GetRequiredService<IUserTokenRepository>();
            var claims = context.Principal?.Claims;

            var userId = claims?.FirstOrDefault(c => c.Type == "userId")?.Value;
            var jti = claims?.FirstOrDefault(c => c.Type == "tokenId")?.Value;

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(jti))
            {
                context.Fail("Token inválido (sin JTI o Sub).");
                return;
            }

            var tokenInDb = await userTokenRepo.GetValidTokenAsync(Guid.Parse(userId), jti);

            if (tokenInDb is null)
            {
                context.Fail("Token inválido o caducado en la base de datos.");
                throw new ResponseException("No se encontro sesion activa para este usuario.",
                    HttpStatusCode.NotFound);
            }
        },
        OnChallenge = context =>
        {
            context.HandleResponse();

            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.ContentType = "application/json";

            var response = new BaseGenericResponse
            {
                Success = false,
                Message = "No autorizado. Debe iniciar sesión para acceder a este recurso."
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        },
    };
});

builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.EnsureCreated();
        
        if (!context.Database.CanConnect())
        {
            throw new Exception("No se pudo conectar a la base de datos.");
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error al inicializar la base de datos.");
        throw;
    }
}

if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Docker")
{
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Book Rentals API v1");
        c.RoutePrefix = "swagger";
        c.DocumentTitle = "Book Rentals API Documentation";
    });
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(corsConfiguration);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapHealthChecks("/healthcheck", new()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
