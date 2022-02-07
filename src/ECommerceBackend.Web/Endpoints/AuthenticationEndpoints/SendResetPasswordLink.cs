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
    public class SendResetPasswordLink : BaseAsyncEndpoint<string, LoginResponse>
    {
        private readonly IAuthenticationService _authenticationService;
        protected readonly IConfiguration _configuration;

        public SendResetPasswordLink(IAuthenticationService authenticationService, IConfiguration configuration)
        {
            _authenticationService = authenticationService;
            _configuration = configuration;
        }

        [HttpGet("SendResetPasswordLink/{em}")]
        [SwaggerOperation(
            Summary = "Send's reset password link to user's email",
            Description = "Send's reset password link to user's email",
            OperationId = "Authentication.SendResetPasswordLink",
            Tags = new[] { "Authentication Endpoints" })
        ]
        public override async Task<ActionResult<LoginResponse>> HandleAsync(string em, CancellationToken cancellationToken)
        {
            try
            {
                User user = await _authenticationService.SendResetPasswordLinkAsync(em);

                if (user == null)
                    return BadRequest(new BadRequestError("Please make sure the email you've entered is the email you used to create your account."));

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
