using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Instructors;
using CleanArchAcceleratorTools.Infrastructure.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchAcceleratorTools.Examples.Infrastructure.Data.Mappings.Aggregates.Instructors;

public class ProfileMap : EntityTypeConfiguration<Profile>
{
    public override void Map(EntityTypeBuilder<Profile> builder)
    {
        builder.ToTable("TB_PROFILE");

        builder.HasKey(x => x.Id)
               .HasName("SQ_PROFILE");

        builder.Property(x => x.Id)
               .HasColumnName("SQ_PROFILE")
               .HasColumnType("bigint")
               .IsRequired()
               .UseIdentityColumn();

        builder.Property(x => x.InstructorId)
               .HasColumnName("SQ_INSTRUCTOR")
               .HasColumnType("bigint");

        builder.Property(x => x.AddressId)
               .HasColumnName("SQ_ADDRESS")
               .HasColumnType("bigint");

        builder.Property(x => x.Bio)
               .HasColumnName("TX_BIO")
               .HasColumnType("nvarchar(500)");

        builder.Property(x => x.LinkedInUrl)
               .HasColumnName("TX_LINKEDIN")
               .HasColumnType("nvarchar(250)");

        builder.Property(x => x.CreatedAt)
               .HasColumnName("DT_CREATION")
               .HasColumnType("datetime");

        builder.HasOne(x => x.Address)
               .WithMany()
               .HasForeignKey(x => x.AddressId);
    }
}
