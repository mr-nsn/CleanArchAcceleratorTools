using CleanArchAcceleratorTools.Infrastructure.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Addresses;

namespace CleanArchAcceleratorTools.Examples.Infrastructure.Data.Mappings.Aggregates.Addresses;

public class StateMap : EntityTypeConfiguration<State>
{
    public override void Map(EntityTypeBuilder<State> builder)
    {
        builder.ToTable("TB_STATE");

        builder.HasKey(x => x.Id)
               .HasName("SQ_STATE");

        builder.Property(x => x.Id)
               .HasColumnName("SQ_STATE")
               .HasColumnType("bigint")
               .IsRequired()
               .UseIdentityColumn();

        builder.Property(x => x.CountryId)
               .HasColumnName("SQ_COUNTRY")
               .HasColumnType("bigint");

        builder.Property(x => x.Code)
               .HasColumnName("CD_STATE")
               .HasColumnType("nvarchar(50)");

        builder.Property(x => x.Name)
               .HasColumnName("NM_STATE")
               .HasColumnType("nvarchar(150)")
               .IsRequired();

        builder.Property(x => x.Abbreviation)
               .HasColumnName("TX_ABBREVIATION")
               .HasColumnType("nvarchar(50)");        

        builder.Property(x => x.CreatedAt)
               .HasColumnName("DT_CREATION")
               .HasColumnType("datetime");        

        builder.HasOne(x => x.Country)
               .WithMany()
               .HasForeignKey(x => x.CountryId);
    }
}