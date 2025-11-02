using MediatR;
using ProductManagement.Application.Cart.Commands;

namespace ProductManagement.Application.Cart.Handlers;

public class ClearCartHandler : IRequestHandler<ClearCartCommand, Unit>
{
    private readonly ICartRepository _cartRepository;

    public ClearCartHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<Unit> Handle(ClearCartCommand request, CancellationToken cancellationToken)
    {
        // Delete cart
        await _cartRepository.DeleteAsync(request.GetUserId(), cancellationToken);

        return Unit.Value;
    }
}