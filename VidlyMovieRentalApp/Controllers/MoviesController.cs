﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Web.Mvc;
using VidlyMovieRentalApp.Models;
using VidlyMovieRentalApp.ViewModels;

namespace VidlyMovieRentalApp.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Movies/Index
        public ActionResult Index()
        {
          //  var movies = _context.Movies.Include(m => m.Genre).ToList();
            if (User.IsInRole("CanManageMovies"))
            {
                return View("List");
            }

            return View("ReadOnlyList");

        }
        [Authorize(Roles = RoleName.CanManageMovies)]
        public ActionResult New()
        {
            var genreList = _context.Genres.ToList();

            var viewModel = new MovieFormViewModel
            {
                
                Genres = genreList
            };

            return View("MovieForm", viewModel);
        }

        [Authorize(Roles = RoleName.CanManageMovies)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Save(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                var genreList = _context.Genres.ToList();

                var viewModel = new MovieFormViewModel(movie)
                {
                    Genres = genreList
                };
                return View("MovieForm", viewModel);
            }
            if (movie.Id == 0)
            {
                movie.DateAdded = DateTime.Now;
                _context.Movies.Add(movie);
            }
            else
            {
                var moviesInDb = _context.Movies.Single(m => m.Id == movie.Id);
                moviesInDb.Name = movie.Name;
                moviesInDb.ReleasedDate = movie.ReleasedDate;
                moviesInDb.GenreId = movie.GenreId;
                moviesInDb.NumberInStock = movie.NumberInStock;
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Movies");
        }

        [Authorize(Roles = RoleName.CanManageMovies)]
        public ActionResult Edit(int id)
        {

            var movies = _context.Movies.SingleOrDefault(m => m.Id == id);
            if (movies == null)
            {
                return HttpNotFound();
            }

            var viewModel = new MovieFormViewModel(movies)
            {
                Genres = _context.Genres
            };
            return View("MovieForm", viewModel);
        }
    }
}

