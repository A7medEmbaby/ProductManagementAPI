using Microsoft.AspNetCore.Mvc;
using MediatR;
using ProductManagement.Application.Products.Commands;
using ProductManagement.Application.Products.Queries;
using ProductManagement.Application.Products.DTOs;
using ProductManagement.Application.Common;
using System.Net;

namespace ProductManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all products with pagination
    /// </summary>
    [HttpGet("GetAllProducts")]
    public async Task<ActionResult<APIResponse<PagedResult<ProductResponse>>>> GetProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var query = new GetProductsQuery(pageNumber, pageSize);
        var result = await _mediator.Send(query);
        var response = new APIResponse<PagedResult<ProductResponse>>(HttpStatusCode.OK, result, "Products retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Get product by ID
    /// </summary>
    [HttpGet("GetProductBy/{id}")]
    public async Task<ActionResult<APIResponse<ProductResponse>>> GetProduct(Guid id)
    {
        var query = new GetProductQuery(id);
        var result = await _mediator.Send(query);

        if (result == null)
        {
            var errorResponse = new APIResponse<ProductResponse>(HttpStatusCode.NotFound, $"Product with ID {id} not found");
            return NotFound(errorResponse);
        }

        var response = new APIResponse<ProductResponse>(HttpStatusCode.OK, result, "Product retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Get products by category ID
    /// </summary>
    [HttpGet("GetProductsByCategoryId/{categoryId}")]
    public async Task<ActionResult<APIResponse<List<ProductResponse>>>> GetProductsByCategory(Guid categoryId)
    {
        var query = new GetProductsByCategoryQuery(categoryId);
        var result = await _mediator.Send(query);
        var response = new APIResponse<List<ProductResponse>>(HttpStatusCode.OK, result, "Products retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    [HttpPost("CreateProduct")]
    public async Task<ActionResult<APIResponse<ProductResponse>>> CreateProduct([FromBody] CreateProductRequest request)
    {
        if (!ModelState.IsValid)
        {
            var validationResponse = new APIResponse<ProductResponse>(HttpStatusCode.BadRequest, "Validation failed", ModelState);
            return BadRequest(validationResponse);
        }

        try
        {
            var command = CreateProductCommand.FromRequest(request);
            var result = await _mediator.Send(command);
            var response = new APIResponse<ProductResponse>(HttpStatusCode.Created, result, "Product created successfully");
            return CreatedAtAction(nameof(GetProduct), new { id = result.Id }, response);
        }
        catch (ArgumentException ex)
        {
            var errorResponse = new APIResponse<ProductResponse>(HttpStatusCode.BadRequest, ex.Message);
            return BadRequest(errorResponse);
        }
    }

    /// <summary>
    /// Update an existing product
    /// </summary>
    [HttpPut("UpdateProductById/{id}")]
    public async Task<ActionResult<APIResponse<ProductResponse>>> UpdateProduct(Guid id, [FromBody] UpdateProductRequest request)
    {
        if (!ModelState.IsValid)
        {
            var validationResponse = new APIResponse<ProductResponse>(HttpStatusCode.BadRequest, "Validation failed", ModelState);
            return BadRequest(validationResponse);
        }

        try
        {
            var command = UpdateProductCommand.FromRequest(id, request);
            var result = await _mediator.Send(command);
            var response = new APIResponse<ProductResponse>(HttpStatusCode.OK, result, "Product updated successfully");
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            var errorResponse = new APIResponse<ProductResponse>(HttpStatusCode.NotFound, ex.Message);
            return NotFound(errorResponse);
        }
    }

    /// <summary>
    /// Delete a product
    /// </summary>
    [HttpDelete("DeleteProductById/{id}")]
    public async Task<ActionResult<APIResponse<string>>> DeleteProduct(Guid id)
    {
        try
        {
            var command = new DeleteProductCommand(id);
            await _mediator.Send(command);
            var response = new APIResponse<string>(HttpStatusCode.NoContent, null, "Product deleted successfully");
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            var errorResponse = new APIResponse<string>(HttpStatusCode.NotFound, ex.Message);
            return NotFound(errorResponse);
        }
    }
}