using DocumentManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocumentManagement.Infra.Data.Mapping
{
    public class AuditLogMap : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("AuditLogs");

            builder.HasKey(a => a.Id);
            builder.Property(e => e.Id)
                .IsRequired();

            builder.Property(a => a.WhenUtc)
                   .IsRequired();

            builder.Property(a => a.Action)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(a => a.UserSub)
                   .IsRequired()
                   .HasMaxLength(255); 

            builder.Property(a => a.UserEmail)
                   .IsRequired()
                   .HasMaxLength(255); 

            builder.Property(a => a.Entity)
                   .IsRequired()
                   .HasMaxLength(100); 

            builder.Property(a => a.EntityId)
                   .IsRequired()
                   .HasMaxLength(255); 

            builder.Property(a => a.Ip)
                   .HasMaxLength(45);


            builder.Property(a => a.Metadata)
                   .HasColumnType("nvarchar(max)");

        }
    }
}
