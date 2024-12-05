using System;
using System.Collections.Generic;

namespace ArbitraryPixel.Platform2D.Time
{
    /// <summary>
    /// An object responsible for managing IStopwatch objects.
    /// </summary>
    public class StopwatchManager : IStopwatchManager
    {
        private IStopwatchFactory _factory;

        private List<IStopwatch> _ownedObjects = new List<IStopwatch>();

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="stopwatchFactory">An object responsible for creating IStopwatch objects.</param>
        public StopwatchManager(IStopwatchFactory stopwatchFactory)
        {
            _factory = stopwatchFactory ?? throw new ArgumentNullException();
        }

        private void Handle_StopwatchDisposed(object sender, EventArgs e)
        {
            IStopwatch sw = sender as IStopwatch;
            if (sw != null)
                _ownedObjects.Remove(sw);
        }

        #region IStopwatchManager Implementation
        /// <summary>
        /// Create a new IStopwatch object to be managed by this manager.
        /// </summary>
        /// <returns>The newly created object.</returns>
        /// <remarks>If the created object is disposed, it will be removed from this manager automatically.</remarks>
        public IStopwatch Create()
        {
            IStopwatch newObject = _factory.Create();
            newObject.Disposed += Handle_StopwatchDisposed;

            _ownedObjects.Add(newObject);

            return newObject;
        }

        /// <summary>
        /// Start all owned IStopwatch objects.
        /// </summary>
        public void Start()
        {
            foreach (IStopwatch stopwatch in _ownedObjects)
                stopwatch.Start();
        }

        /// <summary>
        /// Stop all owned IStopwatch objects.
        /// </summary>
        public void Stop()
        {
            foreach (IStopwatch stopwatch in _ownedObjects)
                stopwatch.Stop();
        }

        /// <summary>
        /// Reset all owned IStopwatch objects.
        /// </summary>
        public void Reset()
        {
            foreach (IStopwatch stopwatch in _ownedObjects)
                stopwatch.Reset();
        }

        /// <summary>
        /// Dispose and remove of all objects owned by this manager.
        /// </summary>
        public void Clear()
        {
            foreach (IStopwatch stopwatch in _ownedObjects)
            {
                stopwatch.Disposed -= Handle_StopwatchDisposed;
                stopwatch.Dispose();
            }

            _ownedObjects.Clear();
        }
        #endregion
    }
}
