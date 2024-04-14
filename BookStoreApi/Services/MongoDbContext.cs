using BookStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BookStoreApi.Services
{
    public interface IMongoDbContext
    {
        IMongoCollection<Author> Authors { get; }
        IMongoCollection<Book> Books { get; }
    }
    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase _database;
        public MongoDbContext(IConfiguration configuration, IOptions<BookStoreDatabaseSettings> bookStoreDatabaseSettings)
        {
            //var connectionString = bookStoreDatabaseSettings.Value.ConnectionString;
            //var client = new MongoClient(connectionString);
            //_database = client.GetDatabase("TuBaseDeDatos");
            var mongoClient = new MongoClient(bookStoreDatabaseSettings.Value.ConnectionString);
            _database = mongoClient.GetDatabase(bookStoreDatabaseSettings.Value.DatabaseName);
        }
        public IMongoCollection<Author> Authors => _database.GetCollection<Author>("autors");
        public IMongoCollection<Book> Books => _database.GetCollection<Book>("Books");
    }
}
