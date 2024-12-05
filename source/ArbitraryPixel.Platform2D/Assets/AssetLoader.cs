using System;
using System.Collections.Generic;

namespace ArbitraryPixel.Platform2D.Assets
{
    /// <summary>
    /// An object responsible for loading assets into an asset bank, ensuring that if the objects have already been loaded, they are not loaded again.
    /// </summary>
    public class AssetLoader : IAssetLoader
    {
        private bool _loaded = false;
        private IAssetBank _bank;
        private List<AssetLoadMethod> _loadMethods = new List<AssetLoadMethod>();

        /// <summary>
        /// Create a new object.
        /// </summary>
        /// <param name="bank">The asset bank object this loader is associated with.</param>
        public AssetLoader(IAssetBank bank)
        {
            _bank = bank ?? throw new ArgumentNullException();

            _bank.AssetsCleared += (sender, e) => _loaded = false;
        }

        /// <summary>
        /// Register a load method with this loader.
        /// </summary>
        /// <param name="method">A method that, when run, will load assets into a supplied asset bank.</param>
        public void RegisterLoadMethod(AssetLoadMethod method)
        {
            _loadMethods.Add(method);
        }

        /// <summary>
        /// Load assets into the supplied bank object, but only if these assets haven't been loaded into the supplied bank yet.
        /// </summary>
        public void LoadBank()
        {
            if (_loaded == false)
            {
                foreach (AssetLoadMethod loadMethod in _loadMethods)
                    loadMethod(_bank);

                _loaded = true;
            }
        }
    }
}
