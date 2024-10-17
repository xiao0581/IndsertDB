using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndsertDB.Model
{
   
    public class Profession
    {
      
        public int ProfessionId { get; set; }

        public string ProfessionName { get; set; }

       
        public ICollection<PersonProfession> PersonProfessions { get; set; }
    }
}
