using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Infrastructure.EntityConfigurations;

public class StudentSubjectConfiguration : IEntityTypeConfiguration<StudentSubject>
{
    public void Configure(EntityTypeBuilder<StudentSubject> builder)
    {
        builder.Property(e => e.StudentId).IsRequired();
        builder.HasKey(x => new { x.StudentId, x.SubjectId });
        // builder.HasOne(e => e.Student)
        //     .WithMany(e => )
        //     .HasForeignKey(x => x.StudentId);
        // builder.HasOne<Subject>().WithMany().HasForeignKey(x => x.SubjectId);
    }
}