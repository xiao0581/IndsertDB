using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndsertDB.Model
{
   
    public class MovieGenre
    {
        [Column("tconst")]
        public string Tconst { get; set; }  

        [Column("genreId")]
        public int GenreId { get; set; }  

        public virtual Movie Movie { get; set; }
        public virtual Genre Genre { get; set; }
    }
}
