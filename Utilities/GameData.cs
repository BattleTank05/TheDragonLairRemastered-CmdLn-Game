using System.Text.Json.Serialization;

namespace TheDragonLairRemastered
{
    public class GameData
    {
        [JsonPropertyName("date")]
        public DateTime SaveTime { get; set; }

        // Progress info
        // public int CurrentDungeonIndex { get; set; } // which level the player is on
        // public int CurrentRoomId { get; set; }          // player location
        [JsonPropertyName("game_loop_state")]
        public string GameLoopState { get; set; } = ""; // e.g. "Exploration", "Combat", "Event"
        [JsonPropertyName("difficulty")]
        public int Difficulty { get; set; } = 0;
        [JsonPropertyName("dungeon_list")]
        public List<Dungeon> Dungeons { get; set; } = new();
        // public PlayerCharacter Player { get; set; } = new();

        public GameData(string gameLoopState, int difficulty, List<Dungeon> dungeons)
        {
            GameLoopState = gameLoopState;
            Difficulty = difficulty;
            Dungeons = dungeons;
            SaveTime = DateTime.Now;
        }
        public int getDifficulty(){ return Difficulty; }
        public string getGameLoopState(){ return GameLoopState;}
        public List<Dungeon> getDungeons(){ return Dungeons;}
    }
}