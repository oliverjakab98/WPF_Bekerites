using Bekerit.Persistence;
using Bekerites.Model;
using Bekerites.Persistence.File;
using Bekerites.Persistence.Db;
using Bekerites.View;
using Bekerites.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Bekerites
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Fields

        private BekeritModel _model;
        private BekeritViewModel _viewModel;
        private MainWindow _view;
        private LoadWindow _loadWindow;
        private SaveWindow _saveWindow;

        #endregion

        #region Constructors

        /// <summary>
        /// Alkalmazás példányosítása.
        /// </summary>
        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        #endregion

        #region Application event handlers
        private void App_Startup(object sender, StartupEventArgs e)
        {
            BekeritDataAccess dataAccess;
            //dataAccess = new BekeritFileDataAccess(AppDomain.CurrentDomain.BaseDirectory); // fájl alapú mentés
            dataAccess = new BekeritDbDataAccess("name=BekeritModel"); // adatbázis alapú mentés

            // modell létrehozása
            _model = new BekeritModel(dataAccess);
            _model.GameOver += new EventHandler<BekeritEventArgs>(Model_GameOver);
            _model.NewGame();

            // nézemodell létrehozása
            _viewModel = new BekeritViewModel(_model);
            
             _viewModel.NewGameSmall += new EventHandler(ViewModel_NewGameSmall);
             _viewModel.NewGameMedium += new EventHandler(ViewModel_NewGameMedium);
             _viewModel.NewGameBig += new EventHandler(ViewModel_NewGameBig);
             _viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);
             _viewModel.LoadGameOpen += new EventHandler(ViewModel_LoadGameOpen);
             _viewModel.LoadGameClose += new EventHandler<String>(ViewModel_LoadGameClose);
             _viewModel.SaveGameOpen += new EventHandler(ViewModel_SaveGameOpen);
             _viewModel.SaveGameClose += new EventHandler<String>(ViewModel_SaveGameClose);


            // nézet létrehozása
            _view = new MainWindow();
            _view.DataContext = _viewModel;
            _view.Closing += new System.ComponentModel.CancelEventHandler(View_Closing); // eseménykezelés a bezáráshoz
            _view.Show();
            
        }

        #endregion

        #region View event handlers

        private void View_Closing(object sender, CancelEventArgs e)
        {
            

            if (MessageBox.Show("Biztos, hogy ki akar lépni?", "Bekerites", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true; // töröljük a bezárást
            }
        }
        #endregion

        #region Viewmodel event handlers

        private void ViewModel_NewGameSmall(object sender, EventArgs e)
        {
            _model._size = 6;
            _model.NewGame();
        }

        private void ViewModel_NewGameMedium(object sender, EventArgs e)
        {
            _model._size = 8;
            _model.NewGame();
        }

        private void ViewModel_NewGameBig(object sender, EventArgs e)
        {
            _model._size = 10;
            _model.NewGame();
        }

        /// <summary>
        /// Játék betöltés választó eseménykezelője.
        /// </summary>
        private void ViewModel_LoadGameOpen(object sender, System.EventArgs e)
        {
            

            _viewModel.SelectedGame = null; // kezdetben nincsen kiválasztott elem

            _loadWindow = new LoadWindow(); // létrehozzuk a játék állapot betöltő ablakot
            _loadWindow.DataContext = _viewModel;
            _loadWindow.ShowDialog(); // megjelenítjük dialógusként

        }

        /// <summary>
        /// Játék betöltésének eseménykezelője.
        /// </summary>
        private async void ViewModel_LoadGameClose(object sender, String name)
        {
            if (name != null)
            {
                try
                {
                    await _model.LoadGameAsync(name);
                }
                catch
                {
                    MessageBox.Show("Játék betöltése sikertelen!", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            _loadWindow.Close(); // játékállapot betöltőtő ablak bezárása
            _viewModel.FinishTable();
            _viewModel.RefreshTable();
        }

        private void ViewModel_SaveGameOpen(object sender, EventArgs e)
        {
            _viewModel.SelectedGame = null; // kezdetben nincsen kiválasztott elem
            _viewModel.NewName = String.Empty;

            _saveWindow = new SaveWindow(); // létrehozzuk a játék állapot mentő ablakot
            _saveWindow.DataContext = _viewModel;
            _saveWindow.ShowDialog(); // megjelenítjük dialógusként

        }

        /// <summary>
        /// Játék mentésének eseménykezelője.
        /// </summary>
        private async void ViewModel_SaveGameClose(object sender, String name)
        {
            if (name != null)
            {
                try
                {
                    // felülírás ellenőrzése
                    var games = await _model.ListGamesAsync();
                    if (games.All(g => g.Name != name) ||
                        MessageBox.Show("Biztos, hogy felülírja a meglévő mentést?", "Bekerites",
                            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        await _model.SaveGameAsync(name);
                    }
                }
                catch
                {
                    MessageBox.Show("Játék mentése sikertelen!", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            _saveWindow.Close(); // játékállapot mentő ablak bezárása
        }

        private void ViewModel_ExitGame(object sender, System.EventArgs e)
        {
            _view.Close(); // ablak bezárása
        }
        #endregion

        #region Model event handlers

        /// <summary>
        /// Játék végének eseménykezelője.
        /// </summary>
        private void Model_GameOver(object sender, BekeritEventArgs e)
        {

            if (_model.BlueScore == _model.RedScore)
            {
                MessageBox.Show("A játék döntetlen lett!\nKérlek indíts új játékot!");
            }
            else if (_model.BlueScore > _model.RedScore)
            {
                MessageBox.Show("A Kék játékos nyert!\nPontszáma: " + _model.BlueScore + "\nKérlek indíts új játékot!");
            }
            else
            {
                MessageBox.Show("A Piros játékos nyert!\nPontszáma: " + _model.RedScore + "\nKérlek indíts új játékot!");
            }
        }

        #endregion
    }
}
