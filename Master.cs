using System.Reflection.Metadata;

namespace TheDragonLairRemastered
{
    class Master
    {
        static bool bEnableSingleKeyPress = true;
        static string[] sDungeonList = new string[]{""};
        static int difficulty = 0; // 1 = Easy, 2 = Normal, 3 = Hard
        static void Main(string[] args)
        {
            Print("Hello, Welcome to the Dragon Lair Remastered!\n1) Continue\n2) New Game\n3) Settings\n4) Quit Game");
            switch (readUserNum())
            {
                case 1: Print("You picked Continue!", ConsoleColor.Cyan);
                CreateGame(true, false);
                break;
                case 2: Print("You picked New Game!", ConsoleColor.Green);
                CreateGame(false, false);
                break;
                case 3:
                changeSettings();
                break;
                case 4: Print("Exiting Game!", ConsoleColor.Gray);
                Exit(0);
                break;
                case 5:
                CreateGame(false, true);
                break;
                default: Print("Invalid Response", ConsoleColor.Red);
                Main(args);
                break;
            }
        }

        /*
        <<<--- UTILITY METHODS --->>>
        */

        public static void Print(string sMsg) // Prints given message to the terminal. Uses \n at start for clarity
        {
            Console.WriteLine("\n" + sMsg);
        }
        public static void Print(string sMsg, ConsoleColor cColor) // Same as regular print, but can set a custom color. Reverts to back to white after writing the message
        {
            Console.ForegroundColor = cColor;
            Print(sMsg);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static string readUserMsg() // Simply gets a single line of User Input.
        {
            try
            {
                return Console.ReadLine();   
            }
            catch {
                return "ERROR - Getting string User Input has failed";
            }
        }
        public static int readUserNum() // Simply gets a single line of User Input, and parses it to int. If the parse fails, default return is -1
        {
            if (bEnableSingleKeyPress)
            {
                try
                {
                    ConsoleKeyInfo keyPress = Console.ReadKey();
                    if (keyPress.KeyChar >= '0' && keyPress.KeyChar <= '9')
                    {
                        return keyPress.KeyChar - '0';   // gives you 0–9   
                    }
                    else
                    return -1;
                }
                catch {
                    return -1;
                }
            }
            else
            {
                try
                {
                    return Int32.Parse(Console.ReadLine());   
                }
                catch {
                    return -1;
                }
            }
        }
        public static int GetRandom(int iMin, int iMax) // Gets a random number between given min/max values.
        {
            Random r = new Random();
            return r.Next(iMin, iMax);
        }
        public static void Exit(int iExitCode) // Force stops the program. Code 0 for success, or non-0 for error / failure
        {
            Environment.Exit(iExitCode);
        }

        /*
        <<<--- GAMEPLAY METHODS --->>>
        */
        public static void changeSettings() // Allows user to tweak various gameplay settings
        {
            Print("Welcome to the Settings Menu!\nPress the number of the setting you wish to alter to change it\nPress 0 to return to the main menu");
            if (bEnableSingleKeyPress)
            {
                Print("1) Enable Single Key Press (Terminal reads keypresses without requiring the user to press 'Enter') - Set to: true");
            }
            else
            {
                Print("1) Enable Single Key Press (Terminal reads keypresses without requiring the user to press 'Enter') - Set to: false");
            }
            switch (readUserNum())
            {
                case 0: return;
                case 1: bEnableSingleKeyPress = !bEnableSingleKeyPress;
                break;
                default:
                break;
            }
            changeSettings();
        }
        public static void CreateGame(bool bLoadGame, bool bDebugMode) // Creates a new game
        {
            if (bDebugMode)
            {
                Print("Entering Debug Mode!");
            }
            else if (bLoadGame)
            {
                Print("Loading previous game data....");
            }
            else
                Print("Choose game difficulty:\n1) Coward (Easy)\n2) Stalwart (Normal)\n3) Honor (Hard)");
                switch (readUserNum())
                {
                    case 1: Print("You picked Coward!", ConsoleColor.Green);
                    difficulty = 1;
                    break;
                    case 2: Print("You picked Stalwart!", ConsoleColor.Green);
                    difficulty = 2;
                    break;
                    case 3: Print("You picked Honor!", ConsoleColor.Green);
                    difficulty = 3;
                    break;
                    default: Print("Invalid Response", ConsoleColor.Red);
                    CreateGame(bLoadGame, bDebugMode);
                    break;
                }
            {
                CreateCharacter();
                GenerateDungeons(10);
                Print("You have advanced to the final dungeon!");
                EnterDungeon("The Dragon's Lair");
                Victory();
            }
        }
        
        public static void CreateCharacter() // Character creation menu
        {
            Print("Welcome to the Character Creation menu!\nPick a starting class:\n1) Warrior\n2) Mage\n3) Rogue");
            switch (readUserNum())
            {
                case 1: Print("You picked Warrior!", ConsoleColor.Green);
                break;
                case 2: Print("You picked Mage!", ConsoleColor.Green);
                break;
                case 3: Print("You picked Rogue!", ConsoleColor.Green);
                break;
                default: Print("Invalid Response", ConsoleColor.Red);
                CreateCharacter();
                break;
            }
        }
        public static void GenerateDungeons(int iDunCount) // Dungeon Generator
        {
            int iCount = iDunCount;
            if (iCount > 0)
            {  
                Print("Generating New Dungeon");
                // Create a list of dungeons
                sDungeonList = new string[] { "The Forgotten Crypt", "The Cursed Forest", "The Abandoned Mines" };
                EnterDungeon(ChooseDungeon());
                GenerateDungeons(iCount - 1);
            }
        }
        public static string ChooseDungeon() // Dungeon Selection Menu
        {
            Print("Choose a dungeon to enter:\n1) " + sDungeonList[0] + "\n2) " + sDungeonList[1] + "\n3) " + sDungeonList[2]);
            switch (readUserNum())
            {
                case 1: Print("You picked The Forgotten Crypt!", ConsoleColor.Green);
                return sDungeonList[0];
                case 2: Print("You picked The Cursed Forest!", ConsoleColor.Green);
                return sDungeonList[1];
                case 3: Print("You picked The Abandoned Mines!", ConsoleColor.Green);
                return sDungeonList[2];
                default: Print("Invalid Response", ConsoleColor.Red);
                return ChooseDungeon();
            }
        }
        public static void EnterDungeon(string sDungeonName) // Dungeon Gameplay Loop
        {
            Print("Entering " + sDungeonName + "...");
            sDungeonList = new string[]{""}; // Clear the dungeon list after entering a dungeon
            Print("You have cleared the Dungeon!");
        }
        public static void Victory() // Victory Screen
        {
            Print("Congratulations! You have defeated the Dragon and completed the game!");
            Print("Press any key to exit...");
            Console.ReadKey();
            Exit(0);
        }
        public static void Defeat() // Defeat Screen
        {
            Print("You have been defeated! Better luck next time!");
            Print("Press any key to exit...");
            Console.ReadKey();
            Exit(0);
        }
    }
}