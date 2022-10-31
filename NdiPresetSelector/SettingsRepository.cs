using System.IO;
using System.Text.Json;

namespace NdiPresetSelector;


internal class SettingsRepository : ISettingsRepository
{
    public void SaveSettings(Settings settings)
    {
        string json = JsonSerializer.Serialize(settings);
        string filePath = GetSettingsFilePath();
        using FileStream stream = new(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
        using StreamWriter writer = new(stream);
        writer.Write(json);
    }


    public Settings LoadSettings()
    {
        Settings settings = null;
        string filePath = GetSettingsFilePath();
        try
        {
            using FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using StreamReader reader = new(stream);
            string json = reader.ReadToEnd();
            settings = JsonSerializer.Deserialize<Settings>(json);
        }
        catch (FileNotFoundException) { }
        return settings ?? new Settings();
    }


    private string GetSettingsFilePath()
    {
        string tempFolderPath = Path.GetTempPath();
        return Path.Combine(tempFolderPath, "NdiPresetSelector.Settings.json");
    }
}


internal interface ISettingsRepository
{
    public void SaveSettings(Settings settings);
    public Settings LoadSettings();
}