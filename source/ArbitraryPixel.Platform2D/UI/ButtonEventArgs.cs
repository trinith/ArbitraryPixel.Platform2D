using Microsoft.Xna.Framework;

namespace ArbitraryPixel.Platform2D.UI
{
    /// <summary>
    /// Arguments for an event that involves a button event.
    /// </summary>
    public class ButtonEventArgs
    {
        /// <summary>
        /// The location where the button was touched.
        /// </summary>
        public Vector2 Location { get; private set; }

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="touchLocation">The location where the button was touched.</param>
        public ButtonEventArgs(Vector2 touchLocation)
        {
            this.Location = touchLocation;
        }
    }
}
