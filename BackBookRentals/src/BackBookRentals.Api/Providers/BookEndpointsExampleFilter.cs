using BackBookRentals.Dto.Request;
using BackBookRentals.Dto.Response;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json;

namespace BackBookRentals.Api.Providers;

public class BookEndpointsExampleFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.MethodInfo.DeclaringType?.Name != "BooksController") return;

        var path = context.ApiDescription.RelativePath?.ToLowerInvariant() ?? "";
        var method = context.ApiDescription.HttpMethod?.ToUpperInvariant() ?? "";

        // POST /books
        if (method == "POST" && path == "api/books")
        {
            var schema = context.SchemaGenerator.GenerateSchema(typeof(BookRequestDto), context.SchemaRepository);
            operation.RequestBody = new OpenApiRequestBody
            {
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Schema = schema,
                        Example = ToOpenApiObject(new BookRequestDto
                        {
                            Name = "Cien años de soledad",
                            Author = "Gabriel García Márquez",
                            Isbn = "9781234567890",
                            Status = true
                        })
                    }
                },
                Required = true
            };

            if (operation.Responses.TryGetValue("201", out var response))
            {
                var responseBody = new GenericResponse<Guid>
                {
                    Success = true,
                    Message = "Libro creado correctamente.",
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

        // PATCH /books/{id}
        if (method == "PATCH" && path.StartsWith("api/books"))
        {
            var schema = context.SchemaGenerator.GenerateSchema(typeof(BookUpdateRequestDto), context.SchemaRepository);
            operation.RequestBody = new OpenApiRequestBody
            {
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Schema = schema,
                        Example = ToOpenApiObject(new BookUpdateRequestDto
                        {
                            Name = "Edición revisada",
                            Author = "Gabo",
                            Isbn = "9781234567891",
                            Status = false
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
                    Message = "Libro actualizado correctamente."
                };

                var responseSchema = context.SchemaGenerator.GenerateSchema(typeof(BaseGenericResponse), context.SchemaRepository);
                response.Content["application/json"] = new OpenApiMediaType
                {
                    Schema = responseSchema,
                    Example = ToOpenApiObject(responseBody)
                };
            }
        }

        // GET /books
        if (method == "GET" && path == "api/books")
        {
            if (operation.Responses.TryGetValue("200", out var response))
            {
                var libro = new BookResponseDto
                {
                    BookId = Guid.Parse("5c52f3a7-f23b-4a13-8d3f-4be3d6efabcd"),
                    Name = "Cien años de soledad",
                    Author = "Gabriel García Márquez",
                    Isbn = "9781234567890",
                    Status = true
                };

                var responseBody = new GenericResponse<ICollection<BookResponseDto>>
                {
                    Success = true,
                    Message = "Lista de libros obtenida correctamente.",
                    Data = new List<BookResponseDto> { libro }
                };

                var responseSchema = context.SchemaGenerator.GenerateSchema(typeof(GenericResponse<ICollection<BookResponseDto>>), context.SchemaRepository);
                response.Content["application/json"] = new OpenApiMediaType
                {
                    Schema = responseSchema,
                    Example = ToOpenApiObject(responseBody)
                };
            }
        }

        // GET /books/{id}
        if (method == "GET" && path.StartsWith("api/books/{id}"))
        {
            if (operation.Responses.TryGetValue("200", out var response))
            {
                var libro = new BookResponseDto
                {
                    BookId = Guid.Parse("5c52f3a7-f23b-4a13-8d3f-4be3d6efabcd"),
                    Name = "Cien años de soledad",
                    Author = "Gabriel García Márquez",
                    Isbn = "9781234567890",
                    Status = true
                };

                var responseBody = new GenericResponse<BookResponseDto>
                {
                    Success = true,
                    Message = "Libro obtenido correctamente.",
                    Data = libro
                };

                var responseSchema = context.SchemaGenerator.GenerateSchema(typeof(GenericResponse<BookResponseDto>), context.SchemaRepository);
                response.Content["application/json"] = new OpenApiMediaType
                {
                    Schema = responseSchema,
                    Example = ToOpenApiObject(responseBody)
                };
            }
        }

        // DELETE /books/{id}
        if (method == "DELETE" && path.StartsWith("api/books/{id}"))
        {
            if (operation.Responses.TryGetValue("200", out var response))
            {
                var responseBody = new BaseGenericResponse
                {
                    Success = true,
                    Message = "Libro eliminado correctamente."
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
