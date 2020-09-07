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

            // If station name has more than one word -> Combine station name/arguments into one

            if (args.Length > 2)    //If more than two arguments
            {
                for (int i = 1; i < args.Length - 1; i++)
                {
                    stationName += " " + args[i];
                }
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
                Console.WriteLine("Invalid argument: " + ex.Message);
            }
            catch (NotFoundException ex)
            {
                Console.WriteLine("Not found: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

