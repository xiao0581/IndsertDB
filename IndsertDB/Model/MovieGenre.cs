﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndsertDB.Model
{
   
    public class MovieGenre
    {
        
        public string Tconst { get; set; }  // 外键

      
        public int GenreId { get; set; }  // 外键

        public virtual Movie Movies { get; set; }
        public virtual Genre Genres { get; set; }
    }
}
