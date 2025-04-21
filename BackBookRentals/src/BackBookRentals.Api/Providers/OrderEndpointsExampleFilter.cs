using BackBookRentals.Dto.Request;
using BackBookRentals.Dto.Response;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json;

namespace BackBookRentals.Api.Providers;

public class OrderEndpointsExampleFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.MethodInfo.DeclaringType?.Name != "OrdersController") return;

        var path = context.ApiDescription.RelativePath?.ToLowerInvariant() ?? "";
        var method = context.ApiDescription.HttpMethod?.ToUpperInvariant() ?? "";

        // POST /orders
        if (method == "POST" && path == "api/orders")
        {
            var schema = context.SchemaGenerator.GenerateSchema(typeof(OrderRequestDto), context.SchemaRepository);
            operation.RequestBody = new OpenApiRequestBody
            {
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Schema = schema,
                        Example = ToOpenApiObject(new OrderRequestDto
                        {
                            ClientId = Guid.Parse("5c52f3a7-f23b-4a13-8d3f-4be3d6efabcd"),
                            BookIds = new List<Guid> { 
                                Guid.Parse("6d52f3a7-f23b-4a13-8d3f-4be3d6efabcd"),
                                Guid.Parse("7d52f3a7-f23b-4a13-8d3f-4be3d6efabcd")
                            }
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
                    Message = "Orden creada correctamente.",
                    Data = Guid.Parse("7c52f3a7-f23b-4a13-8d3f-4be3d6efabcd")
                };

                var responseSchema = context.SchemaGenerator.GenerateSchema(typeof(GenericResponse<Guid>), context.SchemaRepository);
                response.Content["application/json"] = new OpenApiMediaType
                {
                    Schema = responseSchema,
                    Example = ToOpenApiObject(responseBody)
                };
            }
        }

        // GET /orders
        if (method == "GET" && path == "api/orders")
        {
            if (operation.Responses.TryGetValue("200", out var response))
            {
                var cliente = new ClientResponseDto
                {
                    ClientId = Guid.Parse("5c52f3a7-f23b-4a13-8d3f-4be3d6efabcd"),
                    Dni = "12345678",
                    Names = "Juan",
                    LastNames = "Pérez",
                    Age = 25
                };

                var libro = new BookResponseDto
                {
                    BookId = Guid.Parse("6d52f3a7-f23b-4a13-8d3f-4be3d6efabcd"),
                    Name = "Cien años de soledad",
                    Author = "Gabriel García Márquez",
                    Isbn = "9781234567890",
                    Status = true
                };

                var orden = new OrderResponseDto
                {
                    Id = Guid.Parse("7c52f3a7-f23b-4a13-8d3f-4be3d6efabcd"),
                    RegisterTime = DateTime.Now,
                    Status = true,
                    Client = cliente,
                    Books = new List<BookResponseDto> { libro }
                };

                var responseBody = new GenericResponse<ICollection<OrderResponseDto>>
                {
                    Success = true,
                    Message = "Lista de órdenes obtenida correctamente.",
                    Data = new List<OrderResponseDto> { orden }
                };

                var responseSchema = context.SchemaGenerator.GenerateSchema(typeof(GenericResponse<ICollection<OrderResponseDto>>), context.SchemaRepository);
                response.Content["application/json"] = new OpenApiMediaType
                {
                    Schema = responseSchema,
                    Example = ToOpenApiObject(responseBody)
                };
            }
        }

        // GET /orders/client/{dni}/books
        if (method == "GET" && path.StartsWith("api/orders/client/") && path.EndsWith("/books"))
        {
            if (operation.Responses.TryGetValue("200", out var response))
            {
                var libro = new BookResponseDto
                {
                    BookId = Guid.Parse("6d52f3a7-f23b-4a13-8d3f-4be3d6efabcd"),
                    Name = "Cien años de soledad",
                    Author = "Gabriel García Márquez",
                    Isbn = "9781234567890",
                    Status = true
                };

                var responseBody = new GenericResponse<ICollection<BookResponseDto>>
                {
                    Success = true,
                    Message = "Libros prestados obtenidos correctamente.",
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

        // GET /orders/client/{clientId}/orders
        if (method == "GET" && path.StartsWith("api/orders/client/") && path.EndsWith("/orders"))
        {
            if (operation.Responses.TryGetValue("200", out var response))
            {
                var cliente = new ClientResponseDto
                {
                    ClientId = Guid.Parse("5c52f3a7-f23b-4a13-8d3f-4be3d6efabcd"),
                    Dni = "12345678",
                    Names = "Juan",
                    LastNames = "Pérez",
                    Age = 25
                };

                var libro = new BookResponseDto
                {
                    BookId = Guid.Parse("6d52f3a7-f23b-4a13-8d3f-4be3d6efabcd"),
                    Name = "Cien años de soledad",
                    Author = "Gabriel García Márquez",
                    Isbn = "9781234567890",
                    Status = true
                };

                var orden = new OrderByClientResponseDto
                {
                    Id = Guid.Parse("7c52f3a7-f23b-4a13-8d3f-4be3d6efabcd"),
                    RegisterTime = DateTime.Now,
                    Status = true,
                    Books = new List<BookResponseDto> { libro }
                };

                var responseBody = new GenericResponse<ICollection<OrderByClientResponseDto>>
                {
                    Success = true,
                    Message = "Órdenes del cliente obtenidas correctamente.",
                    Data = new List<OrderByClientResponseDto> { orden }
                };

                var responseSchema = context.SchemaGenerator.GenerateSchema(typeof(GenericResponse<ICollection<OrderByClientResponseDto>>), context.SchemaRepository);
                response.Content["application/json"] = new OpenApiMediaType
                {
                    Schema = responseSchema,
                    Example = ToOpenApiObject(responseBody)
                };
            }
        }

        // GET /orders/book/{bookId}/orders
        if (method == "GET" && path.StartsWith("api/orders/book/") && path.EndsWith("/orders"))
        {
            if (operation.Responses.TryGetValue("200", out var response))
            {
                var cliente = new ClientResponseDto
                {
                    ClientId = Guid.Parse("5c52f3a7-f23b-4a13-8d3f-4be3d6efabcd"),
                    Dni = "12345678",
                    Names = "Juan",
                    LastNames = "Pérez",
                    Age = 25
                };

                var orden = new OrderByBookResponseDto
                {
                    Id = Guid.Parse("7c52f3a7-f23b-4a13-8d3f-4be3d6efabcd"),
                    RegisterTime = DateTime.Now,
                    Status = true,
                    Client = cliente
                };

                var responseBody = new GenericResponse<ICollection<OrderByBookResponseDto>>
                {
                    Success = true,
                    Message = "Órdenes del libro obtenidas correctamente.",
                    Data = new List<OrderByBookResponseDto> { orden }
                };

                var responseSchema = context.SchemaGenerator.GenerateSchema(typeof(GenericResponse<ICollection<OrderByBookResponseDto>>), context.SchemaRepository);
                response.Content["application/json"] = new OpenApiMediaType
                {
                    Schema = responseSchema,
                    Example = ToOpenApiObject(responseBody)
                };
            }
        }

        // PATCH /orders/{orderId}/status
        if (method == "PATCH" && path.StartsWith("api/orders/") && path.EndsWith("/status"))
        {
            if (operation.Responses.TryGetValue("200", out var response))
            {
                var responseBody = new BaseGenericResponse
                {
                    Success = true,
                    Message = "Estado de la orden actualizado correctamente."
                };

                var responseSchema = context.SchemaGenerator.GenerateSchema(typeof(BaseGenericResponse), context.SchemaRepository);
                response.Content["application/json"] = new OpenApiMediaType
                {
                    Schema = responseSchema,
                    Example = ToOpenApiObject(responseBody)
                };
            }
        }

        // DELETE /orders/{orderId}
        if (method == "DELETE" && path.StartsWith("api/orders/"))
        {
            if (operation.Responses.TryGetValue("200", out var response))
            {
                var responseBody = new BaseGenericResponse
                {
                    Success = true,
                    Message = "Orden eliminada correctamente."
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
                case DateTime dt:
                    openApiObj[key] = new OpenApiString(dt.ToString("yyyy-MM-ddTHH:mm:ss"));
                    break;
                case ICollection<OrderResponseDto> list:
                    var array = new OpenApiArray();
                    foreach (var item in list)
                        array.Add(ToOpenApiObject(item));
                    openApiObj[key] = array;
                    break;
                case ICollection<OrderByClientResponseDto> list:
                    var clientArray = new OpenApiArray();
                    foreach (var item in list)
                        clientArray.Add(ToOpenApiObject(item));
                    openApiObj[key] = clientArray;
                    break;
                case ICollection<OrderByBookResponseDto> list:
                    var bookArray = new OpenApiArray();
                    foreach (var item in list)
                        bookArray.Add(ToOpenApiObject(item));
                    openApiObj[key] = bookArray;
                    break;
                case ICollection<BookResponseDto> list:
                    var bookResponseArray = new OpenApiArray();
                    foreach (var item in list)
                        bookResponseArray.Add(ToOpenApiObject(item));
                    openApiObj[key] = bookResponseArray;
                    break;
                case ICollection<Guid> list:
                    var guidArray = new OpenApiArray();
                    foreach (var item in list)
                        guidArray.Add(new OpenApiString(item.ToString()));
                    openApiObj[key] = guidArray;
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