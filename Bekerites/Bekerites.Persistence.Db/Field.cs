using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bekerites.Persistence.Db
{/// <summary>
 /// Mező entitás típusa.
 /// </summary>
    class Field
    {
        /// <summary>
        /// Egyedi azonosító.
        /// </summary>
        [Key]
        public Int32 Id { get; set; }

        /// <summary>
        /// Vízszintes koordináta.
        /// </summary>
        public Int32 X { get; set; }
        /// <summary>
        /// Függőleges koordináta.
        /// </summary>
        public Int32 Y { get; set; }
        /// <summary>
        /// Tárolt érték.
        /// </summary>
        public Int32 Value { get; set; }
        /// <summary>
        /// Zárolt tulajdonság lekérdezése.
        /// </summary>
        public Boolean IsLocked { get; set; }

        /// <summary>
        /// Kapcsolt játék.
        /// </summary>
        public Game Game { get; set; }
    }
}
