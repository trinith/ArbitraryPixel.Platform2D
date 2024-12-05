using System;

namespace ArbitraryPixel.Platform2D.Engine
{
    /// <summary>
    /// Event arguments for an ExternalActionOccurred event.
    /// </summary>
    public class ExternalActionEventArgs : EventArgs
    {
        /// <summary>
        /// The data associated with the event.
        /// </summary>
        public object Data { get; private set; }

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="data">The data associated with the event.</param>
        public ExternalActionEventArgs(object data)
        {
            this.Data = data;
        }
    }
}
