using System;
using ECommerceBackend.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceBackend.Web.ApiErrors
{
    public class CustomErrorResuslt : Controller
    {
        public ActionResult Error(CustomException ex)
        {
            ExceptionCode exceptionCode;
            Enum.TryParse(ex.ExceptionCode, out exceptionCode);

            if(exceptionCode.ToString().Contains("AlreadyExists"))
                return Conflict(new ConflictError(ex.Message, ex.ExceptionCode));
            if (exceptionCode.ToString().Contains("NotFound"))
                return NotFound(new NotFoundError(ex.Message, ex.ExceptionCode));

            return BadRequest(new BadRequestError(ex.Message, ex.ExceptionCode));
        }
    }
}
