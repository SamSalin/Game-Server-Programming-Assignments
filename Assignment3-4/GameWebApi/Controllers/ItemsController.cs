using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GameWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository _repo;

        public ItemsController(IRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("/players/{playerId}/items/create")]
        public Task<Item> CreateItem(Guid playerId, NewItem item)
        {

            Item newItem = new Item()
            {
                Id = Guid.NewGuid(),
                Level = item.Level,
                Type = item.Type,
                CreationDate = DateTime.Today
                //CreationDate = new DateTime(2009, 8, 1, 0, 0, 0)
            };
            return _repo.CreateItem(playerId, newItem);
        }
        [HttpGet("/players/{playerId}/items/get/{itemId}")]
        public Task<Item> GetItem(Guid playerId, Guid itemId)
        {
            return _repo.GetItem(playerId, itemId);
        }

        [HttpGet("/players/{playerId}/items/all")]
        public Task<Item[]> GetAllItems(Guid playerId)
        {
            return _repo.GetAllItems(playerId);
        }

        [HttpPut("/players/{playerId}/items/modify/{itemId}")]
        public Task<Item> UpdateItem(Guid playerId, Guid itemId, ModifiedItem modifiedItem)
        {
            Item newItem = new Item()
            {
                Id = itemId,
                Level = modifiedItem.Level,
                Type = modifiedItem.Type,
                CreationDate = DateTime.Today
            };

            return _repo.UpdateItem(playerId, newItem);
        }

        [HttpDelete("/players/{playerId}/items/delete/{itemId}")]
        public Task<Item> DeleteItem(Guid playerId, Guid itemId)
        {
            return _repo.DeleteItem(playerId, itemId);
        }

        //---------Queries------------

        [HttpPost("/players/{playerId}/items/createquery")]
        public Task<Item> CreateItemQuery(Guid playerId, NewItem item)
        {

            Item newItem = new Item()
            {
                Id = Guid.NewGuid(),
                Level = item.Level,
                Type = item.Type,
                CreationDate = DateTime.Today
            };

            return _repo.CreateItemQuery(playerId, newItem);
        }

        //---------------------------
    }
}