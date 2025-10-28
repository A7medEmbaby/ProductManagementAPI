using System.Net;

namespace ProductManagement.Contracts.Common
{
    public class APIResponse<T>
    {
        public HttpStatusCode? StatusCode { get; set; }
        public object Error { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }

        // Constructor for a Successful Response
        public APIResponse(HttpStatusCode statusCode, T data, string message = "")
        {
            StatusCode = statusCode;
            Data = data;
            Message = message;
            Error = null;
        }

        // Constructor for an Error Response
        public APIResponse(HttpStatusCode statusCode, string message, object error = null)
        {
            StatusCode = statusCode;
            Data = default;
            Message = message;
            Error = error;
        }
    }
}
