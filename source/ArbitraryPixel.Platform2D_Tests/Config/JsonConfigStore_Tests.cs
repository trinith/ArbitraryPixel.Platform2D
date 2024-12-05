using System;
using System.Collections.Generic;
using ArbitraryPixel.Common.Json;
using ArbitraryPixel.Common.SimpleFileSystem;
using ArbitraryPixel.Platform2D.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace ArbitraryPixel.Platform2D_Tests.Config
{
    [TestClass]
    public class JsonConfigStore_Tests
    {
        private JsonConfigStore _sut;
        private ISimpleFileSystem _mockFileSystem;
        private IJsonConvert _mockJsonConvert;

        private string _fileName = "TestFile.ext";

        [TestInitialize]
        public void Initialize()
        {
            _mockFileSystem = Substitute.For<ISimpleFileSystem>();
            _mockJsonConvert = Substitute.For<IJsonConvert>();
        }

        private void Construct()
        {
            _sut = new JsonConfigStore(_mockFileSystem, _mockJsonConvert, _fileName);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_FileSystem()
        {
            _sut = new JsonConfigStore(null, _mockJsonConvert, _fileName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_JsonConvert()
        {
            _sut = new JsonConfigStore(_mockFileSystem, null, _fileName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_DataFile()
        {
            _sut = new JsonConfigStore(_mockFileSystem, _mockJsonConvert, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithEmptyParameterShouldThrowException_DataFile()
        {
            _sut = new JsonConfigStore(_mockFileSystem, _mockJsonConvert, "");
        }
        #endregion

        #region ContainsKey Tests
        [TestMethod]
        public void ContainsKeyWithExistingKeyShouldReturnTrue()
        {
            Construct();

            _sut.Store("abcd", "1234");

            Assert.IsTrue(_sut.ContainsKey("abcd"));
        }

        [TestMethod]
        public void ContainsKeyWithNonExistentKeyShouldReturnFalse()
        {
            Construct();

            Assert.IsFalse(_sut.ContainsKey("abcd"));
        }
        #endregion

        #region Get/Store Tests
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void GetWithNonExistentKeyShouldThrowException()
        {
            Construct();

            var r = _sut.Get("asdf");
        }

        [TestMethod]
        public void GetWithExistingKeyShouldReturnExpectedValue()
        {
            Construct();

            _sut.Store("asdf", "someValue");

            Assert.AreEqual<string>("someValue", _sut.Get("asdf"));
        }
        #endregion

        #region PersistCache Tests
        [TestMethod]
        public void PersistCacheShouldCallJsonConvertSerialize()
        {
            Construct();

            _sut.PersistCache();

            _mockJsonConvert.Received(1).SerializeObject(Arg.Any<object>());
        }

        [TestMethod]
        public void PersistCacheShouldCallFileSystemWriteFileContents()
        {
            _mockJsonConvert.SerializeObject(Arg.Any<object>()).Returns("fakeJson");
            Construct();

            _sut.PersistCache();

            _mockFileSystem.Received(1).WriteFileContents(_fileName, "fakeJson");
        }

        [TestMethod]
        public void PersistCacheShouldCheckFileFolderExists()
        {
            _fileName = @"root\directory\name.ext";
            Construct();

            _sut.PersistCache();

            _mockFileSystem.Received(1).FolderExists(@"root\directory");
        }

        [TestMethod]
        public void PersistCacheWithFileInMissingFolderShouldCreateFolder()
        {
            _fileName = @"root\directory\name.ext";
            _mockFileSystem.FolderExists(@"root\directory").Returns(false);
            Construct();

            _sut.PersistCache();

            Received.InOrder(
                () =>
                {
                    _mockFileSystem.FolderExists(@"root\directory");
                    _mockFileSystem.CreateFolder(@"root\directory");
                }
            );
        }

        [TestMethod]
        public void PersistCacheWithIsTransientTrueShouldWriteFileContents()
        {
            Construct();
            _sut.IsTransient = true;
            _mockFileSystem.FileExists(_fileName).Returns(true);

            _sut.PersistCache();

            _mockFileSystem.Received(0).WriteFileContents(Arg.Any<string>(), Arg.Any<string>());
        }
        #endregion

        #region LoadCache Tests
        [TestMethod]
        public void LoadCacheShouldCheckFileExists()
        {
            Construct();

            _sut.LoadCache();

            _mockFileSystem.Received(1).FileExists(_fileName);
        }

        [TestMethod]
        public void LoadCacheWithExistingFileShouldReadFileContents()
        {
            Construct();
            _mockFileSystem.FileExists(_fileName).Returns(true);

            _sut.LoadCache();

            _mockFileSystem.Received(1).ReadFileContents(_fileName);
        }

        [TestMethod]
        public void LoadCacheWithNonExistentFileShouldNotCallFileSystemReadFileContents()
        {
            Construct();
            _mockFileSystem.FileExists(_fileName).Returns(false);

            _sut.LoadCache();

            _mockFileSystem.Received(0).ReadFileContents(_fileName);
        }

        [TestMethod]
        public void LoadCacheShouldClearExistingCache()
        {
            Construct();
            _sut.Store("Blah", "1234");

            _sut.LoadCache();

            Assert.IsFalse(_sut.ContainsKey("Blah"));
        }

        [TestMethod]
        public void LoadCacheWithoutFileContentsShouldNotCallJsonConvertDeserializeObject()
        {
            Construct();
            _mockFileSystem.FileExists(_fileName).Returns(false);

            _sut.LoadCache();

            _mockJsonConvert.Received(0).DeserializeObject<Dictionary<string, string>>(Arg.Any<string>());
        }

        [TestMethod]
        public void LoadCacheWithFileContentsShouldCallJsonConvertDeserializeObject()
        {
            Construct();
            _mockFileSystem.FileExists(_fileName).Returns(true);
            _mockFileSystem.ReadFileContents(_fileName).Returns("fakeJson");
            _mockJsonConvert.DeserializeObject<Dictionary<string, string>>(Arg.Any<string>()).Returns(new Dictionary<string, string>());

            _sut.LoadCache();

            _mockJsonConvert.Received(1).DeserializeObject<Dictionary<string, string>>("fakeJson");
        }

        [TestMethod]
        public void LoadCacheWithFileContentsShouldPopulateCacheWithExpectedData_TestA()
        {
            Dictionary<string, string> fakeData = new Dictionary<string, string>();
            fakeData.Add("KeyA", "ValueA");
            fakeData.Add("KeyB", "ValueB");

            Construct();
            _mockFileSystem.FileExists(_fileName).Returns(true);
            _mockFileSystem.ReadFileContents(_fileName).Returns("fakeJson");
            _mockJsonConvert.DeserializeObject<Dictionary<string, string>>(Arg.Any<string>()).Returns(fakeData);

            _sut.LoadCache();

            Assert.AreEqual<string>("ValueA", _sut.Get("KeyA"));
        }

        [TestMethod]
        public void LoadCacheWithFileContentsShouldPopulateCacheWithExpectedData_TestB()
        {
            Dictionary<string, string> fakeData = new Dictionary<string, string>();
            fakeData.Add("KeyA", "ValueA");
            fakeData.Add("KeyB", "ValueB");

            Construct();
            _mockFileSystem.FileExists(_fileName).Returns(true);
            _mockFileSystem.ReadFileContents(_fileName).Returns("fakeJson");
            _mockJsonConvert.DeserializeObject<Dictionary<string, string>>(Arg.Any<string>()).Returns(fakeData);

            _sut.LoadCache();

            Assert.AreEqual<string>("ValueB", _sut.Get("KeyB"));
        }

        [TestMethod]
        public void LoadCacheWithIsTransientTrueShouldReadFileContents()
        {
            Construct();
            _mockFileSystem.FileExists(_fileName).Returns(true);

            _sut.IsTransient = true;

            _sut.LoadCache();

            _mockFileSystem.Received(0).ReadFileContents(Arg.Any<string>());
        }
        #endregion

        #region CacheChanged Tests
        [TestMethod]
        public void CacheChangedShouldDefaultFalse()
        {
            Construct();

            Assert.IsFalse(_sut.CacheChanged);
        }

        [TestMethod]
        public void CacheChangedWhenDataStoredShouldReturnTrue()
        {
            Construct();

            _sut.Store("Blah", "asdf");

            Assert.IsTrue(_sut.CacheChanged);
        }

        [TestMethod]
        public void CacheChangedWhenDataPersistedShouldReturnFalse()
        {
            Construct();
            _sut.Store("Blah", "asdf");

            _sut.PersistCache();

            Assert.IsFalse(_sut.CacheChanged);
        }

        [TestMethod]
        public void CacheChangedWhenValueExistsForGivenKeyShouldReturnFalse()
        {
            Construct();
            _sut.Store("Blah", "asdf");
            _sut.PersistCache();

            _sut.Store("Blah", "asdf");

            Assert.IsFalse(_sut.CacheChanged);
        }
        #endregion
    }
}
