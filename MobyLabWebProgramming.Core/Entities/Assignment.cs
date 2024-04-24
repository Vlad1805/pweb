namespace MobyLabWebProgramming.Core.Entities;

public class Assignment : BaseEntity
{
    public string Name { get; set; } = default!;
    public int DueDate { get; set; } = default!;
    public AssignmentStatus Grade { get; set; }
    public string Answer { get; set; } = default!;
    
    public Guid StudentSubjectId { get; set; }
    public StudentSubject StudentSubject { get; set; } = default!;
}