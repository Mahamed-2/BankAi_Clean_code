using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Bankai.Application.Commands;
using Bankai.Application.DTOs;
using Bankai.Application.Queries;

namespace Bankai.API.Controllers;

/// <summary>
/// Products API Controller — Full CRUD via CQRS/MediatR.
/// 
/// GET    /api/products         → GetAllProductsQuery     [Public]
/// GET    /api/products/{id}    → GetProductByIdQuery     [Public]
/// POST   /api/products         → CreateProductCommand    [Admin only]
/// PUT    /api/products/{id}    → UpdateProductCommand    [Admin only]
/// DELETE /api/products/{id}    → DeleteProductCommand    [Admin only]
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator) => _mediator = mediator;

    // ── GET /api/products ─────────────────────────────────────
    /// <summary>Retrieve all products (public endpoint).</summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IReadOnlyList<ProductDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var products = await _mediator.Send(new GetAllProductsQuery(), cancellationToken);
        return Ok(products);
    }

    // ── GET /api/products/{id} ────────────────────────────────
    /// <summary>Retrieve a single product by ID (public endpoint).</summary>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var product = await _mediator.Send(new GetProductByIdQuery(id), cancellationToken);
        return product is null ? NotFound() : Ok(product);
    }

    // ── POST /api/products ────────────────────────────────────
    /// <summary>Create a new product (Admin only).</summary>
    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create(
        [FromBody] CreateProductCommand command,
        CancellationToken cancellationToken)
    {
        var product = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    // ── PUT /api/products/{id} ────────────────────────────────
    /// <summary>Update an existing product (Admin only).</summary>
    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateProductCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest("Route ID and body ID must match.");

        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    // ── DELETE /api/products/{id} ─────────────────────────────
    /// <summary>Delete a product permanently (Admin only).</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteProductCommand(id), cancellationToken);
        return NoContent();
    }
}
