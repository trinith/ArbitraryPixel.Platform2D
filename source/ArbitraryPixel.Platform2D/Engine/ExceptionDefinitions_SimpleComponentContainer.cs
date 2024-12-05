using System;

namespace ArbitraryPixel.Platform2D.Engine
{
    /// <summary>
    /// An exception that occurs when attempting to register a componet that is already registered.
    /// </summary>
    public class ComponentAlreadyRegisteredException : Exception { }

    /// <summary>
    /// An exception that occurs when an engine is constructed with a component container that does not contain a required component.
    /// </summary>
    public class RequiredComponentMissingException : Exception
    {
        /// <summary>
        /// The type of component that is missing.
        /// </summary>
        public Type MissingComponentType { get; private set; }

        /// <summary>
        /// Create a new object with an unspecified missing component type.
        /// </summary>
        public RequiredComponentMissingException()
        {
            this.MissingComponentType = null;
        }

        /// <summary>
        /// Create a new object, specifying the type of component that is missing.
        /// </summary>
        /// <param name="missingComponentType">The type of component that is missing.</param>
        public RequiredComponentMissingException(Type missingComponentType)
        {
            this.MissingComponentType = missingComponentType;
        }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        public override string Message
        {
            get
            {
                if (this.MissingComponentType != null)
                    return $"A component of type, '{this.MissingComponentType.Name}', is required but was not found.";
                else
                    return $"A required component was not found.";
            }
        }
    }

    /// <summary>
    /// An exception that occurs when an engine is constructed with a component container that does not contain a required component.
    /// </summary>
    public class RequiredComponentMissingException<T> : RequiredComponentMissingException
    {
        /// <summary>
        /// Create a new object.
        /// </summary>
        public RequiredComponentMissingException() : base(typeof(T)) { }
    }
}
