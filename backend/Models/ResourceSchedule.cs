namespace SpaceDC.Models;

public class ResourceSchedule
{
    public Guid Id { get; set; }
    public Guid ResourceId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    public Resource Resource { get; set; } = null!;
}
