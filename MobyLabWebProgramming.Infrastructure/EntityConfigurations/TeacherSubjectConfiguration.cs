using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Infrastructure.EntityConfigurations;

public class TeacherSubjectConfiguration : IEntityTypeConfiguration<TeacherSubject>
{
    public void Configure(EntityTypeBuilder<TeacherSubject> builder)
    {
        builder.Property(e => e.TeacherId).IsRequired();
        builder.HasKey(x => new { x.TeacherId, x.SubjectId });
        // builder.HasOne<Teacher>().WithMany().HasForeignKey(x => x.TeacherId);
        // builder.HasOne<Subject>().WithMany().HasForeignKey(x => x.SubjectId);
    }
}