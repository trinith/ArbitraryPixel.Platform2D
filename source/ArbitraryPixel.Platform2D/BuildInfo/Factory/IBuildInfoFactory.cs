using ArbitraryPixel.Common;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Layer;
using ArbitraryPixel.Platform2D.Text;
using Microsoft.Xna.Framework;

namespace ArbitraryPixel.Platform2D.BuildInfo.Factory
{
    /// <summary>
    /// Represents an object that creates BuildInfo objects.
    /// </summary>
    public interface IBuildInfoFactory
    {

        /// <summary>
        /// Create a new IBuildInfoOverlay object.
        /// </summary>
        /// <param name="buildInfoStore">An object responsible for providing data about a build.</param>
        /// <param name="textBuilder">An object responsible for building ITextObjects from a formatted string.</param>
        /// <param name="textColour">The colour of the overlay text. Of not specified, white with an alpha of 0.25 will be used.</param>
        /// <returns>The newly created object.</returns>
        IBuildInfoOverlayLayerModel CreateBuildInfoOverlayModel(IBuildInfoStore buildInfoStore, ITextObjectBuilder textBuilder, Color? textColour = null);

        /// <summary>
        /// Create a new BuildInfoOverlayLayer object.
        /// </summary>
        /// <param name="host">An object acting as the host engine for the game.</param>
        /// <param name="spriteBatch">An object responsible for drawing.</param>
        /// <param name="model">An object responsible for providing build info data to be rendered.</param>
        /// <param name="random">An object responsible for generating random numbers.</param>
        /// <returns>The newly created object.</returns>
        ILayer CreateBuildInfoOverlayLayer(IEngine host, ISpriteBatch spriteBatch, IBuildInfoOverlayLayerModel model, IRandom random);
    }
}
