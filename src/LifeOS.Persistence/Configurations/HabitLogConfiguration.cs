using LifeOS.Domain.Habits;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeOS.Persistence.Configurations;

public class HabitLogConfiguration : IEntityTypeConfiguration<HabitLog>
{
    public void Configure(EntityTypeBuilder<HabitLog> builder)
    {
        builder.ToTable("HabitLogs");
        builder.HasKey(l => l.Id);
        builder.HasIndex(l => new { l.HabitId, l.Date }).IsUnique();
    }
}
