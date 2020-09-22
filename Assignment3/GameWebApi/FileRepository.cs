using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

namespace GameWebApi
{
    public class FileRepository : IRepository
    {
        public Task<Player> Get(Guid id)
        {
            string[] lines = File.ReadAllLines(@"game-dev.txt");
            Player player = new Player();

            for (int i = 0; i < lines.Length; i++)
            {
                player = JsonConvert.DeserializeObject<Player>(lines[i]);   //Deserialize lines to Player objects

                if (player.Id == id)
                {
                    return Task.FromResult<Player>(player);
                }
            }

            return Task.FromResult<Player>(null);
        }

        public Task<Player[]> GetAll()
        {
            string[] lines = File.ReadAllLines(@"game-dev.txt");
            Player[] players = new Player[lines.Length];

            for (int i = 0; i < lines.Length; i++)
            {
                Player player = JsonConvert.DeserializeObject<Player>(lines[i]);
                players[i] = player;
            }

            return Task.FromResult<Player[]>(players);
        }

        public Task<Player> Create(Player player)
        {
            using (StreamWriter sw = File.AppendText("game-dev.txt"))
            {
                string json = JsonConvert.SerializeObject(player);
                sw.WriteLine(json);
            }

            return Task.FromResult<Player>(player);
        }

        public Task<Player> Modify(Guid id, ModifiedPlayer player)
        {
            string[] lines = File.ReadAllLines(@"game-dev.txt");
            List<string> newLines = new List<string>();
            Player oldPlayer = new Player();

            //Rewrite all the lines and modify the one line with new data

            for (int i = 0; i < lines.Length; i++)
            {
                oldPlayer = JsonConvert.DeserializeObject<Player>(lines[i]);

                if (oldPlayer.Id == id)
                {
                    oldPlayer.Score = player.Score;
                }

                newLines.Add(JsonConvert.SerializeObject(oldPlayer));
            }

            File.WriteAllLines("game-dev.txt", newLines.ToArray());
            return Task.FromResult(oldPlayer);
        }

        public Task<Player> Delete(Guid id)
        {
            string[] lines = File.ReadAllLines(@"game-dev.txt");
            List<string> newLines = new List<string>();
            Player[] players = new Player[lines.Length];
            Player player = new Player();

            //Rewrite all the lines except the line that is the player to be deleted

            for (int i = 0; i < lines.Length; i++)
            {
                player = JsonConvert.DeserializeObject<Player>(lines[i]);

                if (player.Id == id)
                {
                    continue;
                }
                else
                {
                    newLines.Add(lines[i]);
                }
            }

            File.WriteAllLines("game-dev.txt", newLines.ToArray());
            return Task.FromResult<Player>(player);
        }

        public Task<Item> CreateItem(Guid playerId, Item item)
        {
            bool playerFound = false;
            string[] lines = File.ReadAllLines(@"game-dev.txt");
            List<string> newLines = new List<string>();
            Player player = new Player();

            for (int i = 0; i < lines.Length; i++)
            {
                player = JsonConvert.DeserializeObject<Player>(lines[i]);

                if (player.Id == playerId)
                {
                    playerFound = true;
                    player.PlayerItems.Add(item);
                }

                newLines.Add(JsonConvert.SerializeObject(player));
            }

            if (!playerFound)
            {
                throw new NotFoundException();
            }

            File.WriteAllLines("game-dev.txt", newLines.ToArray());
            return Task.FromResult(item);
        }

        public Task<Item> GetItem(Guid playerId, Guid itemId)
        {
            string[] lines = File.ReadAllLines(@"game-dev.txt");
            Player player = new Player();

            for (int i = 0; i < lines.Length; i++)
            {
                player = JsonConvert.DeserializeObject<Player>(lines[i]);

                if (player.Id == playerId)
                {
                    for (int j = 0; j < player.PlayerItems.Count; j++)
                    {
                        if (player.PlayerItems.ElementAt(j).Id == itemId)
                        {
                            return Task.FromResult(player.PlayerItems.ElementAt(j));
                        }
                    }
                }
            }
            return null;
        }

        public Task<Item[]> GetAllItems(Guid playerId)
        {
            string[] lines = File.ReadAllLines(@"game-dev.txt");
            Player player = new Player();

            for (int i = 0; i < lines.Length; i++)
            {
                player = JsonConvert.DeserializeObject<Player>(lines[i]);

                if (player.Id == playerId)
                {
                    return Task.FromResult(player.PlayerItems.ToArray());
                }
            }

            return null;
        }

        public Task<Item> UpdateItem(Guid playerId, Item item)
        {
            string[] lines = File.ReadAllLines(@"game-dev.txt");
            List<string> newLines = new List<string>();
            Player player = new Player();
            Item modifiedItem = new Item();

            for (int i = 0; i < lines.Length; i++)
            {
                player = JsonConvert.DeserializeObject<Player>(lines[i]);

                if (player.Id == playerId)
                {
                    for (int j = 0; j < player.PlayerItems.Count; j++)
                    {
                        if (player.PlayerItems.ElementAt(j).Id == item.Id)
                        {
                            player.PlayerItems.ElementAt(j).Level = item.Level;
                            modifiedItem = player.PlayerItems.ElementAt(j);
                        }
                    }
                }

                newLines.Add(JsonConvert.SerializeObject(player));
            }
            File.WriteAllLines("game-dev.txt", newLines.ToArray());
            return Task.FromResult(modifiedItem);
        }

        public Task<Item> DeleteItem(Guid playerId, Guid itemId)
        {

            string[] lines = File.ReadAllLines(@"game-dev.txt");
            List<string> newLines = new List<string>();
            Player player = new Player();
            Item deletedItem = new Item();

            for (int i = 0; i < lines.Length; i++)
            {
                player = JsonConvert.DeserializeObject<Player>(lines[i]);

                if (player.Id == playerId)
                {

                    for (int j = 0; j < player.PlayerItems.Count; j++)
                    {
                        if (player.PlayerItems.ElementAt(j).Id == itemId)
                        {
                            deletedItem = player.PlayerItems.ElementAt(j);
                            player.PlayerItems.Remove(player.PlayerItems.ElementAt(j));
                        }
                    }
                }

                newLines.Add(JsonConvert.SerializeObject(player));
            }

            File.WriteAllLines("game-dev.txt", newLines.ToArray());
            return Task.FromResult(deletedItem);
        }
    }
}