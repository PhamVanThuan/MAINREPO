//namespace DomainService2.Specs.DomainServiceSpecs
//{
//    using System.Collections.ObjectModel;
//    using DomainService2.Specs.DomainService.Stubs;
//    using Machine.Fakes;
//    using Machine.Specifications;
//    using SAHL.Common.BusinessModel.Interfaces.Configuration;

//    [Subject(typeof(DomainService2.DomainService), "On Start")]
//    public class When_the_domainservice_is_started : WithFakes
//    {
//        static DomainService2.DomainService domainService;
//        static IActiveRecordInitialiser activeRecordInitialiser;
//        static IRemotingServiceConfigurator remotingServiceConfigurator;
//        static IDomainServiceIOC ioc;
//        static IRemotingService remotingService;

//        Establish context = () =>
//        {
//            // mock the constructor arguments
//            activeRecordInitialiser = An<IActiveRecordInitialiser>();
//            remotingServiceConfigurator = An<IRemotingServiceConfigurator>();
//            ioc = An<IDomainServiceIOC>();

//            // stub the available domain remoting services
//            remotingService = new ARemotingService();
//            ioc.WhenToldTo(x => x.GetAllDomainServices())
//                .Return(new ReadOnlyCollection<IRemotingService>(new IRemotingService[] { remotingService }));

//            // create an instance of the system under test
//            domainService = new DomainService2.DomainService(activeRecordInitialiser, remotingServiceConfigurator, ioc);
//        };

//        Because of = () =>
//            {
//                domainService.OnStart();
//            };

//        It should_initialise_activerecord = () =>
//            {
//                activeRecordInitialiser.WasToldTo(x => x.InitialiseActiveRecord());
//            };

//        It should_register_a_remoting_tcp_channel = () =>
//            {
//                remotingServiceConfigurator.WasToldTo(x => x.RegisterChannel());
//            };

//        It should_retrieve_all_services_to_be_registered = () =>
//            {
//                ioc.WasToldTo(x => x.GetAllDomainServices());
//            };

//        It should_add_each_available_service_to_the_collection_of_managed_services = () =>
//            {
//                remotingServiceConfigurator.WasToldTo(x => x.AddService(Param.IsAny<IRemotingService>())).OnlyOnce();
//            };
//    }
//}