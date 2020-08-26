using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Assignment1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string stationName = args[0];

            // If station name has more than one word

            if (args[1] != "realtime" && args[1] != "offline")
            {
                stationName = args[0] + " " + args[1];
            }

            try
            {
                if (args[args.Length - 1] == "realtime")
                {
                    RealTimeCityBikeDataFetcher onlineFetcher = new RealTimeCityBikeDataFetcher();
                    Console.WriteLine("Number of Available Bikes: " + await onlineFetcher.GetBikeCountInStation(stationName));
                }
                else if (args[args.Length - 1] == "offline")
                {
                    OfflineCityBikeFetcher offlineFetcher = new OfflineCityBikeFetcher();
                    Console.WriteLine("Number of Available Bikes: " + await offlineFetcher.GetBikeCountInStation(stationName));
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Invalid argument: " + ex);
            }
            catch (NotFoundException ex)
            {
                Console.WriteLine("Invalid argument: " + ex);
            }
        }
    }
}

