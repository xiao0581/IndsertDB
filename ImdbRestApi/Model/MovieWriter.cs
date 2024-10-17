using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndsertDB.Model
{
   
    public class MovieWriter
    {
        [Column("tconst")]
        public string Tconst { get; set; } 

        [Column("nconst")]
        public string Nconst { get; set; }  

        public virtual Movie Movie { get; set; }
        public virtual People People { get; set; }
    }

}
