using BaseBackend.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BaseBackend.Application.DTOs.Transaction;

namespace BaseBackend.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly TransactionService _service;

    public TransactionController(TransactionService service)
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
        var transaction = await _service.GetByIdAsync(UserId, id);
        return Ok(transaction);
    }

    // ─────────────────────────────
    // GET BY FILTER
    // ─────────────────────────────
    [HttpGet("filter")]
    public async Task<IActionResult> GetByFilter(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] int? categoryId,
        [FromQuery] int? type
    )
    {
        var result = await _service.GetByFilterAsync(
            UserId,
            from,
            to,
            categoryId,
            type
        );

        return Ok(result);
    }

    // ─────────────────────────────
    // CREATE
    // ─────────────────────────────
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateTransactionDto dto
    )
    {
        await _service.CreateAsync(UserId, dto);
        return StatusCode(StatusCodes.Status201Created);
    }

    // ─────────────────────────────
    // UPDATE
    // ─────────────────────────────
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateTransactionDto dto
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