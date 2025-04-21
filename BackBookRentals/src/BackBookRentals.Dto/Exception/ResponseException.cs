using System.Net;

namespace BackBookRentals.Dto.Exception;

public class ResponseException : System.Exception
{
    public HttpStatusCode StatusCode { get; }

    public ResponseException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base(message)
    {
        StatusCode = statusCode;
    }
}