using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TheaterManager.Domain
{
    public class TheaterMovie
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int MovieID { get; set; }
        public int TheaterID { get; set; }

        public virtual Movie Movie { get; set; }
        public virtual Theater Theater { get; set; }

        public DateTime Showtime { get; set; }
    }
}
