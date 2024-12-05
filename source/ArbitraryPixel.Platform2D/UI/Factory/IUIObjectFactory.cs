﻿using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Text;
using Microsoft.Xna.Framework;

namespace ArbitraryPixel.Platform2D.UI.Factory
{
    /// <summary>
    /// Represents an object responsible for creating UI objects.
    /// </summary>
    public interface IUIObjectFactory
    {
        /// <summary>
        /// Create a new IButtonObjectDefinitionFactory object.
        /// </summary>
        /// <returns>The created object.</returns>
        IButtonObjectDefinitionFactory CreateButtonObjectDefinitionFactory();

        /// <summary>
        /// Create a new IFormattedTextLabel object.
        /// </summary>
        /// <param name="host">The IEngine object that this entity belongs to.</param>
        /// <param name="bounds">The bounds in the gameworld of this entity.</param>
        /// <param name="spriteBatch">The ISpriteBatch object that will be used to render the label.</param>
        /// <param name="builder">An object responsible for building ITextObject objects.</param>
        /// <param name="renderer">An object responsible for rendering ITextObject objects to an ITexture object.</param>
        /// <param name="textFormat">The format string representing the text to show in this label.</param>
        /// <returns>The created object.</returns>
        IFormattedTextLabel CreateFormattedTextLabel(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch, ITextObjectBuilder builder, ITextObjectRenderer renderer, string textFormat);

        /// <summary>
        /// Create a new IGenericButton object.
        /// </summary>
        /// <param name="host">The IEngine object that owns this button.</param>
        /// <param name="bounds">The bounds of the button.</param>
        /// <param name="controllerFactory">An object responsible for creating a controller for the button.</param>
        /// <param name="spriteBatch">A sprite batch object this button can use for rendering.</param>
        /// <returns>The created object.</returns>
        IGenericButton CreateGenericButton(IEngine host, RectangleF bounds, IButtonControllerFactory controllerFactory, ISpriteBatch spriteBatch);

        /// <summary>
        /// Create a new IGenericButton object using a controller factory obtained from the host's component container.
        /// </summary>
        /// <param name="host">The IEngine object that owns this button.</param>
        /// <param name="bounds">The bounds of the button.</param>
        /// <param name="spriteBatch">A sprite batch object this button can use for rendering.</param>
        /// <returns>The created object.</returns>
        IGenericButton CreateGenericButton(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch);

        /// <summary>
        /// Create a new ITextLabel object.
        /// </summary>
        /// <param name="host">The owner of this entity.</param>
        /// <param name="location">The location of the text.</param>
        /// <param name="spriteBatch">The spritebatch used to render to the screen.</param>
        /// <param name="font">The font to use.</param>
        /// <param name="text">The message for this text.</param>
        /// <param name="colour">The colour for this text.</param>
        /// <returns>The created object.</returns>
        ITextLabel CreateTextLabel(IEngine host, Vector2 location, ISpriteBatch spriteBatch, ISpriteFont font, string text, Color colour);

        /// <summary>
        /// Create a n ew IStaticTexture object.
        /// </summary>
        /// <param name="host">The IEngine object that owns this button.</param>
        /// <param name="bounds">The bounds of the button.</param>
        /// <param name="spriteBatch">The spritebatch used to render to the screen.</param>
        /// <param name="texture">The texture to draw.</param>
        /// <returns>The created object.</returns>
        IStaticTexture CreateStaticTexture(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch, ITexture2D texture);
    }
}
