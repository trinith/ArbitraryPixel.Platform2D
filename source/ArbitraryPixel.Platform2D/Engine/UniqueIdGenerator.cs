using ArbitraryPixel.Common;
using System;
using System.Collections.Generic;

namespace ArbitraryPixel.Platform2D.Engine
{
    /// <summary>
    /// An object responsible for generating unique identifiers.
    /// </summary>
    public class UniqueIdGenerator : IUniqueIdGenerator
    {
        private long _lastTicks = -1;
        private List<string> _resolvedDuplicates = new List<string>();
        private IDateTimeFactory _dateTimeFactory;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="dateTimeFactory">An object responsible for managing IDateTime objects.</param>
        public UniqueIdGenerator(IDateTimeFactory dateTimeFactory)
        {
            _dateTimeFactory = dateTimeFactory ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Generate a new unique identifier.
        /// </summary>
        /// <returns>The generated identifier.</returns>
        public string GenerateUniqueId()
        {
            long ticks = _dateTimeFactory.Now.Ticks;
            string baseId = ticks.ToString("X");

            if (_lastTicks == ticks)
            {
                int duplicateCount = 1;
                string testId = GetDuplicateId(baseId, duplicateCount);

                // Find a duplicate id we haven't used yet. This shouldn't happen often so we can just brute force it.
                while (_resolvedDuplicates.Contains(testId = GetDuplicateId(baseId, duplicateCount++))) { }

                _resolvedDuplicates.Add(testId);

                return testId;
            }
            else
            {
                _resolvedDuplicates.Clear();
                _lastTicks = ticks;
                return baseId;
            }
        }

        private string GetDuplicateId(string baseId, int duplicateNumber)
        {
            return string.Format("{0}-{1}", baseId, duplicateNumber);
        }
    }
}
