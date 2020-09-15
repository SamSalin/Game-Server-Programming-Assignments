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
                player = JsonConvert.DeserializeObject<Player>(lines[i]);

                if (player.Id == id)
                {
                    Console.WriteLine("Löytyi!");
                    return Task.FromResult<Player>(player);
                }
            }

            Console.WriteLine("Ei löytynyt!");
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
            Player oldPlayer = new Player();
            Console.WriteLine(id);

            for (int i = 0; i < lines.Length; i++)
            {
                oldPlayer = JsonConvert.DeserializeObject<Player>(lines[i]);
                Console.WriteLine(oldPlayer.Id);

                if (oldPlayer.Id == id)
                {
                    Console.WriteLine(player.Score);
                    oldPlayer.Score = player.Score;
                    break;
                }
            }

            return Task.FromResult(oldPlayer);
        }

        public Task<Player> Delete(Guid id)
        {
            string[] lines = File.ReadAllLines(@"game-dev.txt");
            List<string> newLines = new List<string>();
            Player[] players = new Player[lines.Length];
            Player player = new Player();

            //Rewrite all the lines that are not the deleted player

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