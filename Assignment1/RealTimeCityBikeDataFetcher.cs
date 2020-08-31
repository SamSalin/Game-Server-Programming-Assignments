using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Linq;
using System;

public class RealTimeCityBikeDataFetcher : ICityBikeDataFetcher
{

    public async Task<int> GetBikeCountInStation(string stationName)
    {
        bool numbersInString = stationName.Any(char.IsDigit);

        if(numbersInString)
        {
            throw new ArgumentException();
        }

        BikeRentalStationList asemaLista;
        int bikesAvailable = 0;
        bool stationFound = false;
        string url = "http://api.digitransit.fi/routing/v1/routers/hsl/bike_rental";

        HttpClient client = new HttpClient();
        asemaLista = JsonConvert.DeserializeObject<BikeRentalStationList>(await client.GetStringAsync(url));    //Parse JSON into a readable format
    
        for (int i = 0; i < asemaLista.stations.Count; i++)
        {
            if (asemaLista.stations[i].name == stationName)
            {
                stationFound = true;
                bikesAvailable = asemaLista.stations[i].bikesAvailable;
            }
        }

        if(!stationFound)
        {
            throw new NotFoundException("Station not found!");
        }

        return bikesAvailable;
    }

}