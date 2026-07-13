using System.Text.Json;
using LifeOS.Domain.Settings;

namespace LifeOS.Infrastructure.Settings;

public class SettingsService : ISettingsService
{
    private readonly string _dataFolder;
    private readonly string _settingsPath;

    public AppSettings Current { get; private set; }
    public string DatabasePath { get; }

    public SettingsService()
    {
        _dataFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LifeOS");
        Directory.CreateDirectory(_dataFolder);

        _settingsPath = Path.Combine(_dataFolder, "settings.json");
        DatabasePath = Path.Combine(_dataFolder, "lifeos.db");

        Current = Load();
    }

    private AppSettings Load()
    {
        if (!File.Exists(_settingsPath)) return new AppSettings();

        try
        {
            var json = File.ReadAllText(_settingsPath);
            return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
        }
        catch
        {
            return new AppSettings();
        }
    }

    public void Save(AppSettings settings)
    {
        Current = settings;
        var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_settingsPath, json);
    }
}
