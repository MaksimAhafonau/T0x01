using SpaceDC.Models.Enums;

namespace SpaceDC.DTOs.Resources;

public sealed class UpdateResourceRequest
{
    public ResourceType Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public ResourceStatus Status { get; set; }
}
