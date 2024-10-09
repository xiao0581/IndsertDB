using Microsoft.AspNetCore.Mvc;
using IndsertDB.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ImdbRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
     
        // GET: api/<MoviesController>
        [HttpGet("searchByTittle")]
        public async Task<IActionResult> SearchMovies(string tittle)
        {
            using (var context = new ImdbDbContext())
            {
                var searchParam = new SqlParameter("@searchPattern", $"%{tittle}%");

                var movies = await context.Movies
                    .FromSqlRaw("EXEC SearchMoviesByTitle @searchPattern", tittle)
                    .ToListAsync();

                return Ok(movies);
            }
        }
        [HttpGet("searchByName")]
        public async Task<IActionResult> SearchPerson(string person)
        {
            using (var context = new ImdbDbContext())
            {
                var searchParam = new SqlParameter("@searchPattern", $"%{person}%");

                var persons = await context.Movies
                    .FromSqlRaw("EXEC SearchPeopleWithDetails  @searchPattern", searchParam)
                    .ToListAsync();

                return Ok(persons);
            }
        }
        // GET api/<MoviesController>/5


        // POST api/<MoviesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<MoviesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MoviesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
