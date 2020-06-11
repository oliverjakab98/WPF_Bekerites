using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bekerites.Model;
using Bekerit.Persistence;
using System.Collections.ObjectModel;
using System.Windows;

namespace Bekerites.ViewModel
{
    public class BekeritViewModel : ViewModelBase
    {


        #region Fields
        private BekeritModel _model;
        private SaveEntry _selectedGame;
        private String _newName = String.Empty;
        private Boolean EnableSave;
        #endregion

        #region Properties
        /// <summary>
        /// Új játék kezdése parancs lekérdezése.
        /// </summary>
        public DelegateCommand NewGameSmallCommand { get; private set; }

        public DelegateCommand NewGameMediumCommand { get; private set; }

        public DelegateCommand NewGameBigCommand { get; private set; }

        /// <summary>
        /// Játék betöltés választó parancs lekérdezése.
        /// </summary>
        public DelegateCommand LoadGameOpenCommand { get; private set; }

        /// <summary>
        /// Játék betöltése parancs lekérdezése.
        /// </summary>
        public DelegateCommand LoadGameCloseCommand { get; private set; }

        /// <summary>
        /// Játék mentés választó parancs lekérdezése.
        /// </summary>
        public DelegateCommand SaveGameOpenCommand { get; private set; }

        /// <summary>
        /// Játék mentése parancs lekérdezése.
        /// </summary>
        public DelegateCommand SaveGameCloseCommand { get; private set; }

        /// <summary>
        /// Kilépés parancs lekérdezése.
        /// </summary>
        public DelegateCommand ExitCommand { get; private set; }

        /// <summary>
        /// Játékmező gyűjtemény lekérdezése.
        /// </summary>
        public ObservableCollection<BekeritField> Fields { get; set; }

        /// <summary>
        /// Lépések számának lekérdezése.
        /// </summary>
        public Int32 GameStepCount { get { return _model._gameSteps; } }


        /// <summary>
        /// Lépések számának lekérdezése.
        /// </summary>
        public Int32 BlueScore { get { return _model.BlueScore; } }


        /// <summary>
        /// Lépések számának lekérdezése.
        /// </summary>
        public Int32 RedScore { get { return _model.RedScore; } }

        /// <summary>
        /// Perzisztens játékállapot mentések lekérdezése.
        /// </summary>
        public ObservableCollection<SaveEntry> Games { get; set; }

