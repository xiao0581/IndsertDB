using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndsertDB.Model
{
   
   
    public class People
    {
  
        public string Nconst { get; set; }  // 主键

        public string PrimaryName { get; set; }
        public int? BirthYear { get; set; }
        public int? DeathYear { get; set; }

        // 关系映射
        public ICollection<PersonProfession> PersonProfessions { get; set; }
        public ICollection<KnownFor> KnownForTitles { get; set; }
        public ICollection<MovieDirector> MovieDirectors { get; set; }
        public ICollection<MovieWriter> MovieWriters { get; set; }
    }


}
