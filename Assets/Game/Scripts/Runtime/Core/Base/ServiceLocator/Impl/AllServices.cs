namespace Game.Core.Base.ServiceLocator
{
    /// <summary>
    /// Static class for managing services with a static container for services.
    /// </summary>
    public static class AllServices
    {
        private static IServiceLocator _instance;
        
        /// <summary>
        /// A static container containing one of the IServiceLocator implementations.
        /// </summary>
        public static IServiceLocator Container => _instance != null ? _instance : new StaticServiceLocator();
    }
}