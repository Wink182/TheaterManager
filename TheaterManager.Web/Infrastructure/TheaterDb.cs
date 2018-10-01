using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TheaterManager.Domain;

namespace TheaterManager.Web.Infrastructure
{
    public class TheaterDb : DbContext, ITheaterDataSource
    {
        public TheaterDb() : base("DefaultConnection")
        {

        }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<Theater> Theaters { get; set; }

        void ITheaterDataSource.Save()
        {
            SaveChanges();
        }

        IQueryable<Movie> ITheaterDataSource.Movies
        {
            get { return Movies; }
        }

        IQueryable<Theater> ITheaterDataSource.Theaters
        {
            get { return Theaters; }
        }

        public System.Data.Entity.DbSet<TheaterManager.Domain.TheaterMovie> TheaterMovies { get; set; }
    }
}