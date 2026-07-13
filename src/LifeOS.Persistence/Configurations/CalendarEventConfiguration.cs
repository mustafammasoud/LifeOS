using LifeOS.Domain.Calendar;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeOS.Persistence.Configurations;

public class CalendarEventConfiguration : IEntityTypeConfiguration<CalendarEvent>
{
    public void Configure(EntityTypeBuilder<CalendarEvent> builder)
    {
        builder.ToTable("CalendarEvents");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Title).IsRequired().HasMaxLength(200);
    }
}
