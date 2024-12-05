using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ArbitraryPixel.Platform2D.Assets
{
    /// <summary>
    /// An object responsible for storing and retrieving assets.
    /// </summary>
    public class AssetBank : IAssetBank
    {
        #region Private Classes
        // Store an asset and handle type casting... there's probably a better way to do this but it escapes me right now :(
        private class AssetObject
        {
            private object _asset = null;

            public AssetObject(object asset)
            {
                _asset = asset;
            }

            public T GetAsset<T>()
            {
                return (T)_asset;
            }
        }
        #endregion

        private Dictionary<Type, Dictionary<string, AssetObject>> _assets = new Dictionary<Type, Dictionary<string, AssetObject>>();

        #region IAssetBank Implementation
        /// <summary>
        /// An event that occurs when this AssetBank clears all its assets.
        /// </summary>
        public event EventHandler<EventArgs> AssetsCleared;

        /// <summary>
        /// Remove all assets from this bank.
        /// </summary>
        public void Clear()
        {
            _assets.Clear();

            if (this.AssetsCleared != null)
                this.AssetsCleared(this, new EventArgs());
        }

        /// <summary>
        /// Get an asset by name, for a specified type.
        /// </summary>
        /// <typeparam name="T">The type of asset to retrieve.</typeparam>
        /// <param name="assetName">The name of the asset to retrieve.</param>
        /// <returns>The asset, if it exists in the bank.</returns>
        public T Get<T>(string assetName)
        {
            return _assets[typeof(T)][assetName].GetAsset<T>();
        }

        /// <summary>
        /// Put an asset, of a specified type, into the bank.
        /// </summary>
        /// <typeparam name="T">The type of asset to store.</typeparam>
        /// <param name="assetName">The name of the asset.</param>
        /// <param name="asset">The asset to store.</param>
        /// <param name="overwrite">If true, any existing asset will be overwritten, otherwise an exception will be thrown if an asset for this type and name already exists. Defaults false.</param>
        public void Put<T>(string assetName, T asset, bool overwrite = false)
        {
            this.Put(typeof(T), assetName, asset, overwrite);
        }

        /// <summary>
        /// Put an asset into the bank.
        /// </summary>
        /// <param name="assetType">The type of object the asset is.</param>
        /// <param name="assetName">The name of the asset.</param>
        /// <param name="asset">The asset object.</param>
        /// <param name="overwrite">If true, any existing asset will be overwritten, otherwise an exception will be thrown if an asset for this type and name already exists. Defaults false.</param>
        public void Put(Type assetType, string assetName, object asset, bool overwrite = false)
        {
            if (!_assets.ContainsKey(assetType))
            {
                _assets.Add(assetType, new Dictionary<string, AssetObject>());
            }

            if (overwrite && _assets[assetType].ContainsKey(assetName))
                _assets[assetType].Remove(assetName);

            _assets[assetType].Add(assetName, new AssetObject(asset));
        }

        /// <summary>
        /// Check if an asset name exists for a given type.
        /// </summary>
        /// <typeparam name="T">The type of asset to check for.</typeparam>
        /// <param name="assetName">The name of the asset to check for.</param>
        /// <returns>True if an asset name exists for the given type, false otherwise.</returns>
        public bool Exists<T>(string assetName)
        {
            bool exists = false;

            if (_assets.ContainsKey(typeof(T)))
            {
                exists = _assets[typeof(T)].ContainsKey(assetName);
            }

            return exists;
        }

        /// <summary>
        /// Get all assets for a specified type.
        /// </summary>
        /// <typeparam name="T">The type of asset to get.</typeparam>
        /// <returns>An array of all assets that currently exist in the bank for the specified type.</returns>
        public T[] GetAllAssets<T>()
        {
            List<T> assets = new List<T>();

            if (_assets.ContainsKey(typeof(T)))
            {
                Dictionary<string, AssetObject> typeAssets = _assets[typeof(T)];
                foreach (AssetObject assetObject in typeAssets.Values)
                {
                    assets.Add(assetObject.GetAsset<T>());
                }
            }

            return (assets.Count > 0) ? assets.ToArray() : null;
        }

        /// <summary>
        /// Get all assets for a specified type where the name of the asset matches a given regular expression.
        /// </summary>
        /// <typeparam name="T">The type of asset to get.</typeparam>
        /// <param name="assetNameRegex">The regular expression used to match asset names.</param>
        /// <param name="matchOptions">The options for regular expression matching.</param>
        /// <returns>An array of all assets that currently exist in the bank for the specified type and that match the specified regular expression.</returns>
        public T[] GetAllMatchingAssets<T>(string assetNameRegex, RegexOptions matchOptions = RegexOptions.None)
        {
            List<T> assets = new List<T>();

            if (_assets.ContainsKey(typeof(T)))
            {
                Regex regex = new Regex(assetNameRegex, matchOptions);

                Dictionary<string, AssetObject> typeAssets = _assets[typeof(T)];
                foreach (string assetName in typeAssets.Keys)
                {
                    if (regex.IsMatch(assetName))
                        assets.Add(typeAssets[assetName].GetAsset<T>());
                }
            }

            return (assets.Count > 0) ? assets.ToArray() : null;
        }

        /// <summary>
        /// Create a new asset loader for this asset bank.
        /// </summary>
        /// <returns>A newly created asset loader that can load assets into this asset bank.</returns>
        public IAssetLoader CreateLoader()
        {
            return new AssetLoader(this);
        }
        #endregion
    }
}
