namespace ArbitraryPixel.Platform2D.Theme
{
    /// <summary>
    /// Represents an object that manages themes.
    /// </summary>
    public interface IThemeManager
    {
        /// <summary>
        /// A theme that is returned if a theme ID for CurrentThemeID doesen't exist in this manager. This can bet set to null.
        /// </summary>
        ITheme DefaultTheme { get; set; }

        /// <summary>
        /// The currently selected theme.
        /// </summary>
        string CurrentThemeID { get; set; }

        /// <summary>
        /// Register a theme with this manager.
        /// </summary>
        /// <param name="theme">The theme to register.</param>
        void RegisterTheme(ITheme theme);

        /// <summary>
        /// Get the currently selected theme, or the default theme if the currently selected theme ID does not exist in this manager.
        /// </summary>
        /// <returns>The currently selected theme, or the default theme if the currently selected theme ID does not exist in this manager.</returns>
        ITheme GetCurrentTheme();

        /// <summary>
        /// Get the currently selected theme, casted to the specified theme type.
        /// </summary>
        /// <typeparam name="TTheme">An ITheme object type to cast to.</typeparam>
        /// <returns>The currently selected theme, cast to the specified theme type.</returns>
        TTheme GetCurrentTheme<TTheme>() where TTheme : class, ITheme;

        /// <summary>
        /// Gets the theme stored at the specified theme ID.
        /// </summary>
        /// <param name="themeID">The ID of the theme to retrieve.</param>
        /// <returns>The theme at the specified theme ID, if it exists. If it doesn't exist, KeyNotFoundException will be thrown.</returns>
        ITheme this[string themeID] { get; }

        /// <summary>
        /// Whether or not a theme exists for the specified theme ID.
        /// </summary>
        /// <param name="themeID">The theme ID to check for.</param>
        /// <returns>True if the theme exists in this manager, false otherwise.</returns>
        bool ContainsTheme(string themeID);
    }
}
