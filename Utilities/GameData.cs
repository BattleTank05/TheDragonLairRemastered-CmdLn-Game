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
        public string sGameLoopState { get; set; } = ""; // e.g. "Exploration", "Combat", "Event"
        [JsonPropertyName("difficulty")]
        public int iDifficulty { get; set; } = 0;
        [JsonPropertyName("campaign_progress")]
        public int iCampaignProgress { get; set; } = 0;
        [JsonPropertyName("dungeon_list")]
        public List<Dungeon> dDungeons { get; set; } = new();
        // public PlayerCharacter Player { get; set; } = new();

        public GameData(string sGameLoopState, int iDifficulty, int iCampaignProgress, List<Dungeon> dDungeons)
        {
            SaveTime = DateTime.Now;
            this.sGameLoopState = sGameLoopState;
            this.iDifficulty = iDifficulty;
            this.iCampaignProgress = iCampaignProgress;
            this.dDungeons = dDungeons;
        }
        public int getDifficulty(){ return iDifficulty; }
        public int getCampaignProgress(){ return iCampaignProgress; }
        public string getGameLoopState(){ return sGameLoopState;}
        public List<Dungeon> getDungeons(){ return dDungeons;}
    }
}