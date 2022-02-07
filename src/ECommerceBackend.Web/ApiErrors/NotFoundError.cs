using System.Net;

namespace ECommerceBackend.Web.ApiErrors
{
    public class NotFoundError : ApiError
    {
        public NotFoundError(string message = "", string errorCode = "") :
            base(message, errorCode, (int)HttpStatusCode.NotFound, HttpStatusCode.NotFound.ToString())
        {

        }
    }
}
