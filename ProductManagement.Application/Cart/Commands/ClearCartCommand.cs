using MediatR;
using ProductManagement.Domain.Cart.ValueObjects;

namespace ProductManagement.Application.Cart.Commands;

public record ClearCartCommand(
    Guid UserId
) : IRequest<Unit>
{
    public UserId GetUserId() => Domain.Cart.ValueObjects.UserId.Create(UserId);
}