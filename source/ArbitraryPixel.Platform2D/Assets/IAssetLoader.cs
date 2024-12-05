namespace ArbitraryPixel.Platform2D.Assets
{
    /// <summary>
    /// Represents an object that will load assets into an asset bank.
    /// </summary>
    public interface IAssetLoader
    {
        /// <summary>
        /// Register a load method with this loader.
        /// </summary>
        /// <param name="method">A method that, when run, will load assets into a supplied asset bank.</param>
        void RegisterLoadMethod(AssetLoadMethod method);

        /// <summary>
        /// Load assets into the supplied bank object.
        /// </summary>
        void LoadBank();
    }
}
