using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndsertDB.Model
{
   
    public class PersonProfession
    {
        [Column("nconst")]
        public string Nconst { get; set; }  

        [Column("professionId")]
        public int ProfessionId { get; set; }  

        public virtual People People { get; set; }
        public virtual Profession Profession { get; set; }
    }
}
