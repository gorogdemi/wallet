using FluentValidation;

namespace DevQuarter.Wallet.Application.Categories
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