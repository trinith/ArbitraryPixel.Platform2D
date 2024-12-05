using System;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.UI;
using ArbitraryPixel.Platform2D.UI.Factory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;

namespace ArbitraryPixel.Platform2D_Tests.UI
{
    [TestClass]
    public class GenericButton_Tests
    {
        private GenericButton _sut;
        private IEngine _mockEngine;
        private IButtonControllerFactory _mockButtonControllerFactory;
        private ISpriteBatch _mockSpriteBatch;

        private RectangleF _bounds = new RectangleF(200, 100, 400, 300);

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockButtonControllerFactory = Substitute.For<IButtonControllerFactory>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();

            _sut = new GenericButton(_mockEngine, _bounds, _mockButtonControllerFactory, _mockSpriteBatch);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_SpriteBatch()
        {
            _sut = new GenericButton(_mockEngine, _bounds, _mockButtonControllerFactory, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructUsingComponentContainerAndNullSpriteBatchShouldThrowException()
        {
            _mockEngine.GetComponent<IButtonControllerFactory>().Returns(_mockButtonControllerFactory);
            _sut = new GenericButton(_mockEngine, _bounds, null);
        }

        [TestMethod]
        [ExpectedException(typeof(RequiredComponentMissingException))]
        public void ConstructUsingComponentContainerAndUnregisteredControllerFactroryShouldThrowException()
        {
            _mockEngine.GetComponent<IButtonControllerFactory>().Returns((IButtonControllerFactory)null);
            _sut = new GenericButton(_mockEngine, _bounds, _mockSpriteBatch); 
        }
        #endregion

        #region AddButtonObject Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddButtonObjectWithNullParameterShouldThrowException()
        {
            _sut.AddButtonObject(null);
        }

        [TestMethod]
        public void AddButtonObjectShouldAddObjectToButtonObjectsList()
        {
            IButtonObjectDefinition mockButtonObject = Substitute.For<IButtonObjectDefinition>();

            _sut.AddButtonObject(mockButtonObject);

            Assert.IsTrue(_sut.ButtonObjects.Contains(mockButtonObject));
        }
        #endregion

        #region Draw Tests
        [TestMethod]
        public void OnDrawShouldCallDrawOnAllObjectDefinitions()
        {
            _sut.ButtonObjects.Add(Substitute.For<IButtonObjectDefinition>());
            _sut.ButtonObjects.Add(Substitute.For<IButtonObjectDefinition>());

            _sut.Draw(new GameTime());

            Received.InOrder(
                () =>
                {
                    _sut.ButtonObjects[0].Draw(_sut, _mockSpriteBatch);
                    _sut.ButtonObjects[1].Draw(_sut, _mockSpriteBatch);
                }
            );
        }
        #endregion
    }
}
