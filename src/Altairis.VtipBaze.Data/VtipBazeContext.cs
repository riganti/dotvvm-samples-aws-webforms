using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altairis.VtipBaze.Data
{
    public class VtipBazeContext : DbContext
    {

        public DbSet<Joke> Jokes { get; set; }

        public DbSet<Tag> Tags { get; set; }

    }
}
