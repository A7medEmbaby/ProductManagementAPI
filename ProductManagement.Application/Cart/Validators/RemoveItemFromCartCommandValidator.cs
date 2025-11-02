using FluentValidation;
using ProductManagement.Application.Cart.Commands;

namespace ProductManagement.Application.Cart.Validators;

public class RemoveItemFromCartCommandValidator : AbstractValidator<RemoveItemFromCartCommand>
{
    public RemoveItemFromCartCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");

        RuleFor(x => x.CartItemId)
            .NotEmpty().WithMessage("Cart item ID is required");
    }
}