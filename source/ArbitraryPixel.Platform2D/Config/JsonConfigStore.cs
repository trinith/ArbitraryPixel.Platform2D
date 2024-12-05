using ArbitraryPixel.Common.Json;
using ArbitraryPixel.Common.SimpleFileSystem;
using System;
using System.Collections.Generic;
using System.IO;

namespace ArbitraryPixel.Platform2D.Config
{
    /// <summary>
    /// An object responsible for storing and retrieving configuration data via a JSON file.
    /// </summary>
    public class JsonConfigStore : IConfigStore
    {
        #region Private Members
        private ISimpleFileSystem _fileSystem;
        private IJsonConvert _jsonConvert;
        private string _dataFile;
        private Dictionary<string, string> _data = new Dictionary<string, string>();
        #endregion

        #region Constructor
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="fileSystem">An object responsible for simple file system interactions.</param>
        /// <param name="jsonConvert">An object responsible for serializing to, and deserializing from, Json.</param>
        /// <param name="dataFile">The Json file where data is stored.</param>
        public JsonConfigStore(ISimpleFileSystem fileSystem, IJsonConvert jsonConvert, string dataFile)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException();
            _jsonConvert = jsonConvert ?? throw new ArgumentNullException();
            _dataFile = (!string.IsNullOrEmpty(dataFile)) ? dataFile : throw new ArgumentNullException();
        }
        #endregion

        #region Private Methods
        private void PopulateData(string dataFileContents)
        {
            _data.Clear();
            if (!string.IsNullOrEmpty(dataFileContents))
            {
                var readData = _jsonConvert.DeserializeObject<Dictionary<string, string>>(dataFileContents);
                foreach (string key in readData.Keys)
                    _data.Add(key, readData[key]);
            }
        }

        private string GetDataString()
        {
            return _jsonConvert.SerializeObject(_data);
        }
        #endregion

        #region IConfigStore Implementation
        /// <summary>
        /// If True, data cannot be loaded or stored. Only in-memory values can be used.
        /// </summary>
        public bool IsTransient { get; set; } = false;

        /// <summary>
        /// Whether or not the cache has changed since the last time it was loaded or persisted.
        /// </summary>
        public bool CacheChanged { get; private set; } = false;

        /// <summary>
        /// Whether or not data exists at the specified key.
        /// </summary>
        /// <param name="key">The key to check to see if data exists at.</param>
        /// <returns>True if the key exists in the store, false otherwise.</returns>
        public bool ContainsKey(string key)
        {
            return _data.ContainsKey(key);
        }

        /// <summary>
        /// Get data from the config store.
        /// </summary>
        /// <param name="key">The key representing where data is stored.</param>
        /// <returns>The data, at the specified key. If no data at that key exists, or it is not the specified type, an exception is thrown.</returns>
        public string Get(string key)
        {
            return _data[key];
        }

        /// <summary>
        /// Store a value at the specified key.
        /// </summary>
        /// <param name="key">A key representing where the data is to be stored.</param>
        /// <param name="value">The data to store.</param>
        public void Store(string key, string value)
        {
            if (!_data.ContainsKey(key))
            {
                _data.Add(key, value);
                this.CacheChanged = true;
            }
            else if (_data[key] != value)
            {
                _data[key] = value;
                this.CacheChanged = true;
            }
        }

        /// <summary>
        /// Persist the current cache to the store.
        /// </summary>
        public void PersistCache()
        {
            if (!this.IsTransient)
            {
                string folder = Path.GetDirectoryName(_dataFile);
                if (!_fileSystem.FolderExists(folder))
                    _fileSystem.CreateFolder(folder);

                _fileSystem.WriteFileContents(_dataFile, GetDataString());
                this.CacheChanged = false;
            }
        }

        /// <summary>
        /// Replace the current cache with data found in the store.
        /// </summary>
        public void LoadCache()
        {
            if (!this.IsTransient)
            {
                string dataString = (_fileSystem.FileExists(_dataFile)) ? _fileSystem.ReadFileContents(_dataFile) : "";
                PopulateData(dataString);
            }
        }
        #endregion
    }
}
