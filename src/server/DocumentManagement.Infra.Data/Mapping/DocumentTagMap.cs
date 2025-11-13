using DocumentManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocumentManagement.Infra.Data.Mapping
{
    public class DocumentTagMap : IEntityTypeConfiguration<DocumentTag>
    {
        public void Configure(EntityTypeBuilder<DocumentTag> builder)
        {
            builder.ToTable("DocumentTag");

            builder.HasKey(dt => new { dt.DocumentId, dt.TagId });

            builder.HasOne(dt => dt.Document)
                  .WithMany(d => d.DocumentTags) 
                  .HasForeignKey(dt => dt.DocumentId);

            builder.HasOne(dt => dt.Tag)
                  .WithMany(t => t.DocumentTags) 
                  .HasForeignKey(dt => dt.TagId);
        }
    }
}
