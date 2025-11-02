using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Application.Common;
using ProductManagement.Application.Orders.Commands;
using ProductManagement.Application.Orders.DTOs;
using ProductManagement.Application.Orders.Queries;
using ProductManagement.Contracts.Common;
using ProductManagement.Contracts.Orders;
using ProductManagement.Domain.Common.ValueObjects;
using ProductManagement.Domain.Orders.Commands;
using ProductManagement.Domain.Orders.Events;
using ProductManagement.Domain.Products.ValueObjects;
using System.Net;

namespace ProductManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all orders with pagination
    /// </summary>
    [HttpGet("GetAllOrders")]
    public async Task<ActionResult<APIResponse<PagedResult<OrderResponse>>>> GetOrders(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = new GetOrdersQuery(pageNumber, pageSize);
        var result = await _mediator.Send(query);
        var response = new APIResponse<PagedResult<OrderResponse>>(HttpStatusCode.OK, result, "Orders retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Get order by ID
    /// </summary>
    [HttpGet("GetOrderById/{id}")]
    public async Task<ActionResult<APIResponse<OrderResponse>>> GetOrder(Guid id)
    {
        var query = new GetOrderQuery(id);
        var result = await _mediator.Send(query);

        if (result == null)
        {
            var errorResponse = new APIResponse<OrderResponse>(HttpStatusCode.NotFound, $"Order with ID {id} not found");
            return NotFound(errorResponse);
        }

        var response = new APIResponse<OrderResponse>(HttpStatusCode.OK, result, "Order retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Get orders by user ID with pagination
    /// </summary>
    [HttpGet("GetOrdersByUserId/{userId}")]
    public async Task<ActionResult<APIResponse<PagedResult<OrderResponse>>>> GetOrdersByUser(
        Guid userId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = new GetOrdersByUserQuery(userId, pageNumber, pageSize);
        var result = await _mediator.Send(query);
        var response = new APIResponse<PagedResult<OrderResponse>>(HttpStatusCode.OK, result, "User orders retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Create a new order (typically called internally after checkout)
    /// </summary>
    [HttpPost("CreateOrder")]
    public async Task<ActionResult<APIResponse<OrderResponse>>> CreateOrder([FromBody] CreateOrderRequest request)
    {
        try
        {
            // Map request to domain items
            var items = request.Items.Select(item => new OrderItemData(
                ProductId.Create(item.ProductId),
                new ProductName(item.ProductName),
                item.Quantity,
                Money.Create(item.UnitPrice, item.Currency)
            )).ToList();

            var command = new CreateOrderCommand(request.UserId, items);
            var result = await _mediator.Send(command);
            var response = new APIResponse<OrderResponse>(HttpStatusCode.Created, result, "Order created successfully");
            return CreatedAtAction(nameof(GetOrder), new { id = result.OrderId }, response);
        }
        catch (ArgumentException ex)
        {
            var errorResponse = new APIResponse<OrderResponse>(HttpStatusCode.BadRequest, ex.Message);
            return BadRequest(errorResponse);
        }
        catch (InvalidOperationException ex)
        {
            var errorResponse = new APIResponse<OrderResponse>(HttpStatusCode.BadRequest, ex.Message);
            return BadRequest(errorResponse);
        }
    }

    /// <summary>
    /// Confirm an order
    /// </summary>
    [HttpPut("ConfirmOrder/{id}")]
    public async Task<ActionResult<APIResponse<OrderResponse>>> ConfirmOrder(Guid id)
    {
        try
        {
            var command = new ConfirmOrderCommand(id);
            var result = await _mediator.Send(command);
            var response = new APIResponse<OrderResponse>(HttpStatusCode.OK, result, "Order confirmed successfully");
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            var errorResponse = new APIResponse<OrderResponse>(HttpStatusCode.NotFound, ex.Message);
            return NotFound(errorResponse);
        }
        catch (InvalidOperationException ex)
        {
            var errorResponse = new APIResponse<OrderResponse>(HttpStatusCode.BadRequest, ex.Message);
            return BadRequest(errorResponse);
        }
    }

    /// <summary>
    /// Complete an order
    /// </summary>
    [HttpPut("CompleteOrder/{id}")]
    public async Task<ActionResult<APIResponse<OrderResponse>>> CompleteOrder(Guid id)
    {
        try
        {
            var command = new CompleteOrderCommand(id);
            var result = await _mediator.Send(command);
            var response = new APIResponse<OrderResponse>(HttpStatusCode.OK, result, "Order completed successfully");
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            var errorResponse = new APIResponse<OrderResponse>(HttpStatusCode.NotFound, ex.Message);
            return NotFound(errorResponse);
        }
        catch (InvalidOperationException ex)
        {
            var errorResponse = new APIResponse<OrderResponse>(HttpStatusCode.BadRequest, ex.Message);
            return BadRequest(errorResponse);
        }
    }

    /// <summary>
    /// Cancel an order
    /// </summary>
    [HttpPut("CancelOrder/{id}")]
    public async Task<ActionResult<APIResponse<OrderResponse>>> CancelOrder(Guid id, [FromBody] CancelOrderRequest request)
    {
        try
        {
            var command = new CancelOrderCommand(id, request.Reason);
            var result = await _mediator.Send(command);
            var response = new APIResponse<OrderResponse>(HttpStatusCode.OK, result, "Order cancelled successfully");
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            var errorResponse = new APIResponse<OrderResponse>(HttpStatusCode.NotFound, ex.Message);
            return NotFound(errorResponse);
        }
        catch (InvalidOperationException ex)
        {
            var errorResponse = new APIResponse<OrderResponse>(HttpStatusCode.BadRequest, ex.Message);
            return BadRequest(errorResponse);
        }
    }
}