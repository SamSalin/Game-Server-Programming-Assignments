using System;
using System.Collections.Generic;
using System.Linq;

namespace Assignment2
{
    class Program
    {
        static void Main(string[] args)
        {

            // 1. Instantiate 1 000 000 players
            // Create list of Iplayer because we will use the InstantiatePlayers fucntion later for creating Players for antoher game (kohta 7)

            List<IPlayer> players = new List<IPlayer>();                 // Create list for the 1 000 000 players
            List<IPlayer> playersForAnotherGame = new List<IPlayer>();  // Create list for the 1 000 000 players for antoher game
            List<Guid> guids = new List<Guid>();                        // Get the player GUIDS in a seperate list o check for duplicates
            List<Guid> playersForAnotherGameGuids = new List<Guid>();

            try
            {
                InstantiatePlayers(players, guids, false);                                                   // Create 1 000 000 players for one game
                InstantiatePlayers(playersForAnotherGame, playersForAnotherGameGuids, true);                 // Create 1 000 000 players for another game
            }
            catch (Exception)
            {
                Console.WriteLine("Duplicates found!\n");
            }

            // 2. Generates a test player and 10 items with random stats

            Player testPlayer = new Player();
            testPlayer.Items = new List<Item>();
            Random rnd = new Random();

            for (int i = 0; i < 10; i++)
            {
                Item item = new Item();
                item.Id = Guid.NewGuid();
                //item.Level = i;
                item.Level = rnd.Next(1, 51);
                testPlayer.Items.Add(item);
            }

            Console.WriteLine("Test player highest level item: " + testPlayer.GetHighestLevelItem().Level + "\n");

            // 3. Calls GetItems() & GetItemsWithLinq() for the same test player that we created in previous exercise

            Item[] itemsArray1 = GetItems(testPlayer.Items);
            Item[] itemsArray2 = GetItemsWithLinq(testPlayer.Items);

            // Test-loop for the GetItems functions

            /* for (int i = 0; i < itemsArray1.Length; i++)
            {
                Console.WriteLine(itemsArray1[i].Level);
            } */

            // 4. Calls FirstItem() & FirstItemWithLinq() for the same test player that we created in previous exercise 

            Item firstItem1 = FirstItem(testPlayer);
            Item firstItem2 = FirstItemWithLinq(testPlayer);

            // Test

            Console.WriteLine("First item was item with level: " + firstItem1.Level + "\n");

            // 5. Delegates

            Del handler = ProcessEachItem;
            handler(testPlayer, PrintItem);
            Console.WriteLine("\n");

            // 6. Lambda

            Action<Item> delegateInstance = (Item item) => Console.WriteLine("Item ID: " + item.Id.ToString() + " & level: " + item.Level.ToString());
            handler(testPlayer, delegateInstance);

            // 7. Let's create Game & PlayerForAnotherGame instances and try to find their top players

            var game = new Game<IPlayer>(players);           // Use the 1 000 000 players we created earlier
            IPlayer[] top10Players = game.GetTop10Players();
            Console.WriteLine("\nTop 10 players are:\n");

            for (int i = 0; i < top10Players.Length; i++)
            {
                Console.WriteLine((i + 1) + ". Player ID: " + top10Players[i].Id + " and score: " + top10Players[i].Score);
            }

            var anotherGame = new Game<IPlayer>(playersForAnotherGame); // Use the 1 000 000 players for another game we created earlier
            IPlayer[] top10PlayersForAnotherGame = anotherGame.GetTop10Players();
            Console.WriteLine("\nTop 10 players for another game are:\n");

            for (int i = 0; i < top10PlayersForAnotherGame.Length; i++)
            {
                Console.WriteLine((i + 1) + ". Player ID: " + top10PlayersForAnotherGame[i].Id + " and score: " + top10PlayersForAnotherGame[i].Score);
            }
        }
        public static void InstantiatePlayers(List<IPlayer> players, List<Guid> ids, bool playerForAntoherGame)
        {
            Random rnd = new Random();
            IPlayer player;

            for (int i = 0; i < 1000000; i++)
            {
                if (playerForAntoherGame)
                {
                    player = new Player();
                }
                else
                {
                    player = new PlayerForAnotherGame();
                }

                player.Id = Guid.NewGuid();
                player.Score = rnd.Next(1, 10000000);
                ids.Add(player.Id);
                players.Add(player);

            }

            // Check if any players have same GUID

            if (ids.Count != ids.Distinct().Count())
            {
                throw new Exception();
            }
        }

        public static Item[] GetItems(List<Item> items)
        {
            Item[] itemsArray = new Item[items.Count];

            for (int i = 0; i < itemsArray.Length; i++)
            {
                itemsArray[i] = items.ElementAt(i);
            }

            return itemsArray;
        }
        public static Item[] GetItemsWithLinq(List<Item> items)
        {
            return items.ToArray();
        }

        public static Item FirstItem(Player player)
        {
            if (player.Items == null)
            {
                return null;
            }
            return player.Items[0];
        }

        public static Item FirstItemWithLinq(Player player)
        {
            return player.Items.FirstOrDefault();
        }

        public delegate void Del(Player player, Action<Item> process);

        public static void ProcessEachItem(Player player, Action<Item> process)
        {
            for (int i = 0; i < player.Items.Count; i++)
            {
                process(player.Items[i]);
            }
        }

        public static void PrintItem(Item item)
        {
            Console.WriteLine("Item ID: " + item.Id.ToString() + " & level: " + item.Level.ToString());
        }
    }
    public static class PlayerExtensions
    {
        public static Item GetHighestLevelItem(this Player player)
        {
            Item highestLevelItem = new Item();
            highestLevelItem.Level = 0;

            for (int i = 0; i < player.Items.Count(); i++)
            {
                if (player.Items[i].Level >= highestLevelItem.Level)
                {
                    highestLevelItem = player.Items[i];
                }
            }
            return highestLevelItem;
        }
    }
}
