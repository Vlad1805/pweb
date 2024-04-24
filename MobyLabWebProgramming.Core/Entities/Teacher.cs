namespace MobyLabWebProgramming.Core.Entities;

public class Teacher : BaseEntity
{
    public string Name { get; set; } = default!;
    public ICollection<TeacherSubject> TeacherSubjects = default!;
}