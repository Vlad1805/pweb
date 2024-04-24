namespace MobyLabWebProgramming.Core.Entities;

public class Student : BaseEntity
{
    public string Name { get; set; } = default!;
    public ICollection<StudentSubject> StudentSubjects { get; set; } = default!;
}