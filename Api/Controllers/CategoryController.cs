using BaseBackend.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BaseBackend.Domain.Dtos;

namespace BaseBackend.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly CategoryService _service;

    public CategoryController(CategoryService service)
    {
        _service = service;
    }

    private int UserId =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // ─────────────────────────────
    // GET ALL
    // ─────────────────────────────
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync(UserId);
        return Ok(result);
    }

    // ─────────────────────────────
    // GET BY ID
    // ─────────────────────────────
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _service.GetByIdAsync(UserId, id);
        return Ok(category);
    }

    // ─────────────────────────────
    // CREATE
    // ─────────────────────────────
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
    {
        await _service.CreateAsync(UserId, dto);
        return StatusCode(StatusCodes.Status201Created);
    }

    // ─────────────────────────────
    // UPDATE (SIN ID EN BODY)
    // ─────────────────────────────
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateCategoryDto dto
    )
    {
        await _service.UpdateAsync(UserId, id, dto);
        return NoContent();
    }

    // ─────────────────────────────
    // DELETE
    // ─────────────────────────────
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(UserId, id);
        return NoContent();
    }
}