using System.Text.Json.Serialization; // Used for specifying various aspects of properties.

namespace TheDragonLairRemastered
{
    public class Dungeon
    {
        // Serializable Properties:
        [JsonPropertyName("dungeon_name")]
        public string name {get; set;} = "";

        [JsonPropertyName("dungeon_rooms")]
        public string[] rooms {get; set;} = { "", "","", ""};

        public Dungeon(string name, string[] rooms) {
            this.name = name;
            this.rooms = rooms;
        }

        public string getName() { return name; }
        public string[] getRooms() { return rooms; }
    }
}