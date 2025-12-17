using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Addresses;
using CleanArchAcceleratorTools.Infrastructure.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchAcceleratorTools.Examples.Infrastructure.Data.Mappings.Aggregates.Addresses;

public class CityMap : EntityTypeConfiguration<City>
{
    public override void Map(EntityTypeBuilder<City> builder)
    {
        builder.ToTable("TB_CITY");

        builder.HasKey(x => x.Id)
               .HasName("SQ_CITY");

        builder.Property(x => x.Id)
               .HasColumnName("SQ_CITY")
               .HasColumnType("bigint")
               .IsRequired()
               .UseIdentityColumn();

        builder.Property(x => x.StateId)
               .HasColumnName("SQ_STATE")
               .HasColumnType("bigint");

        builder.Property(x => x.Code)
               .HasColumnName("CD_CITY")
               .HasColumnType("nvarchar(50)");

        builder.Property(x => x.Name)
               .HasColumnName("NM_CITY")
               .HasColumnType("nvarchar(150)")
               .IsRequired();

        builder.Property(x => x.CreatedAt)
               .HasColumnName("DT_CREATION")
               .HasColumnType("datetime");

        builder.HasOne(x => x.State)
               .WithMany()
               .HasForeignKey(x => x.StateId);
    }
}