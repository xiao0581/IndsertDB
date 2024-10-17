using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace IndsertDB.Model
{
    


    public class Movie
    {

        public string Tconst { get; set; }  

        public string TitleType { get; set; }
        public string PrimaryTitle { get; set; }
        public string OriginalTitle { get; set; }
        public bool? IsAdult { get; set; }
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
        public int? RuntimeMinutes { get; set; }

       
        public ICollection<MovieGenre> MovieGenres { get; set; }
        public ICollection<KnownFor> KnownForTitles { get; set; }
        public ICollection<MovieDirector> MovieDirectors { get; set; }
        public ICollection<MovieWriter> MovieWriters { get; set; }
    }


}
