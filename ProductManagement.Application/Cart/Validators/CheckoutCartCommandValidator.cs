using FluentValidation;
using ProductManagement.Application.Cart.Commands;

namespace ProductManagement.Application.Cart.Validators;

public class CheckoutCartCommandValidator : AbstractValidator<CheckoutCartCommand>
{
    public CheckoutCartCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");
    }
}