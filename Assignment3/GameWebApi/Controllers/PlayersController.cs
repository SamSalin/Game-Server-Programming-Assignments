using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        public async Task<Player> Get(string id)
        {
            Guid g = Guid.Parse(id);
            return await _repo.Get(g);
        }

        [HttpGet("/players/all")]
        public async Task<Player[]> GetAll()
        {
            return await _repo.GetAll();
        }

        [HttpPost("/players/create")]
        public async Task<Player> Create(NewPlayer player)
        {
            Player newPlayer = new Player()
            {
                Name = player.Name,
                Id = Guid.NewGuid(),
                CreationTime = DateTime.Today
            };

            return await _repo.Create(newPlayer);
        }

        [HttpPut("/players/modify/{id}")]
        public async Task<Player> Modify(string id, ModifiedPlayer player)
        {
            Guid g = Guid.Parse(id);
            return await _repo.Modify(g, player);
        }

        [HttpDelete("/players/delete/{id}")]
        public async Task<Player> Delete(string id)
        {
            Guid g = Guid.Parse(id);
            return await _repo.Delete(g);
        }
    }
}