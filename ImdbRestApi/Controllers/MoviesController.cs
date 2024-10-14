using Microsoft.AspNetCore.Mvc;
using IndsertDB.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using ImdbRestApi.Dto;
using System.Data.Entity;
using ImdbRestApi.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ImdbRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;

        public MoviesController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        [HttpGet("{tconst}")]
        public async Task<IActionResult> GetMovieById(string tconst)
        {
            var movie = await _movieRepository.GetByIdAsync(tconst);
            if (movie == null)
            {
                return NotFound(new { Message = "Movie not found." });
            }

            return Ok(movie);
        }

        [HttpGet("searchByTittle")]
        public async Task<IActionResult> SearchMovies(string title)
        {
            var movies = await _movieRepository.SearchMoviesByTitle(title);
            return Ok(movies);
        }

        [HttpGet("searchByName")]
        public async Task<IActionResult> SearchPerson(string name)
        {
            var people = await _movieRepository.SearchPersonByName(name);
            return Ok(people);
        }

        [HttpPost("addMovie")]
        public async Task<IActionResult> AddMovie([FromBody] MovieDto movieDto)
        {
            await _movieRepository.AddMovie(movieDto);
            return Ok(new { Message = "Movie added successfully" });
        }

        [HttpPost("addPerson")]
        public async Task<IActionResult> AddPerson([FromBody] PersonDetailsDto personDto)
        {
            await _movieRepository.AddPerson(personDto);
            return Ok(new { Message = "Person added successfully" });
        }

        [HttpPut("updateMovie/{tconst}")]
        public async Task<IActionResult> UpdateMovie(string tconst, [FromBody] MovieDto movieUpdateDto)
        {
            if (movieUpdateDto == null)
            {
                return BadRequest("Movie data is missing.");
            }

            try
            {
                await _movieRepository.UpdateMovie(tconst, movieUpdateDto);
                return Ok(new { Message = "Movie updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error occurred while updating the movie.", Exception = ex.Message });
            }
        }

        [HttpDelete("deleteMovie/{tconst}")]
        public async Task<IActionResult> DeleteMovie(string tconst)
        {
            if (string.IsNullOrEmpty(tconst))
            {
                return BadRequest("Movie ID is missing.");
            }

            try
            {
                // 调用 MovieRepository 中的方法
                await _movieRepository.DeleteMovieAsync(tconst);

                return Ok(new { Message = "Movie deleted successfully!" });
            }
            catch (Exception ex)
            {
                // 处理异常并返回错误信息
                return StatusCode(500, new { Message = "Error occurred while deleting the movie.", Exception = ex.Message });
            }
        }

    }


}




