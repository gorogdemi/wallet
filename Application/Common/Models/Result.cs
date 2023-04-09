namespace DevQuarter.Wallet.Application.Common.Models
{
    public class Result
    {
        internal Result(bool succeeded, IEnumerable<string> errors)
        {
            Succeeded = succeeded;
            Errors = errors.ToArray();
        }

        public IEnumerable<string> Errors { get; init; }

        public bool Succeeded { get; init; }

        public static Result Failure(IEnumerable<string> errors) => new(false, errors);

        public static Result Success() => new(true, Array.Empty<string>());
    }
}