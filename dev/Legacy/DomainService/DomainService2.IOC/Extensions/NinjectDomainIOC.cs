using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ninject;

namespace DomainService2.IOC.Extensions
{
    public class NinjectDomainIOC : IDomainServiceIOC
    {
        private IKernel kernel;

        public NinjectDomainIOC(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public ReadOnlyCollection<IDomainHost> GetAllDomainServices()
        {
            return new ReadOnlyCollection<IDomainHost>(new List<IDomainHost>(this.kernel.GetAll<IDomainHost>()));
        }

        public IHandlesDomainServiceCommand<T> GetCommandHandler<T>() where T : IDomainServiceCommand
        {
            return this.kernel.Get<IHandlesDomainServiceCommand<T>>();
        }

        public T Get<T>()
        {
            return this.kernel.Get<T>();
        }

        public void Dispose()
        {
            if (this.kernel != null)
            {
                this.kernel.Dispose();
            }
        }

		public void Bind<T, I>() where I : T
		{
			this.kernel.Rebind<T>().To<I>();
		}
	}
}