using LifeOS.Domain.Goals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeOS.Persistence.Configurations;

public class GoalConfiguration : IEntityTypeConfiguration<Goal>
{
    public void Configure(EntityTypeBuilder<Goal> builder)
    {
        builder.ToTable("Goals");
        builder.HasKey(g => g.Id);
        builder.Property(g => g.Title).IsRequired().HasMaxLength(200);
        builder.Property(g => g.Description).HasMaxLength(2000);

        builder.HasMany(g => g.Milestones)
            .WithOne()
            .HasForeignKey(m => m.GoalId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
