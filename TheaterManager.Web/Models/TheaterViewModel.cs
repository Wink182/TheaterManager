using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TheaterManager.Web.Models
{
    public class TheaterViewModel
    {
        public string Action { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int TheaterID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [HiddenInput(DisplayValue = false)]
        public double Latitude { get; set; }

        [HiddenInput(DisplayValue = false)]
        public double Longitude { get; set; }
    }
}