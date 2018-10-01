using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheaterManager.Domain
{
    public interface ITheaterDataSource
    {
        IQueryable<Theater> Theaters { get; }
        IQueryable<Movie> Movies { get; }
        void Save();
    }
}
