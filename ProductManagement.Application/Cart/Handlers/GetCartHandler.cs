using MediatR;
using ProductManagement.Application.Cart.DTOs;
using ProductManagement.Application.Cart.Queries;

namespace ProductManagement.Application.Cart.Handlers;

public class GetCartHandler : IRequestHandler<GetCartQuery, CartResponse?>
{
    private readonly ICartRepository _cartRepository;

    public GetCartHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<CartResponse?> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByUserIdAsync(request.GetUserId(), cancellationToken);
        return cart?.ToResponse();
    }
}