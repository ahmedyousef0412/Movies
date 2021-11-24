using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Models.VM
{
    public class GenreVM
    {
     


        [Required]
        [StringLength(255)]
        [Display(Name ="Genre")]
        public string GenreName { get; set; }
    }
}
