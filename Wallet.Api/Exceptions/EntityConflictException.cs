using System;
using Microsoft.AspNetCore.Mvc;
using Wallet.Api.Helpers;

namespace Wallet.Api.Exceptions
{
    public class EntityConflictException : WalletServiceException
    {
        public EntityConflictException()
        {
        }

        public EntityConflictException(ProblemDetails problemDetails)
            : base(problemDetails?.Detail)
        {
            ProblemDetails = problemDetails ?? throw new ArgumentNullException(nameof(problemDetails));
        }

        public EntityConflictException(string message)
            : base(message)
        {
            ProblemDetails = ProblemDetailsCreator.CreateConflictMessage(message);
        }
    }
}