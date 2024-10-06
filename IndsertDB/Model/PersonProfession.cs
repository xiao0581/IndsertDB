using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndsertDB.Model
{
    [Table("PersonProfessions")]
    public class PersonProfession
    {
        [ForeignKey("People")]
        public string Nconst { get; set; }  // 外键

        [ForeignKey("Professions")]
        public int ProfessionId { get; set; }  // 外键

        public virtual People People { get; set; }
        public virtual Profession Professions { get; set; }
    }
}
