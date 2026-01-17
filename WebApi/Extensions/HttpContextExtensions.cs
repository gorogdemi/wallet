using System.Security.Claims;

namespace Wallet.WebApi.Extensions;

internal static class HttpContextExtensions
{
    public static string GetId(this ClaimsPrincipal user) => user.FindFirstValue("userid");
}