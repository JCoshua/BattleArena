using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BattleArena
{  
    public enum ItemType
    {
        DEFENSE,
        ATTACK,
        NA
    }

    public enum Scene
    {
        STARTMENU,
        CHARACTERSELECTION,
        BATTLE,
        RESTARTMENU
    }

    public struct Item
    {
        public string Name;
        public float StatBoost;
        public ItemType Type;
    }

    class Game
    {
        private bool _gameOver;
        private Scene _currentScene = 0;
        private Player _player;
        private Entity[] _enemies;
        private string _playerName;

        private int _currentEnemyIndex = 0;
        private Entity _currentEnemy;


        private Item[] _wizardItems;
        private Item[] _knightItems;
        /// <summary>
        /// Function that starts the main game loop
        /// </summary>
        public void Run()
        {
            Start();

            while (!_gameOver)
            {
                Update();
            }

            End();
        }

        /// <summary>
        /// Function used to initialize any starting values by default
        /// </summary>
        public void Start()
        {
            _gameOver = false;
            _currentScene = 0;
            InitializeEnemies();
            IntitalizeItems();
        }

        public void IntitalizeItems()
        {   
            //Wizard Items
            Item bigWand = new Item { Name = "Big Wand", StatBoost = 5, Type = ItemType.ATTACK };
            Item bigShield = new Item { Name = "Big Shield", StatBoost = 15, Type = ItemType.DEFENSE };

            //Knight Items
            Item maghoganyStick = new Item { Name = "Mahogany Stick", StatBoost = 25 , Type = ItemType.ATTACK };
            Item shoes = new Item { Name = "Rubber Shoes", StatBoost = 20 , Type = ItemType.DEFENSE };

            //Initalize arrays
            _wizardItems = new Item[] { bigWand, bigShield };
            _knightItems = new Item[] { maghoganyStick, shoes };
        }

        public void InitializeEnemies()
        {
            Entity slime = new Entity("Slime", 10, 5, 0);
            Entity zombie = new Entity("Zom-B", 20, 25, 5);
            Entity kris = new Entity("guy named Kris", 30, 15, 20);

            _enemies = new Entity[] { slime, zombie, kris };
            ResetEnemy();
        }

        /// <summary>
        /// This function is called every time the game loops.
        /// </summary>
        public void Update()
        {
            DisplayCurrentScene();
            Console.Clear();
        }

        /// <summary>
        /// This function is called before the applications closes
        /// </summary>
        public void End()
        {
            Console.WriteLine("Farewell... Coward.");
        }

        public void Save()
        {
            //Creates a new stream writer
            StreamWriter writer = new StreamWriter("SaveData.txt");

            //Saves current enemy index
            writer.WriteLine(_currentEnemyIndex);

            //Saves player and Enemy Stats
            _player.Save(writer);
            _currentEnemy.Save(writer);

            //Closes after finishing
            writer.Close();
        }

        public bool Load()
        {
            bool loadSuccessful = true;
            //File doesn't exist
            if (!File.Exists("SaveData.txt"))
                loadSuccessful = false;

            //Creates new reader
            StreamReader reader = new StreamReader("SaveData.txt");

            //If the first line can't be converted into an int
            if (!int.TryParse(reader.ReadLine(), out _currentEnemyIndex))
                loadSuccessful = false;

            //Load player Name and job
            string job = reader.ReadLine();

            //Assign items based on player job
            if (!(job == "Wizard"))
                _player = new Player(_wizardItems);
            else if (job == "Knight")
                _player = new Player(_knightItems);
            else
                loadSuccessful = false;

            if (!_player.Load(reader))
                loadSuccessful = false;

            //Create a new instance and try to load the player
            _currentEnemy = new Entity();
            if (!_currentEnemy.Load(reader))
                loadSuccessful = false;

            _enemies[_currentEnemyIndex] = _currentEnemy;

            reader.Close();
            return loadSuccessful;
        }

        public void DisplayOpeningMenu()
            {
            int choice = GetInput("Welcome to the Battle Arena", "Start New Game", "Load Game");

            if (choice == 0)
            {
                _currentScene = Scene.CHARACTERSELECTION;
            }
            else if (choice == 1)
            {
                if (Load())
                {
                    Console.WriteLine("Loading Successful");
                    Console.ReadKey(true);
                    Console.Clear();
                    _currentScene = Scene.BATTLE;
                }
                else
                {
                    Console.WriteLine("Woops, something messed up");
                    Console.ReadKey(true);
                    Console.Clear();
                }
            }
        }
        /// <summary>
        /// Resets the enemies
        /// </summary>
        void ResetEnemy()
        {
            _currentEnemyIndex = 0;
            _currentEnemy = _enemies[_currentEnemyIndex];
        }

        /// <summary>
        /// Gets an input from the player based on some given decision
        /// </summary>
        /// <param name="description">The context for the input</param>
        /// <param name="option1">The first option the player can choose</param>
        /// <param name="option2">The second option the player can choose</param>
        /// <returns></returns>
        int GetInput(string description, params string[] options)
        {
            string input = "";
            int inputReceived = -1;

            while (inputReceived == -1)
            {
                //Print options
                Console.WriteLine(description);
                for (int i = 0; i < options.Length; i++)
                {
                    Console.WriteLine((i + 1) + ". " + options[i]);
                }
                Console.Write("> ");

                //Get input from player
                input = Console.ReadLine();

                //If the player typed an int...
                if (int.TryParse(input, out inputReceived))
                {
                    //...decrement the input and check if it's within the bounds of the array
                    inputReceived--;
                    if (inputReceived < 0 || inputReceived >= options.Length)
                    {
                        //Set input received to be the default value
                        inputReceived = -1;
                        //Display error message
                        Console.WriteLine("Invalid Input. Not an Option");
                        Console.ReadKey(true);
                    }
                }
                //If the player didn't type an int
                else
                {
                    //Set input received to be the default value
                    inputReceived = -1;
                    //Display error message
                    Console.WriteLine("Invalid Input. Not a Number");
                    Console.ReadKey(true);
                }
            }
            return inputReceived;
        }

            /// <summary>
            /// Calls the appropriate function(s) based on the current scene index
            /// </summary>
            void DisplayCurrentScene()
        {
            switch (_currentScene)
            {
                case Scene.STARTMENU:
                    DisplayOpeningMenu();
                    break;

                case Scene.CHARACTERSELECTION:
                    DisplayMainMenu();
                    break;

                case Scene.BATTLE:
                    Battle();
                    Console.ReadKey(true);
                    Console.Clear();
                    break;

                case Scene.RESTARTMENU:
                    DisplayRestartMenu();
                    break;

                default:
                    Console.WriteLine("Invalid Scene Index");
                    break;
            }
        }

        /// <summary>
        /// The Player creates their character.
        /// </summary>
        void DisplayMainMenu()
        {
            GetPlayerName();
            CharacterSelection();
            _currentScene = Scene.BATTLE;
        }

        /// <summary>
        /// Displays the menu that allows the player to restart or quit the game
        /// </summary>
        void DisplayRestartMenu()
        {
           int input = GetInput("Would you like to play again?", "Yes", "No");
            if (input == 0)
            {
                InitializeEnemies();
                _currentScene = Scene.CHARACTERSELECTION;
            }
            else if (input == 1)
            {
                _gameOver = true;
            }
        }

        /// <summary>
        /// Displays text asking for the players name. Doesn't transition to the next section
        /// until the player decides to keep the name.
        /// </summary>
        void GetPlayerName()
        {
            bool validName = false;
            while (validName == false)
            {
                Console.WriteLine("Hello. Please enter your Name.");
                _playerName = Console.ReadLine();
                int input = GetInput("Are you okay with this name?", "Yes", "No");
                if (input == 0)
                {
                    validName = true;
                }
                else if(input == 1)
                {

                }
            }

        }

        /// <summary>
        /// Gets the players choice of character. Updates player stats based on
        /// the character chosen.
        /// </summary>
        public void CharacterSelection()
        {
            int input = GetInput("Please Select your class:", "Wizard", "Knight");
            if (input == 0)
            {
                _player = new Player(_playerName, 50, 25, 5, _wizardItems, "Wizard");
            }
            else if (input == 1)
            {
                _player = new Player(_playerName, 75, 15, 10, _knightItems, "Knight");
            }
        }

        /// <summary>
        /// Prints a characters stats to the console
        /// </summary>
        /// <param name="character">The character that will have its stats shown</param>
        void DisplayStats(Entity character)
        {
            Console.WriteLine(character.Name + " Health: " + character.Health);
            Console.WriteLine(character.Name + " Attack: " + character.AttackPower);
            Console.WriteLine(character.Name + " Defence: " + character.DefensePower);
            Console.WriteLine();
        }


        public void DisplayEquipMenu()
        {
            //Get Item Index
            int choice = GetInput("Select an item to equip", _player.GetItemNames());

            //Equip Item at given index
            if (!(_player.TryEquipItem(choice)))
            {
                Console.WriteLine("You are already holding that item!");
            }

            //Print feedback
            Console.WriteLine("You equipped " + _player.CurrentItem.Name + "!");
        }


        public void Battle()
        {   
            float damageDealt = 0;

            DisplayStats(_player);
            DisplayStats(_currentEnemy);
            

            int input = GetInput("A " + _currentEnemy.Name + " stands in front of you. What will you do?","Attack", "Equip Item", "Remove Current Item", "Save");
            if (input == 0)
            {
                damageDealt = _player.Attack(_currentEnemy);
                Console.WriteLine("You dealt " + damageDealt + " damage to " + _currentEnemy.Name + ".");

                
            }
            else if (input == 1)
            {
                Console.Clear();
                DisplayEquipMenu();
                Console.ReadKey(true);
                return;
            }
            else if (input == 2)
            {
                Console.Clear();
                if (!_player.TryUnequip())
                    Console.WriteLine("You have nothing equipped!");

                else
                    Console.WriteLine("You placed the item in your bag");

                return;
            }
            else if (input == 3)
            {
                Console.Clear();
                Save();
                Console.WriteLine("Saved Game");
                return;
            }

            damageDealt = _currentEnemy.Attack(_player);
            Console.WriteLine("You took " + damageDealt + " damage.");
            CheckBattleResults();
        }

        /// <summary>
        /// Checks to see if either the player or the enemy has won the current battle.
        /// Updates the game based on who won the battle..
        /// </summary>
        void CheckBattleResults()
        {
            //If the player somehow loses
            if (_player.Health <= 0)
            {
                Console.WriteLine("\nYou Died");
                _currentScene = Scene.RESTARTMENU;
            }

            //If the enemy dies...
            if (_currentEnemy.Health <= 0)
            {
                Console.WriteLine("\nYou slayed the " + _currentEnemy.Name + "!");

                _currentEnemyIndex++;
                if (_currentEnemyIndex >= _enemies.Length)
                {
                    Console.WriteLine("\nCongratulation, You Win!");
                    _currentScene = Scene.RESTARTMENU;
                }
                else
                {
                    _currentEnemy = _enemies[_currentEnemyIndex];
                }
            }
        }

    }
}
