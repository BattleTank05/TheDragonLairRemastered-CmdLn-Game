using System.Text.Json.Serialization; // Used to set property names for clearer .json files

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
        [JsonPropertyName("player_character")]
        public PlayerCharacter pPlayer {get; set;} = new PlayerCharacter("",0,"",new("",new string[]{}));
        [JsonPropertyName("dungeon_lists")]
        public Queue<List<Dungeon>> dDungeons { get; set; } = new();

        public GameData(string sGameLoopState, int iDifficulty, int iCampaignProgress, Queue<List<Dungeon>> dDungeons, PlayerCharacter pPlayer)
        {
            SaveTime = DateTime.Now;
            this.sGameLoopState = sGameLoopState;
            this.iDifficulty = iDifficulty;
            this.iCampaignProgress = iCampaignProgress;
            this.dDungeons = dDungeons;
            this.pPlayer = pPlayer;
        }
        public int getDifficulty(){ return iDifficulty; }
        public int getCampaignProgress(){ return iCampaignProgress; }
        public string getGameLoopState(){ return sGameLoopState;}
        public Queue<List<Dungeon>> getDungeons(){ return dDungeons;}
        public PlayerCharacter getPlayerCharacter(){ return pPlayer;}
    }
}