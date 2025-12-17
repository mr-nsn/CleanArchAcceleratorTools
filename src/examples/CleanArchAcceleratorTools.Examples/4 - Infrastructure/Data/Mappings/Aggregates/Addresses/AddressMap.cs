using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Addresses;
using CleanArchAcceleratorTools.Infrastructure.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchAcceleratorTools.Examples.Infrastructure.Data.Mappings.Aggregates.Addresses;

public class AddressMap : EntityTypeConfiguration<Address>
{
    public override void Map(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("TB_ADDRESS");

        builder.HasKey(x => x.Id)
               .HasName("SQ_ADDRESS");

        builder.Property(x => x.Id)
               .HasColumnName("SQ_ADDRESS")
               .HasColumnType("bigint")
               .IsRequired()
               .UseIdentityColumn();

        builder.Property(x => x.CityId)
               .HasColumnName("SQ_CITY")
               .HasColumnType("bigint");

        builder.Property(x => x.StreetAvenue)
               .HasColumnName("TX_STREET_AVENUE")
               .HasColumnType("nvarchar(250)");

        builder.Property(x => x.Number)
               .HasColumnName("TX_NUMBER")
               .HasColumnType("nvarchar(50)");

        builder.Property(x => x.Complement)
               .HasColumnName("TX_COMPLEMENT")
               .HasColumnType("nvarchar(250)");

        builder.Property(x => x.Neighborhood)
               .HasColumnName("TX_NEIGHBORHOOD")
               .HasColumnType("nvarchar(150)");

        builder.Property(x => x.PostalCode)
               .HasColumnName("TX_POSTAL_CODE")
               .HasColumnType("nvarchar(20)");

        builder.Property(x => x.CreatedAt)
               .HasColumnName("DT_CREATION")
               .HasColumnType("datetime");

        builder.HasOne(x => x.City)
               .WithMany()
               .HasForeignKey(x => x.CityId);
    }
}