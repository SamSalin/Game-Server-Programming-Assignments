using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GameWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly IRepository _repo;
        public PlayersController(IRepository fileRepository)
        {
            _repo = fileRepository;
        }

        [HttpGet("/players/{id}")]
        public Task<Player> Get(Guid id)
        {
            return _repo.Get(id);
        }

        [HttpGet("/players/all")]
        public Task<Player[]> GetAll()
        {
            return _repo.GetAll();
        }

        [HttpPost("/players/create")]
        public Task<Player> Create(NewPlayer player)
        {
            Player newPlayer = new Player()
            {
                Name = player.Name,
                Id = Guid.NewGuid(),
                CreationTime = DateTime.Now,
                PlayerItems = new List<Item>()
            };

            return _repo.Create(newPlayer);
        }

        [HttpPut("/players/modify/{id}")]
        public Task<Player> Modify(Guid id, ModifiedPlayer player)
        {
            return _repo.Modify(id, player);
        }

        [HttpDelete("/players/delete/{id}")]
        public Task<Player> Delete(Guid id)
        {
            return _repo.Delete(id);
        }

        //---------Queries & Aggregation------------

        [HttpGet("/players")]
        public Task<List<Player>> GetPlayersMinScore([FromQuery] int minScore)
        {
            return _repo.GetPlayersMinScore(minScore);
        }

        [HttpPut("/players/{id}/updatename")]
        public Task<Player> UpdatePlayerName(Guid id, [FromQuery] string name)
        {
            return _repo.UpdatePlayerName(id, name);
        }

        [HttpGet("/players/items")]
        public Task<List<Player>> GetPlayersByItemListSize([FromQuery] int numOfItems)
        {
            return _repo.GetPlayersByItemListSize(numOfItems);
        }

        [HttpGet("/players/descendingscore")]
        public Task<List<Player>> GetPlayersByDescendingScore()
        {
            return _repo.GetPlayersByDescendingScore();
        }

        [HttpGet("/players/mostcommonlevel")]
        public Task<LevelCount[]> GetMostCommonLevel()
        {
            return _repo.GetMostCommonLevel();
        }

        //----------------------------
    }
}