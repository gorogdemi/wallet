using FluentValidation;
using Wallet.Contracts.Requests;

namespace Wallet.Validation
{
    public class CategoryRequestValidator : AbstractValidator<CategoryRequest>
    {
        public CategoryRequestValidator()
        {
            RuleFor(request => request.Name)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}