using FluentValidation;
using ProductManagement.Application.Cart.Commands;

namespace ProductManagement.Application.Cart.Validators;

public class UpdateCartItemQuantityCommandValidator : AbstractValidator<UpdateCartItemQuantityCommand>
{
    public UpdateCartItemQuantityCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");

        RuleFor(x => x.CartItemId)
            .NotEmpty().WithMessage("Cart item ID is required");

        RuleFor(x => x.NewQuantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0");
    }
}