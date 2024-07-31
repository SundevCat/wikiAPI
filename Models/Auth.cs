
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace wikiAPI.Models;

public class Auth
{
    [BsonId]
    public string _Id { get; set; }
    public string authUsername { get; set; }
    public string authPassword { get; set; }
    public string role { get; set; }

}
