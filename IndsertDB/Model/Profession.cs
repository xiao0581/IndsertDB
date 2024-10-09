using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndsertDB.Model
{
   
    public class Profession
    {
       // 主键
        public int ProfessionId { get; set; }

        public string ProfessionName { get; set; }

        // 关系映射
        public ICollection<PersonProfession> PersonProfessions { get; set; }
    }
}
