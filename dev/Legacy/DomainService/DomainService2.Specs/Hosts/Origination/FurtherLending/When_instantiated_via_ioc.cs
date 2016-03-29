//using DomainService2.Hosts.Origination.FurtherLending;
//using Machine.Specifications;
//using X2DomainService.Interface.Origination;

//namespace DomainService2.Specs.Hosts.Origination.FurtherLending
//{
//    [Subject(typeof(FurtherLendingHost))]
//    public class When_instantiated_via_ioc : IOCHostInstantiationBase<IFL>
//    {
//        Because of = () =>
//            {
//                host = loader.Get<IFL>();
//            };

//        It should_have_returned_a_host = () =>
//        {
//            host.ShouldNotBeNull();
//        };
//    }
//}