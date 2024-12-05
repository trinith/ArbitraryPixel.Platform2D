namespace ArbitraryPixel.Platform2D.Theme
{
    /// <summary>
    /// Represents an object that stores a collection of IThemeManager objects given a specified type that acts as an object ID provider.
    /// </summary>
    public interface IThemeManagerCollection
    {
        /// <summary>
        /// Get the theme manager associated with the specified object ID from this collection.
        /// </summary>
        /// <param name="objectID">The object ID that identifies the desired theme manager.</param>
        IThemeManager this[string objectID] { get; }

        /// <summary>
        /// Whether or not a manager exists for the specified object ID.
        /// </summary>
        /// <param name="objectID">The object ID to check if a manager exists for.</param>
        /// <returns>True if a manager is registered with this collection for the specified ID, false otherwise.</returns>
        bool ManagerExists(string objectID);

        /// <summary>
        /// Register a manager with this collection.
        /// </summary>
        /// <param name="objectID">The object ID this manager is for.</param>
        /// <param name="manager">The manager to register.</param>
        void RegisterManager(string objectID, IThemeManager manager);
    }
}