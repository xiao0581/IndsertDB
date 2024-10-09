using Microsoft.AspNetCore.Mvc;
using IndsertDB.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using ImdbRestApi.Dto;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ImdbRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {

        [HttpGet("searchByTittle")]
        public async Task<IActionResult> SearchMovies(string tittle)
        {
            using (var context = new ImdbDbContext())
            {
                var searchParam = new SqlParameter("@searchPattern", $"%{tittle}%");

                using (var connection = context.Database.GetDbConnection())
                {
                    await connection.OpenAsync();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SearchMoviesByTitle";
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add(searchParam);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var movies = new List<MovieDto>();

                            while (await reader.ReadAsync())
                            {
                                var movie = new MovieDto
                                {
                                    Tconst = reader["tconst"].ToString(),
                                    TitleType = reader["titleType"].ToString(),
                                    PrimaryTitle = reader["primaryTitle"].ToString(),
                                    OriginalTitle = reader["originalTitle"].ToString(),
                                    IsAdult = Convert.ToBoolean(reader["isAdult"]),
                                    StartYear = reader["startYear"] != DBNull.Value ? (int?)reader["startYear"] : null,
                                    EndYear = reader["endYear"] != DBNull.Value ? (int?)reader["endYear"] : null,
                                    RuntimeMinutes = reader["runtimeMinutes"] != DBNull.Value ? (int?)reader["runtimeMinutes"] : null,
                                    GenreName = reader["GenreName"].ToString()
                                };

                                movies.Add(movie);
                            }

                            return Ok(movies);
                        }
                    }
                }
            }
        }
        [HttpGet("searchByName")]
        public async Task<IActionResult> SearchPerson(string person)
        {
            using (var context = new ImdbDbContext())
            {
                var searchParam = new SqlParameter("@namePattern", $"%{person}%");

                // 手动创建连接并执行存储过程
                using (var connection = context.Database.GetDbConnection())
                {
                    await connection.OpenAsync();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "GetPersonDetails";
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add(searchParam);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var results = new List<PersonDetailsDto>();

                            // 手动将结果映射到 DTO
                            while (await reader.ReadAsync())
                            {
                                var dto = new PersonDetailsDto
                                {
                                    Nconst = reader["nconst"].ToString(),
                                    PrimaryName = reader["primaryName"].ToString(),
                                    BirthYear = reader["birthYear"] != DBNull.Value ? (int?)reader["birthYear"] : null,
                                    DeathYear = reader["deathYear"] != DBNull.Value ? (int?)reader["deathYear"] : null,
                                    PersonProfessions = reader["personProfessions"].ToString(),
                                    KnownForTitles = reader["knownForTitles"].ToString()
                                };

                                results.Add(dto);
                            }

                            return Ok(results);
                        }
                    }
                }
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
