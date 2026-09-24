namespace TheDragonLairRemastered
{
    class Master
    {
        public static string versionID = "v0.1";
        static bool bIsDebugMode = false; // Switch that toggles printing debug logs to the console
        
        // Global settings variables
        static bool bEnableSingleKeyPress = true; // Toggles whether user input is read as single key presses or full lines. Default is true.
        
        // Game settings variables
        static int iDifficulty = 0; // 1 = Easy, 2 = Normal, 3 = Hard | Affects the RNG during dungeon creation.
        static string sGameLoopState = ""; /* Valid gamestates listed below:
        "" = no game state
        "main_menu" = player is at main menu
        "settings_menu" = player is at settings menu
        "new_game_menu" = player is at new game creation menu
        "character_creation" = player is at character creation menu
        "choose_dungeon" = player is choosing dungeon
        "choose_room" = player is choosing a room in a dungeon
        */
        static int iCampaignProgress = 0; // This holds the value of the dungeon iteration that the player is currently at
        static List<Dungeon> dDungeonList = new List<Dungeon>(); // Master Dungeon list, used across multiple gameplay methods
        static PlayerCharacter pPlayer = new PlayerCharacter("",1,"",new("",new string[]{})); // Object which holds all player related attributes
        static void Main(string[] args)
        {
            // Loads game settings
            LoadSettings();

            // set game state
            sGameLoopState = "main_menu";

            // Launch menu
            DisplayPretext();
            Print("1) Continue\n2) New Game\n3) Settings\n4) Quit Game");
            DebugPrint("Debug Mode enabled.", ConsoleColor.Gray);
            switch (readUserNum())
            {
                case 0:
                if (confirmAction("enter debug mode"))
                    bIsDebugMode = !bIsDebugMode;
                Main(args);
                break;
                case 1: Print("You picked Continue!", ConsoleColor.Cyan);
                CreateGame(true); // Creates a new game instance in Load mode
                break;
                case 2:
                if (bIsDebugMode)
                    CreateGame(false);
                else if (confirmAction("create new game")){
                    CreateGame(false); // Creates a default new game
                }
                Main(args);
                break;
                case 3:
                changeSettings(); Main(args); // Launches the settings menu, then restarts main menu
                break;
                case 4:
                Exit(0); // Force exits program
                break;
                default: Print("Invalid Response", ConsoleColor.Red); // Unrecognized input defaults to restart.
                Main(args);
                break;
            }
        }

        /*
        <<<--- UTILITY METHODS --->>>
        */
#region Utility Methods
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
        public static void DisplayPretext() // Clears the Console and prints relevant information as pretext based on the current game state
        {
            Console.Clear();
            Print("The Dragon Lair Remastered " + versionID + "\n", ConsoleColor.Gray);
            
            switch (sGameLoopState)
            {
                case "main_menu":
                    Print("    The Dragon Lair Remastered Main Menu", ConsoleColor.DarkYellow);
                    break;
                case "settings_menu":
                    Print("   Settings Menu", ConsoleColor.DarkYellow);
                    break;
                case "new_game_menu":
                Print("  New Game Menu", ConsoleColor.DarkYellow);
                    break;
                case "character_creation":
                Print("  Character Creation Menu", ConsoleColor.DarkYellow);
                    break;
                case "choose_dungeon":
                Print("  Choosing Dungeon | Game Progress: " + iCampaignProgress, ConsoleColor.Yellow);
                Print("   " + pPlayer.getInfo(), ConsoleColor.Blue);
                    break;
                case "choose_room":
                Print("  Exploring " + pPlayer.getCurrentDungeon().getName(), ConsoleColor.Yellow);
                Print("   " + pPlayer.getInfo(), ConsoleColor.Blue);
                    break;
            }
        }
        public static void DebugPrint(string sMsg, ConsoleColor cColor)
        {
            if (bIsDebugMode)
                Print(sMsg, cColor);
        }
        public static void Stall()
        {
            Print("Press any key to continue...", ConsoleColor.Gray);
            Console.ReadKey(); // Stalls the program until the user presses a key
        }
        public static string readUserMsg() // Reads, cleans, and returns a single line of User Input as string.
        {
            string? input = Console.ReadLine(); // Reads a line of user input

            // Check for empty string
            if (string.IsNullOrWhiteSpace(input))
            {
                Print("Please enter something.", ConsoleColor.Red);
                return readUserMsg();
            }

            // Clean the input string
            string cleaned = new string(input
                .Where(c => !char.IsControl(c)) // Filters out control/non-printable characters
                .ToArray()) // Compiles valid characters back into a string
                .Trim(); // Removes extra white space

            // Check again for empty string
            if (string.IsNullOrWhiteSpace(cleaned))
            {
                Print("Please enter valid characters", ConsoleColor.Red);
                return readUserMsg();
            }
            return cleaned;
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
                string? input = Console.ReadLine(); // Reads a line of user input and stores it as a nullable string.
                if (int.TryParse(input, out int result)){ // Uses TryParse to test whether the input is valid
                    return result; // If valid, returns the result
                }   
                else { // If this fails, defaults to -1
                    return -1;
                }
            }
        }
        public static bool confirmAction(string action)
        {
            Print("Really " + action + "?\n(y/n)", ConsoleColor.Cyan);
            ConsoleKeyInfo keyPress = Console.ReadKey();
            if (keyPress.KeyChar == 'y' || keyPress.KeyChar == '1')
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static int GetRandom(int iMin, int iMax) // Gets a random number between given min/max values.
        {
            Random r = new Random();
            return r.Next(iMin, iMax);
        }
        public static void Exit(int iExitCode) // Force stops the program. Code 0 for success, or non-0 for error / failure
        {
            if (bIsDebugMode)
                Environment.Exit(iExitCode);

            else if (confirmAction("Exit Game"))
            {
                Print("Exiting program with code " + iExitCode + "...", ConsoleColor.Gray);
                Stall(); // Stalls the program until the user presses a key
                Environment.Exit(iExitCode);
            }    
        }
#endregion
        
        /*
        <<<--- SAVE/LOAD METHODS --->>>
        */
#region Save Load Methods
        public static void changeSettings() // Allows user to tweak various gameplay settings
        {
            DisplayPretext();
            Print("Press the number of the setting you wish to alter to change it\nPress 0 to return to the main menu");
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
                case 0: if (confirmAction("save changes and return to main menu")) SaveSettings(); return; // Saves and exits the settings menu and returns to main
                case 1: bEnableSingleKeyPress = !bEnableSingleKeyPress; // Toggles setting
                break;
                default:
                break;
            }
            changeSettings(); // Loops until user exits settings
        }
        static void LoadSettings()
        {
            SettingsData? load = SaveSystem.LoadSettings();
            if (load != null)
            {
                bEnableSingleKeyPress = load.bEnableSingleKeyPress;

                DebugPrint("Load Settings operation successful", ConsoleColor.Gray);
            }
            else
            {
                DebugPrint("Load Settings operation returned null", ConsoleColor.Gray);
            }
        }
        static void SaveSettings()
        {
            SaveSystem.SaveSettings(new SettingsData(bEnableSingleKeyPress));
            DebugPrint("Settings Saved", ConsoleColor.Gray);
        }

        static void LoadGame()
        {
            GameData? load = SaveSystem.LoadGame();
            if (load != null)
            {
                iDifficulty = load.getDifficulty();
                iCampaignProgress = load.getCampaignProgress();
                sGameLoopState = load.getGameLoopState();
                dDungeonList = load.getDungeons();
                pPlayer = load.getPlayerCharacter();

                DebugPrint("Load game operation successful", ConsoleColor.Gray);
            }
            else
            {
                DebugPrint("ERROR - Load game operation returned null", ConsoleColor.Gray);
            }
        }

        public static void SaveGame()
        {
            SaveSystem.SaveGame(new GameData(sGameLoopState, iDifficulty, iCampaignProgress, dDungeonList, pPlayer));
            DebugPrint("Game Saved", ConsoleColor.Gray);
        }

#endregion
        
        /*
        <<<--- GAMEPLAY METHODS --->>>
        */
#region Gameplay Methods
        public static void CreateGame(bool bLoadGame) // Creates a new game
        {
            if (bLoadGame) // Launches loading sequence
            {
                DebugPrint("Loading previous game data...", ConsoleColor.Gray);
                LoadGame();

                switch (sGameLoopState)
                {
                    case "choose_dungeon": // Finish old loop
                        EnterDungeon(ChooseDungeon());
                        iCampaignProgress++;
                        break;
                    case "choose_room":
                        EnterDungeon(pPlayer.getCurrentDungeon());
                        break;
                    default:
                        DebugPrint("Invalid game state", ConsoleColor.Gray);
                        break;
                }
            }
            else
            { // Standard new game
                sGameLoopState = "new_game_menu";
                DisplayPretext();
                Print("Choose game difficulty:\n1) Coward (Easy)\n2) Stalwart (Normal)\n3) Honor (Hard)", ConsoleColor.White);
                switch (readUserNum())
                { // User can choose game difficulty. Most settings will be hidden until unlocked. Difficulty affects Dungeon RNG
                    case 1: Print("You picked Coward!", ConsoleColor.Green);
                    iDifficulty = 1;
                    break;
                    case 2: Print("You picked Stalwart!", ConsoleColor.Green);
                    iDifficulty = 2;
                    break;
                    case 3: Print("You picked Honor!", ConsoleColor.Green);
                    iDifficulty = 3;
                    break;
                    default: Print("Invalid Response", ConsoleColor.Red);
                    CreateGame(bLoadGame); // Loops on fail
                    break;
                }
                CreateCharacter(); // Character Creation, runs once.
            }
                // The following is the Core Gameplay loop:
            // Generate Dungeons will create a set of 2-4 dungeons and prompt the player to choose one.
            // After choosing, the selected dungeon is passed to the Enter Dungeon method, which loops through rooms until the dungeon is empty.
            // Once the dungeon is completed, Generate Dungeons will recursively loop in this manner until the passed int value is depleted.
            // Default value is 10, will be affected by the difficulty variable
            DebugPrint("Saving Game...", ConsoleColor.Gray);
            SaveGame();
            while (iCampaignProgress < 10)
            {
                
                DebugPrint("Campaign Progress " + iCampaignProgress, ConsoleColor.Gray);    
                if(dDungeonList.Count == 0)
                {
                    DebugPrint("Dungeon List empty, generating new...", ConsoleColor.Gray);
                    GenerateDungeons(); 
                }
                iCampaignProgress++;
                EnterDungeon(ChooseDungeon());
                DebugPrint("Saving Game...", ConsoleColor.Gray);
                SaveGame();
            }

            Print("You have advanced to the final dungeon!"); // After completing the 10 dungeons, the player moves on to the final dungeon
            EnterDungeon(new Dungeon("The Dragon's Lair", new string[]{"Boss Room"})); // As there is only one variation of this dungeon, Generate Dungeons can be skipped.
            Victory(); // Runs victory sequence and closes the game.
        }
        
        public static void CreateCharacter() // Character creation menu
        {
            sGameLoopState = "character_creation";
            DisplayPretext();
            Print("Pick a starting class:\n1) Warrior\n2) Mage\n3) Rogue");
            // Each of the following options will instantiate a new player.
            // The given class value determines the stats, gear, and abilities the new character will have.
            switch (readUserNum())
            { // Simple choice block
                case 1: Print("You picked Warrior!", ConsoleColor.Green);
                pPlayer.setClass("Warrior");
                break;
                case 2: Print("You picked Mage!", ConsoleColor.Green);
                pPlayer.setClass("Mage");
                break;
                case 3: Print("You picked Rogue!", ConsoleColor.Green);
                pPlayer.setClass("Rogue");
                break;
                default: Print("Invalid Response", ConsoleColor.Red);
                CreateCharacter();
                break;
            }

            Print("Type a name for your character:\n");
            pPlayer.setName(readUserMsg());
            while (!confirmAction("name your character \"" + pPlayer.getName() + "\""))
            {
                Print("Type a name for your character:\n");
                pPlayer.setName(readUserMsg());
            }
        }
        public static void GenerateDungeons() // Dungeon Generator
        {  
            // Create a fresh list of dungeons
            for(int j = 0; j <= GetRandom(1,4); j++)
            {   
                dDungeonList.Add(new Dungeon("Dungeon #" + (j+1), generateRooms()));
            }
        }
        public static string[] generateRooms()
        {
            return new string[] {"Room1","Room2","Room3","Room4","Room5"}; // Barebone implementation
        }
        public static Dungeon ChooseDungeon() // Dungeon Selection Menu
        {
            sGameLoopState = "choose_dungeon";
            DisplayPretext();
            
            DebugPrint("Saving Game...", ConsoleColor.Gray);
            SaveGame();

            Print("Choose a dungeon to enter:");
            for (int i = 0; i < dDungeonList.Count(); i++)
            { // Prints the list of dungeons and their descriptions
                if (dDungeonList[i].getName() != "")
                    Print((i+1) + ") " + dDungeonList[i].getName());
            }
            switch (readUserNum())
            { // Simple choice block
                case 0: Exit(0); // Hidden option for debug. Simply exits the game
                return ChooseDungeon(); 
                case 1: 
                    if (dDungeonList.Count() > 0)
                        if(confirmAction("travel to " + dDungeonList[0].getName()))
                        {
                            pPlayer.setDungeon(dDungeonList[0]);
                            return dDungeonList[0];
                        }
                return ChooseDungeon();
                case 2: 
                    if (dDungeonList.Count() > 1)
                    {
                        if(confirmAction("travel to " + dDungeonList[1].getName()))
                        {
                            pPlayer.setDungeon(dDungeonList[1]);
                            return dDungeonList[1];
                        }
                        else
                        return ChooseDungeon();
                    }
                    else
                    {
                        Print("Invalid Response", ConsoleColor.Red);
                        Stall();
                        return ChooseDungeon(); // repeat until successful
                    }
                case 3: 
                    if (dDungeonList.Count() > 2)
                    {
                        if(confirmAction("travel to " + dDungeonList[2].getName()))
                        {
                            pPlayer.setDungeon(dDungeonList[2]);
                            return dDungeonList[2];
                        }
                        else
                        return ChooseDungeon();
                    }
                    else
                    {
                        Print("Invalid Response", ConsoleColor.Red);
                        Stall();
                        return ChooseDungeon(); // repeat until successful
                    }
                case 4: 
                    if (dDungeonList.Count() > 3)
                    {
                        if(confirmAction("travel to " + dDungeonList[3].getName()))
                        {
                            pPlayer.setDungeon(dDungeonList[3]);
                            return dDungeonList[3];
                        }
                        else
                        return ChooseDungeon();
                    }
                    else
                    {
                        Print("Invalid Response", ConsoleColor.Red);
                        Stall();
                        return ChooseDungeon(); // repeat until successful
                    }
                default: 
                    Print("Invalid Response", ConsoleColor.Red);
                    Stall();
                    return ChooseDungeon(); // repeat until successful
            }
        }
        public static void EnterDungeon(Dungeon dungeon) // Dungeon Gameplay Loop
        {
            sGameLoopState = "choose_room";
            DisplayPretext();

            Print("Entering " + dungeon.getName() + "..."); // loop init
            dDungeonList.Clear(); // Clear the dungeon list after entering a dungeon
            SaveGame();
            
            Stall();
            foreach(string Room in dungeon.getRooms())
            {
                EnterRoom(Room);   
            }
            Print("You have cleared the Dungeon!"); // End of loop
        }
        public static void EnterRoom(string roomName)
        {
            Print("Entering " + roomName + "...");
            Stall(); // Placeholder for room gameplay.
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
#endregion
    }
}