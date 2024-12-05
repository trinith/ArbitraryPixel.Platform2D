namespace ArbitraryPixel.Platform2D.BuildInfo
{
    /// <summary>
    /// Represents an object responsible for providing information about a build.
    /// </summary>
    public interface IBuildInfoStore
    {
        /// <summary>
        /// The platform for the build (ie: Windows, Android, iOS, etc...).
        /// </summary>
        string Platform { get; }

        /// <summary>
        /// The title for this build.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// The version of this build.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// The date the build was created.
        /// </summary>
        string Date { get; }

        /// <summary>
        /// The name of the product this build is for.
        /// </summary>
        string ProductName { get; }
    }
}
