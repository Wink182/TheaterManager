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

namespace TheaterManager.Web.Controllers
{
    public class TheatersController : Controller
    {
        private TheaterDb db = new TheaterDb();

        // GET: Theaters
        public async Task<ActionResult> Index()
        {
            return View(await db.Theaters.ToListAsync());
        }

        // GET: Theaters/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Theater theater = await db.Theaters.FindAsync(id);
            if (theater == null)
            {
                return HttpNotFound();
            }

            ViewBag.Movies = theater.TheaterMovies
                                .Where(tm => tm.Showtime >= DateTime.Now)
                                .GroupBy(tm => tm.MovieID,
                                         tm => tm,
                                         (key, showtimes) => new Movie
                                         {
                                             MovieID = key,
                                             Name = showtimes.First().Movie.Name,
                                             TheaterMovies = showtimes.ToList()
                                         })
                                .ToList();
            return View(theater);
        }

        // GET: Theaters/Create
        public ActionResult Create()
        {
            var model = new TheaterViewModel();
            model.Action = "Create";
            return View(model);
        }

        // POST: Theaters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TheaterID,Name,Address,Latitude,Longitude")] TheaterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var theater = new Theater();
                theater.Name= viewModel.Name;
                theater.Address = viewModel.Address;
                theater.Latitude = viewModel.Latitude;
                theater.Longitude = viewModel.Longitude;

                db.Theaters.Add(theater);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        // GET: Theaters/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Theater theater = await db.Theaters.FindAsync(id);
            if (theater == null)
            {
                return HttpNotFound();
            }

            TheaterViewModel viewModel = new TheaterViewModel();

            viewModel.Action = "Update";
            viewModel.TheaterID = theater.TheaterID;
            viewModel.Name = theater.Name;
            viewModel.Address = theater.Address;
            viewModel.Latitude = theater.Latitude;
            viewModel.Longitude = theater.Longitude;

            return View("Create", viewModel);
        }

        // POST: Theaters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TheaterID,Name,Address,Latitude,Longitude")] TheaterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var theater = new Theater();
                theater.TheaterID = viewModel.TheaterID;
                theater.Name = viewModel.Name;
                theater.Address = viewModel.Address;
                theater.Latitude = viewModel.Latitude;
                theater.Longitude = viewModel.Longitude;

                db.Entry(theater).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View("Create", viewModel);
        }

        // GET: Theaters/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Theater theater = await db.Theaters.FindAsync(id);
            if (theater == null)
            {
                return HttpNotFound();
            }
            return View(theater);
        }

        // POST: Theaters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Theater theater = await db.Theaters.FindAsync(id);
            db.Theaters.Remove(theater);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
