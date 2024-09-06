using MongoDB.Bson.Serialization.Attributes;

namespace MoneyMgtMongo.Models
{
    public class Accounts
    {
        [BsonId]
        [BsonElement("id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? id { get; set; }
        [BsonElement("AccountName"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string? AccountName { get; set; }
        [BsonElement("TransactionType"), BsonRepresentation(MongoDB.Bson.BsonType.Int32)]
        public int TransactionType { get; set; }
    }
}
