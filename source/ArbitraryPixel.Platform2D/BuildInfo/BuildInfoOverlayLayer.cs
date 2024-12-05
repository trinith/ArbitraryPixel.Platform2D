using ArbitraryPixel.Common;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Layer;
using ArbitraryPixel.Platform2D.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ArbitraryPixel.Platform2D.BuildInfo
{
    /// <summary>
    /// An object responsible for drawing the data in an IBuildInfoOverlayLayerModel to a layer.
    /// </summary>
    public class BuildInfoOverlayLayer : LayerBase
    {
        //*/
        private const int UPDATEINTERVAL_MIN = 15;
        private const int UPDATEINTERVAL_MAX = 45;
        /*/
        private const int UPDATEINTERVAL_MIN = 3;
        private const int UPDATEINTERVAL_MAX = 7;
        //*/

        private IBuildInfoOverlayLayerModel _model;
        private IRandom _random;
        private SizeF _modelTextSize;
        private double _secondsUntilMove = 999;

        /// <summary>
        /// Create a new instance, setting this object's Matrix property to the ScaleMatrix of the host's ScreenManager.
        /// </summary>
        /// <param name="host">The IEngine object this layer will be hosted by. The Matrix property will be set to the ScaleMatrix property of host's ScreenManager.</param>
        /// <param name="mainSpriteBatch">The main sprite batch that this layer will use to draw entities it contains.</param>
        /// <param name="model">An object responisble for providing build info data to render.</param>
        /// <param name="random">An object responsible for providing random numbers.</param>
        public BuildInfoOverlayLayer(IEngine host, ISpriteBatch mainSpriteBatch, IBuildInfoOverlayLayerModel model, IRandom random)
            : base(host, mainSpriteBatch)
        {
            this.SpriteSortMode = SpriteSortMode.Deferred;
            this.BlendState = BlendState.NonPremultiplied;

            _model = model ?? throw new ArgumentNullException();
            _random = random ?? throw new ArgumentNullException();

            _modelTextSize = _model.TextSize;
            _model.TextAnchor = GetRandomModelPosition();

            _secondsUntilMove = _random.Next(UPDATEINTERVAL_MIN, UPDATEINTERVAL_MAX);
        }

        private Vector2 GetRandomModelPosition()
        {
            return new Vector2(
                _random.Next(0, (int)(this.Host.ScreenManager.World.X - _modelTextSize.Width)),
                _random.Next(0, (int)(this.Host.ScreenManager.World.Y - _modelTextSize.Height))
            );
        }

        /// <summary>
        /// Occurs when Draw is called and draws all entities belonging to this scene, if they are visible.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            _secondsUntilMove -= gameTime.ElapsedGameTime.TotalSeconds;

            if (_secondsUntilMove <= 0)
            {
                _secondsUntilMove += _random.Next(UPDATEINTERVAL_MIN, UPDATEINTERVAL_MAX);
                _model.TextAnchor = GetRandomModelPosition();
            }
        }

        protected override void OnDraw(GameTime gameTime)
        {
            //base.OnDraw(gameTime); // Don't allow base to draw in this case, we'll take direct control.

            this.MainSpriteBatch.Begin(this.SpriteSortMode, this.BlendState, this.SamplerState, this.DepthStencilState, this.RasterizerState, this.Effect, this.Host.ScreenManager.ScaleMatrix);
            foreach (ITextObject textObject in _model.TextObjects)
            {
                this.MainSpriteBatch.DrawString(textObject.TextDefinition.Font, textObject.TextDefinition.Text, textObject.Location, textObject.TextDefinition.Colour);
            }
            this.MainSpriteBatch.End();
        }
    }
}
