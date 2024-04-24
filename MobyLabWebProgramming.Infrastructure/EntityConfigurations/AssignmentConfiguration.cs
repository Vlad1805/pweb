using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Infrastructure.EntityConfigurations;

public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
{
    public void Configure(EntityTypeBuilder<Assignment> builder)
    {
        builder.Property(e => e.Id).IsRequired();
        builder.HasKey(x => x.Id);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(255);
        builder.Property(e => e.DueDate).IsRequired();
        builder.Property(e => e.Answer).HasMaxLength(255);

        builder.HasOne(e => e.StudentSubject)
            .WithMany(e => e.Assignments)
            .HasForeignKey(e => e.StudentSubjectId)
            .HasPrincipalKey(e => e.Id)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}