using System.Text.Json;

namespace TheDragonLairRemastered
{
    public static class DataSystem
    {
        // Core utility: takes a filename and folder name and constructs paths
        private static string GetDataPath(string fileName, string folder)
        {
            string folderPath = Path.Combine(AppContext.BaseDirectory, folder); // Writes to path: TheDragonLairRemastered\bin\Debug\net10.0\[folder]\[filename]
            Directory.CreateDirectory(folder);
            return Path.Combine(folderPath, fileName);
        }

#region Game State I/O
        public static void SaveGame(GameData data)
        {
            data.SaveTime = DateTime.Now;
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(GetDataPath("savegame.json", "Saves"), json);
        }

        public static GameData? LoadGame()
        {
            string path = GetDataPath("savegame.json", "Saves");
            if (!File.Exists(path)) return null;

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<GameData>(json, options);
        }
#endregion

#region Game Settings I/O
        public static void SaveSettings(SettingsData data)
        {
            data.SaveTime = DateTime.Now;
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(GetDataPath("TDLR_settings.json", "Saves"), json);
        }

        public static SettingsData? LoadSettings()
        {
            string path = GetDataPath("TDLR_settings.json", "Saves");
            if (!File.Exists(path)) return null;

            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<SettingsData>(json);
        }
#endregion

#region Library I/O

        public static List<string> loadDungeonNames()
        {
            string path = GetDataPath("dungeon_name_library.json", "Data/Libraries");

            if (!File.Exists(path))
            {
                return new List<string>(){"ERROR"};
            }

            string json = File.ReadAllText(path);

            var deserializeOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<string>? names = JsonSerializer.Deserialize<List<string>>(json, deserializeOptions);

            if (names != null)
            {
                return names;
            }
            else
            {
                return new List<string>(){"ERROR"};
            }
        }

#endregion
    }
}