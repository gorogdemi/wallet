using System.Security.Claims;
using DevQuarter.Wallet.Application.Common.Interfaces;

namespace DevQuarter.Wallet.WebApi.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId => _httpContextAccessor.HttpContext?.User.FindFirstValue("userid");
    }
}