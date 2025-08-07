public class TaskItem
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }

    public required TaskStatus Status { get; set; }
    public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public enum TaskStatus
{
    Incomplete = 0,
    InProgress = 1,
    Complete = 2
}