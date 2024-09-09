using MongoDB.Bson.Serialization.Attributes;

namespace MoneyMgtMongo.Models
{
    public class Users
    {
        [BsonId]
        [BsonElement("id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? id { get; set; }
        [BsonElement("username"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string? username { get; set; }
        [BsonElement("email"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string? email { get; set; }
        [BsonElement("password"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string? Password { get; set; }
        [BsonElement("customAccounts")]
        public List<Accounts>? customAccounts { get; set; }
        [BsonElement("transactiondetails")]
        public List<Transactions>? transactiondetails { get; set; }
        [BsonElement("cashBalance"), BsonRepresentation(MongoDB.Bson.BsonType.Int32)]
        public int? cashBalance { get; set; }
        [BsonElement("bankBalance"), BsonRepresentation(MongoDB.Bson.BsonType.Int32)]
        public int? bankBalance { get; set; }
    }
}
