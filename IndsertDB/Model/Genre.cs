﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndsertDB.Model
{
    [Table("Genres")]
    public class Genre
    {
        [Key]  // 主键
        public int GenreId { get; set; }

        public string GenreName { get; set; }

        public ICollection<MovieGenre> MovieGenres { get; set; }
    }
}
