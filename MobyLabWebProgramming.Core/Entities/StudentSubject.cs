namespace MobyLabWebProgramming.Core.Entities;

public class StudentSubject : BaseEntity
{
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = default!;
    
    public Guid SubjectId { get; set; }
    public Subject Subject { get; set; } = default!;

    public ICollection<Assignment> Assignments { get; set; } = default!;
}