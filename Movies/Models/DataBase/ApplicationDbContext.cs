using Microsoft.EntityFrameworkCore;
using Movies.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Models.DataBase
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options ): base(options)
        {

        }


        public DbSet<Movie> Movies { get; set; }

        public DbSet<Genre> Genres { get; set; }

    }
}
