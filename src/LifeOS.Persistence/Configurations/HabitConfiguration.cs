using LifeOS.Domain.Habits;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeOS.Persistence.Configurations;

public class HabitConfiguration : IEntityTypeConfiguration<Habit>
{
    public void Configure(EntityTypeBuilder<Habit> builder)
    {
        builder.ToTable("Habits");
        builder.HasKey(h => h.Id);
        builder.Property(h => h.Name).IsRequired().HasMaxLength(150);
    }
}
