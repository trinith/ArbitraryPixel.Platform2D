using ArbitraryPixel.Common;
using ArbitraryPixel.Platform2D.Time;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;

namespace ArbitraryPixel.Platform2D_Tests.Time
{
    [TestClass]
    public class Stopwatch_Tests
    {
        private Stopwatch _sut;
        private IDateTimeFactory _mockDateTimeFactory;

        [TestInitialize]
        public void Initialize()
        {
            _mockDateTimeFactory = Substitute.For<IDateTimeFactory>();
        }

        private void Construct()
        {
            _sut = new Stopwatch(_mockDateTimeFactory);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_DateTimeFactory()
        {
            _sut = new Stopwatch(null);
        }
        #endregion

        #region Property Default Tests
        [TestMethod]
        public void PropertyShouldReturnExpectedDefaultValue_IsDisposed()
        {
            Construct();

            Assert.IsFalse(_sut.IsDisposed);
        }

        [TestMethod]
        public void PropertyShouldReturnExpectedDefaultValue_IsPaused()
        {
            Construct();

            Assert.IsTrue(_sut.IsPaused);
        }

        [TestMethod]
        public void PropertyShouldReturnExpectedDefaultValue_ElapsedTime()
        {
            Construct();

            Assert.AreEqual<TimeSpan>(TimeSpan.Zero, _sut.ElapsedTime);
        }
        #endregion

        #region Start Tests
        [TestMethod]
        public void ElapsedTimeAfterStartShouldReturnExpectedValue_TestA()
        {
            IDateTime mockStartTime = Substitute.For<IDateTime>(); mockStartTime.Ticks.Returns(1);
            IDateTime mockNextTime = Substitute.For<IDateTime>(); mockNextTime.Ticks.Returns(2);
            mockNextTime.Subtract(Arg.Any<IDateTime>()).Returns(TimeSpan.FromSeconds(5));
            _mockDateTimeFactory.Now.Returns(mockStartTime, mockNextTime, Substitute.For<IDateTime>());

            Construct();
            _sut.Start();

            Assert.AreEqual<TimeSpan>(TimeSpan.FromSeconds(5), _sut.ElapsedTime);
        }

        [TestMethod]
        public void ElapsedTimeAfterStartShouldReturnExpectedValue_TestB()
        {
            IDateTime mockStartTime = Substitute.For<IDateTime>(); mockStartTime.Ticks.Returns(1);
            IDateTime mockNextTime = Substitute.For<IDateTime>(); mockNextTime.Ticks.Returns(2);
            mockNextTime.Subtract(Arg.Any<IDateTime>()).Returns(TimeSpan.FromSeconds(11));
            _mockDateTimeFactory.Now.Returns(mockStartTime, mockNextTime, Substitute.For<IDateTime>());

            Construct();
            _sut.Start();

            Assert.AreEqual<TimeSpan>(TimeSpan.FromSeconds(11), _sut.ElapsedTime);
        }

        [TestMethod]
        public void ElapsedTimeAfterStartAndSubsequentCheckShouldReturnExpectedValue_TestA()
        {
            IDateTime mockStartTime = Substitute.For<IDateTime>(); mockStartTime.Ticks.Returns(1);

            IDateTime mockCheckA = Substitute.For<IDateTime>(); mockCheckA.Ticks.Returns(2);
            mockCheckA.Subtract(Arg.Any<IDateTime>()).Returns(TimeSpan.FromSeconds(5));

            IDateTime mockCheckB = Substitute.For<IDateTime>(); mockCheckA.Ticks.Returns(3);
            mockCheckB.Subtract(Arg.Any<IDateTime>()).Returns(TimeSpan.FromSeconds(8));

            _mockDateTimeFactory.Now.Returns(mockStartTime, mockCheckA, mockCheckB, Substitute.For<IDateTime>());

            Construct();
            _sut.Start();
            var dummy = _sut.ElapsedTime;

            Assert.AreEqual<TimeSpan>(TimeSpan.FromSeconds(8), _sut.ElapsedTime);
        }

        [TestMethod]
        public void ElapsedTimeAfterStartAndSubsequentCheckShouldReturnExpectedValue_TestB()
        {
            IDateTime mockStartTime = Substitute.For<IDateTime>(); mockStartTime.Ticks.Returns(1);

            IDateTime mockCheckA = Substitute.For<IDateTime>(); mockCheckA.Ticks.Returns(2);
            mockCheckA.Subtract(Arg.Any<IDateTime>()).Returns(TimeSpan.FromSeconds(5));

            IDateTime mockCheckB = Substitute.For<IDateTime>(); mockCheckA.Ticks.Returns(3);
            mockCheckB.Subtract(Arg.Any<IDateTime>()).Returns(TimeSpan.FromSeconds(12));

            _mockDateTimeFactory.Now.Returns(mockStartTime, mockCheckA, mockCheckB, Substitute.For<IDateTime>());

            Construct();
            _sut.Start();
            var dummy = _sut.ElapsedTime;

            Assert.AreEqual<TimeSpan>(TimeSpan.FromSeconds(12), _sut.ElapsedTime);
        }

        [TestMethod]
        public void ElapsedTimeAfterStartStopStartShouldReturnExpectedValue_TestA()
        {
            IDateTime mockStartTime = Substitute.For<IDateTime>(); mockStartTime.Ticks.Returns(1);

            IDateTime mockCheckA = Substitute.For<IDateTime>(); mockCheckA.Ticks.Returns(2);
            mockCheckA.Subtract(Arg.Any<IDateTime>()).Returns(TimeSpan.FromSeconds(3));

            IDateTime mockCheckB = Substitute.For<IDateTime>(); mockCheckA.Ticks.Returns(3);
            mockCheckB.Subtract(Arg.Any<IDateTime>()).Returns(TimeSpan.FromSeconds(7));
            _mockDateTimeFactory.Now.Returns(mockStartTime, mockCheckA, Substitute.For<IDateTime>(), mockCheckB, Substitute.For<IDateTime>());

            Construct();
            _sut.Start();
            _sut.Stop();
            _sut.Start();

            Assert.AreEqual<TimeSpan>(TimeSpan.FromSeconds(10), _sut.ElapsedTime);
        }

        [TestMethod]
        public void ElapsedTimeAfterStartStopStartShouldReturnExpectedValue_TestB()
        {
            IDateTime mockStartTime = Substitute.For<IDateTime>(); mockStartTime.Ticks.Returns(1);

            IDateTime mockCheckA = Substitute.For<IDateTime>(); mockCheckA.Ticks.Returns(2);
            mockCheckA.Subtract(Arg.Any<IDateTime>()).Returns(TimeSpan.FromSeconds(12));

            IDateTime mockCheckB = Substitute.For<IDateTime>(); mockCheckA.Ticks.Returns(3);
            mockCheckB.Subtract(Arg.Any<IDateTime>()).Returns(TimeSpan.FromSeconds(12));
            _mockDateTimeFactory.Now.Returns(mockStartTime, mockCheckA, Substitute.For<IDateTime>(), mockCheckB, Substitute.For<IDateTime>());

            Construct();
            _sut.Start();
            _sut.Stop();
            _sut.Start();

            Assert.AreEqual<TimeSpan>(TimeSpan.FromSeconds(24), _sut.ElapsedTime);
        }

        [TestMethod]
        public void IsPausedAfterStartShouldReturnExpectedValue()
        {
            Construct();
            _sut.Start();

            Assert.IsFalse(_sut.IsPaused);
        }

        [TestMethod]
        public void StartWhenStartedShouldNotCallDateTimeFactoryNow()
        {
            Construct();
            _sut.Start();
            _mockDateTimeFactory.ClearReceivedCalls();

            _sut.Start();

            var t = _mockDateTimeFactory.Received(0).Now;
        }
        #endregion

        #region Stop Tests
        [TestMethod]
        public void ElapsedTimeAfterStartStopShouldReturnExpectedValue_TestA()
        {
            IDateTime mockStartTime = Substitute.For<IDateTime>(); mockStartTime.Ticks.Returns(1);
            IDateTime mockNextTime = Substitute.For<IDateTime>(); mockNextTime.Ticks.Returns(2);
            mockNextTime.Subtract(Arg.Any<IDateTime>()).Returns(TimeSpan.FromSeconds(5));
            _mockDateTimeFactory.Now.Returns(mockStartTime, mockNextTime, Substitute.For<IDateTime>());

            Construct();
            _sut.Start();

            _sut.Stop();

            IDateTime mockOtherTime = Substitute.For<IDateTime>(); mockOtherTime.Ticks.Returns(3);
            mockOtherTime.Subtract(Arg.Any<IDateTime>()).Returns(TimeSpan.FromSeconds(10));
            _mockDateTimeFactory.Now.Returns(mockOtherTime);

            Assert.AreEqual<TimeSpan>(TimeSpan.FromSeconds(5), _sut.ElapsedTime);
        }

        [TestMethod]
        public void ElapsedTimeAfterStartStopShouldReturnExpectedValue_TestB()
        {
            IDateTime mockStartTime = Substitute.For<IDateTime>(); mockStartTime.Ticks.Returns(1);
            IDateTime mockNextTime = Substitute.For<IDateTime>(); mockNextTime.Ticks.Returns(2);
            mockNextTime.Subtract(Arg.Any<IDateTime>()).Returns(TimeSpan.FromSeconds(11));
            _mockDateTimeFactory.Now.Returns(mockStartTime, mockNextTime, Substitute.For<IDateTime>());

            Construct();
            _sut.Start();

            _sut.Stop();

            IDateTime mockOtherTime = Substitute.For<IDateTime>(); mockOtherTime.Ticks.Returns(3);
            mockOtherTime.Subtract(Arg.Any<IDateTime>()).Returns(TimeSpan.FromSeconds(22));
            _mockDateTimeFactory.Now.Returns(mockOtherTime);

            Assert.AreEqual<TimeSpan>(TimeSpan.FromSeconds(11), _sut.ElapsedTime);
        }

        [TestMethod]
        public void IsPausedAfterStartThenStopShouldReturnExpectedValue()
        {
            Construct();
            _sut.Start();

            _sut.Stop();

            Assert.IsTrue(_sut.IsPaused);
        }

        [TestMethod]
        public void StopWhenAlreadyStoppedShouldNotCallSubtractOnDateTimeFactoryNow()
        {
            Construct();
            _sut.Stop();

            IDateTime mockDT = Substitute.For<IDateTime>();
            _mockDateTimeFactory.Now.Returns(mockDT);

            _sut.Stop();

            mockDT.Received(0).Subtract(Arg.Any<IDateTime>());
        }
        #endregion

        #region Reset Tests
        [TestMethod]
        public void ElapsedTimeAfterResetShouldReturnExpectedValue()
        {
            IDateTime mockStartTime = Substitute.For<IDateTime>(); mockStartTime.Ticks.Returns(1);
            IDateTime mockNextTime = Substitute.For<IDateTime>(); mockNextTime.Ticks.Returns(2);
            mockNextTime.Subtract(Arg.Any<IDateTime>()).Returns(TimeSpan.FromSeconds(5));
            _mockDateTimeFactory.Now.Returns(mockStartTime, mockNextTime);

            Construct();
            _sut.Start();

            _sut.Reset();

            Assert.AreEqual<TimeSpan>(TimeSpan.Zero, _sut.ElapsedTime);
        }

        [TestMethod]
        public void IsPausedAfterStartThenResetShouldReturnExpectedValue()
        {
            Construct();
            _sut.Start();

            _sut.Reset();

            Assert.IsTrue(_sut.IsPaused);
        }
        #endregion

        #region Dispose Tests
        [TestMethod]
        public void IsDisposedAfterDisposeShouldReturnExpectedValue()
        {
            Construct();

            _sut.Dispose();

            Assert.IsTrue(_sut.IsDisposed);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void StopAfterDisposeShouldThrowExpectedException()
        {
            Construct();
            _sut.Dispose();

            _sut.Stop();
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void StartAfterDisposeShouldThrowExpectedException()
        {
            Construct();
            _sut.Dispose();

            _sut.Start();
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void ResetAfterDisposeShouldThrowExpectedException()
        {
            Construct();
            _sut.Dispose();

            _sut.Reset();
        }

        [TestMethod]
        public void DisposeShouldFireDisposedEvent()
        {
            Construct();

            EventHandler<EventArgs> mockHandler = Substitute.For<EventHandler<EventArgs>>();
            _sut.Disposed += mockHandler;

            _sut.Dispose();

            mockHandler.Received(1)(_sut, Arg.Any<EventArgs>());
        }

        [TestMethod]
        public void DisposeWhenAlreadyDisposedShouldNotFireDisposedEvent()
        {
            Construct();
            _sut.Dispose();

            EventHandler<EventArgs> mockHandler = Substitute.For<EventHandler<EventArgs>>();
            _sut.Disposed += mockHandler;

            mockHandler.Received(0)(Arg.Any<object>(), Arg.Any<EventArgs>());
        }
        #endregion
    }
}
