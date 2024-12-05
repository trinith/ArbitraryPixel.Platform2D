using System;
using System.Collections.Generic;

namespace ArbitraryPixel.Platform2D.Theme
{
    /// <summary>
    /// An object that manages themes.
    /// </summary>
    public class ThemeManagerBase : IThemeManager
    {
        private Dictionary<string, ITheme> _themes = new Dictionary<string, ITheme>();

        /// <summary>
        /// The currently selected theme.
        /// </summary>
        public string CurrentThemeID { get; set; }

        /// <summary>
        /// A theme that is returned if a theme ID for CurrentThemeID doesen't exist in this manager. This can bet set to null.
        /// </summary>
        public ITheme DefaultTheme { get; set; } = null;

        /// <summary>
        /// Register a theme with this manager.
        /// </summary>
        /// <param name="theme">The theme to register.</param>
        public void RegisterTheme(ITheme theme)
        {
            _themes.Add(theme.ThemeID, theme);
        }

        /// <summary>
        /// Get the currently selected theme, or the default theme if the currently selected theme ID does not exist in this manager.
        /// </summary>
        /// <returns>The currently selected theme, or the default theme if the currently selected theme ID does not exist in this manager.</returns>
        public ITheme GetCurrentTheme()
        {
            return (_themes.ContainsKey(this.CurrentThemeID)) ? _themes[this.CurrentThemeID] : this.DefaultTheme;
        }

        /// <summary>
        /// Get the currently selected theme, casted to the specified theme type.
        /// </summary>
        /// <typeparam name="TTheme">An ITheme object type to cast to.</typeparam>
        /// <returns>The currently selected theme, cast to the specified theme type. Null if the cast is unsuccessful.</returns>
        public TTheme GetCurrentTheme<TTheme>() where TTheme : class, ITheme
        {
            return GetCurrentTheme() as TTheme;
        }

        /// <summary>
        /// Gets the theme stored at the specified theme ID.
        /// </summary>
        /// <param name="themeID">The ID of the theme to retrieve.</param>
        /// <returns>The theme at the specified theme ID, if it exists. If it doesn't exist, KeyNotFoundException will be thrown.</returns>
        public ITheme this[string themeID]
        {
            get { return this._themes[themeID]; }
        }

        /// <summary>
        /// Whether or not a theme exists for the specified theme ID.
        /// </summary>
        /// <param name="themeID">The theme ID to check for.</param>
        /// <returns>True if the theme exists in this manager, false otherwise.</returns>
        public bool ContainsTheme(string themeID)
        {
            return this._themes.ContainsKey(themeID);
        }
    }
}
