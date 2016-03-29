//namespace DomainService2.Specs.DomainServiceSpecs
//{
//    using System;
//    using Machine.Fakes;
//    using Machine.Specifications;
//    using SAHL.Common.BusinessModel.Interfaces.Configuration;

//    [Subject(typeof(DomainService2.DomainService), "On Construction")]
//    public class When_the_domainservice_is_instantiated_with_a_null_remotingserviceconfigurator : WithFakes
//    {
//        static DomainService2.DomainService domainService;
//        static IActiveRecordInitialiser activeRecordInitialiser;
//        static IRemotingServiceConfigurator remotingServiceConfigurator;
//        static IDomainServiceIOC ioc;
//        static Exception exception;

//        Establish context = () =>
//        {
//            // mock the constructor arguments
//            activeRecordInitialiser = An<IActiveRecordInitialiser>();
//            remotingServiceConfigurator = null;
//            ioc = An<IDomainServiceIOC>();
//        };

//        Because of = () =>
//        {
//            // create an instance of the system under test
//            exception = Catch.Exception(() => domainService = new DomainService2.DomainService(activeRecordInitialiser, remotingServiceConfigurator, ioc));
//        };

//        It should_thrown_a_ArgumentNullException = () =>
//        {
//            exception.ShouldBeOfType<ArgumentNullException>();
//        };

//        It should_set_the_exception_parametername_property = () =>
//        {
//            ArgumentNullException argEx = exception as ArgumentNullException;
//            argEx.ParamName.ShouldEqual<string>(Strings.ArgRemoteConfigurator);
//        };
//    }
//}