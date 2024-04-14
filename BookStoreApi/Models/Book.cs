using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookStoreApi.Models
{
    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("name")]
        public string BookName { get; set; } = null!;
        [BsonElement("price")]
        public decimal Price { get; set; }
        [BsonElement("category")]
        public string Category { get; set; } = null!;
        [BsonElement("autor")]
        public string Author { get; set; } = null!;

    }
}
