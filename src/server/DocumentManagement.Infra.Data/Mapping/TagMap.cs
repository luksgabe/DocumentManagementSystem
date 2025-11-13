using DocumentManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocumentManagement.Infra.Data.Mapping
{
    public class TagMapping : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable("Tag");
            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.Id)
                .IsRequired();

            builder.Property(e => e.Name)
                .HasColumnType("nvarchar(100)")
                .IsRequired();

            builder.HasMany(t => t.DocumentTags)
              .WithOne(dt => dt.Tag)
              .HasForeignKey(dt => dt.TagId);
        }
    }
}
