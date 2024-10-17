using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
namespace IndsertDB.Model
{


  
    public class MovieDirector
    {
        [Column("tconst")]
        public string Tconst { get; set; }  

        [Column("nconst")]
        public string Nconst { get; set; } 

        public virtual Movie Movie { get; set; }
        public virtual People People { get; set; }
    }

}
