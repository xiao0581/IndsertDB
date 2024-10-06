using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
namespace IndsertDB.Model
{


    [Table("MovieDirectors")]
    public class MovieDirector
    {
        [ForeignKey("Movies")]
        public string Tconst { get; set; }  // 外键

        [ForeignKey("People")]
        public string Nconst { get; set; }  // 外键

        public virtual Movie Movies { get; set; }
        public virtual People People { get; set; }
    }

}
