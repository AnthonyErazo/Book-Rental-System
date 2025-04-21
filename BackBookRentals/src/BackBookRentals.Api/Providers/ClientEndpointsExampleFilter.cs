using BackBookRentals.Dto.Request;
using BackBookRentals.Dto.Response;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json;

namespace BackBookRentals.Api.Providers;

public class ClientEndpointsExampleFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.MethodInfo.DeclaringType?.Name != "ClientsController") return;

        var path = context.ApiDescription.RelativePath?.ToLowerInvariant() ?? "";
        var method = context.ApiDescription.HttpMethod?.ToUpperInvariant() ?? "";

        // Agregar el token de autorización a todos los endpoints
        operation.Security = new List<OpenApiSecurityRequirement>
        {
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            }
        };

        // POST /clients
        if (method == "POST" && path == "api/clients")
        {
            var schema = context.SchemaGenerator.GenerateSchema(typeof(ClientRequestDto), context.SchemaRepository);
            operation.RequestBody = new OpenApiRequestBody
            {
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Schema = schema,
                        Example = ToOpenApiObject(new ClientRequestDto
                        {
                            Names = "Juan Carlos",
                            LastNames = "Pérez Gómez",
                            Dni = "12345678",
                            Age = 30
                        })
                    }
                },
                Required = true
            };

            if (operation.Responses.TryGetValue("200", out var response))
            {
                var responseBody = new GenericResponse<Guid>
                {
                    Success = true,
                    Message = "Cliente creado correctamente.",
                    Data = Guid.Parse("5c52f3a7-f23b-4a13-8d3f-4be3d6efabcd")
                };

                var responseSchema = context.SchemaGenerator.GenerateSchema(typeof(GenericResponse<Guid>), context.SchemaRepository);
                response.Content["application/json"] = new OpenApiMediaType
                {
                    Schema = responseSchema,
                    Example = ToOpenApiObject(responseBody)
                };
            }
        }

        // PATCH /clients/{id}
        if (method == "PATCH" && path.StartsWith("api/clients"))
        {
            var schema = context.SchemaGenerator.GenerateSchema(typeof(ClientUpdateRequestDto), context.SchemaRepository);
            operation.RequestBody = new OpenApiRequestBody
            {
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Schema = schema,
                        Example = ToOpenApiObject(new ClientUpdateRequestDto
                        {
                            Names = "Editado",
                            LastNames = "Apellido Editado",
                            Dni = "87654321",
                            Age = 40
                        })
                    }
                },
                Required = true
            };

            if (operation.Responses.TryGetValue("200", out var response))
            {
                var responseBody = new BaseGenericResponse
                {
                    Success = true,
                    Message = "Cliente actualizado correctamente."
                };

                var responseSchema = context.SchemaGenerator.GenerateSchema(typeof(BaseGenericResponse), context.SchemaRepository);
                response.Content["application/json"] = new OpenApiMediaType
                {
                    Schema = responseSchema,
                    Example = ToOpenApiObject(responseBody)
                };
            }
        }

        // GET /clients
        if (method == "GET" && path == "api/clients")
        {
            if (operation.Responses.TryGetValue("200", out var response))
            {
                var cliente = new ClientResponseDto
                {
                    ClientId = Guid.Parse("5c52f3a7-f23b-4a13-8d3f-4be3d6efabcd"),
                    Names = "Juan",
                    LastNames = "Pérez",
                    Dni = "12345678",
                    Age = 30
                };

                var responseBody = new GenericResponse<ICollection<ClientResponseDto>>
                {
                    Success = true,
                    Message = "Lista de clientes obtenida correctamente.",
                    Data = new List<ClientResponseDto> { cliente }
                };

                var responseSchema = context.SchemaGenerator.GenerateSchema(typeof(GenericResponse<ICollection<ClientResponseDto>>), context.SchemaRepository);
                response.Content["application/json"] = new OpenApiMediaType
                {
                    Schema = responseSchema,
                    Example = ToOpenApiObject(responseBody)
                };
            }
        }

        // GET /clients/{id}
        if (method == "GET" && path.StartsWith("api/clients/{id}"))
        {
            if (operation.Responses.TryGetValue("200", out var response))
            {
                var cliente = new ClientResponseDto
                {
                    ClientId = Guid.Parse("5c52f3a7-f23b-4a13-8d3f-4be3d6efabcd"),
                    Names = "Juan",
                    LastNames = "Pérez",
                    Dni = "12345678",
                    Age = 30
                };

                var responseBody = new GenericResponse<ClientResponseDto>
                {
                    Success = true,
                    Message = "Cliente obtenido correctamente.",
                    Data = cliente
                };

                var responseSchema = context.SchemaGenerator.GenerateSchema(typeof(GenericResponse<ClientResponseDto>), context.SchemaRepository);
                response.Content["application/json"] = new OpenApiMediaType
                {
                    Schema = responseSchema,
                    Example = ToOpenApiObject(responseBody)
                };
            }
        }

        // DELETE /clients/{id}
        if (method == "DELETE" && path.StartsWith("api/clients/{id}"))
        {
            if (operation.Responses.TryGetValue("200", out var response))
            {
                var responseBody = new BaseGenericResponse
                {
                    Success = true,
                    Message = "Cliente eliminado correctamente."
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
                case ICollection<ClientResponseDto> list:
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
