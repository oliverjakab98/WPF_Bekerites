using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bekerit.Persistence
{
    public class SaveEntry
    {
        /// <summary>
        /// Mentés neve vagy elérési útja.
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// Mentés időpontja.
        /// </summary>
        public DateTime Time { get; set; }
    }
}
