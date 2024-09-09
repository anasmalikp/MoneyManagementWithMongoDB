using MongoDB.Bson.Serialization.Attributes;

namespace MoneyMgtMongo.Models
{
    public class Transactions
    {
        [BsonId]
        [BsonElement("id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? id { get; set; }
        [BsonElement("accountId"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string? AccountId { get; set; }
        [BsonElement("transactionTime"), BsonRepresentation(MongoDB.Bson.BsonType.DateTime)]
        public DateTime? transactionTime { get; set; }
        [BsonElement("amount"), BsonRepresentation(MongoDB.Bson.BsonType.Int32)]
        public int? amount { get; set; }
        [BsonElement("transactionType"), BsonRepresentation(MongoDB.Bson.BsonType.Int32)]
        public int? transactionType { get; set; }
    }
}
