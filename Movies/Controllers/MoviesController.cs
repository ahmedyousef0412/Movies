using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Models.DataBase;
using Movies.Models.Entity;
using Movies.Models.VM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext context;
        //private new List<string> _allowedExtenstions = new() { ".jpg", ".png" };
        private readonly IMapper map;

        public MoviesController(ApplicationDbContext _context, IMapper map)
        {
            context = _context;
            this.map = map;
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


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(MovieVM model)
        {

            if (!ModelState.IsValid)
            {

                model.Genres = await context.Genres.OrderBy(o => o.GenreName).ToListAsync();
                return View(model);
            }




            var files = Request.Form.Files;
            if (!files.Any())
            {
                model.Genres = await context.Genres.OrderBy(o => o.GenreName).ToListAsync();
                ModelState.AddModelError("Poster", "Please Select Movie Poster.");
                return View(model);
            }


            var poster = files.FirstOrDefault();
            var extensionAllowed = new List<string>  {".jpg",".png"};

            if (!extensionAllowed.Contains(Path.GetExtension(poster.FileName).ToLower()))
            {
                model.Genres = await context.Genres.OrderBy(o => o.GenreName).ToListAsync();
                ModelState.AddModelError("Poster", "Only Accept JPG and PNG.");
                return View(model);
            }

            if (poster.Length > 1048576)
            {
                model.Genres = await context.Genres.OrderBy(o => o.GenreName).ToListAsync();
                ModelState.AddModelError("Poster", "The Poster can't be More than 1 MB.");
                return View(model);
            }


            using var dateStream = new MemoryStream();

            await poster.CopyToAsync(dateStream);

            var movie = map.Map<Movie>(model);

           

            context.Movies.Add(movie);
            context.SaveChanges();


            return RedirectToAction(nameof(Index));
        }


    }
}
