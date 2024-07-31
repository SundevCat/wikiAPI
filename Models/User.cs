using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace wikiAPI.Models;

public class User
{
    [BsonId]
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Status { get; set; }
}
