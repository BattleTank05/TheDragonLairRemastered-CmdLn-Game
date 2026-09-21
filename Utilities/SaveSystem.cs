using System.Text.Json;

namespace TheDragonLairRemastered
{
    public static class SaveSystem
    {
        private static string GetSavePath(string fileName = "savegame.json")
        {
            // AppDomain.CurrentDomain.BaseDirectory; Writes to path: TheDragonLairRemastered\bin\Debug\net10.0\[file]
            string folder = Path.Combine(AppContext.BaseDirectory, "Saves"); // Writes to path: TheDragonLairRemastered\bin\Debug\net10.0\Saves
            Directory.CreateDirectory(folder);
            return Path.Combine(folder, fileName);
        }

        public static void SaveGame(GameData data)
        {
            data.SaveTime = DateTime.Now;
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(GetSavePath(), json);
        }

        public static GameData? LoadGame()
        {
            string path = GetSavePath();
            if (!File.Exists(path)) return null;

            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<GameData>(json);
        }
        public static void SaveSettings(SettingsData data)
        {
            data.SaveTime = DateTime.Now;
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(GetSavePath("TDLR_settings.json"), json);
        }

        public static SettingsData? LoadSettings()
        {
            string path = GetSavePath("TDLR_settings.json");
            if (!File.Exists(path)) return null;

            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<SettingsData>(json);
        }
    }
}