using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Courses;
using CleanArchAcceleratorTools.Infrastructure.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchAcceleratorTools.Examples.Infrastructure.Data.Mappings.Aggregates.Courses;

public class ModuleMap : EntityTypeConfiguration<Module>
{
    public override void Map(EntityTypeBuilder<Module> builder)
    {
        builder.ToTable("TB_MODULE");

        builder.HasKey(x => x.Id)
               .HasName("SQ_MODULE");

        builder.Property(x => x.Id)
               .HasColumnName("SQ_MODULE")
               .HasColumnType("bigint")
               .IsRequired()
               .UseIdentityColumn();

        builder.Property(x => x.CourseId)
               .HasColumnName("SQ_COURSE")
               .HasColumnType("bigint");

        builder.Property(x => x.Name)
               .HasColumnName("TX_NAME")
               .HasColumnType("nvarchar(100)");

        builder.Property(x => x.CreatedAt)
               .HasColumnName("DT_CREATION")
               .HasColumnType("datetime");

        builder.HasMany(x => x.Lessons)
               .WithOne()
               .HasForeignKey(x => x.ModuleId);
    }
}
