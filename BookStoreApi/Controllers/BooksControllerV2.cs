using BookStoreApi.Models;
using BookStoreApi.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksControllerV2 : ControllerBase
    {
        private readonly IMongoDbContext _mongoDbContext;

        public BooksControllerV2(IMongoDbContext mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }
        // GET: api/<BooksControllerV2>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var books= await _mongoDbContext.Books.Find(x=>true).ToListAsync();
            return Ok(books);
        }

        // GET api/<BooksControllerV2>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var book = await _mongoDbContext.Books.Find(x => x.Id==id).FirstOrDefaultAsync();
            if (book==null)
            {
                return NotFound($"el libro con el id:{id} no existe");
            }
            return Ok(book);
        }

        // POST api/<BooksControllerV2>
        [HttpPost]
        public async Task<IActionResult> Post(Book newBook)
        {
            await _mongoDbContext.Books.InsertOneAsync(newBook);
            return CreatedAtAction(nameof(Get),new {id=newBook.Id,newBook});
        }

        // PUT api/<BooksControllerV2>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, Book updatedBook)
        {
            var book = await _mongoDbContext.Books.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (book == null)
            {
                return NotFound($"el libro con el id:{id} no existe");
            }
            updatedBook.Id = book.Id;
            await _mongoDbContext.Books.ReplaceOneAsync(x => x.Id == id, updatedBook);
            return Ok();
        }

        // DELETE api/<BooksControllerV2>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id){}
    }
}
