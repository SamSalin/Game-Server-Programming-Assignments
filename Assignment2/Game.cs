using System.Collections.Generic;
using System.Linq;

public class Game<T> where T : IPlayer
{
    private List<T> _players;

    public Game(List<T> players)
    {
        _players = players;
    }

    public T[] GetTop10Players()
    {
        // ... write code that returns 10 players with highest scores

        return _players.OrderByDescending(i => i.Score).Take(10).ToArray();
    }
}