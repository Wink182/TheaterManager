using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheaterManager.Domain
{
    public class Theater
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TheaterID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public virtual ICollection<TheaterMovie> TheaterMovies { get; set; }

        [NotMapped]
        public double? Distance { get; set; }
    }
}
