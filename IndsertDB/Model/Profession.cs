using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndsertDB.Model
{
    [Table("Professions")]
    public class Profession
    {
        [Key]  // 主键
        public int ProfessionId { get; set; }

        public string ProfessionName { get; set; }

        // 关系映射
        public ICollection<PersonProfession> PersonProfessions { get; set; }
    }
}
