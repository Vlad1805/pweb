namespace MobyLabWebProgramming.Core.Entities;

public class Subject : BaseEntity
{
    public string Name { get; set; } = default!;
    public ICollection<StudentSubject> StudentSubjects = default!;
    public ICollection<TeacherSubject> TeacherSubjects = default!;
}