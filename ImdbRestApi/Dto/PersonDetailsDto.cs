namespace ImdbRestApi.Dto
{
    public class PersonDetailsDto
    {
        public string? Nconst { get; set; }        // 人物的唯一标识符
        public string? PrimaryName { get; set; }   // 人物的名字
        public int? BirthYear { get; set; }       // 出生年份
        public int? DeathYear { get; set; }       // 去世年份
        public string? Professions { get; set; }  // 职业信息
        public string? KnownFor{ get; set; }
    }
}
