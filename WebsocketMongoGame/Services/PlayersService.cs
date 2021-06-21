using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using WebsocketMongoGame.Domain;

namespace WebsocketMongoGame.Services
{
	public class PlayersService
    {
		readonly IMongoCollection<Player> players;

		public PlayersService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

			players = database.GetCollection<Player>(settings.PlayerCollection);
        }

        public List<Player> Get() => players.Find(emp => true).ToList();

		public Player Get(string id) => players.Find(p => p.Id == id).SingleOrDefault();
	
		public List<Player> Get(string[] ids) => players.Find(p => ids.Contains(p.Id)).ToList();

		public List<string> Generate()
		{
			var newPlayers = new List<Player>();
			var rnd = new Random();
			var prefix = DateTime.Now.ToString()
				.Replace(" ", "")
				.Replace("-", "")
				.Replace(":", "");

			for (int i = 0; i < 1000; i++)
			{
				newPlayers.Add(new Player
				{
					Name = $"player{i}",
					SkillPoints = rnd.Next(200),
				});
			}

			players.InsertMany(newPlayers);

			var newPlayerIds = newPlayers.Select(p => p.Id).ToList();
			newPlayers.ForEach(p => p.FriendIds = newPlayerIds.Where(id => id != p.Id).ToList());

			foreach(var newPlayer in newPlayers)
			{
				players.UpdateOne(
				   p => p.Id == newPlayer.Id,
				   Builders<Player>.Update.Set("FriendIds", newPlayerIds.Where(id => id != newPlayer.Id).ToList()),
				   new UpdateOptions { IsUpsert = true });
			}
			

			return default;
		}
	}
}