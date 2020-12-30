using System.Security.Claims;

namespace TaskQueue.API.Util
{
    public static class PrincipalExtensions
    {
        public static int GetId(this ClaimsPrincipal principal)
        {
            return int.Parse(principal.FindFirst("UserId").Value);
        }
    }
}