using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArbitraryPixel.Platform2D.Layer
{
    /// <summary>
    /// Represents an object that acts as a layer, containing entities and rendering them as one group.
    /// </summary>
    public interface ILayer : IEntity, IEntityContainer
    {
        /// <summary>
        /// The main sprite batch this layer will use to draw the entities it contains. Begin/End will always be called on this batch when Draw is called.
        /// </summary>
        ISpriteBatch MainSpriteBatch { get; }

        /// <summary>
        /// The SpriteSortMode this layer will use when it calls Begin on MainSpriteBatch.
        /// </summary>
        SpriteSortMode SpriteSortMode { get; set; }

        /// <summary>
        /// The BlendState this layer will use when it calls Begin on MainSpriteBatch.
        /// </summary>
        BlendState BlendState { get; set; }

        /// <summary>
        /// The SamplerState this layer will use when it calls Begin on MainSpriteBatch.
        /// </summary>
        SamplerState SamplerState { get; set; }

        /// <summary>
        /// The DepthStencilState this layer will use when it calls Begin on MainSpriteBatch.
        /// </summary>
        DepthStencilState DepthStencilState { get; set; }

        /// <summary>
        /// The RasterizerState this layer will use when it calls Begin on MainSpriteBatch.
        /// </summary>
        RasterizerState RasterizerState { get; set; }

        /// <summary>
        /// The IEffect this layer will use when it calls Begin on MainSpriteBatch.
        /// </summary>
        IEffect Effect { get; set; }

        /// <summary>
        /// The matrix this layer will use when it calls Begin on MainSpriteBatch.
        /// </summary>
        Matrix? Matrix { get; set; }
    }
}