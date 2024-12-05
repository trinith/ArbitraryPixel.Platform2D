namespace ArbitraryPixel.Platform2D.Theme
{
    /// <summary>
    /// Represents an object that stores information about a theme and where assets for that theme are located.
    /// </summary>
    public interface ITheme
    {
        /// <summary>
        /// An ID representing an object this theme will be applied to.
        /// </summary>
        string ObjectID { get; }

        /// <summary>
        /// An ID representing this theme.
        /// </summary>
        string ThemeID { get; }

        /// <summary>
        /// Get the full asset path for a given asset, based on the ObjectID and ThemeID for this theme.
        /// </summary>
        /// <param name="baseAssetName">The base asset name, without any path.</param>
        /// <returns>The fully qualified asset name for this theme.</returns>
        string GetFullAssetName(string baseAssetName);
    }
}
