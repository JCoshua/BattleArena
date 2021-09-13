using System;
using System.Collections.Generic;
using System.Text;

namespace BattleArena
{   
    /// <summary>
    /// Represents any entity that exists in game
    /// </summary>
    struct Character
    {
        public string name;
        public float health;
        public float attackPower;
        public float defensePower;
    }

    class Game
    {
        bool gameOver;
        int currentScene = 0;
        Character player;
        Character[] enemies;

        private int currentEnemyIndex = 0;
        private Character currentEnemy;

        /// <summary>
        /// Function that starts the main game loop
        /// </summary>
        public void Run()
        {
            Start();

            while (!gameOver)
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
            player.name = "Player";
            player.health = 1;
            player.attackPower = 1;
            player.defensePower = 1;

            Character slime = new Character { name = "Slime", health = 10, attackPower = 5, defensePower = 0 };
            Character zombie = new Character { name = "Zom-B", health = 20, attackPower = 15, defensePower = 5 };
            Character kris = new Character { name = "guy named Kris", health = 30, attackPower = 20, defensePower = 10};

            enemies = new Character[] { slime, zombie, kris };
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
            currentEnemyIndex = 0;
            currentEnemy = enemies[currentEnemyIndex];
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
            switch (currentScene)
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
            currentScene = 1;
        }

        /// <summary>
        /// Displays the menu that allows the player to restart or quit the game
        /// </summary>
        void DisplayRestartMenu()
        {
           int input = GetInput("Would you like to play again?", "Yes", "No");
            if (input == 1)
            {
                ResetEnemy();
                currentScene = 0;
            }
            else if (input == 2)
            {
                gameOver = true;
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
                player.name = Console.ReadLine();
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
                player.health = 50;
                player.attackPower = 25;
                player.defensePower = 5;
            }
            else if (input == 2)
            {
                player.health = 75;
                player.attackPower = 15;
                player.defensePower = 10;
            }
        }

        /// <summary>
        /// Prints a characters stats to the console
        /// </summary>
        /// <param name="character">The character that will have its stats shown</param>
        void DisplayStats(Character character)
        {
            Console.WriteLine(character.name + " Health: " + character.health);
            Console.WriteLine(character.name + " Attack: " + character.attackPower);
            Console.WriteLine(character.name + " Defence: " + character.defensePower);
            Console.WriteLine();
        }

        /// <summary>
        /// Calculates the amount of damage that will be done to a character
        /// </summary>
        /// <param name="attackPower">The attacking character's attack power</param>
        /// <param name="defensePower">The defending character's defense power</param>
        /// <returns>The amount of damage done to the defender</returns>
        float CalculateDamage(float attackPower, float defensePower)
        {
            if (attackPower > defensePower)
            {
                return attackPower - defensePower;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// Deals damage to a character based on an attacker's attack power
        /// </summary>
        /// <param name="attacker">The character that initiated the attack</param>
        /// <param name="defender">The character that is being attacked</param>
        /// <returns>The amount of damage done to the defender</returns>
        public float Attack(ref Character attacker, ref Character defender)
        {
            float damageTaken = CalculateDamage(attacker.attackPower, defender.defensePower);
            defender.health -= damageTaken;

            if (defender.health == 0)
            {
                defender.health = 0;
            }

            return damageTaken;
        }

        /// <summary>
        /// Simulates one turn in the current monster fight
        /// </summary>
        public void Battle()
        {
            DisplayStats(player);
            DisplayStats(currentEnemy);
            float damageDealt;

            int input = GetInput("A " + currentEnemy.name + " stands in front of you. What will you do?","Attack", "Dodge");
            if (input == 1)
            {
                damageDealt = Attack(ref player, ref currentEnemy);
                Console.WriteLine("You dealt " + damageDealt + " damage to " + currentEnemy.name + ".");

                
            }
            if (input == 2)
            {
                Console.WriteLine("You rolled out of the way.");
                return;
            }


            damageDealt = Attack(ref currentEnemy, ref player);
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
            if (player.health <= 0)
            {
                Console.WriteLine("\nYou Died");
                currentScene = 2;
            }

            //If the enemy dies...
            if (currentEnemy.health <= 0)
            {
                Console.WriteLine("\nYou slayed the " + currentEnemy.name + "!");

                currentEnemyIndex++;
                if (currentEnemyIndex >= enemies.Length)
                {
                    Console.WriteLine("\nCongratulation, You Win!");
                    currentScene = 2;
                }
                else
                {
                    currentEnemy = enemies[currentEnemyIndex];
                }
            }
        }

    }
}
