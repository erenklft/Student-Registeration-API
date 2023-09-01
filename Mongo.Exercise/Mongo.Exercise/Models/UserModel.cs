using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Mongo.Exercise.Models
{
	public class UserModel
	{
		public UserModel()
		{
			Id = ObjectId.GenerateNewId().ToString();
		}
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; private set; }
		

		[Required]
		public string Username { get; set; }

		[Required]
		public string Password { get; set; }
	}
}
