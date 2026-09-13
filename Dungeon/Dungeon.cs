

namespace TheDragonLairRemastered
{
    public class Dungeon
    {
        string name = "";
        string[] rooms = new string[] { "", "","", ""};

        public Dungeon(string name, string[] rooms) {
            this.name = name;
            this.rooms = rooms;
        }

        public string getName()
        {
            return name;
        }
        public string[] getRooms()
        {
            return rooms;
        }
    }
}