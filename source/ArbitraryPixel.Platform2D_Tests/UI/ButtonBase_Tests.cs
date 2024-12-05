using System;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.UI;
using ArbitraryPixel.Platform2D.UI.Controller;
using ArbitraryPixel.Platform2D.UI.Factory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace ArbitraryPixel.Platform2D_Tests.UI
{
    [TestClass]
    public class ButtonBase_Tests
    {
        private IEngine _mockEngine;
        private IButtonControllerFactory _mockControllerFactory;

        private IButtonController _mockController;
        private RectangleF _bounds = new RectangleF(50, 100, 200, 100);

        private ButtonBase _sut;

        [TestInitialize]
        public void Init()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockControllerFactory = Substitute.For<IButtonControllerFactory>();

            _mockEngine.GetComponent<IButtonControllerFactory>().Returns(_mockControllerFactory);

            _mockController = Substitute.For<IButtonController>();
            _mockControllerFactory.Create(Arg.Any<IButton>()).Returns(_mockController);
        }

        private void Construct()
        {
            _sut = new ButtonBase(_mockEngine, _bounds, _mockControllerFactory);
        }

        private void ConstructComponent()
        {
            _sut = new ButtonBase(_mockEngine, _bounds);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_FactoryParam_NullFactoryShouldThrowException()
        {
            _sut = new ButtonBase(_mockEngine, _bounds, null);
        }

        [TestMethod]
        public void Construct_FactoryParam_ShouldCreateController()
        {
            Construct();

            _mockControllerFactory.Received(1).Create(_sut);
        }

        [TestMethod]
        public void Construct_FactoryParam_ShouldSubscribeToEventHandler_Tapped()
        {
            Construct();

            _mockController.Received(1).Tapped += Arg.Any<EventHandler<ButtonEventArgs>>();
        }

        [TestMethod]
        public void Construct_FactoryParam_ShouldSubscribeToEventHandler_Touched()
        {
            Construct();

            _mockController.Received(1).Touched += Arg.Any<EventHandler<ButtonEventArgs>>();
        }

        [TestMethod]
        public void Construct_FactoryParam_ShouldSubscribeToEventHandler_Released()
        {
            Construct();

            _mockController.Received(1).Released += Arg.Any<EventHandler<ButtonEventArgs>>();
        }

        [TestMethod]
        [ExpectedException(typeof(RequiredComponentMissingException))]
        public void Construct_FactoryComponent_MissingComponentShouldThrowException()
        {
            _mockEngine.GetComponent<IButtonControllerFactory>().Returns((IButtonControllerFactory)null);
            ConstructComponent();
        }

        [TestMethod]
        public void Construct_FactoryComponent_ShouldCreateController()
        {
            ConstructComponent();

            _mockControllerFactory.Received(1).Create(_sut);
        }

        [TestMethod]
        public void Construct_FactoryComponent_ShouldSubscribeToEventHandler_Tapped()
        {
            ConstructComponent();

            _mockController.Received(1).Tapped += Arg.Any<EventHandler<ButtonEventArgs>>();
        }

        [TestMethod]
        public void Construct_FactoryComponent_ShouldSubscribeToEventHandler_Touched()
        {
            ConstructComponent();

            _mockController.Received(1).Touched += Arg.Any<EventHandler<ButtonEventArgs>>();
        }

        [TestMethod]
        public void Construct_FactoryComponent_ShouldSubscribeToEventHandler_Released()
        {
            ConstructComponent();

            _mockController.Received(1).Released += Arg.Any<EventHandler<ButtonEventArgs>>();
        }
        #endregion
    }
}
