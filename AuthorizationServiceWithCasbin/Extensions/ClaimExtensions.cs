using System.Linq;
using System.Security.Claims;

namespace AuthorizationServiceWithCasbin.Extensions
{
    public static class ClaimExtensions
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            var idClaim = user.Claims.FirstOrDefault(i => i.Type.Equals("UserId"));
            if (idClaim != null)
            {
                return idClaim.Value;
            }
            return "";
        }
    }
}
