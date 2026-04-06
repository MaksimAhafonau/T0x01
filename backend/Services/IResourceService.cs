using SpaceDC.DTOs.Resources;

namespace SpaceDC.Services;

public interface IResourceService
{
    Task<IReadOnlyList<ResourceDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ResourceDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ResourceDto> CreateAsync(CreateResourceRequest request, CancellationToken cancellationToken = default);
    Task<ResourceDto> UpdateAsync(Guid id, UpdateResourceRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
