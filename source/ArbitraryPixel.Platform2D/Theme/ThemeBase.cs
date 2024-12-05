using System;

namespace ArbitraryPixel.Platform2D.Theme
{
    /// <summary>
    /// An object that stores information about a theme and where assets for that theme are located.
    /// </summary>
    public abstract class ThemeBase : ITheme
    {
        /// <summary>
        /// An ID representing this theme.
        /// </summary>
        public abstract string ThemeID { get; }

        /// <summary>
        /// An ID representing an object this theme will be applied to.
        /// </summary>
        public abstract string ObjectID { get; }

        /// <summary>
        /// An optional path to be added as a prefix to any path built by GetFullAssetName.
        /// <para>NOTE: Trailing backslashes will be trimmed accordingly so that the path builds as expected.</para>
        /// </summary>
        protected abstract string AssetPathPrefix { get; }

        /// <summary>
        /// Get the full asset path for a given asset, based on the ObjectID and ThemeID for this theme.
        /// </summary>
        /// <param name="baseAssetName">The base asset name, without any path.</param>
        /// <returns>The fully qualified asset name for this theme.</returns>
        public string GetFullAssetName(string baseAssetName)
        {
            return $@"{GetPrefix()}{this.ObjectID.ToString()}\{this.ThemeID.ToString()}\{baseAssetName}";
        }

        private string GetPrefix()
        {
            string prefix = this.AssetPathPrefix;
            if (prefix.EndsWith("\\"))
                prefix = prefix.TrimEnd(new char[] { '\\' });

            return (string.IsNullOrEmpty(prefix)) ? "" : $@"{prefix}\";
        }
    }
}
