using System.Collections.Generic;

namespace ArbitraryPixel.Platform2D.Theme
{
    /// <summary>
    /// An object that stores a collection of IThemeManager objects given a specified type that acts as an object ID provider.
    /// </summary>
    public class ThemeManagerCollection : IThemeManagerCollection
    {
        private Dictionary<string, IThemeManager> _managers = new Dictionary<string, IThemeManager>();

        /// <summary>
        /// Get the theme manager associated with the specified object ID from this collection.
        /// </summary>
        /// <param name="objectID">The object ID that identifies the desired theme manager.</param>
        /// <returns>The requested theme manager, if it exists. If it does not, a KeyNotFoundException will be thrown.</returns>
        public IThemeManager this[string objectID]
        {
            get { return _managers[objectID]; }
        }

        /// <summary>
        /// Whether or not a manager exists for the specified object ID.
        /// </summary>
        /// <param name="objectID">The object ID to check if a manager exists for.</param>
        /// <returns>True if a manager is registered with this collection for the specified ID, false otherwise.</returns>
        public bool ManagerExists(string objectID)
        {
            return _managers.ContainsKey(objectID);
        }

        /// <summary>
        /// Register a manager with this collection.
        /// </summary>
        /// <param name="objectID">The object ID this manager is for.</param>
        /// <param name="manager">The manager to register.</param>
        public void RegisterManager(string objectID, IThemeManager manager)
        {
            _managers.Add(objectID, manager);
        }
    }
}
