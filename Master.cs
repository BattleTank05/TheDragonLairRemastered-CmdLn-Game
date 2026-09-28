namespace TheDragonLairRemastered
{
    class Master
    {
        public static string versionID = "v0.1";
        static bool bIsDebugMode = false; // Switch that toggles printing debug logs to the console
        static bool bEnableSingleKeyPress = true; // Toggles whether user input is read as single key presses or full lines. Default is true.
        
        static Difficulty gameDifficulty = Difficulty.None;
        public enum Difficulty
        {
            None, // Default value. Should never be used for math.
            Coward, // 1 = Easy
            Stalwart, // 2 = Normal
            Valor // 3 = Hard
        }
        
        static GameState gameLoopState = GameState.None;
        public enum GameState
        {
            None,                // no game state
            MainMenu,            // player is at main menu
            SettingsMenu,        // player is at settings menu
            NewGameMenu,         // player is at new game creation menu
            CharacterCreation,   // player is at character creation menu
            ChooseDungeon,       // player is choosing dungeon
            ChooseRoom,          // player is choosing a room in a dungeon
            Encounter            // player is in an encounter
        }

        static int iCampaignProgress = 0; // This holds the value of the dungeon iteration that the player is currently at
        static Queue<List<Dungeon>> dDungeonList = new Queue<List<Dungeon>>(); // Master Dungeon list, used across multiple gameplay methods
        static PlayerCharacter pPlayer = new PlayerCharacter("",1,"",new("",new())); // Object which holds all player related attributes
        static void Main(string[] args)
        {
            // Loads game settings
            LoadSettings();

            // set game state
            gameLoopState = GameState.MainMenu;

            // Launch menu
            DisplayPretext();
            Print("1) Continue\n2) New Game\n3) Settings\n4) Quit Game");
            DebugPrint("Debug Mode enabled.");
            switch (readUserNum())
            {
                case 0:
                if (confirmAction("enter debug mode"))
                    bIsDebugMode = !bIsDebugMode;
                Main(args);
                break;
                case 1:
                if(confirmAction("continue last saved game"))
                    CreateGame(true); // Creates a new game instance in Load mode
                Main(args);
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
        public static void Print(string sMsg, ConsoleColor cColor = ConsoleColor.White) // Same as regular print, but can set a custom color. Reverts to back to white after writing the message
        {
            Console.ForegroundColor = cColor;
            Console.WriteLine("\n" + sMsg);
        }
        public static void DebugPrint(string sMsg, ConsoleColor cColor = ConsoleColor.Gray)
        {
            if (bIsDebugMode)
                Print(sMsg, cColor);
        }
        public static void DisplayPretext() // Clears the Console and prints relevant information as pretext based on the current game state
        {
            Console.Clear();
            Print("The Dragon Lair Remastered " + versionID + "\n", ConsoleColor.Gray);
            
            string displayTitle = gameLoopState switch
            {
                GameState.MainMenu          => "=== MAIN MENU ===",
                GameState.SettingsMenu      => "=== SETTINGS ===",
                GameState.NewGameMenu       => "=== START NEW ADVENTURE ===",
                GameState.CharacterCreation => "=== HERO CREATION ===",
                GameState.ChooseDungeon     => "=== SELECT A DUNGEON | CAMPAIGN PROGRESS: " + iCampaignProgress + "/10 ===" + "\n   " + pPlayer.getInfo(),
                GameState.ChooseRoom        => "=== CHOOSE YOUR PATH | " + pPlayer.getCurrentDungeon().getName() + " ===" + "\n   " + pPlayer.getInfo(),
                GameState.Encounter         => "=== ENCOUNTER ===",
                _                           => "Loading..."
            };

            Print(displayTitle, ConsoleColor.Yellow);
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
                Print("Please enter valid characters.", ConsoleColor.Red);
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
            SettingsData? load = DataSystem.LoadSettings();
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
            DataSystem.SaveSettings(new SettingsData(bEnableSingleKeyPress));
            DebugPrint("Settings Saved", ConsoleColor.Gray);
        }

        static void LoadGame()
        {
            DebugPrint("Loading Game Data...");
            GameData? load = DataSystem.LoadGame();
            if (load != null)
            {
                gameDifficulty = (Difficulty)load.getDifficulty();
                if (Enum.TryParse(load.getGameLoopState(), out GameState loadedState))
                {
                    gameLoopState = loadedState;
                }
                else
                {
                    gameLoopState = GameState.None;
                }
                iCampaignProgress = load.getCampaignProgress();
                dDungeonList = load.getDungeons();
                pPlayer = load.getPlayerCharacter();

                DebugPrint("Load game operation successful");
            }
            else
            {
                DebugPrint("ERROR - Load game operation returned null");
            }
            DebugPrint("Loading Successful");
        }

        public static void SaveGame()
        {
            DebugPrint("Saving Game...");
            DataSystem.SaveGame(new GameData(gameLoopState.ToString(), (int)gameDifficulty, iCampaignProgress, dDungeonList, pPlayer));
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
                LoadGame();

                switch (gameLoopState)
                {
                    case GameState.ChooseDungeon: // Finish old loop
                        EnterDungeon(ChooseDungeon());
                        break;
                    case GameState.ChooseRoom:
                        EnterDungeon(pPlayer.getCurrentDungeon());
                        break;
                    default:
                        DebugPrint("Invalid game state");
                        break;
                }
            }
            else
            { // Standard new game
                gameLoopState = GameState.NewGameMenu;
                DisplayPretext();
                Print("Choose game difficulty:\n1) Coward (Easy)\n2) Stalwart (Normal)\n3) Honor (Hard)");
                switch (readUserNum())
                { // User can choose game difficulty. Most settings will be hidden until unlocked. Difficulty affects Dungeon RNG
                    case 1: Print("You picked Coward!", ConsoleColor.Green);
                    gameDifficulty = Difficulty.Coward;
                    break;
                    case 2: Print("You picked Stalwart!", ConsoleColor.Green);
                    gameDifficulty = Difficulty.Stalwart;
                    break;
                    case 3: Print("You picked Honor!", ConsoleColor.Green);
                    gameDifficulty = Difficulty.Valor;
                    break;
                    default: Print("Invalid Response", ConsoleColor.Red);
                    CreateGame(bLoadGame); // Loops on fail
                    break;
                }
                CreateCharacter(); // Character Creation, runs once.
                GenerateDungeons();
                SaveGame();
            }
                
                // The following is the Core Gameplay loop:
            while (iCampaignProgress < 10) // By default, this loop should only run 10 times
            {
                iCampaignProgress++; // Increment dungeon progress
                DebugPrint("Campaign Progress " + iCampaignProgress);
                
                if(bIsDebugMode)
                    Stall(); // Halt the program so dev can read Debug Messages before the Console is cleared

                EnterDungeon(ChooseDungeon());
                SaveGame();
            }

            Print("You have advanced to the final dungeon!"); // After completing the 10 dungeons, the player moves on to the final dungeon
            EnterDungeon(new Dungeon("The Dragon's Lair",generateRooms())); // As there is only one variation of this dungeon, Generate Dungeons can be skipped.
            Victory(); // Runs victory sequence and closes the game.
        }
        
        public static void CreateCharacter() // Character creation menu
        {
            gameLoopState = GameState.CharacterCreation;
            DisplayPretext();
            Print("Pick a starting class:\n1) Warrior\n2) Mage\n3) Rogue");
            // The class value determines the stats, gear, and abilities the new character will have.
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
        public static void GenerateDungeons() // Generates all dungeons for the entire run
        {  
            DebugPrint("Generating Dungeons...");
            // Load relevant libraries:
            List<string> availableNames = DataSystem.loadDungeonNames();

            // Create a series of 10 dungeon lists
            for(int i = 0; i < 10; i++)
            {
                List<Dungeon> dList = new List<Dungeon>();    
                for(int j = 0; j <= GetRandom(1,4); j++) // This will create between 2-4 dungeons for each list
                {   
                    int nameIndex = GetRandom(0,availableNames.Count); // Pull a random name from the available names pool
                    dList.Add(new Dungeon(availableNames[nameIndex], generateRooms())); // Add new dungeon to the list
                    availableNames.Remove(availableNames[nameIndex]); // Remove the name to ensure there are no duplicates
                }
                dDungeonList.Enqueue(dList);
            }
            DebugPrint("Dungeon Generation Complete");
        }
        public static Dictionary<int,string> generateRooms()
        {
            int numRooms = GetRandom(5,8);
            Dictionary<int,string> rooms = new();
            for (int i = 0; i <= numRooms; i++)
            {
                rooms.Add(i, "Room " + (i+1));
            }
            
            return rooms;
        }
        public static Dungeon ChooseDungeon() // Dungeon Selection Menu
        {
            gameLoopState = GameState.ChooseDungeon;
            DisplayPretext();
            
            SaveGame();

            Print("Choose a dungeon to enter:");
            for (int i = 0; i < dDungeonList.First().Count(); i++)
            { // Prints the list of dungeons and their descriptions
                if (dDungeonList.First()[i].getName() != "")
                    Print((i+1) + ") " + dDungeonList.First()[i].getName());
            }
            switch (readUserNum())
            { // Simple choice block
                case 0: Exit(0); // Hidden option for debug. Simply exits the game
                return ChooseDungeon(); 
                case 1: 
                    if (dDungeonList.Count() > 0)
                        if(confirmAction("travel to " + dDungeonList.First()[0].getName()))
                        {
                            pPlayer.setDungeon(dDungeonList.First()[0]);
                            return dDungeonList.First()[0];
                        }
                return ChooseDungeon();
                case 2: 
                    if (dDungeonList.Count() > 1)
                    {
                        if(confirmAction("travel to " + dDungeonList.First()[1].getName()))
                        {
                            pPlayer.setDungeon(dDungeonList.First()[1]);
                            return dDungeonList.First()[1];
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
                        if(confirmAction("travel to " + dDungeonList.First()[2].getName()))
                        {
                            pPlayer.setDungeon(dDungeonList.First()[2]);
                            return dDungeonList.First()[2];
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
                        if(confirmAction("travel to " + dDungeonList.First()[3].getName()))
                        {
                            pPlayer.setDungeon(dDungeonList.First()[3]);
                            return dDungeonList.First()[3];
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
            gameLoopState = GameState.ChooseRoom;
            DisplayPretext();

            Print("Entering " + dungeon.getName() + "..."); // loop init
            dDungeonList.Dequeue(); // Remove the beginning entry
            SaveGame();
            
            Stall();
            for(int i = 0; i < dungeon.getRooms().Count; i++)
            {
                EnterRoom(dungeon.getRooms()[i]);
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
            Exit(0);
        }
        public static void Defeat() // Defeat Screen
        {
            Print("You have been defeated! Better luck next time!");
            Exit(0);
        }
#endregion
    }
}