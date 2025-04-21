using BackBookRentals.Dto.Request;
using BackBookRentals.Dto.Response;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json;

namespace BackBookRentals.Api.Providers;

public class AuthEndpointsExampleFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var path = context.ApiDescription.RelativePath?.ToLowerInvariant() ?? "";
        var method = context.ApiDescription.HttpMethod?.ToUpperInvariant() ?? "";

        if (method == "POST" && (path=="api/auth/login" || path=="api/auth/register"))
        {
            var schema = context.SchemaGenerator.GenerateSchema(typeof(AuthRequestDto), context.SchemaRepository);
            operation.RequestBody = new OpenApiRequestBody
            {
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Schema = schema,
                        Example = ToOpenApiObject(new AuthRequestDto
                        {
                            UserName = "juanperez",
                            Password = "123456"
                        })
                    }
                },
                Required = true
            };

            if (path.Contains("auth/login"))
            {
                if (operation.Responses.TryGetValue("200", out var response))
                {
                    var token = new LoginResponseDto
                    {
                        Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
                        ExpirationDate = DateTime.UtcNow
                    };

                    var responseBody = new GenericResponse<LoginResponseDto>
                    {
                        Success = true,
                        Message = "Login exitoso",
                        Data = token
                    };

                    var responseSchema = context.SchemaGenerator.GenerateSchema(typeof(GenericResponse<LoginResponseDto>), context.SchemaRepository);
                    response.Content["application/json"] = new OpenApiMediaType
                    {
                        Schema = responseSchema,
                        Example = ToOpenApiObject(responseBody)
                    };
                }
            }
            else if (path.Contains("auth/register"))
            {
                if (operation.Responses.TryGetValue("200", out var response))
                {
                    var responseBody = new BaseGenericResponse
                    {
                        Success = true,
                        Message = "Registro exitoso"
                    };

                    var responseSchema = context.SchemaGenerator.GenerateSchema(typeof(BaseGenericResponse), context.SchemaRepository);
                    response.Content["application/json"] = new OpenApiMediaType
                    {
                        Schema = responseSchema,
                        Example = ToOpenApiObject(responseBody)
                    };
                }
            }
        }
    }

    private OpenApiObject ToOpenApiObject(object obj)
    {
        var openApiObj = new OpenApiObject();
        var props = obj.GetType().GetProperties();
        var naming = JsonNamingPolicy.CamelCase;

        foreach (var prop in props)
        {
            var key = naming.ConvertName(prop.Name);
            var value = prop.GetValue(obj);
            if (value == null)
            {
                openApiObj[key] = new OpenApiString("null");
                continue;
            }

            switch (value)
            {
                case string s:
                    openApiObj[key] = new OpenApiString(s);
                    break;
                case int i:
                    openApiObj[key] = new OpenApiInteger(i);
                    break;
                case bool b:
                    openApiObj[key] = new OpenApiBoolean(b);
                    break;
                case Guid g:
                    openApiObj[key] = new OpenApiString(g.ToString());
                    break;
                case DateTime dt:
                    openApiObj[key] = new OpenApiString(dt.ToString("o")); // ← ISO 8601
                    break;
                case ICollection<BookResponseDto> list:
                    var array = new OpenApiArray();
                    foreach (var item in list)
                        array.Add(ToOpenApiObject(item));
                    openApiObj[key] = array;
                    break;
                default:
                    openApiObj[key] = value.GetType().Namespace?.StartsWith("BackBookRentals.Dto") == true
                        ? ToOpenApiObject(value)
                        : new OpenApiString(value.ToString());
                    break;
            }
        }

        return openApiObj;
    }
}
