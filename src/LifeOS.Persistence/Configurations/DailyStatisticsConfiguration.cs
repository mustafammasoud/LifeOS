using LifeOS.Domain.Statistics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class DailyStatisticsConfiguration
    : IEntityTypeConfiguration<DailyStatistics>
{
    public void Configure(EntityTypeBuilder<DailyStatistics> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Date)
               .IsRequired();

        builder.HasIndex(x => x.Date)
               .IsUnique();
    }
}
