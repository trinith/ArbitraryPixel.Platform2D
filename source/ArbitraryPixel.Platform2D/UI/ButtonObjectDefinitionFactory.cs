using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArbitraryPixel.Platform2D.UI
{
    /// <summary>
    /// An object responsible for creating button object definition objects.
    /// </summary>
    public class ButtonObjectDefinitionFactory : IButtonObjectDefinitionFactory
    {
        #region IButtonTextureDefinition
        /// <summary>
        /// Create a new, empty IButtonTextureDefinition object.
        /// </summary>
        /// <returns>The newly created object.</returns>
        public IButtonTextureDefinition CreateButtonTextureDefinition()
        {
            return new ButtonTextureDefinition();
        }

        /// <summary>
        /// Create a new IButtonTextureDefinition with the specified parameters.
        /// </summary>
        /// <param name="texture">A texture to use for both unpressed and pressed states.</param>
        /// <param name="colour">A colour to use for both unpressed and pressed states.</param>
        /// <param name="spriteEffects">The sprite effects to use.</param>
        /// <returns>The newly created object.</returns>
        public IButtonTextureDefinition CreateButtonTextureDefinition(ITexture2D texture, Color colour, SpriteEffects spriteEffects = SpriteEffects.None)
        {
            return new ButtonTextureDefinition()
            {
                ImageNormal = texture,
                ImagePressed = texture,
                MaskNormal = colour,
                MaskPressed = colour,
                SpriteEffects = spriteEffects
            };
        }

        /// <summary>
        /// Create a new IButtonTextureDefinition with the specified parameters.
        /// </summary>
        /// <param name="texture">A texture to use for both unpressed and pressed states.</param>
        /// <param name="colourNormal">The colour to use for the unpressed state.</param>
        /// <param name="colourPressed">The colour to use for the pressed state.</param>
        /// <param name="spriteEffects">The sprite effects to use.</param>
        /// <returns>The newly created object.</returns>
        public IButtonTextureDefinition CreateButtonTextureDefinition(ITexture2D texture, Color colourNormal, Color colourPressed, SpriteEffects spriteEffects = SpriteEffects.None)
        {
            return new ButtonTextureDefinition()
            {
                ImageNormal = texture,
                ImagePressed = texture,
                MaskNormal = colourNormal,
                MaskPressed = colourPressed,
                SpriteEffects = spriteEffects
            };
        }
        #endregion

        #region IButtonTextDefinition
        /// <summary>
        /// Creatre a new, empty, IButtonTextDefinition object.
        /// </summary>
        /// <returns>The newly created object.</returns>
        public IButtonTextDefinition CreateButtonTextDefinition()
        {
            return new ButtonTextDefinition();
        }

        /// <summary>
        /// Create a new IButtonTextDefinition with the specified parameters.
        /// </summary>
        /// <param name="font">A font to use for both unpressed and pressed states.</param>
        /// <param name="text">The text to use for both unpressed and pressed states.</param>
        /// <param name="colourNormal">The colour to use for the unpressed state.</param>
        /// <param name="colourPressed">The colour to use for the pressed state.</param>
        /// <returns>The newly created object.</returns>
        public IButtonTextDefinition CreateButtonTextDefinition(ISpriteFont font, string text, Color colourNormal, Color colourPressed)
        {
            return new ButtonTextDefinition()
            {
                TextNormal = new TextDefinition(font, text, colourNormal),
                TextPressed = new TextDefinition(font, text, colourPressed)
            };
        }

        /// <summary>
        /// Create a new IButtonTextDefinition with the specified parameters.
        /// </summary>
        /// <param name="font">A font to use for both unpressed and pressed states.</param>
        /// <param name="text">The text to use for both unpressed and pressed states.</param>
        /// <param name="alignment"></param>
        /// <param name="colourNormal">The colour to use for the unpressed state.</param>
        /// <param name="colourPressed">The colour to use for the pressed state.</param>
        /// <returns>The newly created object.</returns>
        public IButtonTextDefinition CreateButtonTextDefinition(ISpriteFont font, string text, TextLineAlignment alignment, Color colourNormal, Color colourPressed)
        {
            return new ButtonTextDefinition()
            {
                Alignment = alignment,
                TextNormal = new TextDefinition(font, text, colourNormal),
                TextPressed = new TextDefinition(font, text, colourPressed)
            };
        }
        #endregion
    }
}
