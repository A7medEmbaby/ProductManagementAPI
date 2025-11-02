using FluentValidation;
using ProductManagement.Application.Orders.Commands;
using ProductManagement.Domain.Orders.Commands;

namespace ProductManagement.Application.Orders.Validators;

public class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
{
    public CancelOrderCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("Order ID is required");

        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Cancellation reason is required")
            .MaximumLength(500).WithMessage("Reason cannot exceed 500 characters");
    }
}