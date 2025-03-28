using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace ConferencesManagementAPI.Utils
{
    public static class JwtHelper
    {
        public static int? GetUserIdFromToken(HttpContext httpContext)
        {
            try
            {
                var authHeader = httpContext.Request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                {
                    return null;
                }

                var token = authHeader.Substring("Bearer ".Length).Trim();
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "id");

                return userIdClaim != null ? int.Parse(userIdClaim.Value) : null;
            }
            catch(Exception ex) {
                System.Console.Out.WriteAsync("Exception in ConferencesManagementAPI.Utils.JwtHelper function GetUserIdFromToken: " +ex.Message);
                return null;
            }

        }
    }
}
