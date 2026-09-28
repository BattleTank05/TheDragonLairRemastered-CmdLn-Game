using System.Text.Json.Serialization; // Used for specifying various aspects of properties.

namespace TheDragonLairRemastered
{
    public class Dungeon
    {
        // Serializable Properties:
        [JsonPropertyName("dungeon_name")]
        public string name {get; set;} = "";

        [JsonPropertyName("dungeon_rooms")]
        public Dictionary<int,string> rooms {get; set;} = new();

        public Dungeon(string name, Dictionary<int,string> rooms) {
            this.name = name;
            this.rooms = rooms;
        }

        public string getName() { return name; }
        public Dictionary<int,string> getRooms() { return rooms; }
    }
}