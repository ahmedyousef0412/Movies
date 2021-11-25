using Movies.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Models.VM
{
    public class MovieVM
    {



        public int Id { get; set; }


        [Required]
        [StringLength(255)]
        public string Title { get; set; }


        [Required]
        public int Year { get; set; }

        [Required]
        [Range(1,10)]
        public double Rate { get; set; }

        [Required]
        [StringLength(2550)]
        public string StoryLine { get; set; }

        
        [Display(Name ="Select Poster....")]
        public byte[] Poster { get; set; }

        [Display(Name ="Genre")]
        public byte GenreId { get; set; }

        public IEnumerable<Genre> Genres { get; set; }


    }
}
