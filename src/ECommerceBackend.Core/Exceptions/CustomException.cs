using System;
using ECommerceBackend.Core.Helpers;

namespace ECommerceBackend.Core.Exceptions
{
    public class CustomException : Exception
    {
        public string ExceptionCode { get; set; }
        public CustomException(ExceptionCode exceptionCode)
            : base(ExceptionHelper.GetExceptionMessage(exceptionCode))
        {
            ExceptionCode = exceptionCode.ToString();
        }

        public CustomException(ExceptionCode exceptionCode, string message)
            : base(message)
        {
            ExceptionCode = exceptionCode.ToString();
        }
    }
}
