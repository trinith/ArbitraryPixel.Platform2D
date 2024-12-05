using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArbitraryPixel.Platform2D.UI
{
    /// <summary>
    /// Represents an object that creates IButtonObjectDefinition objects.
    /// </summary>
    public interface IButtonObjectDefinitionFactory
    {
        #region IButtonTextureDefinition
        /// <summary>
        /// Create a new, empty IButtonTextureDefinition object.
        /// </summary>
        /// <returns>The newly created object.</returns>
        IButtonTextureDefinition CreateButtonTextureDefinition();

        /// <summary>
        /// Create a new IButtonTextureDefinition with the specified parameters.
        /// </summary>
        /// <param name="texture">A texture to use for both unpressed and pressed states.</param>
        /// <param name="colour">A colour to use for both unpressed and pressed states.</param>
        /// <param name="spriteEffects">The sprite effects to use.</param>
        /// <returns>The newly created object.</returns>
        IButtonTextureDefinition CreateButtonTextureDefinition(ITexture2D texture, Color colour, SpriteEffects spriteEffects = SpriteEffects.None);

        /// <summary>
        /// Create a new IButtonTextureDefinition with the specified parameters.
        /// </summary>
        /// <param name="texture">A texture to use for both unpressed and pressed states.</param>
        /// <param name="colourNormal">The colour to use for the unpressed state.</param>
        /// <param name="colourPressed">The colour to use for the pressed state.</param>
        /// <param name="spriteEffects">The sprite effects to use.</param>
        /// <returns>The newly created object.</returns>
        IButtonTextureDefinition CreateButtonTextureDefinition(ITexture2D texture, Color colourNormal, Color colourPressed, SpriteEffects spriteEffects = SpriteEffects.None);
        #endregion

        #region IButtonTextDefinition
        /// <summary>
        /// Creatre a new, empty, IButtonTextDefinition object.
        /// </summary>
        /// <returns>The newly created object.</returns>
        IButtonTextDefinition CreateButtonTextDefinition();

        /// <summary>
        /// Create a new IButtonTextDefinition with the specified parameters.
        /// </summary>
        /// <param name="font">A font to use for both unpressed and pressed states.</param>
        /// <param name="text">The text to use for both unpressed and pressed states.</param>
        /// <param name="colourNormal">The colour to use for the unpressed state.</param>
        /// <param name="colourPressed">The colour to use for the pressed state.</param>
        /// <returns>The newly created object.</returns>
        IButtonTextDefinition CreateButtonTextDefinition(ISpriteFont font, string text, Color colourNormal, Color colourPressed);

        /// <summary>
        /// Create a new IButtonTextDefinition with the specified parameters.
        /// </summary>
        /// <param name="font">A font to use for both unpressed and pressed states.</param>
        /// <param name="text">The text to use for both unpressed and pressed states.</param>
        /// <param name="alignment"></param>
        /// <param name="colourNormal">The colour to use for the unpressed state.</param>
        /// <param name="colourPressed">The colour to use for the pressed state.</param>
        /// <returns>The newly created object.</returns>
        IButtonTextDefinition CreateButtonTextDefinition(ISpriteFont font, string text, TextLineAlignment alignment, Color colourNormal, Color colourPressed);
        #endregion
    }
}
