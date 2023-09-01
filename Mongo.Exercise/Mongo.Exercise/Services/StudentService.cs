using Mongo.Exercise.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mongo.Exercise.Services
{
	public class StudentService
	{
		private readonly IMongoCollection<Student> _students;
		private readonly string _collectionName;

		public StudentService(IMongoClient client, Settings settings)
		{
			var database = client.GetDatabase(settings.DatabaseName);
			_students = database.GetCollection<Student>(settings.CollectionName);
			_collectionName = settings.CollectionName;
		}

		public async Task<IEnumerable<Student>> GetStudentsAsync()
		{
			return await _students.Find(s => true).ToListAsync();
		}

		public async Task<Student> GetStudentAsync(string id)
		{
			return await _students.Find(s => s.Id == id).FirstOrDefaultAsync();
		}

		public async Task CreateStudentAsync(Student student)
		{
			await _students.InsertOneAsync(student);
		}

		public async Task<bool> UpdateStudentAsync(string id, Student updatedStudent)
		{
			var filter = Builders<Student>.Filter.Eq(s => s.Id, id);
			var update = Builders<Student>.Update
				.Set(s => s.Name, updatedStudent.Name)
				.Set(s => s.Age, updatedStudent.Age)
				.Set(s => s.Number, updatedStudent.Number)
				.Set(s => s.Class, updatedStudent.Class);

			var result = await _students.UpdateOneAsync(filter, update);
			return result.ModifiedCount > 0;
		}

		public async Task<bool> DeleteStudentAsync(string id)
		{
			var filter = Builders<Student>.Filter.Eq(s => s.Id, id);
			var result = await _students.DeleteOneAsync(filter);
			return result.DeletedCount > 0;
		}
	}
}
