namespace MobyLabWebProgramming.Core.Entities;

public class UserFile : BaseEntity
{
    public string Path { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Description { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
}
