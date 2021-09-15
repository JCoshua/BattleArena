using System;
using System.Collections.Generic;
using System.Text;

namespace BattleArena
{  

    class Game
    {
        private bool _gameOver;
        private int _currentScene = 0;
        private Entity _player;
        private Entity[] _enemies;
        private string _playerName;

        private int _currentEnemyIndex = 0;
        private Entity _currentEnemy;

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
        }

        public void InitializeEnemies()
        {
            Entity slime = new Entity("Slime", 10, 5, 0);
            Entity zombie = new Entity("Zom-B", 20, 15, 5);
            Entity kris = new Entity("guy named Kris", 30, 20, 10);

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
        int GetInput(string description, string option1, string option2)
        {
            string input = "";
            int inputReceived = 0;

            while (inputReceived != 1 && inputReceived != 2)
            {//Print options
                Console.WriteLine(description);
                Console.WriteLine("1. " + option1);
                Console.WriteLine("2. " + option2);
                Console.Write("> ");

                //Get input from player
                input = Console.ReadLine();

                //If player selected the first option...
                if (input == "1" || input == option1)
                {
                    //Set input received to be the first option
                    inputReceived = 1;
                }
                //Otherwise if the player selected the second option...
                else if (input == "2" || input == option2)
                {
                    //Set input received to be the second option
                    inputReceived = 2;
                }
                //If neither are true...
                else
                {
                    //...display error message
                    Console.WriteLine("Invalid Input");
                    Console.ReadKey();
                }

                Console.Clear();
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
                case 0:
                    DisplayMainMenu();
                    break;

                case 1:
                    Battle();
                    Console.ReadKey(true);
                    Console.Clear();
                    break;

                case 2:
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
            _currentScene = 1;
        }

        /// <summary>
        /// Displays the menu that allows the player to restart or quit the game
        /// </summary>
        void DisplayRestartMenu()
        {
           int input = GetInput("Would you like to play again?", "Yes", "No");
            if (input == 1)
            {
                InitializeEnemies();
                _currentScene = 0;
            }
            else if (input == 2)
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
            int input = 2;
            while (input != 1)
            {
                Console.WriteLine("Hello. Please enter your Name.");
                _playerName = Console.ReadLine();
                input = GetInput("Are you okay with this name?", "Yes", "No");
            }

        }

        /// <summary>
        /// Gets the players choice of character. Updates player stats based on
        /// the character chosen.
        /// </summary>
        public void CharacterSelection()
        {
            int input = GetInput("Please Select your class:", "Wizard", "Knight");
            if (input == 1)
            {
                _player = new Entity(_playerName, 50, 25, 5);
            }
            else if (input == 2)
            {
                _player = new Entity(_playerName, 75, 15, 10);
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

        
        public void Battle()
        {
            DisplayStats(_player);
            DisplayStats(_currentEnemy);
            float damageDealt = 0;

            int input = GetInput("A " + _currentEnemy.Name + " stands in front of you. What will you do?","Attack", "Dodge");
            if (input == 1)
            {
                damageDealt = _player.Attack(_currentEnemy);
                Console.WriteLine("You dealt " + damageDealt + " damage to " + _currentEnemy.Name + ".");

                
            }
            if (input == 2)
            {
                Console.WriteLine("You rolled out of the way.");
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
                _currentScene = 2;
            }

            //If the enemy dies...
            if (_currentEnemy.Health <= 0)
            {
                Console.WriteLine("\nYou slayed the " + _currentEnemy.Name + "!");

                _currentEnemyIndex++;
                if (_currentEnemyIndex >= _enemies.Length)
                {
                    Console.WriteLine("\nCongratulation, You Win!");
                    _currentScene = 2;
                }
                else
                {
                    _currentEnemy = _enemies[_currentEnemyIndex];
                }
            }
        }

    }
}
