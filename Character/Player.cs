using System.Text.Json.Serialization;

namespace TheDragonLairRemastered
{
    public class PlayerCharacter
    {
        [JsonPropertyName("name")]
        public string sName {get; set;} = "";
        [JsonPropertyName("level")]
        public int iLevel {get; set;} = 0;
        [JsonPropertyName("class")]
        public string sClass {get; set;} = "";

        [JsonPropertyName("current_dungeon")]
        public Dungeon currentDungeon {get; set;} = new("",new string[]{});

        public PlayerCharacter(string sName, int iLevel, string sClass, Dungeon currentDungeon)
        {
            this.sName = sName;
            this.iLevel = iLevel;
            this.sClass = sClass;
            this.currentDungeon = currentDungeon;
        }
        public string getName()
        {
            return sName;
        }
        public int getLevel()
        {
            return iLevel;
        }
        public string getClass()
        {
            return sClass;
        }
        public Dungeon getCurrentDungeon()
        {
            return currentDungeon;
        }
        public void setName(string sName)
        {
            this.sName = sName;
        }
        public void setLevel(int iLevel)
        {
            this.iLevel = iLevel;
        }
        public void setClass(string sClass)
        {
            this.sClass = sClass;
        }
        public void setDungeon(Dungeon currentDungeon)
        {
            this.currentDungeon = currentDungeon;
        }

        public string getInfo()
        {
            return sName + ", Level " + iLevel + " " + sClass;
        }
    }
}