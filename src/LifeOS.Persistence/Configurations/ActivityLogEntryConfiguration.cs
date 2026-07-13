using LifeOS.Domain.Activity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeOS.Persistence.Configurations;

public class ActivityLogEntryConfiguration : IEntityTypeConfiguration<ActivityLogEntry>
{
    public void Configure(EntityTypeBuilder<ActivityLogEntry> builder)
    {
        builder.ToTable("ActivityLog");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Title).IsRequired().HasMaxLength(200);
    }
}