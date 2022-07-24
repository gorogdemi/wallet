using System;
using Microsoft.AspNetCore.Mvc;
using Wallet.Api.Helpers;

namespace Wallet.Api.Exceptions
{
    public class EntityNotFoundException : WalletServiceException
    {
        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(ProblemDetails problemDetails)
            : base(problemDetails?.Detail)
        {
            ProblemDetails = problemDetails ?? throw new ArgumentNullException(nameof(problemDetails));
        }

        public EntityNotFoundException(string message)
            : base(message)
        {
            ProblemDetails = ProblemDetailsCreator.CreateNotFoundMessage(message);
        }
    }
}