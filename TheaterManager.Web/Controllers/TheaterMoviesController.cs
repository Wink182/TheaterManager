using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TheaterManager.Domain;
using TheaterManager.Web.Infrastructure;
using TheaterManager.Web.Models;
using System.Device.Location;

namespace TheaterManager.Web.Controllers
{
    public class TheaterMoviesController : Controller
    {
        private TheaterDb db = new TheaterDb();

        // GET: TheaterMovies
        public ActionResult Index(SearchShowtimesViewModel viewModel)
        {
            
            viewModel.Showtimes = db.TheaterMovies.Include(t => t.Movie).Include(t => t.Theater);

            if (viewModel.Latitude.HasValue && viewModel.Longitude.HasValue)
            {
                var location = new GeoCoordinate((double)viewModel.Latitude, (double)viewModel.Longitude);
                foreach (TheaterMovie tm in viewModel.Showtimes)
                {
                    var theaterLocation = new GeoCoordinate(tm.Theater.Latitude, tm.Theater.Longitude);
                    tm.Theater.Distance = Math.Round(location.GetDistanceTo(theaterLocation) * 0.000621371, 1); //convert from meters to miles
                }
            }

            viewModel.Showtimes = viewModel.Showtimes.Where(tm => tm.Showtime >= (viewModel.SearchTime.HasValue ? viewModel.SearchTime : DateTime.Now));

            if (!string.IsNullOrEmpty(viewModel.SearchString))
            {
                viewModel.Showtimes = viewModel.Showtimes.Where(tm => tm.Movie.Name.IndexOf(viewModel.SearchString, StringComparison.OrdinalIgnoreCase) != -1 || tm.Movie.Genre.IndexOf(viewModel.SearchString, StringComparison.OrdinalIgnoreCase) != -1);
            }

            switch (viewModel.SearchSortType)
            {
                case "showtime":
                    viewModel.Showtimes = viewModel.Showtimes.OrderBy(tm => tm.Showtime);
                    break;
                case "location":
                    viewModel.Showtimes = viewModel.Showtimes.OrderBy(tm => tm.Theater.Distance);
                    break;
                default:
                    viewModel.Showtimes = viewModel.Showtimes.OrderBy(tm => tm.Showtime);
                    break;
            }

            return View(viewModel);
        }

        // GET: TheaterMovies/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TheaterMovie theaterMovie = await db.TheaterMovies.FindAsync(id);
            if (theaterMovie == null)
            {
                return HttpNotFound();
            }
            return View(theaterMovie);
        }

        // GET: TheaterMovies/Create
        public ActionResult Create(int TheaterID)
        {
            ViewBag.MovieID = new SelectList(db.Movies, "MovieID", "Name");

            var model = new ShowtimeViewModel();
            model.Action = "Create";
            model.TheaterID = TheaterID;
            model.Showtime = DateTime.Now;
            model.NewMovie = false;
            return View(model);
        }

        // POST: TheaterMovies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MovieID,Movie,NewMovie,TheaterID,Showtime")] ShowtimeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.NewMovie)
                {
                    db.Movies.Add(viewModel.Movie);
                    await db.SaveChangesAsync();
                }

                var theaterMovie = new TheaterMovie();
                theaterMovie.TheaterID = viewModel.TheaterID;
                theaterMovie.MovieID = viewModel.NewMovie ? viewModel.Movie.MovieID : viewModel.MovieID;
                theaterMovie.Showtime = viewModel.Showtime;

                db.TheaterMovies.Add(theaterMovie);
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Theaters", new { id = theaterMovie.TheaterID });
            }

            ViewBag.MovieID = new SelectList(db.Movies, "MovieID", "Name", viewModel.MovieID);
            return View(viewModel);
        }

        // GET: TheaterMovies/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TheaterMovie theaterMovie = await db.TheaterMovies.FindAsync(id);
            if (theaterMovie == null)
            {
                return HttpNotFound();
            }
            ViewBag.MovieID = new SelectList(db.Movies, "MovieID", "Name", theaterMovie.MovieID);

            ShowtimeViewModel viewModel = new ShowtimeViewModel();
            viewModel.Action = "Update";
            viewModel.Id = theaterMovie.Id;
            viewModel.MovieID = theaterMovie.MovieID;
            viewModel.TheaterID = theaterMovie.TheaterID;
            viewModel.Showtime = theaterMovie.Showtime;

            return View("Create", viewModel);
        }

        // POST: TheaterMovies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,MovieID,Movie,NewMovie,TheaterID,Showtime")] ShowtimeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.NewMovie)
                {
                    db.Movies.Add(viewModel.Movie);
                    await db.SaveChangesAsync();
                }

                var theaterMovie = new TheaterMovie();
                theaterMovie.Id = viewModel.Id;
                theaterMovie.MovieID = viewModel.NewMovie ? viewModel.Movie.MovieID : viewModel.MovieID;
                theaterMovie.TheaterID = viewModel.TheaterID;
                theaterMovie.Showtime = viewModel.Showtime;

                db.Entry(theaterMovie).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Theaters", new { id = theaterMovie.TheaterID });
            }
            ViewBag.MovieID = new SelectList(db.Movies, "MovieID", "Name", viewModel.MovieID);
            return View("Create", viewModel);
        }

        // GET: TheaterMovies/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TheaterMovie theaterMovie = await db.TheaterMovies.FindAsync(id);
            if (theaterMovie == null)
            {
                return HttpNotFound();
            }
            return View(theaterMovie);
        }

        // POST: TheaterMovies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TheaterMovie theaterMovie = await db.TheaterMovies.FindAsync(id);
            db.TheaterMovies.Remove(theaterMovie);
            await db.SaveChangesAsync();
            return RedirectToAction("Details", "Theaters", new { id = theaterMovie.TheaterID });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
