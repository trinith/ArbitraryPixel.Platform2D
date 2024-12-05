using System;

namespace ArbitraryPixel.Platform2D.Time
{
    /// <summary>
    /// Represents an object that can track the passing of time.
    /// </summary>
    public interface IStopwatch : IDisposable
    {
        /// <summary>
        /// Whether or not this object has been disposed.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Whether or not this object is currently tracking time.
        /// </summary>
        bool IsPaused { get; }

        /// <summary>
        /// The total elapsed time since this object was last reset. Time while stopped is excluded.
        /// </summary>
        TimeSpan ElapsedTime { get; }

        /// <summary>
        /// An event that occurs when this object is disposed.
        /// </summary>
        event EventHandler<EventArgs> Disposed;

        /// <summary>
        /// Start tracking time from the point when this method is called.
        /// </summary>
        void Start();

        /// <summary>
        /// Stop tracking time from the point when this method is called. Tracking can be resumed by calling Start again.
        /// </summary>
        void Stop();

        /// <summary>
        /// Reset time tracking.
        /// </summary>
        void Reset();
    }
}
