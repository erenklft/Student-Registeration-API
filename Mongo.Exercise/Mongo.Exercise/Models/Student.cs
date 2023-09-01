using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Mongo.Exercise.Models
{
	public class Student
	{
        public Student()
        {
            Id = ObjectId.GenerateNewId().ToString();
		}
        [BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; private set; }

		
		public string Name { get; set; }
		public int Age { get; set; }
		public int Number { get; set; }
		public string Class { get; set; }
	}
}
