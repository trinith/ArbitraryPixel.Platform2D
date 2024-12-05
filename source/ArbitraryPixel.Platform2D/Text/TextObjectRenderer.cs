using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Common.Graphics.Factory;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ArbitraryPixel.Platform2D.Text
{
    /// <summary>
    /// An object responsible for rendering and animating ITextureObjects.
    /// </summary>
    public class TextObjectRenderer : ITextObjectRenderer
    {
        private IGrfxDevice _device;
        private IRenderTarget2D _staticTarget;
        private IRenderTarget2D _finalTarget;
        private ISpriteBatch _spriteBatch;
        private IRenderTargetFactory _renderTargetFactory;

        private Queue<ITextObject> _textQueue = new Queue<ITextObject>();
        private double? _accum = null;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="renderTargetFactory">An object responsible for creating IRenderTarget2D objects.</param>
        /// <param name="device">An object responsible for rendering to the screen.</param>
        /// <param name="spriteBatch">An object responsible for drawing textures.</param>
        /// <param name="bounds">Defines the bounds text objects should be rendered within. Should be located at (0, 0).</param>
        public TextObjectRenderer(IRenderTargetFactory renderTargetFactory, IGrfxDevice device, ISpriteBatch spriteBatch, Rectangle bounds)
        {
            _renderTargetFactory = renderTargetFactory ?? throw new ArgumentNullException();
            _device = device ?? throw new ArgumentNullException();
            _spriteBatch = spriteBatch ?? throw new ArgumentNullException();

            _staticTarget = _renderTargetFactory.Create(_device, bounds.Width, bounds.Height, RenderTargetUsage.PreserveContents);
            _finalTarget = _renderTargetFactory.Create(_device, bounds.Width, bounds.Height, RenderTargetUsage.DiscardContents);

            ClearBuffer(_staticTarget, Color.Transparent);
            ClearBuffer(_finalTarget, Color.Transparent);
        }

        #region ITextObjectRenderer Implementation
        /// <summary>
        /// True if this text renderer has rendered all of its text objects, False otherwise.
        /// </summary>
        public bool IsComplete
        {
            get { return (_textQueue.Count == 0); }
        }

        /// <summary>
        /// Enqueue a text object to be rendered at the appropraite time.
        /// </summary>
        /// <param name="textObject">The text object to add to this queue.</param>
        public void Enqueue(ITextObject textObject)
        {
            _textQueue.Enqueue(textObject);
        }

        /// <summary>
        /// Flush all outstanding text objects to a fully rendered state.
        /// </summary>
        public void Flush()
        {
            UpdateStaticRenderTarget(true);
        }

        /// <summary>
        /// Update this renderer.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (_textQueue.Count > 0)
            {
                ITextObject currentObject = _textQueue.Peek();

                if (_accum == null)
                    _accum = currentObject.TimePerCharacter;

                _accum -= gameTime.ElapsedGameTime.TotalSeconds;
                if (_accum <= 0)
                {
                    int numChars = (int)(1 - (_accum.Value / currentObject.TimePerCharacter));
                    currentObject.ShowLength += numChars;
                    _accum += numChars * currentObject.TimePerCharacter;
                }

                UpdateStaticRenderTarget(false);
            }
        }

        /// <summary>
        /// Render the current state of the text objects.
        /// </summary>
        /// <returns>A texture containing the rendered state.</returns>
        public ITexture2D Render()
        {
            _device.SetRenderTarget(_finalTarget);
            _device.Clear(Color.Transparent);

            _spriteBatch.Begin();

            _spriteBatch.Draw(_staticTarget, Vector2.Zero, Color.White);

            if (_textQueue.Count > 0)
            {
                ITextObject currentText = _textQueue.Peek();
                _spriteBatch.DrawString(currentText.TextDefinition.Font, currentText.CurrentText, currentText.Location, currentText.TextDefinition.Colour);
            }

            _spriteBatch.End();

            _device.SetRenderTarget(null);

            return _finalTarget;
        }

        /// <summary>
        /// Clear this renderer of all outstanding text objects and cached image data.
        /// </summary>
        public void Clear()
        {
            _textQueue.Clear();
            ClearBuffer(_staticTarget, Color.Transparent);
            ClearBuffer(_finalTarget, Color.Transparent);
        }
        #endregion

        private void UpdateStaticRenderTarget(bool flush)
        {
            bool renderingOpen = false;

            while (_textQueue.Count > 0 && (_textQueue.Peek().IsComplete || flush))
            {
                ITextObject currentObject = _textQueue.Dequeue();
                if (flush || currentObject.IsComplete)
                    currentObject.ShowLength = currentObject.TextDefinition.Text.Length;

                if (renderingOpen == false)
                {
                    _device.SetRenderTarget(_staticTarget);
                    _spriteBatch.Begin();
                    renderingOpen = true;
                }

                _spriteBatch.DrawString(currentObject.TextDefinition.Font, currentObject.CurrentText, currentObject.Location, currentObject.TextDefinition.Colour);
                _accum = null;
            }

            if (renderingOpen)
            {
                _spriteBatch.End();
                _device.SetRenderTarget(null);
            }
        }

        private void ClearBuffer(ITexture2D texture, Color clearColour)
        {
            Color[] data = new Color[texture.Width * texture.Height];
            for (int i = 0; i < data.Length; i++)
                data[i] = clearColour;
            texture.SetData<Color>(data);
        }
    }
}
