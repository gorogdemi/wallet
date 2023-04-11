namespace DevQuarter.Wallet.Application.Common.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(IEnumerable<string> errors)
        {
            Errors = errors.ToArray();
        }

        public BadRequestException()
        {
        }

        public BadRequestException(string message)
            : base(message)
        {
        }

        public BadRequestException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public IEnumerable<string> Errors { get; }
    }
}