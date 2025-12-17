using CleanArchAcceleratorTools.Infrastructure.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Addresses;

namespace CleanArchAcceleratorTools.Examples.Infrastructure.Data.Mappings.Aggregates.Addresses;

public class CountryMap : EntityTypeConfiguration<Country>
{
    public override void Map(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable("TB_COUNTRY");

        builder.HasKey(x => x.Id)
               .HasName("SQ_COUNTRY");

        builder.Property(x => x.Id)
               .HasColumnName("SQ_COUNTRY")
               .HasColumnType("bigint")
               .IsRequired()
               .UseIdentityColumn();

        builder.Property(x => x.Code)
               .HasColumnName("CD_COUNTRY")
               .HasColumnType("nvarchar(50)");

        builder.Property(x => x.Name)
               .HasColumnName("NM_COUNTRY")
               .HasColumnType("nvarchar(150)")
               .IsRequired();        

        builder.Property(x => x.CreatedAt)
               .HasColumnName("DT_CREATION")
               .HasColumnType("datetime");
    }
}