        /// <summary>
        /// Kiválasztott játékállapot mentés lekérdezése.
        /// </summary>
        public SaveEntry SelectedGame
        {
            get { return _selectedGame; }
            set
            {
                _selectedGame = value;
                if (_selectedGame != null)
                    NewName = String.Copy(_selectedGame.Name);

                OnPropertyChanged();
                SaveGameCloseCommand.RaiseCanExecuteChanged();
                LoadGameCloseCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Új játék mentés nevének lekérdezése.
        /// </summary>
        public String NewName
        {
            get { return _newName; }
            set
            {
                _newName = value;

                OnPropertyChanged();
                SaveGameCloseCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Új játék eseménye.
        /// </summary>
        public event EventHandler NewGameSmall;
        public event EventHandler NewGameMedium;
        public event EventHandler NewGameBig;

        /// <summary>
        /// Játék betöltés választásának eseménye.
        /// </summary>
        public event EventHandler LoadGameOpen;

        /// <summary>
        /// Játék betöltésének eseménye.
        /// </summary>
        public event EventHandler<String> LoadGameClose;

        /// <summary>
        /// Játék mentés választásának eseménye.
        /// </summary>
        public event EventHandler SaveGameOpen;

        /// <summary>
        /// Játék mentésének eseménye.
        /// </summary>
        public event EventHandler<String> SaveGameClose;

        /// <summary>
        /// Játékból való kilépés eseménye.
        /// </summary>
        public event EventHandler ExitGame;
        #endregion

        #region Constructors

        public BekeritViewModel(BekeritModel model) 
        {
            _model = model;
            _model.GameOver += new EventHandler<BekeritEventArgs>(Model_GameOver);
            _model.GameCreated += new EventHandler<BekeritEventArgs>(Model_GameCreated);

            NewGameSmallCommand = new DelegateCommand(param => OnNewGameSmall());
            NewGameMediumCommand = new DelegateCommand(param => OnNewGameMedium());
            NewGameBigCommand = new DelegateCommand(param => OnNewGameBig());

            SaveGameOpenCommand = new DelegateCommand(param => EnableSave, async param=>
            {
                Games = new ObservableCollection<SaveEntry>(await _model.ListGamesAsync());
                OnSaveGameOpen();
            });
            SaveGameCloseCommand = new DelegateCommand(
                param => NewName.Length > 0, // parancs végrehajthatóságának feltétele
                param => { OnSaveGameClose(NewName); });


            LoadGameOpenCommand = new DelegateCommand(async param =>
            {
                Games = new ObservableCollection<SaveEntry>(await _model.ListGamesAsync());
                OnLoadGameOpen();
            });
            LoadGameCloseCommand = new DelegateCommand(
                param => SelectedGame != null, // parancs végrehajthatóságának feltétele
                param => { OnLoadGameClose(SelectedGame.Name); });

            ExitCommand = new DelegateCommand(param => OnExitGame());

            // játéktábla létrehozása
            Fields = new ObservableCollection<BekeritField>();

            for (Int32 i = 0; i < _model._size; i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < _model._size; j++)
                {
                    Fields.Add(new BekeritField
                    {
                        IsLocked = false,
                        Player = Model.Players.NoPlayer,
                        PlayerColor = Model.Players.NoPlayer,
                        Size = 6,
                        X = i,
                        Y = j,
                        Number = i * _model.Table.Size + j, // a gomb sorszáma, amelyet felhasználunk az azonosításhoz
                        StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                        // ha egy mezőre léptek, akkor jelezzük a léptetést, változtatjuk a lépésszámot
                    }) ;
                }
            }

            //FinishTable();
            EnableSave = true;
        }
        #endregion

        #region Private Methods
        public void FinishTable()
        {

            if (Fields.Count != _model._size) 
            {
                Fields.Clear();
                for (Int32 i = 0; i < _model._size; i++) // inicializáljuk a mezőket
                {
                    for (Int32 j = 0; j < _model._size; j++)
                    {
                        Fields.Add(new BekeritField
                        {
                            IsLocked = false,
                            Player = Model.Players.NoPlayer,
                            PlayerColor = Model.Players.NoPlayer,
                            Size = _model._size,
                            X = i,
                            Y = j,
                            Number = i * _model.Table.Size + j, // a gomb sorszáma, amelyet felhasználunk az azonosításhoz
                            StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                            // ha egy mezőre léptek, akkor jelezzük a léptetést, változtatjuk a lépésszámot
                        });
                    }
                }
            }
            OnPropertyChanged("BlueScore");
            OnPropertyChanged("RedScore");

        }

        public void RefreshTable() 
        {
            foreach (BekeritField field in Fields) 
            {
                if (_model.Table.GetValue(field.X, field.Y) == 0) //NoPlayer
                {
                    field.Player = Model.Players.NoPlayer;
                    field.PlayerColor = Model.Players.NoPlayer;
                }
                else if (_model.Table.GetValue(field.X, field.Y) == 1) //PlayerBlue
                {
                    field.Player = Model.Players.PlayerBlue;
                    field.PlayerColor = Model.Players.PlayerBlue;
                    field.IsLocked = true;
                }
                else if (_model.Table.GetValue(field.X, field.Y) == 2) //PlayerRed
                {
                    field.Player = Model.Players.PlayerRed;
                    field.PlayerColor = Model.Players.PlayerRed;
                    field.IsLocked = true;
                }
                else if (_model.Table.GetValue(field.X, field.Y) == 4) //LightRed
                {
                    field.Player = Model.Players.LightRed;
                    field.PlayerColor = Model.Players.LightRed;
                    field.IsLocked = true;
                }
                else if (_model.Table.GetValue(field.X, field.Y) == 5) //LightBlue
                {
                    field.Player = Model.Players.LightBlue;
                    field.PlayerColor = Model.Players.LightBlue;
                    field.IsLocked = true;
                }
            }

            OnPropertyChanged("BlueScore");
            OnPropertyChanged("RedScore");
        }

        private void StepGame(Int32 index) 
        {
            BekeritField field = Fields[index];
            // field.Player = Model.Players.NoPlayer;

            

            if (_model.Player == Model.Players.PlayerBlue && field.IsLocked == false && field.PlayerColor == Model.Players.NoPlayer)
            {
                if (_model.GameSteps == 1)
                {
                    field.Player = Model.Players.PlayerBlue;
                    field.PlayerColor = Model.Players.PlayerBlue;
                    field.IsLocked = true;
                    _model.Step(field.X, field.Y);
                    NotGray();
                    RefreshTable();
                    EnableSave = true;

                }
                else
                {
                    field.PlayerColor = Model.Players.PlayerBlue;
                    field.Player = Model.Players.PlayerBlue;
                    field.IsLocked = true;
                    _model.Step(field.X, field.Y);
                    CheckAvailablePlaces(field.X, field.Y, index);
                    EnableSave = false;

                }
                


            }
            else if(_model.Player == Model.Players.PlayerRed && field.IsLocked == false && field.PlayerColor == Model.Players.NoPlayer)
            {
                if (_model.GameSteps == 1)
                {
                    field.Player = Model.Players.PlayerRed;
                    field.PlayerColor = Model.Players.PlayerRed;
                    field.IsLocked = true;
                    _model.Step(field.X, field.Y);
                    NotGray();
                    RefreshTable();
                    EnableSave = true;
                }
                else
                {
                    field.Player = Model.Players.PlayerRed;
                    field.PlayerColor = Model.Players.PlayerRed;
                    field.IsLocked = true;
                    _model.Step(field.X, field.Y);
                    CheckAvailablePlaces(field.X, field.Y, index);
                    EnableSave = false;
                   
                }

            }
            _model.CheckGameOver();
        }
        #endregion

        private void CheckAvailablePlaces(Int32 x, Int32 y, Int32 index)
        {
            BekeritField field = Fields[index];
            Boolean[] Places;
            Places = new Boolean[4];
            for (int i = 0; i < 4; i++) { Places[i] = false; }

            foreach (BekeritField mezo in Fields) // inicializálni kell a mezőket is
            {
                if (mezo.IsLocked == false) { mezo.PlayerColor = Model.Players.Gray; }
            }

            
            if (field.X + 1 < _model._size && _model.Table.IsLocked(x + 1, y) == false) 
            {
                BekeritField tmp = Fields[(field.X + 1) * _model._size + field.Y];
                tmp.PlayerColor = Model.Players.NoPlayer;
                Places[0] = true;
            }
            if (field.X - 1 >= 0 && _model.Table.IsLocked(x - 1, y) == false)
            {
                BekeritField tmp = Fields[(field.X - 1) * _model._size + field.Y];
                tmp.PlayerColor = Model.Players.NoPlayer;
                Places[1] = true;
            }
            if (field.Y + 1 < _model._size && _model.Table.IsLocked(x, y + 1) == false)
            {
                BekeritField tmp = Fields[field.X * _model._size + (field.Y + 1)];
                tmp.PlayerColor = Model.Players.NoPlayer;
                Places[2] = true;
            }
            if (field.Y - 1 >= 0 && _model.Table.IsLocked(x, y - 1) == false)
            {
                BekeritField tmp = Fields[field.X * _model._size + (field.Y - 1)];
                tmp.PlayerColor = Model.Players.NoPlayer;
                Places[3] = true;
            }
            if (!Places.Any(z => z)) 
            {
                field.Player = Model.Players.NoPlayer;
                field.PlayerColor = Model.Players.Gray;
                field.IsLocked = true;
                _model.Table.SetValue(field.X, field.Y, 6);
                if (_model.Player == Model.Players.PlayerBlue) { _model.GameStepsDown(); }
                if (_model.Player == Model.Players.PlayerRed) { _model.GameStepsUp(); }
                MessageBox.Show("Ide nem tudsz rakni!");
                _model.PlayerPointsLower(_model.Player);
                NotGray();
            }
            

        }

        private void NotGray() 
        {

            foreach (BekeritField mezo in Fields) // inicializálni kell a mezőket is
            {
                if (mezo.IsLocked == false) { mezo.PlayerColor = Model.Players.NoPlayer; }
            }
        }

        #region Game event handlers

        private void Model_GameCreated(object sender, BekeritEventArgs e)
        {
            FinishTable();
        }

        /// <summary>
        /// Játék végének eseménykezelője.
        /// </summary>
        private void Model_GameOver(object sender, BekeritEventArgs e)
        {
            foreach (BekeritField field in Fields)
            {
                field.IsLocked = true; // minden mezőt lezárunk
            }
        }

        #endregion

        #region Event Methods

        /// <summary>
        /// Új játék indításának eseménykiváltása.
        /// </summary>
        private void OnNewGameSmall()
        {
            if (NewGameSmall != null)
                NewGameSmall(this, EventArgs.Empty);
        }

        private void OnNewGameMedium()
        {
            if (NewGameMedium != null)
                NewGameMedium(this, EventArgs.Empty);
        }

        private void OnNewGameBig()
        {
            if (NewGameBig != null)
                NewGameBig(this, EventArgs.Empty);
        }
        /// <summary>
        /// Játék betöltés választásának eseménykiváltása.
        /// </summary>
        private void OnLoadGameOpen()
        {
            if (LoadGameOpen != null)
                LoadGameOpen(this, EventArgs.Empty);
        }

        /// <summary>
        /// Játék betöltésének eseménykiváltása.
        /// </summary>
        private void OnLoadGameClose(String name)
        {
            if (LoadGameClose != null)
                LoadGameClose(this, name);
        }

        /// <summary>
        /// Játék mentés választásának eseménykiváltása.
        /// </summary>
        private void OnSaveGameOpen()
        {
            if (SaveGameOpen != null)
                SaveGameOpen(this, EventArgs.Empty);
        }

        /// <summary>
        /// Játék mentésének eseménykiváltása.
        /// </summary>
        private void OnSaveGameClose(String name)
        {
            if (SaveGameClose != null)
                SaveGameClose(this, name);
        }

        /// <summary>
        /// Játékból való kilépés eseménykiváltása.
        /// </summary>
        private void OnExitGame()
        {
            if (ExitGame != null)
                ExitGame(this, EventArgs.Empty);
        }
        #endregion
    }
}
