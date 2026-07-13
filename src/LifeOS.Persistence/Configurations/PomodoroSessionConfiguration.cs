using LifeOS.Domain.Study;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeOS.Persistence.Configurations;

public class PomodoroSessionConfiguration : IEntityTypeConfiguration<PomodoroSession>
{
    public void Configure(EntityTypeBuilder<PomodoroSession> builder)
    {
        builder.ToTable("PomodoroSessions");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Notes).HasMaxLength(1000);
    }
}
