using System;
using System.Text.RegularExpressions;

namespace ArbitraryPixel.Platform2D.Assets
{
    /// <summary>
    /// Represents an object responsible for storing and retrieving assets.
    /// </summary>
    public interface IAssetBank
    {
        /// <summary>
        /// An event that occurs when this AssetBank clears all its assets.
        /// </summary>
        event EventHandler<EventArgs> AssetsCleared;

        /// <summary>
        /// Remove all assets from this bank.
        /// </summary>
        void Clear();

        /// <summary>
        /// Put an asset, of a specified type, into the bank.
        /// </summary>
        /// <typeparam name="T">The type of asset to store.</typeparam>
        /// <param name="assetName">The name of the asset.</param>
        /// <param name="asset">The asset to store.</param>
        /// <param name="overwrite">If true, any existing asset will be overwritten, otherwise an exception will be thrown if an asset for this type and name already exists. Defaults false.</param>
        void Put<T>(string assetName, T asset, bool overwrite = false);

        /// <summary>
        /// Put an asset into the bank.
        /// </summary>
        /// <param name="assetType">The type of object the asset is.</param>
        /// <param name="assetName">The name of the asset.</param>
        /// <param name="asset">The asset object.</param>
        /// <param name="overwrite">If true, any existing asset will be overwritten, otherwise an exception will be thrown if an asset for this type and name already exists. Defaults false.</param>
        void Put(Type assetType, string assetName, object asset, bool overwrite = false);

        /// <summary>
        /// Check if an asset name exists for a given type.
        /// </summary>
        /// <typeparam name="T">The type of asset to check for.</typeparam>
        /// <param name="assetName">The name of the asset to check for.</param>
        /// <returns>True if an asset name exists for the given type, false otherwise.</returns>
        bool Exists<T>(string assetName);

        /// <summary>
        /// Get an asset by name, for a specified type.
        /// </summary>
        /// <typeparam name="T">The type of asset to retrieve.</typeparam>
        /// <param name="assetName">The name of the asset to retrieve.</param>
        /// <returns>The asset, if it exists in the bank.</returns>
        T Get<T>(string assetName);

        /// <summary>
        /// Get all assets for a specified type.
        /// </summary>
        /// <typeparam name="T">The type of asset to get.</typeparam>
        /// <returns>An array of all assets that currently exist in the bank for the specified type.</returns>
        T[] GetAllAssets<T>();

        /// <summary>
        /// Get all assets for a specified type where the name of the asset matches a given regular expression.
        /// </summary>
        /// <typeparam name="T">The type of asset to get.</typeparam>
        /// <param name="assetNameRegex">The regular expression used to match asset names.</param>
        /// <param name="matchOptions">The options for regular expression matching.</param>
        /// <returns>An array of all assets that currently exist in the bank for the specified type and that match the specified regular expression.</returns>
        T[] GetAllMatchingAssets<T>(string assetNameRegex, RegexOptions matchOptions = RegexOptions.None);

        /// <summary>
        /// Create a new asset loader for this asset bank.
        /// </summary>
        /// <returns>A newly created asset loader that can load assets into this asset bank.</returns>
        IAssetLoader CreateLoader();
    }
}
