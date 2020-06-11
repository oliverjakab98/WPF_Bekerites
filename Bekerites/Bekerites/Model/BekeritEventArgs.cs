using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bekerites.Model
{
    /// <summary>
    /// Bekerites eseményargumentum típusa.
    /// </summary>
    public class BekeritEventArgs : EventArgs
    {
        private Int32 _playerBlueScore;
        private Int32 _playerRedScore;
        private Boolean _isWon;


        /// <summary>
        /// Győzelem lekérdezése.
        /// </summary>
        public Boolean IsWon { get { return _isWon; } }

        /// <summary>
        /// Piros játékos pontjainak lekérdezése.
        /// </summary>
        public Int32 RedScore { get { return _playerRedScore; } }

        /// <summary>
        /// Kék játékos pontjainak lekérdezése.
        /// </summary>
        public Int32 BlueScore { get { return _playerBlueScore; } }


        public BekeritEventArgs(Boolean isWon, Int32 playerBlueScore, Int32 playerRedScore)
        {
            _isWon = isWon;
            _playerBlueScore = playerBlueScore;
            _playerRedScore = playerRedScore;
        }
    }
}
