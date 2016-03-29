//using DomainService2.Hosts.HelpDesk;
//using Machine.Specifications;
//using X2DomainService.Interface.HelpDesk;

//namespace DomainService2.Specs.Hosts.HelpDesk
//{
//    [Subject(typeof(HelpDeskHost))]
//    public class When_instantiated_via_ioc : IOCHostInstantiationBase<IHelpDesk>
//    {
//        Because of = () =>
//            {
//                host = loader.Get<IHelpDesk>();
//            };

//        It should_have_returned_a_host = () =>
//            {
//                host.ShouldNotBeNull();
//            };
//    }
//}