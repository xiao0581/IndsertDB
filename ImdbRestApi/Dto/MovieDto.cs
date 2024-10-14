namespace ImdbRestApi.Dto
{
    public class MovieDto
    {
       
            public string? Tconst { get; set; }
            public string? TitleType { get; set; }
            public string? PrimaryTitle { get; set; }
            public string? OriginalTitle { get; set; }
            public bool? IsAdult { get; set; }
            public int? StartYear { get; set; }
            public int? EndYear { get; set; }
            public int? RuntimeMinutes { get; set; }

            // 这里映射查询的 genreName
            public string? Genres { get; set; }
        
    }
}
