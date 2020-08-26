using System.Threading.Tasks;

public interface ICityBikeDataFetcher
{
    Task<int> GetBikeCountInStation(string stationName);
}