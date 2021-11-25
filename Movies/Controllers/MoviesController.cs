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
        private readonly List<string> _allowedExtenstions = new() { ".jpg", ".png" };
        private readonly long _maxAllowPosterSize = 1048576;
        private readonly IMapper map;

        public MoviesController(ApplicationDbContext _context, IMapper map)
        {
            context = _context;
            this.map = map;
        }
        public async Task<IActionResult> Index()
        {
            
            var movie = await context.Movies.Include(g =>g.Genre).ToListAsync();
            return View(movie);
        }

        //Get
        public async Task<IActionResult> Create()
        {
            var movieViewModel = new MovieVM
            {
                Genres = await context.Genres.OrderBy(m =>m.GenreName).ToListAsync()
            };
            return View( movieViewModel);

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



            // To select all Files in The Form Like [Imgs, Files]
            var files = Request.Form.Files;
            // [Any] check if exist file or not  [Any => One or More] work like[OR]
            if (!files.Any())
            {
                model.Genres = await context.Genres.OrderBy(o => o.GenreName).ToListAsync();
                ModelState.AddModelError("Poster", "Please Select Movie Poster.");
                return View(model);
            }




            var poster = files.FirstOrDefault();

          

            //Check The Extension of the Files that the end user select it.
            if (!_allowedExtenstions.Contains(Path.GetExtension(poster.FileName).ToLower()))
            {
                model.Genres = await context.Genres.OrderBy(o => o.GenreName).ToListAsync();
                ModelState.AddModelError("Poster", "Only Accept JPG and PNG.");
                return View(model);
            }


            //Check The Size of the Files that the end user select it.
            if (poster.Length > _maxAllowPosterSize)
            {
                model.Genres = await context.Genres.OrderBy(o => o.GenreName).ToListAsync();
                ModelState.AddModelError("Poster", "The Poster can't be More than 1 MB.");
                return View(model);
            }

            // I Use MemoryStream because The [Poster] is a Array of Byte
            using var dateStream = new MemoryStream();


            // Save The Poster
            await poster.CopyToAsync(dateStream);

            //var movie = map.Map<Movie>(model);{Using Of Auto Mapper}

            var movie = new Movie
            {
                Title = model.Title,
                Year = model.Year,
                Rate = model.Rate,
                GenreId = model.GenreId,
                Poster = dateStream.ToArray(),
                StoryLine = model.StoryLine
            };

            context.Movies.Add(movie);

            context.SaveChanges();


            return RedirectToAction(nameof(Index));
        }


        // Get by Id
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
                return BadRequest();

            var movie =await context.Movies.FindAsync(id);

            if (movie == null)
                return NotFound();


            // Here i can use AutoMapper
            var viewModel = new MovieVM
            {
                Id = movie.Id,
                Title = movie.Title,
                Rate = movie.Rate,
                Year = movie.Year,
                GenreId = movie.GenreId,
                Poster = movie.Poster,
                StoryLine = movie.StoryLine,
                Genres = await context.Genres.OrderBy(g => g.GenreName).ToListAsync()

            };


            //I use "Create" to Use The Create view with [Edit] because Create and Edit are two similar
            return View("Create", viewModel);

            
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MovieVM model)
        {


            if(!ModelState.IsValid)
            {

                 //I use Genres because I Need when open form Edit or Create the dropdown has values of Genres
                model.Genres =await context.Genres.OrderBy(g => g.GenreName).ToListAsync();
                return View(model);
            }

            // check if Id exist or Not
            var movie = await context.Movies.FindAsync(model.Id);

            if (movie == null)
                return NotFound();

            var files = Request.Form.Files;

            if (files.Any())
            {

                var poster = files.FirstOrDefault();

                var dataStream = new MemoryStream();

                await poster.CopyToAsync(dataStream);


                model.Poster = dataStream.ToArray();

                //Check The Extension of the Files that the end user select it.
                if (!_allowedExtenstions.Contains(Path.GetExtension(poster.FileName).ToLower()))
                {
                    model.Genres = await context.Genres.OrderBy(o => o.GenreName).ToListAsync();
                    ModelState.AddModelError("Poster", "Only Accept JPG and PNG.");
                    return View(model);
                }


                //Check The Size of the Files that the end user select it.
                if (poster.Length > _maxAllowPosterSize)
                {
                    model.Genres = await context.Genres.OrderBy(o => o.GenreName).ToListAsync();
                    ModelState.AddModelError("Poster", "The Poster can't be More than 1 MB.");
                    return View(model);
                }


                movie.Poster = model.Poster;

            }

            // here I can Use Auto Mapper.

            movie.Title = model.Title;
            movie.Rate = model.Rate;
            movie.Year = model.Year;
            movie.StoryLine = model.StoryLine;
            movie.GenreId = model.GenreId;



            

            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


    }
}
