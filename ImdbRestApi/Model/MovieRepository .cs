using ImdbRestApi.Dto;
using IndsertDB.Model;
using Microsoft.EntityFrameworkCore;

using Microsoft.Data.SqlClient;


namespace ImdbRestApi.Model
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ImdbDbContext _context;

        public MovieRepository(ImdbDbContext context)
        {
            _context = context;
        }

        public async Task<MovieDto?> GetByIdAsync(string tconst)
        {
          
            var movie = await _context.GetByIdView 
                .Where(m => m.Tconst == tconst)
                .Select(m => new MovieDto
                {
                    Tconst = m.Tconst,
                    TitleType = m.TitleType,
                    PrimaryTitle = m.PrimaryTitle,
                    OriginalTitle = m.OriginalTitle,
                    IsAdult = m.IsAdult,
                    StartYear = m.StartYear,
                    EndYear = m.EndYear,
                    RuntimeMinutes = m.RuntimeMinutes,
                    Genres = m.Genres
                })
                .FirstOrDefaultAsync();

            return movie;
        }

       
        public async Task<List<MovieDto>> SearchMoviesByTitle(string title)
        {
            var searchParam = new SqlParameter("@searchPattern", $"%{title}%");

            using (var connection = _context.Database.GetDbConnection())
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
                                Genres = reader["Genres"].ToString()
                            };

                            movies.Add(movie);
                        }

                        return movies;
                    }
                }
            }
        }

        public async Task<List<PersonDetailsDto>> SearchPersonByName(string name)
        {
            var searchParam = new SqlParameter("@searchPattern", $"%{name}%");

            using (var connection = _context.Database.GetDbConnection())
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SearchPeopleByName";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(searchParam);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var results = new List<PersonDetailsDto>();

                        while (await reader.ReadAsync())
                        {
                            var dto = new PersonDetailsDto
                            {
                                Nconst = reader["nconst"].ToString(),
                                PrimaryName = reader["primaryName"].ToString(),
                                BirthYear = reader["birthYear"] != DBNull.Value ? (int?)reader["birthYear"] : null,
                                DeathYear = reader["deathYear"] != DBNull.Value ? (int?)reader["deathYear"] : null,
                                Professions = reader["Professions"].ToString(),
                                KnownFor = reader["knownFor"].ToString()
                            };

                            results.Add(dto);
                        }

                        return results;
                    }
                }
            }
        }

        public async Task AddMovie(MovieDto movieDto)
        {
            var parameters = new[]
            {
            new SqlParameter("@titleType", movieDto.TitleType),
            new SqlParameter("@primaryTitle", movieDto.PrimaryTitle),
            new SqlParameter("@originalTitle", movieDto.OriginalTitle),
            new SqlParameter("@isAdult", movieDto.IsAdult),
            new SqlParameter("@startYear", movieDto.StartYear),
            new SqlParameter("@endYear", movieDto.EndYear.HasValue ? (object)movieDto.EndYear.Value : DBNull.Value),
            new SqlParameter("@runtimeMinutes", movieDto.RuntimeMinutes.HasValue ? (object)movieDto.RuntimeMinutes.Value : DBNull.Value),
            new SqlParameter("@genres", movieDto.Genres)
        };

            await _context.Database.ExecuteSqlRawAsync("EXEC AddeMovie @titleType, @primaryTitle, @originalTitle, @isAdult, @startYear, @endYear, @runtimeMinutes, @genres", parameters);
        }

        public async Task AddPerson(PersonDetailsDto personDto)
        {
            var parameters = new[]
            {
            new SqlParameter("@primaryName", personDto.PrimaryName),
            new SqlParameter("@birthYear", personDto.BirthYear ?? (object)DBNull.Value),
            new SqlParameter("@deathYear", personDto.DeathYear ?? (object)DBNull.Value),
            new SqlParameter("@primaryProfession", personDto.Professions),
            new SqlParameter("@knownForTitles", personDto.KnownFor)
        };

            await _context.Database.ExecuteSqlRawAsync("EXEC AddPeople @primaryName, @birthYear, @deathYear, @primaryProfession, @knownForTitles", parameters);
        }

        public async Task UpdateMovie(string tconst, MovieDto movieUpdateDto)
        {
            if (string.IsNullOrEmpty(tconst))
            {
                throw new ArgumentException("tconst cannot be null or empty", nameof(tconst));
            }

         
            var movieToUpdate = await GetByIdAsync(tconst);

            if (movieToUpdate == null)
            {
                throw new Exception($"Movie with tconst {tconst} not found.");
            }

        
            var parameters = new[]
            {
        new SqlParameter("@tconst", tconst),
        new SqlParameter("@titleType", movieUpdateDto.TitleType ?? (object)DBNull.Value),
        new SqlParameter("@primaryTitle", movieUpdateDto.PrimaryTitle ?? (object)DBNull.Value),
        new SqlParameter("@originalTitle", movieUpdateDto.OriginalTitle ?? (object)DBNull.Value),
        new SqlParameter("@isAdult", movieUpdateDto.IsAdult),
        new SqlParameter("@startYear", movieUpdateDto.StartYear ?? (object)DBNull.Value),
        new SqlParameter("@endYear", movieUpdateDto.EndYear ?? (object)DBNull.Value),
        new SqlParameter("@runtimeMinutes", movieUpdateDto.RuntimeMinutes ?? (object)DBNull.Value),
        new SqlParameter("@genres", movieUpdateDto.Genres ?? (object)DBNull.Value)
    };

         
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC UpdateMovieWithGenres @tconst, @titleType, @primaryTitle, @originalTitle, @isAdult, @startYear, @endYear, @runtimeMinutes, @genres",
                parameters);
        }



        public async Task DeleteMovieAsync(string tconst)
        {
            if (string.IsNullOrEmpty(tconst))
            {
                throw new ArgumentException("tconst cannot be null or empty", nameof(tconst));
            }

            try
            {
             
                var tconstParam = new SqlParameter("@tconst", tconst);

       
                await _context.Database.ExecuteSqlRawAsync("EXEC DeleteMovie @tconst", tconstParam);
            }
            catch (Exception ex)
            {
            
                throw new Exception($"Error deleting movie with tconst: {tconst}", ex);
            }
        }


    }

}
