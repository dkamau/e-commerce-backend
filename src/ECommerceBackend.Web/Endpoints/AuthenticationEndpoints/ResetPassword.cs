using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using ECommerceBackend.Core.Entities.AuthenticationEntities;
using ECommerceBackend.Core.Exceptions;
using ECommerceBackend.Core.Interfaces;
using ECommerceBackend.Web.ApiErrors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;

namespace ECommerceBackend.Web.Endpoints.AuthenticationEndpoints
{
    [Route("/Authentication")]
    [AllowAnonymous]
    public class ResetPassword : BaseAsyncEndpoint<ResetPasswordRequest, LoginResponse>
    {
        private readonly IAuthenticationService _authenticationService;
        protected readonly IConfiguration _configuration;

        public ResetPassword(IAuthenticationService authenticationService, IConfiguration configuration)
        {
            _authenticationService = authenticationService;
            _configuration = configuration;
        }

        [HttpPost("ResetPassword")]
        [SwaggerOperation(
            Summary = "Resets a user's password",
            Description = "Resets a user's password",
            OperationId = "Authentication.ResetPassword",
            Tags = new[] { "Authentication Endpoints" })
        ]
        public override async Task<ActionResult<LoginResponse>> HandleAsync(ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            try
            {
                User user = await _authenticationService.ResetPasswordAsync(request.Email, request.Password);

                if (user == null)
                    return BadRequest(new BadRequestError("Opps! A problem was encountered while resetting your password. Please contact BET Shop for help."));

                return Ok(LoginResponse.Create(user, ""));
            }
            catch (CustomException ex)
            {
                return new CustomErrorResuslt().Error(ex);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestError(ex.Message, ExceptionCode.InternalServerError.ToString()));
            }
        }
    }
}
