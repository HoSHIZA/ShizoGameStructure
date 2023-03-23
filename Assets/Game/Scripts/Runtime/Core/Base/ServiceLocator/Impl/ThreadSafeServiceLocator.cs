using System;
using System.Collections.Concurrent;

namespace Game.Core.Base.ServiceLocator
{
    /// <summary>
    /// Thread-safe implementation of a IServiceLocator that manages services.
    /// </summary>
    public sealed class ThreadSafeServiceLocator : IServiceLocator
    {
        private readonly ConcurrentDictionary<Type, IService> _services = new ConcurrentDictionary<Type, IService>();

        /// <inheritdoc/>
        public void RegisterSingle<TService, TImpl>() where TService : IService where TImpl : TService
        {
            _services.AddOrUpdate(typeof(TService), 
                _ => Activator.CreateInstance<TImpl>(), 
                (_, _) => Activator.CreateInstance<TImpl>());
        }
        
        /// <inheritdoc/>
        public void RegisterSingle<TService>(TService instance) where TService : IService
        {
            _services.AddOrUpdate(typeof(TService), instance, (_, _) => instance);
        }
        
        /// <inheritdoc/>
        public TService Single<TService>() where TService : IService
        {
            if (_services.TryGetValue(typeof(TService), out var service))
            {
                return (TService)service;
            }
            
            throw new InvalidOperationException($"Service {typeof(TService)} not found.");
        }
    }

}