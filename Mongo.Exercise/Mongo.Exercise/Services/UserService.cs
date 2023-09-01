using Microsoft.Extensions.Options;
using Mongo.Exercise.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class UserService
{
	private readonly IMongoCollection<UserModel> _userCollection;

	public UserService(IMongoClient mongoClient, IOptions<Settings> settings)
	{
		var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
		_userCollection = database.GetCollection<UserModel>(nameof(UserModel));
	}

	public async Task<bool> RegisterUserAsync(UserModel user)
	{
		// Kullanıcı adı mevcut değilse kullanıcıyı kaydet
		if (_userCollection.Find(u => u.Username == user.Username).Any())
		{
			return false;
		}

		await _userCollection.InsertOneAsync(user);
		return true;
	}

	public async Task<UserModel> AuthenticateAsync(string username, string password)
	{
		var user = await _userCollection.Find(u => u.Username == username && u.Password == password).SingleOrDefaultAsync();
		return user;
	}

	public async Task<List<UserModel>> GetAllUsersAsync()
	{
		return await _userCollection.Find(u => true).ToListAsync();
	}
}
