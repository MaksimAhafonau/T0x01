using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceDC.DTOs.Resources;
using SpaceDC.Services;

namespace SpaceDC.Controllers;

[ApiController]
[Route("api/resources")]
[Authorize]
public sealed class ResourcesController : ControllerBase
{
    private readonly IResourceService _resources;

    public ResourcesController(IResourceService resources) => _resources = resources;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ResourceDto>>> GetAll(CancellationToken cancellationToken)
    {
        var list = await _resources.GetAllAsync(cancellationToken);
        return Ok(list);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ResourceDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var dto = await _resources.GetByIdAsync(id, cancellationToken);
        return Ok(dto);
    }

    [HttpPost]
    [Authorize(Policy = "Staff")]
    public async Task<ActionResult<ResourceDto>> Create([FromBody] CreateResourceRequest request, CancellationToken cancellationToken)
    {
        var dto = await _resources.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "Staff")]
    public async Task<ActionResult<ResourceDto>> Update(Guid id, [FromBody] UpdateResourceRequest request, CancellationToken cancellationToken)
    {
        var dto = await _resources.UpdateAsync(id, request, cancellationToken);
        return Ok(dto);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "Staff")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _resources.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
