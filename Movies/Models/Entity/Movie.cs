using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Models.Entity
{
    public class Movie
    {

        public int Id { get; set; }


        [Required] 
        [MaxLength(255)]
        public string  Title { get; set; }


        [Required]
        public int Year { get; set; }

        [Required]
        public double Rate { get; set; }

        [Required]
        [MaxLength(2550)]
        public string StoryLine { get; set; }

        [Required]
        public byte[] Poster { get; set; }


        public byte GenreId { get; set; }

        public Genre Genre { get; set; }


    }
}
