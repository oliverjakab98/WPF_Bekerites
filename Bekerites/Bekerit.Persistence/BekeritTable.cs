using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bekerites.Persistance;

namespace Bekerites.Persistance
{
    /// <summary>
    /// Bekerites játéktábla típusa.
    /// </summary>
    public class BekeritTable
    {
        #region Fields

        public Int32 _size; //méret
        public Int32 _gameSteps;
        private Int32[,] _fieldValues; // mezőértékek
        private Boolean[,] _fieldLocks; // zárolt mező


        #endregion

        #region Constructors

        public BekeritTable(Int32 tableSize, Int32 gameSteps)
        {

            _size = tableSize;
            _gameSteps = gameSteps;
            _fieldValues = new Int32 [tableSize, tableSize];
            _fieldLocks = new Boolean[tableSize, tableSize];


        }



        #endregion

        #region Properties

        /// <summary>
        /// Játéktábla kitöltöttségének lekérdezése.
        /// </summary>
        public Boolean IsHalfFilled(Int32 player, Int32 playerLight)
        {
            int count = 0;
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    if (_fieldValues[i, j] == player || _fieldValues[i, j] == playerLight)
                    {
                        count++;
                        if (count >= (_size * _size) / 2)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public Boolean IsFilled()
        {
            foreach (Boolean asd in _fieldLocks)
            {
                if (asd == false)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Játéktábla méretének lekérdezése.
        /// </summary>

        public Int32 Size { get { return _fieldValues.GetLength(0); } }

        /// <summary>
        /// Mező értékének lekérdezése.
        /// </summary>
        /// <param name="x">Vízszintes koordináta.</param>
        /// <param name="y">Függőleges koordináta.</param>
        /// <returns>Mező értéke.</returns>
        public Int32 this[Int32 x, Int32 y] { get { return GetValue(x, y); } }
        #endregion


        #region Public methods

        /// <summary>
        /// Mező kitöltetlenségének lekérdezése.
        /// </summary>
        /// <param name="x">Vízszintes koordináta.</param>
        /// <param name="y">Függőleges koordináta.</param>
        /// <returns>Igaz, ha a mező ki van töltve, egyébként hamis.</returns>
        public Boolean IsEmpty(Int32 x, Int32 y)
        {
            if (x < 0 || x >= _fieldValues.GetLength(0))
                throw new ArgumentOutOfRangeException("x", "The X coordinate is out of range.");
            if (y < 0 || y >= _fieldValues.GetLength(1))
                throw new ArgumentOutOfRangeException("y", "The Y coordinate is out of range.");

            return _fieldValues[x, y] == 0;
        }

        /// <summary>
        /// Mező zároltságának lekérdezése.
        /// </summary>
        /// <param name="x">Vízszintes koordináta.</param>
        /// <param name="y">Függőleges koordináta.</param>
        /// <returns>Igaz, ha a mező zárolva van, különben hamis.</returns>
        public Boolean IsLocked(Int32 x, Int32 y)
        {
            if (x < 0 || x >= _fieldValues.GetLength(0))
                throw new ArgumentOutOfRangeException("x", "The X coordinate is out of range.");
            if (y < 0 || y >= _fieldValues.GetLength(1))
                throw new ArgumentOutOfRangeException("y", "The Y coordinate is out of range.");

            return _fieldLocks[x, y];
        }

        /// <summary>
        /// Mező értékének lekérdezése.
        /// </summary>
        /// <param name="x">Vízszintes koordináta.</param>
        /// <param name="y">Függőleges koordináta.</param>
        /// <returns>A mező értéke.</returns>
        public Int32 GetValue(Int32 x, Int32 y)
        {
            if (x < 0 || x >= _size || y < 0 || y >= _size)
            {
                return 3;
            }
            return _fieldValues[x, y];

        }

        /// <summary>
        /// Mező értékének beállítása.
        /// </summary>
        /// <param name="x">Vízszintes koordináta.</param>
        /// <param name="y">Függőleges koordináta.</param>
        /// <param name="value">Érték.</param>
        /// <param name="lockField">Zárolja-e a mezőt.</param>
        public void SetValue(Int32 x, Int32 y, Int32 player)
        {
            if (x < 0 || x >= _fieldValues.GetLength(0))
                throw new ArgumentOutOfRangeException("x", "The X coordinate is out of range.");
            if (y < 0 || y >= _fieldValues.GetLength(1))
                throw new ArgumentOutOfRangeException("y", "The Y coordinate is out of range.");

            _fieldValues[x, y] = player;
            // _fieldLocks[x, y] = true;
        }

        public void LockValue(Int32 x, Int32 y)
        {
            if (x < 0 || x >= _fieldValues.GetLength(0))
                throw new ArgumentOutOfRangeException("x", "The X coordinate is out of range.");
            if (y < 0 || y >= _fieldValues.GetLength(1))
                throw new ArgumentOutOfRangeException("y", "The Y coordinate is out of range.");


            _fieldLocks[x, y] = true;
        }


        #endregion

        #region Private methods

        

        /// <summary>
        /// Mező léptetése.
        /// </summary>
        /// <param name="x">Vízszintes koordináta.</param>
        /// <param name="y">Függőleges koordináta.</param>
        public void StepValue(Int32 x, Int32 y, Int32 player)
        {
            if (x < 0 || x >= _fieldValues.GetLength(0))
                throw new ArgumentOutOfRangeException("x", "The X coordinate is out of range.");
            if (y < 0 || y >= _fieldValues.GetLength(1))
                throw new ArgumentOutOfRangeException("y", "The Y coordinate is out of range.");


            _fieldValues[x, y] = player;
            _fieldLocks[x, y] = true;
        }


        public void AntiStepValue(Int32 x, Int32 y)
        {
            _fieldValues[x, y] = 0;

        }
        #endregion

    }

}
