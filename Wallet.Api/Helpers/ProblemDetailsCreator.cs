using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Wallet.Api.Helpers
{
    public static class ProblemDetailsCreator
    {
        public static ProblemDetails CreateBadRequestMessage(string detail) =>
            new ()
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1", Title = "Bad Request", Status = StatusCodes.Status400BadRequest, Detail = detail,
            };

        public static ProblemDetails CreateConflictMessage(string detail) =>
            new ()
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.8", Title = "Conflict", Status = StatusCodes.Status409Conflict, Detail = detail,
            };

        public static ProblemDetails CreateInternalServerErrorMessage(string detail) =>
            new ()
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1", Title = "Internal Server Error", Status = StatusCodes.Status500InternalServerError, Detail = detail,
            };

        public static ProblemDetails CreateNotFoundMessage(string detail) =>
            new ()
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4", Title = "Not Found", Status = StatusCodes.Status404NotFound, Detail = detail,
            };
    }
}