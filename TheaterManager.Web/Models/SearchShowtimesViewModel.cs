using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheaterManager.Domain;

namespace TheaterManager.Web.Models
{
    public class SearchShowtimesViewModel
    {
        public IEnumerable<TheaterMovie> Showtimes { get; set; }

        public string SearchString { get; set; }
        public DateTime? SearchTime { get; set; }
        public string SearchSortType { get; set; }
        public string LocationType { get; set; }
        public string Address { get; set; }

        [HiddenInput(DisplayValue = false)]
        public double? Latitude { get; set; }

        [HiddenInput(DisplayValue = false)]
        public double? Longitude { get; set; }
    }
}