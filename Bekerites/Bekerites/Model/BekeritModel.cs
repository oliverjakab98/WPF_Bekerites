using Bekerit.Persistence;
using Bekerites.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bekerites.Model
{
    /// <summary>
    /// Játékosok felsorolási típusa.
    /// </summary>
    public enum Players { NoPlayer, PlayerBlue, PlayerRed, Wall, LightRed, LightBlue, Gray }
    /// <summary>
    /// Sudoku játék típusa.
    /// </summary>
    public class BekeritModel
    {
        #region Fields

        private BekeritDataAccess _dataAccess; // adatelérés
        private BekeritTable _table; // játéktábla
        //pontok
        private Int32 _playerBlueScore;
        private Int32 _playerRedScore;
        //mostani játékos
        private Players _currentPlayer;
        private Players _currentPlayerLight;
        //kövi játékos
        private Players _nextPlayer;
        private Players _nextPlayerLight;
        //rekúrzióhoz szükséges mátrix
        private Boolean[,] _ellenorzott;
        public Int32 _gameSteps;

        public Int32 _size;


        #endregion

        #region Properties

        /// <summary>
        /// Méret lekérdezése
        /// </summary>
        public Int32 GetSize { get { return _size; } }

        /// <summary>
        /// Lépések számának lekérdezése.
        /// </summary>
        public Int32 GameSteps { get { return _table._gameSteps; } }
        public void GameStepsDown()
        {
            _table._gameSteps--;
        }
        public void GameStepsUp()
        {
            _table._gameSteps++;
        }

        /// <summary>
        /// Kék játékos pontjának lekérdezése.
        /// </summary>
        public Int32 BlueScore { get { return _playerBlueScore; } }

        /// <summary>
        /// Piros játékos pontjának lekérdezése.
        /// </summary>
        public Int32 RedScore { get { return _playerRedScore; } }

        /// <summary>
        /// Játékos lekérdezése.
        /// </summary>
        public Players Player { get { return _currentPlayer; } }

        /// <summary>
        /// Játékos pontjának lekérdezése.
        /// </summary>
        public Int32 PlayerPoints
        {
            get
            {
                if (_currentPlayer == Players.PlayerBlue)
                {
                    return _playerBlueScore;
                }
                else
                {
                    return _playerRedScore;
                }
            }
        }

        /// <summary>
        /// Játékos pontjának csökkentése.
        /// </summary>
        public void PlayerPointsLower(Players player)
        {
            if (player == Players.PlayerBlue)
            {
                _playerBlueScore--;
            }
            if (player == Players.PlayerRed)
            {
                _playerRedScore--;
            }
            OnGameAdvanced();
        }


        public Boolean CheckGameOver()
        {
            if (_playerBlueScore >= (_size * _size) / 2 || _playerRedScore >= (_size * _size) / 2)
            {
                OnGameOver(true);
                return true;

            }

            if (_table.IsFilled())
            {
                OnGameOver(true);
                return true;

            }

            if (_table._gameSteps == 0 || _table._gameSteps == 2)
            {
                if (_table.IsHalfFilled( (Int32)_currentPlayer, (Int32)_currentPlayerLight))
                {
                    OnGameOver(true);
                    return true;

                }
            }
            return false;
        }
        /// <summary>
        /// Játéktábla lekérdezése.
        /// </summary>
        public BekeritTable Table { get { return _table; } }

        /// <summary>
        /// Játék végének lekérdezése.
        /// </summary>
        public Boolean IsGameOver { get { return CheckGameOver(); } }

        #endregion

        #region Events
        /// <summary>
        /// Játék előrehaladásának eseménye.
        /// </summary>
        public event EventHandler<BekeritEventArgs> GameAdvanced;

        /// <summary>
        /// Játék végének eseménye.
        /// </summary>
        public event EventHandler<BekeritEventArgs> GameOver;

        /// <summary>
        /// Játék létrehozásának eseménye.
        /// </summary>
        public event EventHandler<BekeritEventArgs> GameCreated;
        #endregion

        #region Constructor
        /// <summary>
        /// Sudoku játék példányosítása.
        /// </summary>
        /// <param name="dataAccess">Az adatelérés.</param>
        public BekeritModel(BekeritDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
            _size = 6;
            _table = new BekeritTable(_size,0);
            _currentPlayer = Players.PlayerBlue;
            _currentPlayerLight = Players.LightBlue;
            _nextPlayer = Players.PlayerRed;
            _nextPlayerLight = Players.LightRed;
            _playerRedScore = 0;
            _playerBlueScore = 0;
            OnGameCreated();
        }

        #endregion

        #region Private methods
        public void OnGameAdvanced()
        {
            if (GameAdvanced != null)
                GameAdvanced(this, new BekeritEventArgs(false, _playerBlueScore, _playerRedScore));
        }

        private void OnGameCreated()
        {
            if (GameCreated != null)
                GameCreated(this, new BekeritEventArgs(false, _playerBlueScore, _playerRedScore));
        }

        /// <summary>
        /// Játék vége eseményének kiváltása.
        /// </summary>
        /// <param name="isWon">Győztünk-e a játékban.</param>
        private void OnGameOver(Boolean isWon)
        {
            if (GameOver != null)
                GameOver(this, new BekeritEventArgs(isWon, _playerBlueScore, _playerRedScore));
        }



        #endregion

        #region Public game methods

        /// <summary>
        /// Új játék kezdése.
        /// </summary>
        public void NewGame()
        {
            _table = new BekeritTable(_size,0);
            _table._size = _size;
            _currentPlayer = Players.PlayerBlue;
            _currentPlayerLight = Players.LightBlue;
            _nextPlayer = Players.PlayerRed;
            _nextPlayerLight = Players.LightRed;
            _playerBlueScore = 0;
            _playerRedScore = 0;
            OnGameCreated();

        }

        /// <summary>
        /// Táblabeli lépés végrehajtása.
        /// </summary>
        /// <param name="x">Vízszintes koordináta.</param>
        /// <param name="y">Függőleges koordináta.</param>
        public void Step(Int32 x, Int32 y)
        {




            if (IsGameOver) // ha már vége a játéknak, nem játszhatunk
                return;

            _table.StepValue(x, y, (Int32)_currentPlayer);


            if (_currentPlayer == Players.PlayerBlue)
            {
                _playerBlueScore++;
                _nextPlayer = Players.PlayerRed;
                _nextPlayerLight = Players.LightRed;
            }
            else
            {
                _playerRedScore++;
                _nextPlayer = Players.PlayerBlue;
                _nextPlayerLight = Players.LightBlue;
            }
            OnGameAdvanced();




            if (_currentPlayer == Players.PlayerBlue)
            {

                _table._gameSteps++;
                if (_table._gameSteps == 2)
                {
                    for (int i = 0; i < _size; i++)
                    {
                        for (int j = 0; j < _size; j++)
                        {
                            _ellenorzott = new Boolean[_size, _size];
                            for (int t = 0; t < _size; t++)
                            {
                                for (int d = 0; d < _size; d++)
                                {
                                    _ellenorzott[t, d] = false;
                                }
                            }
                            if (checkGameEnemy(i, j) == true)
                            {
                                _table.SetValue(i, j, (Int32)_currentPlayerLight);
                            }

                        }
                    }
                    for (int i = 0; i < _size; i++)
                    {
                        for (int j = 0; j < _size; j++)
                        {
                            if (_table.GetValue(i, j) == (Int32)_currentPlayerLight)
                            {
                                _table.LockValue(i, j);
                            }
                        }
                    }



                    _currentPlayer = Players.PlayerRed;
                    _currentPlayerLight = Players.LightRed;
                }
            }
            else
            {

                _table._gameSteps--;
                if (_table._gameSteps == 0)
                {

                    for (int i = 0; i < _size; i++)
                    {
                        for (int j = 0; j < _size; j++)
                        {
                            _ellenorzott = new Boolean[_size, _size];
                            for (int t = 0; t < _size; t++)
                            {
                                for (int d = 0; d < _size; d++)
                                {
                                    _ellenorzott[t, d] = false;
                                }
                            }
                            if (checkGameEnemy(i, j) == true)
                            {
                                _table.SetValue(i, j, (Int32)_currentPlayerLight);
                            }
                        }
                    }
                    for (int i = 0; i < _size; i++)
                    {
                        for (int j = 0; j < _size; j++)
                        {

                            if (_table.GetValue(i, j) == (Int32)_currentPlayerLight)
                            {
                                _table.LockValue(i, j);
                            }
                        }
                    }


                    _currentPlayer = Players.PlayerBlue;
                    _currentPlayerLight = Players.LightBlue;
                }

            }


            _playerBlueScore = 0;
            _playerRedScore = 0;
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    if (Table.GetValue(i, j) == (Int32)Players.PlayerBlue || Table.GetValue(i, j) == (Int32)Players.LightBlue)
                    {
                        _playerBlueScore++;

                    }
                    if (Table.GetValue(i, j) == (Int32)Players.PlayerRed || Table.GetValue(i, j) == (Int32)Players.LightRed)
                    {
                        _playerRedScore++;

                    }
                    OnGameAdvanced();
                }

            }



        }



        public Boolean checkGameEnemy(Int32 x, Int32 y)
        {
            if (_ellenorzott[x, y] == true) { return true; }

            //oldalak
            if (Table.GetValue(x, y) == (Int32)Players.Wall)
                return false;
            if (Table.GetValue(x + 1, y) == (Int32)Players.Wall)
                return false;
            if (Table.GetValue(x - 1, y) == (Int32)Players.Wall)
                return false;
            if (Table.GetValue(x, y + 1) == (Int32)Players.Wall)
                return false;
            if (Table.GetValue(x, y - 1) == (Int32)Players.Wall)
                return false;


            //átlók
            /*
            if (Table.GetValue(x - 1, y - 1) == Players.Wall)
                return false;
            if (Table.GetValue(x + 1, y + 1) == Players.Wall)
                return false;
            if (Table.GetValue(x + 1, y - 1) == Players.Wall)
                return false;
            if (Table.GetValue(x - 1, y + 1) == Players.Wall)
                return false;
             */
            _ellenorzott[x, y] = true;

            Boolean result = true;
            // Players player = Table.GetValue(x, y);
            // Table.SetValue(x, y, Players.DeadPlayer);
            if (Table.GetValue(x + 1, y) == (Int32)Players.NoPlayer || Table.GetValue(x + 1, y) == (Int32)_nextPlayer || Table.GetValue(x + 1, y) == (Int32)_nextPlayerLight)
                result = result && checkGameEnemy(x + 1, y);
            if (Table.GetValue(x - 1, y) == (Int32)Players.NoPlayer || Table.GetValue(x - 1, y) == (Int32)_nextPlayer || Table.GetValue(x - 1, y) == (Int32)_nextPlayerLight)
                result = result && checkGameEnemy(x - 1, y);
            if (Table.GetValue(x, y + 1) == (Int32)Players.NoPlayer || Table.GetValue(x, y + 1) == (Int32)_nextPlayer || Table.GetValue(x, y + 1) == (Int32)_nextPlayerLight)
                result = result && checkGameEnemy(x, y + 1);
            if (Table.GetValue(x, y - 1) == (Int32)Players.NoPlayer || Table.GetValue(x, y - 1) == (Int32)_nextPlayer || Table.GetValue(x, y - 1) == (Int32)_nextPlayerLight)
                result = result && checkGameEnemy(x, y - 1);
            //átlók
            /*
            if (Table.GetValue(x - 1, y - 1) == Players.NoPlayer || Table.GetValue(x - 1, y - 1) == _nextPlayer)
                result = result && checkGameEnemy(x - 1, y - 1);
            if (Table.GetValue(x + 1, y + 1) == Players.NoPlayer || Table.GetValue(x + 1, y + 1) == _nextPlayer)
                result = result && checkGameEnemy(x + 1, y + 1);
            if (Table.GetValue(x + 1, y - 1) == Players.NoPlayer || Table.GetValue(x + 1, y - 1) == _nextPlayer)
                result = result && checkGameEnemy(x + 1, y - 1);
            if (Table.GetValue(x - 1 , y + 1) == Players.NoPlayer || Table.GetValue(x - 1, y + 1) == _nextPlayer)
                result = result && checkGameEnemy(x - 1, y + 1);
             */

            //Table.SetValue(x, y, player);

            return result;
        }

        #endregion
        /// <summary>
        /// Játék betöltése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        public async Task LoadGameAsync(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");


            _table = await _dataAccess.LoadAsync(path);
            _size = _table._size;
            if (_table._gameSteps == 0)
            {
                _currentPlayer = Players.PlayerBlue;
                _currentPlayerLight = Players.LightBlue;
            }
            else if (_table._gameSteps == 2)
            {
                _currentPlayer = Players.PlayerRed;
                _currentPlayerLight = Players.LightRed;
            }
            _playerRedScore = 0;
            _playerBlueScore = 0;
        }

        /// <summary>
        /// Játék mentése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        public async Task SaveGameAsync(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            await _dataAccess.SaveAsync(path, _table);

        }

        /// <summary>
        /// Játék mentések lekérése.
        /// </summary>
        public async Task<ICollection<SaveEntry>> ListGamesAsync()
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            return await _dataAccess.ListAsync();
        }




    }
}
