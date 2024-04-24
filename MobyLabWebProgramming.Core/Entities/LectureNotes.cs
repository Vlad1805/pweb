namespace MobyLabWebProgramming.Core.Entities;

public class LectureNotes : BaseEntity
{
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string File { get; set; } = default!;
    
    public Guid TeacherSubjectId { get; set; }
    public TeacherSubject TeacherSubject { get; set; } = default!;
}