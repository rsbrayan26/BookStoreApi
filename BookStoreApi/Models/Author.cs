using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BookStoreApi.Models
{
    public class Author
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("name")]
        public string nameAuthor { get; set; } = null!;
        [BsonElement("identification_number")]
        public string idNumber { get; set; } = null!;

        [BsonElement("age")]
        public decimal age { get; set; }
    }
}
