using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ArbitraryPixel.Platform2D.Layer
{
    /// <summary>
    /// An basic implementation of a layer.
    /// </summary>
    public class LayerBase : EntityContainerBase, ILayer
    {
        /// <summary>
        /// Create a new instance, setting this object's Matrix property to the ScaleMatrix of the host's ScreenManager.
        /// </summary>
        /// <param name="host">The IEngine object this layer will be hosted by. The Matrix property will be set to the ScaleMatrix property of host's ScreenManager.</param>
        /// <param name="mainSpriteBatch">The main sprite batch that this layer will use to draw entities it contains.</param>
        public LayerBase(IEngine host, ISpriteBatch mainSpriteBatch)
            : base(host)
        {
            this.MainSpriteBatch = mainSpriteBatch ?? throw new ArgumentNullException();
        }

        #region ILayer Implementation
        /// <summary>
        /// The main sprite batch this layer will use to draw the entities it contains. Begin/End will always be called on this batch when Draw is called.
        /// </summary>
        public ISpriteBatch MainSpriteBatch { get; private set; }

        /// <summary>
        /// The SpriteSortMode this layer will use when it calls Begin on MainSpriteBatch.
        /// </summary>
        public SpriteSortMode SpriteSortMode { get; set; } = SpriteSortMode.Deferred;

        /// <summary>
        /// The BlendState this layer will use when it calls Begin on MainSpriteBatch.
        /// </summary>
        public BlendState BlendState { get; set; } = null;

        /// <summary>
        /// The SamplerState this layer will use when it calls Begin on MainSpriteBatch.
        /// </summary>
        public SamplerState SamplerState { get; set; } = null;

        /// <summary>
        /// The DepthStencilState this layer will use when it calls Begin on MainSpriteBatch.
        /// </summary>
        public DepthStencilState DepthStencilState { get; set; } = null;

        /// <summary>
        /// The RasterizerState this layer will use when it calls Begin on MainSpriteBatch.
        /// </summary>
        public RasterizerState RasterizerState { get; set; } = null;

        /// <summary>
        /// The IEffect this layer will use when it calls Begin on MainSpriteBatch.
        /// </summary>
        public IEffect Effect { get; set; } = null;

        /// <summary>
        /// The matrix this layer will use when it calls Begin on MainSpriteBatch. If set to null, this object's Host's ScreenManager's ScaleMatrix will be used instead.
        /// </summary>
        public Matrix? Matrix { get; set; } = null;
        #endregion

        #region Override Methods
        /// <summary>
        /// Occurs when Draw is called and draws all entities belonging to this scene, if they are visible.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        protected override void OnDraw(GameTime gameTime)
        {
            // Check to see if our owner is a layer and if we have the same spritebatch. If we do, our parent already opened it when it started drawing.
            // This allows us to add a layer to an existing layer and let them use the same spritebatch object.
            bool batchAlreadyOpen = this.OwningContainer is ILayer && ((ILayer)this.OwningContainer).MainSpriteBatch == this.MainSpriteBatch;
            if (!batchAlreadyOpen)
            {
                this.MainSpriteBatch.Begin(
                    this.SpriteSortMode,
                    this.BlendState,
                    this.SamplerState,
                    this.DepthStencilState,
                    this.RasterizerState,
                    this.Effect,
                    (this.Matrix == null) ? this.Host.ScreenManager.ScaleMatrix : this.Matrix.Value
                );
            }

            base.OnDraw(gameTime);

            if (!batchAlreadyOpen)
                this.MainSpriteBatch.End();
        }
        #endregion
    }
}
