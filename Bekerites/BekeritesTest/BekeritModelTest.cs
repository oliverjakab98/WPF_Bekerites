using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bekerites.Model;
using Bekerit.Persistence;

namespace BekeritesTest
{
    [TestClass]
    public class BekeritModelTest
    {
        private BekeritModel _model;

        [TestInitialize]
        public void InInitialize()
        {
            _model = new BekeritModel(null);

            //_model.GameAdvanced += new EventHandler<BekeritEventArgs>(Model_GameAdvanced);
            //_model.GameOver += new EventHandler<BekeritEventArgs>(Model_GameOver);
        }

        [TestMethod]
        public void NewGameModelSmallMapTest()
        {
            _model._size = 6;
            _model.NewGame();

            Assert.AreEqual(0, _model.Table._gameSteps);
            Assert.AreEqual(0, _model.RedScore);
            Assert.AreEqual(0, _model.BlueScore);

            Int32 emptyFields = 0;
            for (Int32 i = 0; i < _model._size; i++)
                for (Int32 j = 0; j < _model._size; j++)
                    if (_model.Table.GetValue(i, j) == 0)
                        emptyFields++;

            Assert.AreEqual(_model._size * _model._size, emptyFields);
        }

        [TestMethod]
        public void NewGameModelMediumMapTest()
        {
            _model._size = 8;
            _model.NewGame();

            Assert.AreEqual(0, _model.Table._gameSteps);
            Assert.AreEqual(0, _model.RedScore);
            Assert.AreEqual(0, _model.BlueScore);

            Int32 emptyFields = 0;
            for (Int32 i = 0; i < _model._size; i++)
                for (Int32 j = 0; j < _model._size; j++)
                    if (_model.Table.GetValue(i, j) == 0)
                        emptyFields++;

            Assert.AreEqual(_model._size * _model._size, emptyFields);
        }

        [TestMethod]
        public void NewGameModelBigMapTest()
        {
            _model._size = 10;
            _model.NewGame();

            Assert.AreEqual(0, _model.Table._gameSteps);
            Assert.AreEqual(0, _model.RedScore);
            Assert.AreEqual(0, _model.BlueScore);

            Int32 emptyFields = 0;
            for (Int32 i = 0; i < _model._size; i++)
                for (Int32 j = 0; j < _model._size; j++)
                    if (_model.Table.GetValue(i, j) == 0)
                        emptyFields++;

            Assert.AreEqual(_model._size * _model._size, emptyFields);
        }

        [TestMethod]
        public void BekeritGameModelStepGame()
        {
            _model._size = 6;
            _model.NewGame();

            Assert.AreEqual(0, _model.Table._gameSteps);

            _model.Step(0, 0);
            _model.Step(1, 0);

            Assert.AreEqual(_model.Table.GetValue(0, 0), 1);
            Assert.AreEqual(_model.Table.GetValue(1, 0), 1);

            Assert.AreEqual(2, _model.Table._gameSteps);

            Assert.IsTrue(_model.Table.IsLocked(0, 0));
            Assert.IsTrue(_model.Table.IsLocked(1, 0));


        }
        [TestMethod]
        public void BekeritGameAdvanceTest()
        {
            _model._size = 6;
            _model.NewGame();

            _model.Table._gameSteps = 2;
            _model.Step(4, 4);
            _model.Step(4, 5);

            _model.OnGameAdvanced();
        }

        [TestMethod]
        public void BekeritGameOverTest()
        {
            _model._size = 6;
            _model.NewGame();

            Assert.IsFalse(_model.CheckGameOver());

            for (Int32 i = 0; i < 6; i++)
            {
                for (Int32 j = 0; j < 6; j++)
                {
                    _model.Table.SetValue(i, j, 1);
                }
            }
            Assert.IsTrue(_model.CheckGameOver());
        }

        private void Model_GameAdvanced(Object sender, BekeritEventArgs e)
        {
            Assert.IsTrue(_model.RedScore >= 0);
            Assert.IsTrue(_model.BlueScore >= 0);

            Assert.AreEqual(e.RedScore, _model.RedScore);
            Assert.IsFalse(e.IsWon);
        }

        private void Model_GameOver(Object sender, BekeritEventArgs e)
        {
            Assert.IsTrue(_model.IsGameOver);
        }
    }
}

