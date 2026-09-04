using System.Text.Json;

namespace TheDragonLairRemastered
{
    class Master
    {
        static bool bEnableSingleKeyPress = true;
        static string[] sDungeonList = new string[]{"","","",""};
        static int difficulty = 0; // 1 = Easy, 2 = Normal, 3 = Hard
        static void Main(string[] args)
        {
            // Load game settings
            LoadSettings();

            // Launch menu
            Print("Hello, Welcome to the Dragon Lair Remastered!\n1) Continue\n2) New Game\n3) Settings\n4) Quit Game", true);
            switch (readUserNum())
            {
                case 1: Print("You picked Continue!", ConsoleColor.Cyan);
                CreateGame(true, false); // Creates a new game instance in Load mode
                break;
                case 2: Print("You picked New Game!", ConsoleColor.Green);
                CreateGame(false, false); // Creates a default new game
                break;
                case 3:
                changeSettings(); Main(args); // Launches the settings menu, then restarts main menu
                break;
                case 4: Print("Exiting Game!", ConsoleColor.Gray);
                Exit(0); // Force exits program
                break;
                case 5:
                CreateGame(false, true); // Hidden menu option | Will have debug features for quick unit tests
                break;
                default: Print("Invalid Response", ConsoleColor.Red); // Unrecognized input defaults to restart.
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
        public static void Print(string sMsg, bool bClearConsole) // Clears the Console before printing given message to the terminal
        {
            if (bClearConsole)
            {
                Console.Clear();
            }
            Console.WriteLine("\n" + sMsg);
        }
        public static string readUserMsg() // Reads and returns a single line of User Input as string.
        {
            string? input = Console.ReadLine(); // Reads a line of user input
            if (input != null){ // Tests whether the input is a valid string
                return input; // If so, returns it
            }   
            else{ // Otherwise, returns an error
                return "ERROR - Getting string User Input has failed";
            }
        }
        public static int readUserNum() // Simply gets a single line of User Input, and parses it to int. If the parse fails, default return is -1
        {
            if (bEnableSingleKeyPress)
            { // Check if the single key press setting is on
                try
                {
                    ConsoleKeyInfo keyPress = Console.ReadKey(); // Reads the first key press from the user
                    if (keyPress.KeyChar >= '0' && keyPress.KeyChar <= '9') // Checks if the key char value is between single digits
                    {
                        return keyPress.KeyChar - '0';   // Char values start at '48'(0), so subtracting '0'(48), the result will be 0–9 respectively
                    }
                    else // otherwise, defaults to -1
                    return -1;
                }
                catch { // If the read key operation fails for any reason, default to -1
                    return -1;
                }
            }
            else
            { // If single key press is disabled,
                string? input = Console.ReadLine(); // Reads a line of user input
                if (int.TryParse(input, out int result)){ // Uses TryParse to test whether the input is valid
                    return result; // If valid, returns the rseult
                }   
                else { // If this fails, defaults to -1
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
            Print("Welcome to the Settings Menu!\nPress the number of the setting you wish to alter to change it\nPress 0 to return to the main menu", true);
            if (bEnableSingleKeyPress)
            { // Checks the active status of the option before printing it
                Print("1) Enable Single Key Press (Terminal reads keypresses without requiring the user to press 'Enter') - Set to: true");
            }
            else
            {
                Print("1) Enable Single Key Press (Terminal reads keypresses without requiring the user to press 'Enter') - Set to: false");
            }
            switch (readUserNum())
            {
                case 0: SaveSettings(); return; // Saves and exits the settings menu and returns to main
                case 1: bEnableSingleKeyPress = !bEnableSingleKeyPress; // Toggles setting
                break;
                default:
                break;
            }
            changeSettings(); // Loops until user exits settings
        }
        static void LoadSettings()
        {
            // Reads full contents of the Prefs.ini
            string bEnableSingleKeyPressSettings = File.ReadAllText("TheDragonLairRemasteredPrefs.ini");
            if (bEnableSingleKeyPressSettings.Contains("bEnableSingleKeyPress"))
            { // Checks for specific entry
                if (bEnableSingleKeyPressSettings.Contains("True"))
                { // If the entry exists, checks status and alters accordingly
                    bEnableSingleKeyPress = true;
                }
                else
                {
                    bEnableSingleKeyPress = false;
                }
            }
        }
        static void SaveSettings()
        {
            // Compile each setting into a string
            string bEnableSingleKeyPressSettings = "bEnableSingleKeyPress=" + bEnableSingleKeyPress + "\n";
            // Sends the string to the Prefs.ini file. Will create a new file of none exists
            File.WriteAllText("TheDragonLairRemasteredPrefs.ini", bEnableSingleKeyPressSettings);
        }
        public static void CreateGame(bool bLoadGame, bool bDebugMode) // Creates a new game
        {
            if (bDebugMode) // Launches debug mode
            {
                Print("Entering Debug Mode!");
            }
            else if (bLoadGame) // Launches loading sequence
            {
                Print("Loading previous game data....");
            }
            else
            { // Standard new game
                Print("Choose game difficulty:\n1) Coward (Easy)\n2) Stalwart (Normal)\n3) Honor (Hard)", true);
                switch (readUserNum())
                { // User can choose game difficulty. Most settings will be hidden until unlocked. Difficulty affects Dungeon RNG
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
                    CreateGame(bLoadGame, bDebugMode); // Loops on fail
                    break;
                }

                // The following is the Core Gameplay loop:
                CreateCharacter(); // Character Creation, runs once.
                GenerateDungeons(10); // Generate Dungeons will create a set of 2-4 dungeons and prompt the player to choose one.
                                      // After choosing, the selected dungeon is passed to the Enter Dungeon method, which loops through rooms until the dungeon is empty.
                                      // Once the dungeon is completed, Generate Dungeons will recursively loop in this manner until the passed int value is depleted.
                                      // Default value is 10
                Print("You have advanced to the final dungeon!"); // After completed the 10 dungeons, the player moves on to the final dungeon
                EnterDungeon("The Dragon's Lair"); // As there is only one variation of this dungeon, Generate Dungeons can be skipped.
                Victory(); // Runs victory sequence and closes the game.
            }
        }
        
        public static void CreateCharacter() // Character creation menu
        {
            Print("Welcome to the Character Creation menu!\nPick a starting class:\n1) Warrior\n2) Mage\n3) Rogue", true);
            // Each of the following options will instantiate a new player.
            // The given class value determines the stats, gear, and abilities the new character will have.
            switch (readUserNum())
            { // Simple choice block
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
            int iCount = iDunCount; // Updates count value
            if (iCount > 0) // Checks if count is still valid
            {  
                Print("Generating " + iDunCount + " Dungeon");
                // Create a fresh list of dungeons
                for(int j = 0; j <= GetRandom(1,4); j++)
                {   
                    sDungeonList[j] = "Dungeon #" + (j+1) + "!";
                }
                // Runs a choice block to determine which of the above dungeons to enter, then passes the result to the Enter Dungeon loop
                EnterDungeon(ChooseDungeon());
                GenerateDungeons(iCount - 1); // Recursive loop.
            }
        }
        public static string ChooseDungeon() // Dungeon Selection Menu
        {
            Print("Choose a dungeon to enter:");
            for (int i = 0; i < sDungeonList.Length; i++)
            { // Prints the list of dungeons and their descriptions
                if (sDungeonList[i] != "")
                    Print((i+1) + ") " + sDungeonList[i]);
            }
            switch (readUserNum())
            { // Simple choice block
                case 1: 
                    if (sDungeonList[0] != "")
                        Print("You picked " + sDungeonList[0] + "!", ConsoleColor.Green);
                return sDungeonList[0];
                case 2: 
                    if (sDungeonList[1] != "")
                        Print("You picked " + sDungeonList[1] + "!", ConsoleColor.Green);
                return sDungeonList[1];
                case 3: 
                    if (sDungeonList[2] != "")
                        Print("You picked " + sDungeonList[2] + "!", ConsoleColor.Green);
                return sDungeonList[2];
                case 4: 
                    if (sDungeonList[3] != "")
                        Print("You picked " + sDungeonList[3] + "!", ConsoleColor.Green);
                return sDungeonList[3];
                default: 
                    Print("Invalid Response", ConsoleColor.Red);
                return ChooseDungeon(); // repeat until successful
            }
        }
        public static void EnterDungeon(string sDungeonName) // Dungeon Gameplay Loop
        {
            Print("Entering " + sDungeonName + "..."); // loop init
            sDungeonList = new string[]{"","","",""}; // Clear the dungeon list after entering a dungeon
            Print("You have cleared the Dungeon!"); // End of loop
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