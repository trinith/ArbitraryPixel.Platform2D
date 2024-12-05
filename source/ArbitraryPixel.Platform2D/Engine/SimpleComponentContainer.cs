using System;
using System.Collections.Generic;

namespace ArbitraryPixel.Platform2D.Engine
{
    /// <summary>
    /// A simple implementation of IComponentContainer using a Dictionary.
    /// </summary>
    public class SimpleComponentContainer : IComponentContainer
    {
        // Consider replacing this with a third party object... but it will do for now ;)

        private Dictionary<Type, object> _components = new Dictionary<Type, object>();

        /// <summary>
        /// Register a component with the container.
        /// </summary>
        /// <typeparam name="TComponent">The component type to register.</typeparam>
        /// <param name="component">The component to register.</param>
        public void RegisterComponent<TComponent>(TComponent component)
            where TComponent : class
        {
            if (_components.ContainsKey(typeof(TComponent)))
                throw new ComponentAlreadyRegisteredException();

            _components.Add(typeof(TComponent), component);
        }

        /// <summary>
        /// Get a component from the container. If the container does not have a component for the type specified, null is returned.
        /// </summary>
        /// <typeparam name="TComponent">The type of component to get.</typeparam>
        /// <returns>The component of the specfied type if it exists, null otherwise.</returns>
        public TComponent GetComponent<TComponent>()
            where TComponent : class
        {
            return
                (_components.ContainsKey(typeof(TComponent)))
                    ? (TComponent)_components[typeof(TComponent)]
                    : null;
        }

        /// <summary>
        /// Get a component from the container. If the container does not have a component for the type specified, null is returned.
        /// </summary>
        /// <param name="componentType">The type of component to get.</param>
        /// <returns>The component of the specfied type if it exists, null otherwise.</returns>
        public object GetComponent(Type componentType)
        {
            return
                (_components.ContainsKey(componentType))
                    ? _components[componentType]
                    : null;
        }

        /// <summary>
        /// Check whether or not this container has the specified component type.
        /// </summary>
        /// <param name="componentType">The type of component to check for.</param>
        /// <returns>True if a component of the specified type exists in this container, false otherwise.</returns>
        public bool ContainsComponent(Type componentType)
        {
            return _components.ContainsKey(componentType);
        }

        /// <summary>
        /// Check whether or not this container has the specified component type.
        /// </summary>
        /// <typeparam name="TComponent">The type of component to check for.</typeparam>
        /// <returns>True if a component of the specified type exists in this container, false otherwise.</returns>
        public bool ContainsComponent<TComponent>()
            where TComponent : class
        {
            return _components.ContainsKey(typeof(TComponent));
        }
    }
}
