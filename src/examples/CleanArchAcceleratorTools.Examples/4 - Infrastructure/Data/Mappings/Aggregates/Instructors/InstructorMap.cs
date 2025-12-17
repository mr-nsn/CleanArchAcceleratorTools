using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Courses;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Instructors;
using CleanArchAcceleratorTools.Infrastructure.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchAcceleratorTools.Examples.Infrastructure.Data.Mappings.Aggregates.Instructors;

public class InstructorMap : EntityTypeConfiguration<Instructor>
{
    public override void Map(EntityTypeBuilder<Instructor> builder)
    {
        builder.ToTable("TB_INSTRUCTOR");

        builder.HasKey(x => x.Id)
               .HasName("SQ_INSTRUCTOR");

        builder.Property(x => x.Id)
               .HasColumnName("SQ_INSTRUCTOR")
               .HasColumnType("bigint")
               .IsRequired()
               .UseIdentityColumn();

        builder.Property(x => x.FullName)
               .HasColumnName("TX_FULL_NAME")
               .HasColumnType("nvarchar(100)");

        builder.Property(x => x.CreatedAt)
               .HasColumnName("DT_CREATION")
               .HasColumnType("datetime");

        builder.HasOne(x => x.Profile)
               .WithOne()
               .HasForeignKey<Profile>(x => x.InstructorId);
    }
}

