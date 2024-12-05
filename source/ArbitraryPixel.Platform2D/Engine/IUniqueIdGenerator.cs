namespace ArbitraryPixel.Platform2D.Engine
{
    /// <summary>
    /// Represents an object responsible for generating unique identifiers.
    /// </summary>
    public interface IUniqueIdGenerator
    {
        /// <summary>
        /// Generate a new unique identifier.
        /// </summary>
        /// <returns>The generated identifier.</returns>
        string GenerateUniqueId();
    }
}
