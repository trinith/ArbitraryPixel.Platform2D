using ArbitraryPixel.Platform2D.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArbitraryPixel.Platform2D_Tests.Engine
{
    [TestClass]
    public class SimpleComponentContainer_Tests
    {
        #region Test Component Types
        private interface ITestCompA { }
        private class TestCompA : ITestCompA { }
        private class TestCompAOther : ITestCompA { }

        private interface ITestCompB { }
        private class TestCompB : ITestCompB { }
        #endregion

        private SimpleComponentContainer _sut;

        [TestInitialize]
        public void Initialize()
        {
        }

        private void Construct()
        {
            _sut = new SimpleComponentContainer();
        }

        #region RegisterComponent Tests
        [TestMethod]
        public void RegisterComponentWithUnregisteredComponentShouldNotThrowException()
        {
            Construct();

            _sut.RegisterComponent<ITestCompA>(new TestCompA());
        }

        [TestMethod]
        [ExpectedException(typeof(ComponentAlreadyRegisteredException))]
        public void RegisterComponentWithAlreadyRegisteredComponentShouldThrowException()
        {
            Construct();

            _sut.RegisterComponent<ITestCompA>(new TestCompA());
            _sut.RegisterComponent<ITestCompA>(new TestCompAOther());
        }
        #endregion

        #region GetComponent Tests
        [TestMethod]
        public void GetComponent_Generic_WithRegisteredComponentShouldReturnExpectedComponent()
        {
            Construct();
            ITestCompA component = new TestCompA();
            _sut.RegisterComponent<ITestCompA>(component);

            Assert.AreSame(component, _sut.GetComponent<ITestCompA>());
        }

        [TestMethod]
        public void GetComponent_Generic_WithUnregisteredComponentShouldReturnNull()
        {
            Construct();

            Assert.IsNull(_sut.GetComponent<ITestCompA>());
        }

        [TestMethod]
        public void GetComponent_Param_WithRegisteredComponentShouldReturnExpectedComponent()
        {
            Construct();
            ITestCompA component = new TestCompA();
            _sut.RegisterComponent<ITestCompA>(component);

            Assert.AreSame(component, _sut.GetComponent(typeof(ITestCompA)));
        }

        [TestMethod]
        public void GetComponent_Param_WithUnregisteredComponentShouldReturnNull()
        {
            Construct();

            Assert.IsNull(_sut.GetComponent(typeof(ITestCompA)));
        }
        #endregion

        #region ContainsComponent Tests
        [TestMethod]
        public void ContainsComponent_Generic_WithRegisteredComponentShouldReturnExpectedValue()
        {
            Construct();
            _sut.RegisterComponent<ITestCompA>(new TestCompA());

            Assert.IsTrue(_sut.ContainsComponent<ITestCompA>());
        }

        [TestMethod]
        public void ContainsComponent_Generic_WithUnregisteredComponentShouldReturnExpectedValue()
        {
            Construct();

            Assert.IsFalse(_sut.ContainsComponent<ITestCompA>());
        }

        [TestMethod]
        public void ContainsComponent_Param_WithRegisteredComponentShouldReturnExpectedValue()
        {
            Construct();
            _sut.RegisterComponent<ITestCompA>(new TestCompA());

            Assert.IsTrue(_sut.ContainsComponent(typeof(ITestCompA)));
        }

        [TestMethod]
        public void ContainsComponent_Param_WithUnregisteredComponentShouldReturnExpectedValue()
        {
            Construct();

            Assert.IsFalse(_sut.ContainsComponent(typeof(ITestCompA)));
        }
        #endregion
    }
}
