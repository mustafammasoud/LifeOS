using LifeOS.Domain.Settings;

namespace LifeOS.Infrastructure.Settings;

public interface ISettingsService
{
    AppSettings Current { get; }
    void Save(AppSettings settings);
    string DatabasePath { get; }
}

