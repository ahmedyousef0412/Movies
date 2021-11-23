using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Models.Entity
{
    public class Genre
    {


        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }


        [Required]
        [StringLength(255)]
        [Display (Name = = "Genre")]
        public string GenreName { get; set; }
    }
}
