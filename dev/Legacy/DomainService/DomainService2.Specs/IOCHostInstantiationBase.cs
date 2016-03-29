//using System.Security.Principal;
//using System.Threading;
//using DomainService2.IOC;
//using Machine.Fakes;
//using Machine.Specifications;
//using SAHL.Communication;
//using SAHL.X2.Common;
//using SAHL.Communication;
//using DomainService2.Specs.Fakes;

//namespace DomainService2.Specs
//{
//    public class IOCHostInstantiationBase<T> : WithFakes where T : IX2WorkflowService
//    {
//        protected static IDomainServiceLoader loader;
//        protected static T host;
//        protected static IDomainServiceIOC ioc;
//        protected static IMessageBus messageBus;

//        Establish context = () =>
//            {
//                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("TestUser"), new string[] { });
//                DomainServiceLoader.ProcessName = "NUnitTestMap";
//                loader = new DomainServiceLoader(false);
//                messageBus = An<IMessageBus>();
//                ioc = loader.DomainServiceIOC;
//                ioc.Bind<IMessageBus, FakeMessageBus>();
//            };
//    }
//}