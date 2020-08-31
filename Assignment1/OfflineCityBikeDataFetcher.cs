using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class OfflineCityBikeFetcher : ICityBikeDataFetcher
{
    public async Task<int> GetBikeCountInStation(string stationName)
    {
        bool numbersInString = stationName.Any(char.IsDigit);

        if (numbersInString)
        {
            throw new ArgumentException();
        }

        string[] lines = File.ReadAllLines(@"bikedata.txt");    //Read bikedata.txt
        int bikeAmount = 0;
        bool stationFound = false;

        for (int i = 0; i < lines.Length; i++)
        {
            string[] splitLine = lines[i].Split(":");           //Split individual line into two

            if (splitLine[0].Trim() == stationName)             //Trim to remove whitespace
            {
                stationFound = true;
                String bikeAmountString = splitLine[1].Trim();
                bikeAmount = Int32.Parse(bikeAmountString);     //Convert String to int
                break;
            }
        }

        if(!stationFound)
        {
            throw new NotFoundException("Station not found!");
        }

        return await Task.FromResult(bikeAmount);
    }
}