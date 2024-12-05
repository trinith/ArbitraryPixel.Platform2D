using System;

namespace ArbitraryPixel.Platform2D.Engine
{
    /// <summary>
    /// Represents an object that acts as a container for components.
    /// </summary>
    public interface IComponentContainer
    {
        /// <summary>
        /// Register a component with the container.
        /// </summary>
        /// <typeparam name="TComponent">The component type to register.</typeparam>
        /// <param name="component">The component to register.</param>
        void RegisterComponent<TComponent>(TComponent component) where TComponent : class;

        /// <summary>
        /// Get a component from the container. If the container does not have a component for the type specified, null is returned.
        /// </summary>
        /// <typeparam name="TComponent">The type of component to get.</typeparam>
        /// <returns>The component of the specfied type if it exists, null otherwise.</returns>
        TComponent GetComponent<TComponent>() where TComponent : class;

        /// <summary>
        /// Get a component from the container. If the container does not have a component for the type specified, null is returned.
        /// </summary>
        /// <param name="componentType">The type of component to get.</param>
        /// <returns>The component of the specfied type if it exists, null otherwise.</returns>
        object GetComponent(Type componentType);

        /// <summary>
        /// Check whether or not this container has the specified component type.
        /// </summary>
        /// <param name="componentType">The type of component to check for.</param>
        /// <returns>True if a component of the specified type exists in this container, false otherwise.</returns>
        bool ContainsComponent(Type componentType);

        /// <summary>
        /// Check whether or not this container has the specified component type.
        /// </summary>
        /// <typeparam name="TComponent">The type of component to check for.</typeparam>
        /// <returns>True if a component of the specified type exists in this container, false otherwise.</returns>
        bool ContainsComponent<TComponent>() where TComponent : class;
    }
}
