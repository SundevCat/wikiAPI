
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using wikiAPI.Models;


namespace wikiAPI.Services;

public class UserService
{
    private readonly IMongoCollection<User> _user;
    public UserService(IOptions<DatabaseSettings> databaseSettings)
    {
        var settings = databaseSettings.Value;
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);
        _user = database.GetCollection<User>(settings.UserCollection);
    }
    public async Task<List<User>> Users() => await _user.Find(user => user.UserId != null).ToListAsync();
    public async Task<User> UserById(string id) => await _user.Find(user => user.UserId == id).FirstOrDefaultAsync();
    public async Task<User> UserByUserName(string username) => await _user.Find(user => user.UserName == username).FirstOrDefaultAsync();
    public async Task<User> CreateUser(User user)
    {
        user.UserId = Guid.NewGuid().ToString();
        await _user.InsertOneAsync(user);
        return user;
    }

    public async Task UpdateUser(string id, User newUser) => await _user.ReplaceOneAsync(user => user.UserId == id, newUser);
    public async Task DeleteUser(string id) => await _user.DeleteOneAsync(user => user.UserId == id);

}