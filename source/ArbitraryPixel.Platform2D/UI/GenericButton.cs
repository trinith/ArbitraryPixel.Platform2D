using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.UI.Factory;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ArbitraryPixel.Platform2D.UI
{
    /// <summary>
    /// An object responsible for showing a button that renders generic objects specified by definition on it.
    /// </summary>
    public class GenericButton : ButtonBase, IGenericButton
    {
        private ISpriteBatch _spriteBatch;

        /// <summary>
        /// A list of object definitions that this button can display.
        /// </summary>
        public List<IButtonObjectDefinition> ButtonObjects { get; } = new List<IButtonObjectDefinition>();

        /// <summary>
        /// Create a new instance of this object.
        /// </summary>
        /// <param name="host">The IEngine object that owns this button.</param>
        /// <param name="bounds">The bounds of the button.</param>
        /// <param name="spriteBatch">A sprite batch object this button can use for rendering.</param>
        public GenericButton(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch)
            : base(host, bounds)
        {
            _spriteBatch = spriteBatch ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Create a new instance of this object.
        /// </summary>
        /// <param name="host">The IEngine object that owns this button.</param>
        /// <param name="bounds">The bounds of the button.</param>
        /// <param name="controllerFactory">An object responsible for creating a controller for this button.</param>
        /// <param name="spriteBatch">A sprite batch object this button can use for rendering.</param>
        public GenericButton(IEngine host, RectangleF bounds, IButtonControllerFactory controllerFactory, ISpriteBatch spriteBatch)
            : base(host, bounds, controllerFactory)
        {
            _spriteBatch = spriteBatch ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Add a new button object to this generic button.
        /// </summary>
        /// <param name="buttonObject">The button object to add.</param>
        public void AddButtonObject(IButtonObjectDefinition buttonObject)
        {
            if (buttonObject == null)
                throw new ArgumentNullException();

            this.ButtonObjects.Add(buttonObject);
        }

        /// <summary>
        /// Occurs when Draw is called.
        /// </summary>
        /// <param name="gameTime">The current time state for the game.</param>
        protected override void OnDraw(GameTime gameTime)
        {
            base.OnDraw(gameTime);

            foreach (IButtonObjectDefinition objectDefinition in this.ButtonObjects)
            {
                objectDefinition.Draw(this, _spriteBatch);
            }
        }
    }
}
