using System;

namespace ArbitraryPixel.Platform2D.UI
{
    /// <summary>
    /// The state of a ButtonBase object.
    /// </summary>
    [Flags]
    public enum ButtonState
    {
        /// <summary>
        /// The button is unpressed.
        /// </summary>
        Unpressed = 0x0,

        /// <summary>
        /// The button is pressed.
        /// </summary>
        Pressed = 0x1,
    }
}
