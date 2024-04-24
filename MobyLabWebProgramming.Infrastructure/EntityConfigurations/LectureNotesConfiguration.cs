using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Infrastructure.EntityConfigurations;

public class LectureNotesConfiguration : IEntityTypeConfiguration<LectureNotes>
{
    public void Configure(EntityTypeBuilder<LectureNotes> builder)
    {
        builder.Property(e => e.Id).IsRequired();
        builder.HasKey(x => x.Id);
        builder.Property(e => e.Title).HasMaxLength(255).IsRequired();
        builder.Property(e => e.Description).HasMaxLength(255).IsRequired();
        builder.Property(e => e.File).HasMaxLength(255).IsRequired();
        
        // builder.HasOne<TeacherSubject>().WithMany().HasForeignKey(x => x.TeacherSubject);
    }
}