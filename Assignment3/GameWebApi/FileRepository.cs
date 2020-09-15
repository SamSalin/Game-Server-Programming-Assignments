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
    }
}