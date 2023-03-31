using System;

namespace Game.Core.Base.ServiceLocator
{
    /// <summary>
    /// Static implementation of a IServiceLocator that manages services.
    /// </summary>
    public sealed class StaticServiceLocator : IServiceLocator
    {
        /// <inheritdoc/>
        public void Register<TService, TImpl>() where TService : IService where TImpl : TService
        {
            Implementation<TService>.ServiceInstance = Activator.CreateInstance<TImpl>();
        }
        
        /// <inheritdoc/>
        public void Register<TService>(TService instance) where TService : IService
        {
            Implementation<TService>.ServiceInstance = instance;
        }
        
        /// <inheritdoc/>
        public TService Resolve<TService>() where TService : IService
        {
            return Implementation<TService>.ServiceInstance;
        }
        
        private static class Implementation<TService> where TService : IService
        {
            public static TService ServiceInstance;
        }
    }
}