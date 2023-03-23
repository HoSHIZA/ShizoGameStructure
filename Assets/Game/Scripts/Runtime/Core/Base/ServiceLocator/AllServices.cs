namespace Game.Core.Base.ServiceLocator
{
    /// <summary>
    /// Static class for managing services with a static container for services.
    /// </summary>
    public static class AllServices
    {
        private static IServiceLocator _instance;
        
        private static bool _isSetup;
        
        /// <summary>
        /// A static container containing one of the IServiceLocator implementations.
        /// </summary>
        public static IServiceLocator Container
        {
            get
            {
                if (_instance == null)
                {
                    Setup(new StaticServiceLocator());
                }
                
                return _instance;
            }
            private set => _instance = value;
        }
        
        public static void Setup(IServiceLocator serviceLocator)
        {
            if (_isSetup) return;

            Container = serviceLocator;

            _isSetup = true;
        }
    }
}