using DocumentManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocumentManagement.Infra.Data.Mapping
{
    public class DocumentsMap : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.ToTable("Document");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .IsRequired();

            builder.Property(e => e.Title)
                .HasColumnType("nvarchar(200)")
                .IsRequired();

            builder.Property(e => e.Description)
                .HasColumnType("nvarchar(1000)");

            builder.Property(e => e.AccessType)
                .HasColumnType("tinyint");

            builder.Property(e => e.FileName)
                .HasColumnType("nvarchar(255)")
                .IsRequired();

            builder.Property(e => e.FileSizeBytes)
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(e => e.ContentType)
                .HasColumnType("nvarchar(100)")
                .IsRequired();

            builder.Property(e => e.OwnerSub)
                .HasColumnType("nvarchar(100)")
                .IsRequired();

            builder.Property(e => e.OwnerEmail)
                .HasColumnType("nvarchar(100)")
                .IsRequired();

            builder.Property(e => e.CreatedAt)
                .IsRequired();

            builder.Property(e => e.UpdatedAt)
                .IsRequired(false);

            builder.Property(e => e.StorageUri);

            builder.HasMany(d => d.DocumentTags)
              .WithOne(dt => dt.Document)
              .HasForeignKey(dt => dt.DocumentId);

            builder.HasMany(d => d.Shares)
                .WithOne(s => s.Document)
                .HasForeignKey(s => s.DocumentId);

            builder.Metadata
               .FindNavigation(nameof(Document.Shares))!
               .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Metadata
               .FindNavigation(nameof(Document.DocumentTags))!
               .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
