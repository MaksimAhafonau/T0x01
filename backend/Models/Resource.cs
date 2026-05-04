using SpaceDC.Models.Enums;

namespace SpaceDC.Models;

public class Resource
{
    public Guid Id { get; set; }
    public ResourceType Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public ResourceStatus Status { get; set; }

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<ResourceSchedule> Schedules { get; set; } = new List<ResourceSchedule>();
}
