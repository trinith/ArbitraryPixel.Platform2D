using System.Collections.Generic;

namespace ArbitraryPixel.Platform2D.UI
{
    /// <summary>
    /// Represents a button that has generic images and textures applied to it.
    /// </summary>
    public interface IGenericButton : IButton
    {
        /// <summary>
        /// A list of object definitions that this button can display.
        /// </summary>
        List<IButtonObjectDefinition> ButtonObjects { get; }

        /// <summary>
        /// Add a new button object to this generic button.
        /// </summary>
        /// <param name="buttonObject">The button object to add.</param>
        void AddButtonObject(IButtonObjectDefinition buttonObject);
    }
}
