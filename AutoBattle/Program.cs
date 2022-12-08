using System;
using static AutoBattle.Character;
using static AutoBattle.Grid;
using System.Collections.Generic;
using System.Linq;
using static AutoBattle.Types;

namespace AutoBattle
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\nPick the size of the grid (x, y): ");
            //Reading the x values and y values for the grid size
            int gridSizeX = Convert.ToInt32(Console.ReadLine());
            int gridSizeY = Convert.ToInt32(Console.ReadLine());
            Grid grid = new Grid(gridSizeX, gridSizeY);
            CharacterClass playerCharacterClass;

            //I've decided to initialize this struct due to, if the struct is not initialized it my cause a problem of fields no having values
            GridBox PlayerCurrentLocation = new GridBox();
            GridBox EnemyCurrentLocation;
            Character PlayerCharacter;
            Character EnemyCharacter;
            List<Character> AllPlayers = new List<Character>();
            int currentTurn = 0;
            int numberOfPossibleTiles = grid.grids.Count;
            Setup();


            void Setup()
            {
                GetPlayerChoice();
            }

            void SetupGrid()
            {
                grid.enemyPosition = EnemyCurrentLocation;
                grid.playerPosition = PlayerCurrentLocation;
            }

            void GetPlayerChoice()
            {
                //asks for the player to choose between for possible classes via console.
                Console.WriteLine("Choose Between One of this Classes:\n");
                Console.WriteLine("[1] Paladin, [2] Warrior, [3] Cleric, [4] Archer");
                //store the player choice in a variable
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreatePlayerCharacter(Int32.Parse(choice));
                        break;
                    case "2":
                        CreatePlayerCharacter(Int32.Parse(choice));
                        break;
                    case "3":
                        CreatePlayerCharacter(Int32.Parse(choice));
                        break;
                    case "4":
                        CreatePlayerCharacter(Int32.Parse(choice));
                        break;
                    default:
                        GetPlayerChoice();
                        break;
                }
            }

            void CreatePlayerCharacter(int classIndex)
            {
                CharacterClass characterClass = (CharacterClass)classIndex;
                Console.WriteLine($"Player Class Choice: {characterClass}");
                PlayerCharacter = new Character(characterClass, PlayerCurrentLocation, true);
                PlayerCharacter.Health = 100;
                PlayerCharacter.BaseDamage = 20;
                PlayerCharacter.PlayerIndex = 0;

                CreateEnemyCharacter();
            }

            void CreateEnemyCharacter()
            {
                //randomly choose the enemy class and set up vital variables
                var rand = new Random();
                int randomInteger = rand.Next(1, 4);
                CharacterClass enemyClass = (CharacterClass)randomInteger;
                Console.WriteLine($"Enemy Class Choice: {enemyClass}");
                EnemyCharacter = new Character(enemyClass, PlayerCurrentLocation, false);
                EnemyCharacter.Health = 100;
                PlayerCharacter.BaseDamage = 20;
                PlayerCharacter.PlayerIndex = 1;

                StartGame();
            }

            void StartGame()
            {
                //populates the character variables and targets
                EnemyCharacter.Target = PlayerCharacter;
                PlayerCharacter.Target = EnemyCharacter;
                AllPlayers.Add(PlayerCharacter);
                AllPlayers.Add(EnemyCharacter);
                AlocatePlayers();
                StartTurn();

            }

            void StartTurn()
            {

                if (currentTurn == 0)
                {
                    //AllPlayers.Sort();  
                }

                foreach (Character character in AllPlayers)
                {
                    character.StartTurn(grid);
                }

                currentTurn++;
                HandleTurn();
            }

            void HandleTurn()
            {
                if (PlayerCharacter.Health == 0)
                {
                    return;
                }
                else if (EnemyCharacter.Health == 0)
                {
                    Console.Write(Environment.NewLine + Environment.NewLine);

                    // endgame?

                    Console.Write(Environment.NewLine + Environment.NewLine);

                    return;
                }
                else
                {
                    Console.Write(Environment.NewLine + Environment.NewLine);
                    Console.WriteLine("Click on any key to start the next turn...\n");
                    Console.Write(Environment.NewLine + Environment.NewLine);

                    ConsoleKeyInfo key = Console.ReadKey();
                    StartTurn();
                }
            }

            int GetRandomInt(int min, int max)
            {
                var rand = new Random();
                int index = rand.Next(min, max);
                return index;
            }

            void AlocatePlayers()
            {
                AlocatePlayerCharacter();
                SetupGrid();
            }

            void AlocatePlayerCharacter()
            {
                int random = GetRandomInt(0, (gridSizeX * gridSizeY) - 1);
                GridBox RandomLocation = (grid.grids.ElementAt(random));
                Console.Write($"{random}\n");
                if (!RandomLocation.ocupied)
                {
                    GridBox PlayerCurrentLocation = RandomLocation;
                    RandomLocation.ocupied = true;
                    grid.grids[random] = RandomLocation;
                    PlayerCharacter.currentBox = grid.grids[random];
                    AlocateEnemyCharacter();
                }
                else
                {
                    AlocatePlayerCharacter();
                }
            }

            void AlocateEnemyCharacter()
            {
                int random = GetRandomInt(0, (gridSizeX * gridSizeY) - 1);
                GridBox RandomLocation = (grid.grids.ElementAt(random));
                Console.Write($"{random}\n");
                if (!RandomLocation.ocupied)
                {
                    EnemyCurrentLocation = RandomLocation;
                    RandomLocation.ocupied = true;
                    grid.grids[random] = RandomLocation;
                    EnemyCharacter.currentBox = grid.grids[random];
                    grid.drawBattlefield(gridSizeX, gridSizeY);
                }
                else
                {
                    AlocateEnemyCharacter();
                }


            }
        }
    }
}
