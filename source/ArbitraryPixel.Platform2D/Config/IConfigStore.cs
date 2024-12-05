namespace ArbitraryPixel.Platform2D.Config
{
    /// <summary>
    /// Represents an object that can store and retrieve configuration data.
    /// </summary>
    public interface IConfigStore
    {
        /// <summary>
        /// If True, data cannot be loaded or stored. Only in-memory values can be used.
        /// </summary>
        bool IsTransient { get; set; }

        /// <summary>
        /// Whether or not the cache has changed since the last time it was loaded or persisted.
        /// </summary>
        bool CacheChanged { get; }

        /// <summary>
        /// Store a value at the specified key.
        /// </summary>
        /// <param name="key">A key representing where the data is to be stored.</param>
        /// <param name="value">The data to store.</param>
        void Store(string key, string value);

        /// <summary>
        /// Get data from the config store.
        /// </summary>
        /// <param name="key">The key representing where data is stored.</param>
        /// <returns>The data, at the specified key. If no data at that key exists, or it is not the specified type, an exception is thrown.</returns>
        string Get(string key);

        /// <summary>
        /// Whether or not data exists at the specified key.
        /// </summary>
        /// <param name="key">The key to check to see if data exists at.</param>
        /// <returns>True if the key exists in the store, false otherwise.</returns>
        bool ContainsKey(string key);

        /// <summary>
        /// Persist the current cache to the store.
        /// </summary>
        void PersistCache();

        /// <summary>
        /// Replace the current cache with data found in the store.
        /// </summary>
        void LoadCache();
    }
}
