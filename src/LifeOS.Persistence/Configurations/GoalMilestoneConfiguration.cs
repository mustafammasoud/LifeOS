using LifeOS.Domain.Goals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeOS.Persistence.Configurations;

public class GoalMilestoneConfiguration : IEntityTypeConfiguration<GoalMilestone>
{
    public void Configure(EntityTypeBuilder<GoalMilestone> builder)
    {
        builder.ToTable("GoalMilestones");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Title).IsRequired().HasMaxLength(200);
    }
}
