using System.Net;

namespace ECommerceBackend.Web.ApiErrors
{
    public class ConflictError : ApiError
    {
        public ConflictError(string message = "", string errorCode = "") :
            base(message, errorCode, (int)HttpStatusCode.Conflict, HttpStatusCode.Conflict.ToString())
        {

        }
    }
}
