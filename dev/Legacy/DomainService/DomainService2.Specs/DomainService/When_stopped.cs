//namespace DomainService2.Specs.DomainServiceSpecs
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;
//    using System.Text;
//    using Machine.Fakes;
//    using Machine.Specifications;
//    using SAHL.Common.BusinessModel.Interfaces.Configuration;

//    [Subject(typeof(DomainService2.DomainService), "On Stop")]
//    public class When_stopped : WithFakes
//    {
//        static DomainService2.DomainService domainService;
//        static IActiveRecordInitialiser activeRecordInitialiser;
//        static IRemotingServiceConfigurator remotingServiceConfigurator;
//        static IDomainServiceIOC ioc;

//        Establish context = () =>
//        {
//            // mock the constructor arguments
//            activeRecordInitialiser = An<IActiveRecordInitialiser>();
//            remotingServiceConfigurator = An<IRemotingServiceConfigurator>();
//            ioc = An<IDomainServiceIOC>();

//            // create an instance of the system under test
//            domainService = new DomainService2.DomainService(activeRecordInitialiser, remotingServiceConfigurator, ioc);
//        };

//        Because of = () =>
//        {
//            domainService.OnStop();
//        };

//        It should_unregister_the_remoting_tcp_channel = () =>
//        {
//            remotingServiceConfigurator.WasToldTo(x => x.UnRegisterChannel());
//        };

//        It should_dispose_the_ioc = () =>
//        {
//            ioc.WasToldTo(x => x.Dispose());
//        };
//    }
//}