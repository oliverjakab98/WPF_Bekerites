using Bekerites.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bekerit.Persistence
{
    public interface BekeritDataAccess
    {
        /// <summary>
        /// Játékállapot betöltése.
        /// </summary>
        /// <param name="name">Név vagy elérési útvonal.</param>
        /// <returns>A beolvasott játéktábla.</returns>
        Task<BekeritTable> LoadAsync(String name);

        /// <summary>
        /// Játékállapot mentése.
        /// </summary>
        /// <param name="name">Név vagy elérési útvonal.</param>
        /// <param name="table">A kiírandó játéktábla.</param>
        Task SaveAsync(String name, BekeritTable table);

        /// <summary>
        /// Játékállapot mentések lekérdezése.
        /// </summary>
	    Task<ICollection<SaveEntry>> ListAsync();
    }
}
