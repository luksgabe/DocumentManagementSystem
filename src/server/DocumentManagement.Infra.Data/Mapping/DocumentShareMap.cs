using DocumentManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocumentManagement.Infra.Data.Mapping
{
    public class DocumentShareMap : IEntityTypeConfiguration<DocumentShare>
    {
        public void Configure(EntityTypeBuilder<DocumentShare> builder)
        {
            builder.ToTable("DocumentShare");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .IsRequired();
            
            builder.Property(e => e.DocumentId)
                .IsRequired();


            builder.Property(e => e.TargetType)
                .HasColumnType("tinyint");
                
            builder.Property(e => e.TargetValue)
                .HasColumnType("nvarchar(200)")
                .IsRequired();

            builder.Property(e => e.Permission)
                .HasColumnType("tinyint");

            builder.Property(e => e.SharedAt)
                .IsRequired();

            builder.HasOne(e => e.Document)
                .WithMany()
                .HasForeignKey(ds => ds.DocumentId);
        }
    }
}
