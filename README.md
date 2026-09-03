 ----- Overview -----
A long while ago, I made a text-based Solo RPG game. The player would choose their class and starting gear, then compete in an arena against a variety of enemies, earning potions and gear as they went.
The old game had turn-based combat, and a clunky, checkerboard layout on which the player and enemies would occupy spaces. On their turn, they could move one space, attack adjacent squares, or use an item/consumable. I also implemented some rather scuffed parry, block, and dodge actions.

This was my first coding project, using C#. While it did work, it was very messy. Since then, I've taken a couple of college courses on programming and have a much better design intuition.

Now, I'm going to remaster my old game.
My main approach to the remaster is to go from a checkerboard arena to procedurally generated dungeons for the player to explore. Each dungeon would have chests and special elite/boss mobs which can be looted for treasure and items. Dungeons would be leveled, with different and more difficult mobs as the player progresses. There will be a final massive dungeon, with a dragon as the last boss.

 ----- Core Gameplay -----
 Like Darkest Dungeon, this game will auto save constantly. Every action the player takes is permanent.

 ----- Combat -----
Combat is going to be thoroughly overhauled. Instead of moving on the same board and whacking enemies repetitively until they die, new mechanics like stance, momentum, and reach will add a tactical layer to each fight. Winning will no longer be based on stats and rng, but will require strategy and planning.

 ----- Player -----
The player will have a class. This class determines what special abilities they have, the type of gear they will find, and how their stats are distributed.

The player will have a starting level and can level up. Leveling up will increase stats, upgrade class abilities, and allow traveling to more difficult dungeons. These dungeons will be more rewarding, but also more risky!

The player will have starting gear, and will find better equipment as they explore. Equipment is static, and provides either stat boosts or access to special abilities.

 ----- Exploration -----
Each run will be a series of ten procedurally generated dungeons, with the 3rd, 7th, and 10th dungeons having bosses. Later on, bosses may have variants, as seen in Slay the Spire.
The player will be given the descriptions of 2-3 dungeons, and will have to pick which one to travel to.
After picking the player will arrive in starting room of the dungeon. From here, the player will be given the descriptions of the doors/passages adjacent to their current room. The player will pick which room to enter next. This will continue until the end of the dungeon, where the player will exit and this loop will repeat.

Dungeons will have a type. The type determines the theme of the dungeon, the types of inhabiting monsters, and the type of loot inside.
Dungeons will have a set of rooms, and each room will have links to the next. The types of the rooms, and the weight of each spawn will be determined by the dungeon's "generation" variables.

Some dungeons may have quests. These quests will follow a system inspired by FTL: Faster Than Light. Quests will involve traveling to specific rooms or specific dungeons, encountering specific NPCs, killing certain enemies, and obtaining specific quest items. Quests will be difficult, but will give the player permanent rewards, such as new classes to play.

Rooms will have types. The type determines the encounter inside the room.

I may also implement a town, which will act as a base and trade station which the player can return to in between dungeons.

 ----- Monsters -----
Monsters will have a race. Race determines the monster's stats, gear, and abilities.
Monsters will have a level. This is a static variable set on creation that scales the monster's stats
Monsters will have a fighting style. This is the AI behind the monster's tactical decisions.
Monsters will have an attitude. The attitudes, Hostile, Neutral, and Friendly determine how the monster interacts with the player.
Monsters will have a loot table, which will be used by the encounter manager to determine the reward for combat.
Elite enemies will be stronger variants of regular monsters. These may have special abilities and loot.
Bosses will be stronger variants of Elite enemies. These may have unique abilities and loot.

 ----- NPCs -----
NPCs are characters with whom the player can interact through dialogue.
NPCs will have a race. This determines some possible interactions.
NPCs will have an attitude(Wary, Neutral, Friendly) which determines some possible interactions.
NPCs will have a type(Merchant, Quest) which determines some possible interactions.

 ----- Loot -----
Loot will have a type. The type determines whether the item can be sold for gold, equipped for stats/abilities, consumed for special effects, or is a quest item that can be used in a dungeon.

 ----- Quests -----
 // to be expanded later in the project

 ----- Difficulty -----
Beating the game will unlock new difficulty and game options.
Difficulty modes:
Coward (easy) - This will be the starting difficulty, for the tutorial
Stalwart (normal) - unlocked after completing the tutorial.
Valor (hard) - unlocked after completing Stalwart
Honor (hard, +10 dungeons, steeper enemy scaling) - unlocked after completing Valor
Legacy (hard, +20 dungeons, steeper enemy scaling, reduced gold looted) - unlocked after completing honor
// potential later additions
Heroic (legacy, but with alternating boss variants)

 |<<----- Code Structure ----->>|

class Program: |<-- Rename Master?
    void Main() // Launch and main menu. Listens for user input and performs one of the following options {

    Continue // calls CreateGame, load true
    NewGame // calls CreateGame
    Settings // calls settings
    DebugMode // calls createGame, debug true
    Quit // calls Exit
    }

    // Utility Methods
    void Print(str sMessage, bool bClearConsole, ConsoleColor cColor) // Prints a given message to the console. Can optionally clear console before printing. Also can be given a color which defaults to white if not specified.

    int GetRandom(int iMin, int iMax) // Generates a random number between given min/max values and returns it.
    float GetRandom(float fMin, float fMax) // Generates a random number between given min/max values and returns it.

    str sGetInput() // Listens for player input and returns it as a string. Will also check for commands. /h = help, /o = settings.
    int iGetInput() // Listens for player input, parses to int and returns it. If parse fails, default return is -1
    float fGetInput() // Listens for player input, parses to float and returns it. If parse fails, default return is -1

    // Game Management Methods
    void Settings() // Allows the user to tweak various options. Runs recursively until user terminates.
    void Exit() // Exits the game
    bool SaveGame() // Writes all active class instance's variables to a .json save file. Returns true if successful.

    void CreateGame(bool bDebug, bool bLoadGame) // Creates a new game instance. Optionally can do so in debug mode, or can load last saved game from .json file.

    void ChooseCharacter() // A series of prompts allowing the user to specify what class and gear their character will begin with.
    void ChooseDungeon() // Presents multiple choices of dungeons from which the player can choose.
    void ChooseRoom() // Presents multiple choices of rooms from which the player can choose.

    void Combat(Monster mMonster) // Recursive combat sequence.
    void Loot(LootTable lTable) // Generates loot from given table.
    void Dialogue(DialogueTree dDialogue) // Recursive dialogue sequence.