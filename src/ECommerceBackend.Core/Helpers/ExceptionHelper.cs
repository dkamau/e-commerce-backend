using System;
using ECommerceBackend.Core.Exceptions;

namespace ECommerceBackend.Core.Helpers
{
    public static class ExceptionHelper
    {
        internal static string GetExceptionMessage(ExceptionCode exceptionCode)
        {
            switch (exceptionCode)
            {
                case ExceptionCode.InternalServerError:
                    return "An error occured while processing your request";
                case ExceptionCode.NullReference:
                    return "Value should not be null";
                case ExceptionCode.InvalidImageFile:
                    return "Invalid image file";
                case ExceptionCode.FileTooLarge:
                    return "File too large";
                case ExceptionCode.InvalidUserId:
                    return "Invalid user id";
                case ExceptionCode.UserAlreadyExists:
                    return "User already exists";
                case ExceptionCode.UserNotFound:
                    return "User not found";
                case ExceptionCode.InvalidProductId:
                    return "Invalid product id";
                case ExceptionCode.ProductAlreadyExists:
                    return "Product already exists";
                case ExceptionCode.ProductNotFound:
                    return "Product not found";
                case ExceptionCode.InvalidOrderId:
                    return "Invalid order id";
                case ExceptionCode.OrderAlreadyExists:
                    return "Order already exists";
                case ExceptionCode.OrderNotFound:
                    return "Order not found";
                default:
                    return "Invalid request";
                    
            }
        }

        internal static string GetNullExceptionMessage(string value)
        {
            return $"{value} should not be null.";
        }
    }
}
