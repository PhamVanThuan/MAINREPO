using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using DomainService2;
using Ninject;

namespace SAHLDomainService
{
    public class NinjectDomainIOC : IDomainServiceIOC
    {
        private IKernel kernel;

        public NinjectDomainIOC(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public ReadOnlyCollection<IRemotingService> GetAllDomainServices()
        {
            return new ReadOnlyCollection<IRemotingService>(new List<IRemotingService>(this.kernel.GetAll<IRemotingService>()));
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
    }
}