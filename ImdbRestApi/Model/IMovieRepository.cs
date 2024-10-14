using ImdbRestApi.Dto;

namespace ImdbRestApi.Model
{
    public interface IMovieRepository
    {
        Task AddMovie(MovieDto movieDto);
        Task AddPerson(PersonDetailsDto personDto);
        Task DeleteMovieAsync(string tconst);
        Task<MovieDto?> GetByIdAsync(string tconst);
        Task<List<MovieDto>> SearchMoviesByTitle(string title);
        Task<List<PersonDetailsDto>> SearchPersonByName(string name);
        Task UpdateMovie(string tconst, MovieDto movieUpdateDto);
    }
}