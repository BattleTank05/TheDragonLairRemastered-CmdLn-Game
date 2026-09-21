
using System.Text.Json.Serialization;

namespace TheDragonLairRemastered
{
    public class SettingsData
    {
        [JsonPropertyName("date")]
        public DateTime SaveTime { get; set; }

        [JsonPropertyName("bEnableSingleKeyPress")]
        public bool bEnableSingleKeyPress {get; set;} = true;
        
        public SettingsData(bool bEnableSingleKeyPress)
        {
            SaveTime = DateTime.Now;
            this.bEnableSingleKeyPress = bEnableSingleKeyPress;
        }
    }
}