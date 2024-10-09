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
        public string Tconst { get; set; }  // 外键

        [Column("nconst")]
        public string Nconst { get; set; }  // 外键

        public virtual Movie Movie { get; set; }
        public virtual People People { get; set; }
    }

}
