using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bekerites.Persistence.Db
{
    class Game
    {
        /// <summary>
        /// Név, egyedi azonosító.
        /// </summary>
        [Key]
        [MaxLength(32)]
        public String Name { get; set; }

        /// <summary>
        /// Tábla mérete.
        /// </summary>
        public Int32 TableSize { get; set; }

        /// <summary>
        /// Lépésszám.
        /// </summary>
        public Int32 GameSteps { get; set; }

        /// <summary>
        /// Mentés időpontja.
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Játékmezők.
        /// </summary>
        public ICollection<Field> Fields { get; set; }

        public Game()
        {
            Fields = new List<Field>();
            Time = DateTime.Now;
        }
    }
}
