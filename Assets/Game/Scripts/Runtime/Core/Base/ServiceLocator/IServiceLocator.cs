using System;

namespace Game.Core.Base.ServiceLocator
{
    /// <summary>
    /// Interface for a service locator that is responsible for managing services.
    /// </summary>
    public interface IServiceLocator
    {
        /// <summary>
        /// Registers a single instance of a service and its implementation.
        /// </summary>
        /// <typeparam name="TService">The type of the service to register.</typeparam>
        /// <typeparam name="TImpl">The type of the implementation of the service to register.</typeparam>
        void RegisterSingle<TService, TImpl>() where TService : IService where TImpl : TService;

        /// <summary>
        /// Registers a single instance of a service.
        /// </summary>
        /// <typeparam name="TService">The type of the service to register.</typeparam>
        /// <param name="instance">The instance of the service to register.</param>
        void RegisterSingle<TService>(TService instance) where TService : IService;

        /// <summary>
        /// Gets a single instance of a service.
        /// </summary>
        /// <typeparam name="TService">The type of the service to get.</typeparam>
        /// <returns>The instance of the service.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the service is not found.</exception>
        TService Single<TService>() where TService : IService;
    }
}