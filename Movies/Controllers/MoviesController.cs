using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Models.DataBase;
using Movies.Models.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext context;

        public MoviesController(ApplicationDbContext _context)
        {
            context = _context;
        }
        public async Task<IActionResult> Index()
        {

            var movie = await context.Movies.ToListAsync();
            return View(movie);
        }


        public async Task<IActionResult> Create()
        {
            var movieViewModel = new MovieVM
            {
                Genres = await context.Genres.OrderBy(m =>m.GenreName).ToListAsync()
            };
            return View(movieViewModel);

        }



    }
}
