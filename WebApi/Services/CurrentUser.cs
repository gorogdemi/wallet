using System.Security.Claims;
using Wallet.Application.Common.Interfaces;

namespace Wallet.WebApi.Services;

public sealed class CurrentUser : IUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string Email => _httpContextAccessor.HttpContext?.User.FindFirstValue("useremail");

    public string FullName => _httpContextAccessor.HttpContext?.User.FindFirstValue("fullname");

    public string Id => _httpContextAccessor.HttpContext?.User.FindFirstValue("userid");

    public string UserName => _httpContextAccessor.HttpContext?.User.FindFirstValue("username");
}