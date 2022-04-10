using System;
using System.Collections.Generic;
using System.Net;

namespace API.Helpers
{
    public class ApiErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public IEnumerable<string> Details { get; set; } = Array.Empty<string>();

        public ApiErrorResponse(HttpStatusCode statusCode)
        {
            StatusCode = (int)statusCode;
            Message = GetDefaultErrorMessageByStatusCode(statusCode);
        }

        public ApiErrorResponse(HttpStatusCode statusCode, string message)
        {
            StatusCode = (int)statusCode;
            Message = message ?? GetDefaultErrorMessageByStatusCode(statusCode);
        }

        public ApiErrorResponse(HttpStatusCode statusCode, IEnumerable<string> details) : this(statusCode)
        {
            Details = details;
        }

        public ApiErrorResponse(HttpStatusCode statusCode, string message, IEnumerable<string> details) : this(statusCode, message)
        {
            Details = details;
        }

        private string GetDefaultErrorMessageByStatusCode(HttpStatusCode statusCode)
        {
            return statusCode switch
            {
                HttpStatusCode.BadRequest => "The request could not be understood by the server due to incorrect syntax.",
                HttpStatusCode.Unauthorized => "Unauthorized or authorization required.",
                HttpStatusCode.NotFound => "The server can not find the requested resource.",
                HttpStatusCode.InternalServerError => "Internal server error.",
                _ => null
            };
        }
    }
}
