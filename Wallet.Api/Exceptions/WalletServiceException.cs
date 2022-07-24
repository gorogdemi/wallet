using System;
using Microsoft.AspNetCore.Mvc;
using Wallet.Api.Helpers;

namespace Wallet.Api.Exceptions
{
    public class WalletServiceException : Exception
    {
        public WalletServiceException()
        {
        }

        public WalletServiceException(ProblemDetails problemDetails)
            : base(problemDetails?.Detail)
        {
            ProblemDetails = problemDetails ?? throw new ArgumentNullException(nameof(problemDetails));
        }

        public WalletServiceException(string message)
            : base(message)
        {
            ProblemDetails = ProblemDetailsCreator.CreateInternalServerErrorMessage(message);
        }

        public WalletServiceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ProblemDetails ProblemDetails { get; set; }
    }
}