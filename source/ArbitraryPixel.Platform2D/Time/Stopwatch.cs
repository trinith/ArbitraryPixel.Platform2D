using ArbitraryPixel.Common;
using System;

namespace ArbitraryPixel.Platform2D.Time
{
    /// <summary>
    /// An implementation of IStopWatch, providing time tracking functionaliity.
    /// </summary>
    public class Stopwatch : IStopwatch
    {
        private IDateTimeFactory _dateTimeFactory;

        private TimeSpan _accum = TimeSpan.Zero;
        private IDateTime _startTime;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="dateTimeFactory">An object responsible for providing an object that represents the current instant in time.</param>
        public Stopwatch(IDateTimeFactory dateTimeFactory)
        {
            _dateTimeFactory = dateTimeFactory ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Dispose of this object.
        /// </summary>
        /// <param name="disposing">Whether or not this object should dispose.</param>
        protected virtual void OnDispose(bool disposing)
        {
            if (!this.IsDisposed)
            {
                if (disposing)
                {
                    // Nothing to dispose of :)
                }

                if (this.Disposed != null)
                    this.Disposed(this, new EventArgs());

                this.IsDisposed = true;
            }
        }

        #region IDisposable Implementation
        /// <summary>
        /// Dispose of this object.
        /// </summary>
        public void Dispose()
        {
            OnDispose(true);
        }
        #endregion

        #region IStopwatch Implementation
        /// <summary>
        /// Whether or not this object has been disposed.
        /// </summary>
        public bool IsDisposed { get; private set; } = false;

        /// <summary>
        /// Whether or not this object is currently tracking time.
        /// </summary>
        public bool IsPaused { get; private set; } = true;

        /// <summary>
        /// The total elapsed time since this object was last reset. Time while stopped is excluded.
        /// </summary>
        public TimeSpan ElapsedTime
        {
            get
            {
                TimeSpan elapsedTime = _accum;

                if (!this.IsPaused)
                {
                    elapsedTime = elapsedTime.Add(_dateTimeFactory.Now.Subtract(_startTime));
                }

                return elapsedTime;
            }
        }

        /// <summary>
        /// An event that occurs when this object is disposed.
        /// </summary>
        public event EventHandler<EventArgs> Disposed;

        /// <summary>
        /// Start tracking time from the point when this method is called.
        /// </summary>
        public void Start()
        {
            if (this.IsDisposed)
                throw new ObjectDisposedException("Stopwatch");

            if (this.IsPaused)
            {
                _startTime = _dateTimeFactory.Now;
                this.IsPaused = false;
            }
        }

        /// <summary>
        /// Stop tracking time from the point when this method is called. Tracking can be resumed by calling Start again.
        /// </summary>
        public void Stop()
        {
            if (this.IsDisposed)
                throw new ObjectDisposedException("Stopwatch");

            if (!this.IsPaused)
            {
                _accum = _accum.Add(_dateTimeFactory.Now.Subtract(_startTime));
                this.IsPaused = true;
            }
        }

        /// <summary>
        /// Reset time tracking.
        /// </summary>
        public void Reset()
        {
            if (this.IsDisposed)
                throw new ObjectDisposedException("Stopwatch");

            _accum = TimeSpan.Zero;
            this.IsPaused = true;
        }
        #endregion
    }
}
