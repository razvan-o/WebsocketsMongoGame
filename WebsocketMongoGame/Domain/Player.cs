using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace WebsocketMongoGame.Domain
{
	public class Player
    {
		[BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public int SkillPoints { get; set; }

        public List<string> FriendIds { get; set; }
	}
}
