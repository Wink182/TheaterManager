using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheaterManager.Domain;

namespace TheaterManager.Web.Models
{
    public class ShowtimeViewModel
    {
        public string Action { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [HiddenInput(DisplayValue = false)]
        public bool NewMovie { get; set; }
        
        public Movie Movie { get; set; }
        
        public int MovieID { get; set; }

        [Required]
        [HiddenInput(DisplayValue = false)]
        public int TheaterID { get; set; }

        [Required]
        [DataType("datetime-local")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Showtime { get; set; }
    }
}