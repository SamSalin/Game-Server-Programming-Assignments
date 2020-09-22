using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GameWebApi
{
    public class MongoDbRepository : IRepository
    {
        private readonly IMongoCollection<Player> _playerCollection;
        private readonly IMongoCollection<BsonDocument> _bsonDocumentCollection;

        public MongoDbRepository()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var database = mongoClient.GetDatabase("game");
            _playerCollection = database.GetCollection<Player>("players");
            _bsonDocumentCollection = database.GetCollection<BsonDocument>("players");
        }

        public async Task<Player> Create(Player player)
        {
            await _playerCollection.InsertOneAsync(player);
            return player;
        }

        public async Task<Item> CreateItem(Guid playerId, Item item)
        {
            var filter = Builders<Player>.Filter.Eq(p => p.Id, playerId);
            var add = Builders<Player>.Update.AddToSet("PlayerItems", item);

            Player player = await _playerCollection.FindOneAndUpdateAsync(filter, add);
            return item;
        }

        public async Task<Player> Delete(Guid id)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, id);
            return await _playerCollection.FindOneAndDeleteAsync(filter);
        }

        public async Task<Item> DeleteItem(Guid playerId, Guid itemId)
        {
            Item deletedItem = await GetItem(playerId, itemId);

            var filter = Builders<Player>.Filter.Eq(p => p.Id, playerId);
            var delete = Builders<Player>.Update.PullFilter(p => p.PlayerItems, i => i.Id == itemId);

            await _playerCollection.UpdateOneAsync(filter, delete);
            return deletedItem;
        }

        public async Task<Player> Get(Guid id)
        {
            var filter = Builders<Player>.Filter.Eq("Id", id);

            if (!_playerCollection.Find(filter).Any())
            {
                throw new NotFoundException();
            }
            else
            {
                return await _playerCollection.Find(filter).FirstAsync();
            }

        }

        public async Task<Player[]> GetAll()
        {
            var players = await _playerCollection.Find(new BsonDocument()).ToListAsync();
            return players.ToArray();
        }

        public async Task<Item[]> GetAllItems(Guid playerId)
        {
            var playerFilter = Builders<Player>.Filter.Eq(p => p.Id, playerId);
            Player player = await _playerCollection.Find(playerFilter).FirstAsync();

            return player.PlayerItems.ToArray();
        }

        public async Task<Item> GetItem(Guid playerId, Guid itemId)
        {
            var playerFilter = Builders<Player>.Filter.Eq(p => p.Id, playerId);
            Player player = await _playerCollection.Find(playerFilter).FirstAsync();

            Item item = player.PlayerItems.Single(i => i.Id == itemId);

            return item;
        }

        public async Task<Player> Modify(Guid id, ModifiedPlayer player)
        {
            Player newPlayer = await Get(id);
            newPlayer.Score = player.Score;

            var filter = Builders<Player>.Filter.Eq("Id", newPlayer.Id);
            await _playerCollection.ReplaceOneAsync(filter, newPlayer);

            return newPlayer;
        }

        public async Task<Item> UpdateItem(Guid playerId, Item item)
        {
            var filter = Builders<Player>.Filter.Where(p => p.Id == playerId && p.PlayerItems.Any(i => i.Id == item.Id));
            var update = Builders<Player>.Update.Set(u => u.PlayerItems[-1].Level, item.Level);

            await _playerCollection.FindOneAndUpdateAsync(filter, update);

            Item getItem = await GetItem(playerId, item.Id);
            return getItem;
        }
    }
}