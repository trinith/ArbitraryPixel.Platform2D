using System;
using ArbitraryPixel.Platform2D.Time;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace ArbitraryPixel.Platform2D_Tests.Time
{
    [TestClass]
    public class StopwatchManager_Tests
    {
        private StopwatchManager _sut;
        private IStopwatchFactory _mockFactory;

        [TestInitialize]
        public void Init()
        {
            _mockFactory = Substitute.For<IStopwatchFactory>();
        }

        private void Construct()
        {
            _sut = new StopwatchManager(_mockFactory);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_StopwatchManager()
        {
            _sut = new StopwatchManager(null);
        }
        #endregion

        #region Create Tests
        [TestMethod]
        public void CreateShouldCallFactoryCreate()
        {
            Construct();

            _sut.Create();

            _mockFactory.Received(1).Create();
        }

        [TestMethod]
        public void CreateShouldAttachEventHandlerToNewlyCreatedObject()
        {
            Construct();

            IStopwatch mockObject = Substitute.For<IStopwatch>();
            _mockFactory.Create().Returns(mockObject);

            _sut.Create();

            mockObject.Received(1).Disposed += Arg.Any<EventHandler<EventArgs>>();
        }

        [TestMethod]
        public void CreateShouldReturnNewlyCreatedObject()
        {
            Construct();

            IStopwatch mockObject = Substitute.For<IStopwatch>();
            _mockFactory.Create().Returns(mockObject);

            IStopwatch actual = _sut.Create();

            Assert.AreSame(mockObject, actual);
        }
        #endregion

        #region Start/Stop/Reset Tests
        [TestMethod]
        public void StartShouldCallStartOnCreatedObjects()
        {
            IStopwatch mockObject = Substitute.For<IStopwatch>();
            _mockFactory.Create().Returns(mockObject);
            Construct();
            _sut.Create();

            _sut.Start();

            mockObject.Received(1).Start();
        }

        [TestMethod]
        public void StopShouldCallStopOnCreatedObjects()
        {
            IStopwatch mockObject = Substitute.For<IStopwatch>();
            _mockFactory.Create().Returns(mockObject);
            Construct();
            _sut.Create();

            _sut.Stop();

            mockObject.Received(1).Stop();
        }

        [TestMethod]
        public void ResetShouldCallResetOnCreatedObjects()
        {
            IStopwatch mockObject = Substitute.For<IStopwatch>();
            _mockFactory.Create().Returns(mockObject);
            Construct();
            _sut.Create();

            _sut.Reset();

            mockObject.Received(1).Reset();
        }
        #endregion

        #region Created Object Disposed Event Tests
        [TestMethod]
        public void DisposeOnCreatedObjectShouldRemoveObjectFromManager()
        {
            IStopwatch mockObject = Substitute.For<IStopwatch>();
            _mockFactory.Create().Returns(mockObject);
            Construct();
            _sut.Create();

            mockObject.Disposed += Raise.Event<EventHandler<EventArgs>>(mockObject, new EventArgs());

            _sut.Start();

            mockObject.Received(0).Start();
        }
        #endregion

        #region Clear Tests
        [TestMethod]
        public void ClearShouldRemoveEventHandlerFromManagedObjects()
        {
            IStopwatch mockObject = Substitute.For<IStopwatch>();
            _mockFactory.Create().Returns(mockObject);
            Construct();
            _sut.Create();

            _sut.Clear();

            mockObject.Received(1).Disposed -= Arg.Any<EventHandler<EventArgs>>();
        }

        [TestMethod]
        public void ClearShouldCallDisposeOnManagedObjects()
        {
            IStopwatch mockObject = Substitute.For<IStopwatch>();
            _mockFactory.Create().Returns(mockObject);
            Construct();
            _sut.Create();

            _sut.Clear();

            mockObject.Received(1).Dispose();
        }

        [TestMethod]
        public void ClearShouldStopManagingCreatedObjects()
        {
            IStopwatch mockObject = Substitute.For<IStopwatch>();
            _mockFactory.Create().Returns(mockObject);
            Construct();
            _sut.Create();

            _sut.Clear();

            _sut.Start();
            mockObject.Received(0).Start();
        }
        #endregion
    }
}
