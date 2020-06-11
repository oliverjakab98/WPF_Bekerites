using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;


namespace Bekerites.Persistence.Db
{
    /// <summary>
    /// Adatbázis kontextus típusa.
    /// </summary>
    /// <seealso cref="System.Data.Entity.DbContext" />
    class BekeritContext : DbContext
    {
        public BekeritContext(String connection)
            : base(connection)
        {
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Field> Fields { get; set; }
    }
}
