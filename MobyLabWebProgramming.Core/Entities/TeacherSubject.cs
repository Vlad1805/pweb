namespace MobyLabWebProgramming.Core.Entities;

public class TeacherSubject : BaseEntity
{
    public Guid TeacherId { get; set; }
    public Teacher Teacher { get; set; } = default!;
    
    public Guid SubjectId { get; set; }
    public Subject Subject { get; set; } = default!;
    
    public ICollection<LectureNotes> LectureNotes { get; set; } = default!;
}