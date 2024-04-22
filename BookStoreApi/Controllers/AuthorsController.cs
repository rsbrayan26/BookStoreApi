using System.Linq;
using BookStoreApi.Models;
using BookStoreApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IMongoDbContext _mongoDbContext;

        public AuthorsController(IMongoDbContext mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }
        // GET: api/<AuthorsController>
        [HttpGet]
        public async Task<IActionResult> Get(int page=1,string? search=null)
        {
            var authors =_mongoDbContext.Authors.Find(x=>true).ToListAsync();
            List <Author> res = authors.Result;
            if (search!=null)
            {
                res = res.FindAll(x=> x.nameAuthor.Contains(search) || x.idNumber.Contains(search));
                Console.WriteLine(search);
            }
            //var parge = HttpContext.Request.Headers["parge"];
            //page = page[0].;
            int itemsPerPage = 5;
            int total = res.Count();
            var total_pages = Math.Ceiling(Convert.ToDecimal(total) / Convert.ToDecimal(itemsPerPage));
            int startData = Convert.ToInt32((page - 1) * itemsPerPage);
            int endData = (startData + itemsPerPage < total) ? itemsPerPage : (total-(page-1) * itemsPerPage);
            var data = res.GetRange(startData, endData);
            return Ok(new {
                page, 
                itemsPerPage,
                total,total_pages,
                data
            });
        }

        //GET api/<AuthorsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetId(string id)
        {
            try
            {
                var author = await _mongoDbContext.Authors.Find(x => x.Id == id).FirstOrDefaultAsync();
                if (author == null) return Ok(new { Message= "No se encontró el autor" });
                return Ok(new { author });
        
            }
            catch (Exception ex) {
                return BadRequest(new { ex.Message });
            }
            
        }

        // POST api/<AuthorsController>
        [HttpPost]
        public async Task<IActionResult> Post(Author author)
        {
            try {
                var existsAuthor= await _mongoDbContext.Authors.Find(x=>x.idNumber==author.idNumber).FirstOrDefaultAsync();
                if (existsAuthor==null)
                {
                    await _mongoDbContext.Authors.InsertOneAsync(author);
                    return Ok(new { message= "Autor registrado exitosamente", author});
                }
                else
                {
                    throw new Exception($"El autor con el número de identificación {author.idNumber} ya está registrado");
                }
            }
            catch(Exception ex) 
            {
                return BadRequest(new { ex.Message});
            }
        }

        // PUT api/<AuthorsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, Author author)
        {
            try
            {
                var existsAuthor = await _mongoDbContext.Authors.Find(x => x.Id == id).FirstOrDefaultAsync();
                if (existsAuthor == null) throw new Exception("No existe el ID del autor");

                existsAuthor = await _mongoDbContext.Authors.Find(x => x.idNumber == author.idNumber&&x.Id!=id).FirstOrDefaultAsync();
                if (existsAuthor != null) throw new Exception($"Ya existe un autor con el número de identificación {author.idNumber}");

                await _mongoDbContext.Authors.ReplaceOneAsync(x=>x.Id==author.Id,author);
                return Ok(new { message = "Se actualizo la información del Autor", author });
            }
            catch(Exception e) {
                return BadRequest(new { e.Message });
            }
        }

        // DELETE api/<AuthorsController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id){}
    }
}
