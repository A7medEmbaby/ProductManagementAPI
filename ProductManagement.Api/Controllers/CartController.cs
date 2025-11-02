using Microsoft.AspNetCore.Mvc;
using MediatR;
using ProductManagement.Application.Cart.Commands;
using ProductManagement.Application.Cart.Queries;
using ProductManagement.Application.Cart.DTOs;
using ProductManagement.Contracts.Cart;
using ProductManagement.Contracts.Common;
using System.Net;

namespace ProductManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly IMediator _mediator;

    public CartController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get current user's cart
    /// </summary>
    [HttpGet("GetCartByUserId/{userId}")]
    public async Task<ActionResult<APIResponse<CartResponse>>> GetCart(Guid userId)
    {
        var query = new GetCartQuery(userId);
        var result = await _mediator.Send(query);

        if (result == null)
        {
            var errorResponse = new APIResponse<CartResponse>(HttpStatusCode.NotFound, $"Cart not found for user {userId}");
            return NotFound(errorResponse);
        }

        var response = new APIResponse<CartResponse>(HttpStatusCode.OK, result, "Cart retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Add item to cart
    /// </summary>
    [HttpPost("AddItemByUserId/{userId}")]
    public async Task<ActionResult<APIResponse<CartResponse>>> AddItem(Guid userId, [FromBody] AddItemToCartRequest request)
    {
        try
        {
            var command = new AddItemToCartCommand(userId, request.ProductId, request.Quantity);
            var result = await _mediator.Send(command);
            var response = new APIResponse<CartResponse>(HttpStatusCode.OK, result, "Item added to cart successfully");
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            var errorResponse = new APIResponse<CartResponse>(HttpStatusCode.NotFound, ex.Message);
            return NotFound(errorResponse);
        }
        catch (InvalidOperationException ex)
        {
            var errorResponse = new APIResponse<CartResponse>(HttpStatusCode.BadRequest, ex.Message);
            return BadRequest(errorResponse);
        }
    }

    /// <summary>
    /// Remove item from cart
    /// </summary>
    [HttpDelete("RemoveItem/{userId}/{itemId}")]
    public async Task<ActionResult<APIResponse<CartResponse>>> RemoveItem(Guid userId, Guid itemId)
    {
        try
        {
            var command = new RemoveItemFromCartCommand(userId, itemId);
            var result = await _mediator.Send(command);
            var response = new APIResponse<CartResponse>(HttpStatusCode.OK, result, "Item removed from cart successfully");
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            var errorResponse = new APIResponse<CartResponse>(HttpStatusCode.NotFound, ex.Message);
            return NotFound(errorResponse);
        }
    }

    /// <summary>
    /// Update cart item quantity
    /// </summary>
    [HttpPut("UpdateItemQuantity/{userId}/{itemId}")]
    public async Task<ActionResult<APIResponse<CartResponse>>> UpdateItemQuantity(
        Guid userId,
        Guid itemId,
        [FromBody] UpdateCartItemQuantityRequest request)
    {
        try
        {
            var command = new UpdateCartItemQuantityCommand(userId, itemId, request.NewQuantity);
            var result = await _mediator.Send(command);
            var response = new APIResponse<CartResponse>(HttpStatusCode.OK, result, "Cart item quantity updated successfully");
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            var errorResponse = new APIResponse<CartResponse>(HttpStatusCode.NotFound, ex.Message);
            return NotFound(errorResponse);
        }
        catch (InvalidOperationException ex)
        {
            var errorResponse = new APIResponse<CartResponse>(HttpStatusCode.BadRequest, ex.Message);
            return BadRequest(errorResponse);
        }
    }

    /// <summary>
    /// Clear cart
    /// </summary>
    [HttpDelete("ClearCartByUserId/{userId}")]
    public async Task<ActionResult<APIResponse<string>>> ClearCart(Guid userId)
    {
        try
        {
            var command = new ClearCartCommand(userId);
            await _mediator.Send(command);
            var response = new APIResponse<string>(HttpStatusCode.NoContent, null, "Cart cleared successfully");
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            var errorResponse = new APIResponse<string>(HttpStatusCode.NotFound, ex.Message);
            return NotFound(errorResponse);
        }
    }

    /// <summary>
    /// Checkout cart
    /// </summary>
    [HttpPost("CheckoutByUserId/{userId}")]
    public async Task<ActionResult<APIResponse<CartResponse>>> Checkout(Guid userId)
    {
        try
        {
            var command = new CheckoutCartCommand(userId);
            var result = await _mediator.Send(command);
            var response = new APIResponse<CartResponse>(HttpStatusCode.OK, result, "Cart checked out successfully");
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            var errorResponse = new APIResponse<CartResponse>(HttpStatusCode.NotFound, ex.Message);
            return NotFound(errorResponse);
        }
        catch (InvalidOperationException ex)
        {
            var errorResponse = new APIResponse<CartResponse>(HttpStatusCode.BadRequest, ex.Message);
            return BadRequest(errorResponse);
        }
    }
}