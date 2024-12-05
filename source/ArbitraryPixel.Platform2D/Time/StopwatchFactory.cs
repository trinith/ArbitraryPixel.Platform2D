using ArbitraryPixel.Common;
using System;

namespace ArbitraryPixel.Platform2D.Time
{
    /// <summary>
    /// An object responsible for creating Stopwatch objects.
    /// </summary>
    public class StopwatchFactory : IStopwatchFactory
    {
        private IDateTimeFactory _dateTimeFactory;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="dateTimeFactory">An object responsible for providing an IDateTime object representing the current instant in time.</param>
        public StopwatchFactory(IDateTimeFactory dateTimeFactory)
        {
            _dateTimeFactory = dateTimeFactory ?? throw new ArgumentNullException();
        }

        #region IStopwatchFactory Implementation
        /// <summary>
        /// Create a new IStopwatch object.
        /// </summary>
        /// <returns>The newly created object.</returns>
        public IStopwatch Create()
        {
            return new Stopwatch(_dateTimeFactory);
        }
        #endregion
    }
}
