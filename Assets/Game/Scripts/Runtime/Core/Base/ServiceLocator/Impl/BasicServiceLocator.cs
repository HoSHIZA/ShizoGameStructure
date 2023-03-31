using System;
using System.Collections.Generic;

namespace Game.Core.Base.ServiceLocator
{
    /// <summary>
    /// Basic implementation of a IServiceLocator that manages services.
    /// </summary>
    public sealed class BasicServiceLocator : IServiceLocator
    {
        private readonly Dictionary<Type, IService> _services = new Dictionary<Type, IService>();

        /// <inheritdoc/>
        public void Register<TService, TImpl>() where TService : IService where TImpl : TService
        {
            if (_services.ContainsKey(typeof(TService))) return;
            
            _services[typeof(TService)] = Activator.CreateInstance<TImpl>();
        }
        
        /// <inheritdoc/>
        public void Register<TService>(TService instance) where TService : IService
        {
            if (_services.ContainsKey(typeof(TService))) return;

            _services[typeof(TService)] = instance;
        }
        
        /// <inheritdoc/>
        public TService Resolve<TService>() where TService : IService
        {
            if (_services.TryGetValue(typeof(TService), out var service))
            {
                return (TService)service;
            }
            
            throw new InvalidOperationException($"Service {typeof(TService)} not found.");
        }
    }

}