using Microsoft.Extensions.Options;
using MongoDB.Driver;
using wikiAPI.Models;

namespace wikiAPI.Services;

public class AuthService
{
    private readonly IMongoCollection<Auth> _auth;
    public AuthService(IOptions<DatabaseSettings> databaseSetting)
    {
        var settings = databaseSetting.Value;
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);
        _auth = database.GetCollection<Auth>(settings.AuthCollection);
    }
    public async Task<List<Auth>> Auth() => await _auth.Find(auth => auth.authUsername != null).ToListAsync();
    public async Task<Auth> AuthByUserName(string username) => await _auth.Find(auth => auth.authUsername == username).FirstOrDefaultAsync();

    public async Task<Auth> CreateAuth(Auth auth)
    {
        auth._Id = Guid.NewGuid().ToString();
        await _auth.InsertOneAsync(auth);
        return auth;
    }


}