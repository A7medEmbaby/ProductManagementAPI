using Microsoft.AspNetCore.Mvc;
using MediatR;
using ProductManagement.Application.Categories.Commands;
using ProductManagement.Application.Categories.Queries;
using ProductManagement.Application.Categories.DTOs;
using ProductManagement.Contracts.Common;
using ProductManagement.Contracts.Products;
using System.Net;
using ProductManagement.Contracts.Categories;

namespace ProductManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all categories
    /// </summary>
    [HttpGet("GetAllCategories")]
    public async Task<ActionResult<APIResponse<List<CategoryResponse>>>> GetCategories()
    {
        var query = new GetCategoriesQuery();
        var result = await _mediator.Send(query);
        var response = new APIResponse<List<CategoryResponse>>(HttpStatusCode.OK, result, "Categories retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Get category by ID
    /// </summary>
    [HttpGet("GetCategoryById/{id}")]
    public async Task<ActionResult<APIResponse<CategoryResponse>>> GetCategory(Guid id)
    {
        var query = new GetCategoryQuery(id);
        var result = await _mediator.Send(query);

        if (result == null)
        {
            var errorResponse = new APIResponse<CategoryResponse>(HttpStatusCode.NotFound, $"Category with ID {id} not found");
            return NotFound(errorResponse);
        }

        var response = new APIResponse<CategoryResponse>(HttpStatusCode.OK, result, "Category retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Create a new category
    /// </summary>
    [HttpPost("CreateCategory")]
    public async Task<ActionResult<APIResponse<CategoryResponse>>> CreateCategory([FromBody] CreateCategoryRequest request)
    {
        // ✅ REMOVED: ModelState.IsValid check
        // Validation is now handled automatically by ValidationBehavior before the handler executes
        // If validation fails, ValidationException will be thrown and caught by ValidationMiddleware

        try
        {
            var command = CreateCategoryCommand.FromRequest(request);
            var result = await _mediator.Send(command);
            var response = new APIResponse<CategoryResponse>(HttpStatusCode.Created, result, "Category created successfully");
            return CreatedAtAction(nameof(GetCategory), new { id = result.Id }, response);
        }
        catch (ArgumentException ex)
        {
            var errorResponse = new APIResponse<CategoryResponse>(HttpStatusCode.BadRequest, ex.Message);
            return BadRequest(errorResponse);
        }
    }

    /// <summary>
    /// Update an existing category
    /// </summary>
    [HttpPut("UpdateCategoryById/{id}")]
    public async Task<ActionResult<APIResponse<CategoryResponse>>> UpdateCategory(Guid id, [FromBody] UpdateCategoryRequest request)
    {
        // ✅ REMOVED: ModelState.IsValid check
        // Validation is now handled automatically by ValidationBehavior

        try
        {
            var command = UpdateCategoryCommand.FromRequest(id, request);
            var result = await _mediator.Send(command);
            var response = new APIResponse<CategoryResponse>(HttpStatusCode.OK, result, "Category updated successfully");
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            var errorResponse = new APIResponse<CategoryResponse>(HttpStatusCode.NotFound, ex.Message);
            return NotFound(errorResponse);
        }
    }

    /// <summary>
    /// Delete a category
    /// </summary>
    [HttpDelete("DeleteCategoryById/{id}")]
    public async Task<ActionResult<APIResponse<string>>> DeleteCategory(Guid id)
    {
        try
        {
            var command = new DeleteCategoryCommand(id);
            await _mediator.Send(command);
            var response = new APIResponse<string>(HttpStatusCode.NoContent, null, "Category deleted successfully");
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            var errorResponse = new APIResponse<string>(HttpStatusCode.NotFound, ex.Message);
            return NotFound(errorResponse);
        }
        catch (InvalidOperationException ex)
        {
            var errorResponse = new APIResponse<string>(HttpStatusCode.Conflict, ex.Message);
            return Conflict(errorResponse);
        }
    }
}