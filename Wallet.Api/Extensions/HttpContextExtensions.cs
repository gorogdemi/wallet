using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Wallet.Api.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetUserId(this HttpContext httpContext) => httpContext.User.Claims.Single(x => x.Type == "id").Value;
    }
}