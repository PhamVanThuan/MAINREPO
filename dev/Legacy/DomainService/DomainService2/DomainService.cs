//namespace DomainService2
//{
//    using System;
//    using SAHL.Common.BusinessModel.Interfaces.Configuration;

//    public class DomainService : IDomainService
//    {
//        private IActiveRecordInitialiser activeRecordInitialiser;
//        private IRemotingServiceConfigurator remotingConfigurator;
//        private IDomainServiceIOC ioc;

//        public DomainService(IActiveRecordInitialiser activeRecordInitialiser, IRemotingServiceConfigurator remotingConfigurator, IDomainServiceIOC ioc)
//        {
//            if (activeRecordInitialiser == null)
//            {
//                throw new ArgumentNullException(Strings.ArgActiveRecordInitialiser);
//            }

//            this.activeRecordInitialiser = activeRecordInitialiser;

//            if (remotingConfigurator == null)
//            {
//                throw new ArgumentNullException(Strings.ArgRemoteConfigurator);
//            }

//            this.remotingConfigurator = remotingConfigurator;

//            if (ioc == null)
//            {
//                throw new ArgumentNullException(Strings.ArgIOC);
//            }

//            this.ioc = ioc;
//        }

//        public void OnStart()
//        {
//            // Initialise Active Record
//            this.activeRecordInitialiser.InitialiseActiveRecord();

//            // Register a remoting channel
//            this.remotingConfigurator.RegisterChannel();

//            // Get all the domain services to be registered
//            var services = this.ioc.GetAllDomainServices();

//            foreach (IRemotingService service in services)
//            {
//                this.remotingConfigurator.AddService(service);
//            }
//        }

//        public void OnStop()
//        {
//            this.remotingConfigurator.UnRegisterChannel();

//            this.ioc.Dispose();
//        }
//    }
//}