using Microsoft.EntityFrameworkCore;
using SpaceDC.Data;
using SpaceDC.DTOs.Resources;
using SpaceDC.Models;

namespace SpaceDC.Services;

public sealed class ResourceService : IResourceService
{
    private readonly AppDbContext _db;

    public ResourceService(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<ResourceDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var list = await _db.Resources.AsNoTracking().OrderBy(x => x.Name).ToListAsync(cancellationToken);
        return list.Select(Map).ToList();
    }

    public async Task<ResourceDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _db.Resources.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (entity is null)
            throw new AppException(404, "Resource not found.");
        return Map(entity);
    }

    public async Task<ResourceDto> CreateAsync(CreateResourceRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new Resource
        {
            Id = Guid.NewGuid(),
            Type = request.Type,
            Name = request.Name.Trim(),
            Capacity = request.Capacity,
            Status = request.Status
        };
        await _db.Resources.AddAsync(entity, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
        return Map(entity);
    }

    public async Task<ResourceDto> UpdateAsync(Guid id, UpdateResourceRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _db.Resources.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (entity is null)
            throw new AppException(404, "Resource not found.");

        entity.Type = request.Type;
        entity.Name = request.Name.Trim();
        entity.Capacity = request.Capacity;
        entity.Status = request.Status;
        await _db.SaveChangesAsync(cancellationToken);
        return Map(entity);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _db.Resources.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (entity is null)
            throw new AppException(404, "Resource not found.");
        _db.Resources.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }

    private static ResourceDto Map(Resource r) => new()
    {
        Id = r.Id,
        Type = r.Type,
        Name = r.Name,
        Capacity = r.Capacity,
        Status = r.Status
    };
}
