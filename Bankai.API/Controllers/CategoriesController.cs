using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Bankai.Application.DTOs;
using Bankai.Domain.Interfaces;

namespace Bankai.API.Controllers;

/// <summary>
/// Categories API Controller — CRUD for product categories.
/// 
/// GET    /api/categories       → List all categories   [Public]
/// GET    /api/categories/{id}  → Get one category      [Public]
/// POST   /api/categories       → Create category       [Admin only]
/// PUT    /api/categories/{id}  → Update category       [Admin only]
/// DELETE /api/categories/{id}  → Delete category       [Admin only]
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CategoriesController : ControllerBase
{
    private readonly IRepository<Bankai.Domain.Entities.Category> _categoryRepository;

    public CategoriesController(IRepository<Bankai.Domain.Entities.Category> categoryRepository)
        => _categoryRepository = categoryRepository;

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IReadOnlyList<CategoryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);
        var dtos = categories.Select(c => new CategoryDto(
            c.Id, c.Name, c.Slug, c.Products.Count)).ToList();
        return Ok(dtos);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);
        if (category is null) return NotFound();
        return Ok(new CategoryDto(category.Id, category.Name, category.Slug, category.Products.Count));
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        [FromBody] string name,
        CancellationToken cancellationToken)
    {
        var category = new Bankai.Domain.Entities.Category(name);
        await _categoryRepository.AddAsync(category, cancellationToken);
        await _categoryRepository.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = category.Id },
            new CategoryDto(category.Id, category.Name, category.Slug, 0));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);
        if (category is null) return NotFound();
        _categoryRepository.Delete(category);
        await _categoryRepository.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}
