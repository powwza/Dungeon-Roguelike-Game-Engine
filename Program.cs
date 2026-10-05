using System.Formats.Asn1;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace goodbyeworld
{
    internal class Program
    {
        
        public static Random rand = new Random();
        static void Main(string[] args)
        {
            Menu();
            
        }
        static bool GetMenuChoice(ref int pointer,int max)
        {
            //Allow user to interact with the menu, moving pointer if up/down arrow keys are used or
            //running the appropriate process if the user presses the enter key by returning true to indicate
            //a process must be carried out
            ConsoleKey menuInput = Console.ReadKey().Key;
            if (menuInput == ConsoleKey.UpArrow && pointer > 0)
            {
                pointer--;
            }
            else if (menuInput == ConsoleKey.DownArrow && pointer < max)
            {
                pointer++;
            }
            else if (menuInput == ConsoleKey.Enter)
            {
                return true;
            }
            return false;

        }
        static void Menu()
        {
            //Displays the menu to the user
            double difficulty = 1.5;
            int pointer = 0;
            bool exit = false;
            string[] choices = { "Start Game", "Change Difficulty", "Tutorial", "Exit Game" };
            while (!exit)
            {
                Console.Clear();
                System.Console.WriteLine("ROGUELIKE GAME");
                for (int i = 0; i < 4; i++)
                {
                    if (i == pointer)
                    {
                        System.Console.WriteLine(">" + choices[i] + "<");
                    }
                    else
                    {
                        System.Console.WriteLine(choices[i]);
                    }
                }
                if (GetMenuChoice(ref pointer,choices.Count()-1))
                {
                    switch (pointer)
                    {
                        case 0:
                            PlayGame(difficulty);
                            break;
                        case 1:
                            ChangeDifficulty(ref difficulty);
                            break;
                        case 2:
                            Tutorial();
                            break;
                        case 3:
                            Console.Clear();
                            exit=true;
                            break;
                        default:
                            break;
                    }
                }
            }

        }
        static void PlayGame(double difficultyIn)
        {
            double difficulty = difficultyIn;
            Player player = new Player(-1, -1, 3,0);
            Dungeon dungeon = new Dungeon(ref player, difficulty);
            bool dungeonDefeated = true;
            int level = 1;
            //keep generating dungeons if the player manages to exit without dying
            while (dungeonDefeated)
            {
                //player attempts to exit a dungeon
                dungeonDefeated = dungeon.Play();
                if (dungeonDefeated) //if player manages to exit dungeon, then generate a boss room
                {
                    dungeon = new Dungeon(ref player, difficulty);
                    dungeon.GenerateBossRoom();
                    dungeonDefeated = dungeon.Play();
                    if (dungeonDefeated) //if player manages to exit boss room, then increase difficulty and generate a new dungeon
                    {
                        difficulty = difficulty * 1.25;
                        dungeon = new Dungeon(ref player, difficulty);
                        level++;
                    }
                }
            }
            
            Console.Clear();
            System.Console.WriteLine($"YOU DIED... YOU REACHED LEVEL {level}");
            System.Console.WriteLine($"You got a score of {player.GetNumCoins()}");
            System.Console.WriteLine("(Press ANY KEY to EXIT)");
            Console.ReadKey();
            Console.Clear();
        }
        static void ChangeDifficulty(ref double difficulty)
        {
            int pointer = 0;
            string[] choices = { "Easy", "Normal", "Hard", "Impossible","EXIT" };
            bool exit = false;
            string currentDifficulty = "Normal";

            while (!exit)
            {
                Console.Clear();
                System.Console.WriteLine("CHOOSE DIFFICULTY");
                System.Console.WriteLine($"Current difficulty: {currentDifficulty}");
                for (int i = 0; i < 5; i++)
                {
                    if (i == pointer)
                    {
                        System.Console.WriteLine(">" + choices[i] + "<");
                    }
                    else
                    {
                        System.Console.WriteLine(choices[i]);
                    }
                }
                if (GetMenuChoice(ref pointer,choices.Count()-1))
                {
                    switch (pointer)
                    {
                        case 0:
                            difficulty = 1;
                            currentDifficulty = "Easy";
                            break;
                        case 1:
                            difficulty = 1.5;
                            currentDifficulty = "Normal";
                            break;
                        case 2:
                            difficulty = 2.5;
                            currentDifficulty = "Hard";
                            break;
                        case 3:
                            difficulty = 4;
                            currentDifficulty = "Impossible";
                            break;
                        case 4:
                            exit=true;
                            break;
                        default:
                            break;
                    }
                    
                }
            }
        }
        static void Tutorial()
        {
            Console.Clear();
            System.Console.WriteLine("Welcome to my roguelike game!\n\n");
            System.Console.WriteLine("Controls:");
            System.Console.WriteLine("Move: ARROW KEYS");
            System.Console.WriteLine("Attack: WASD");
            System.Console.WriteLine("Use item: E");
            System.Console.WriteLine("Use ability: Q\n\n");

            System.Console.Write($"ENEMIES are lowercase letters, e.g 'a', 'b' or 'c'");
            System.Console.WriteLine("Each TYPE of enemy has its own DISTINCT way of MOVING around, figuring out each-\n-of their movement patterns is essential to defeating them\n\n");
            System.Console.WriteLine("Enemies drop $ and have a chance of dropping a key");

            System.Console.WriteLine("CHESTS are represented by '+'");
            System.Console.WriteLine("They are opened by moving toward them");
            System.Console.WriteLine("You must have a key to open a chest");
            System.Console.WriteLine("They will drop an item\n\n");

            System.Console.WriteLine("To exit a dungeon, you must find the EXIT TILE");
            System.Console.WriteLine("The exit tile is a ':', but enemies can stand over the tile which-\n-might hide it\n\n");

            System.Console.WriteLine("You are able to BREAK WALLS by moving toward them\n\n");

            System.Console.WriteLine("Try to get as many $ as you can before you meet your inevitable demise!");
            System.Console.WriteLine("GOOD LUCK!\n\n");

            System.Console.WriteLine("(Press ANY KEY to EXIT)");
            Console.ReadKey();



        }

    }
}

