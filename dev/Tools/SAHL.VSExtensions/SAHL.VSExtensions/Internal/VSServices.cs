using SAHL.VSExtensions.Interfaces.Configuration;
using System;

namespace SAHomeloans.SAHL_VSExtensions.Internal
{
    public class VSServices : IVSServices
    {
        private IServiceProvider serviceProvider;

        public VSServices(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public T GetService<T>()
        {
            return (T)serviceProvider.GetService(typeof(T));
        }

        public I GetService<I, T>()
        {
            return (I)serviceProvider.GetService(typeof(T));
        }
    }
}