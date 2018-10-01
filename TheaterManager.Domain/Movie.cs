using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheaterManager.Domain
{
    public class Movie
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MovieID { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public float? Rating { get; set; }
        public virtual ICollection<TheaterMovie> TheaterMovies { get; set; }
    }
}
