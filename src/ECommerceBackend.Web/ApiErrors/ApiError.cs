namespace ECommerceBackend.Web.ApiErrors
{
    public class ApiError
    {
        public ApiError(string message = "", string errorCode = "", int? statusCode = null, string statusDescription = "")
        {
            Message = message;
            ErrorCode = errorCode;
            StatusCode = statusCode;
            StatusDescription = statusDescription;
        }

        public int? StatusCode { get; private set; }
        public string StatusDescription { get; private set; }
        public string ErrorCode { get; private set; }
        public string Message { get; private set; }
    }
}
