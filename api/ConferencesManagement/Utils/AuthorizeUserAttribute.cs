using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace ConferencesManagementAPI.Utils
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class AuthorizeUserAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var authHeader = context.HttpContext.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    isSuccess = false,
                    Message = "Unauthorized: Missing or invalid token"
                });
                return;
            }

            var token = authHeader.Split(" ").Last();
            var handler = new JwtSecurityTokenHandler();

            try
            {
                var jwtToken = handler.ReadJwtToken(token);
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

                if (userIdClaim == null)
                {
                    context.Result = new UnauthorizedObjectResult(new
                    {
                        isSuccess = false,
                        Message = "Unauthorized: Invalid token"
                    });
                }
            }
            catch
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    isSuccess = false,
                    Message = "Unauthorized: Token could not be processed"
                });
            }
        }
    }
}
