using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndsertDB.Model
{
    [Table("MovieWriters")]
    public class MovieWriter
    {
        [ForeignKey("Movies")]
        public string Tconst { get; set; }  // 外键

        [ForeignKey("People")]
        public string Nconst { get; set; }  // 外键

        public virtual Movie Movies { get; set; }
        public virtual People People { get; set; }
    }

}
