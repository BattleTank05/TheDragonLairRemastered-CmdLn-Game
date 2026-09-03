using System.Reflection.Metadata;

namespace TheDragonLairRemastered
{
    class Master
    {
        static bool bEnableSingleKeyPress = true;
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
                case 3: changeSettings();
                break;
                case 4: Print("Exiting Game!", ConsoleColor.Gray);
                Exit(0);
                break;
                case 5: CreateGame(false, true);
                break;
                default: Print("Invalid Response", ConsoleColor.Red);
                break;
            }
            Main(args);
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
            {
                ChooseCharacter();
            }
        }
        
        public static void ChooseCharacter() // Character creation menu
        {
            Print("Welcome to the Character Creation menu!");
        }

    }
}