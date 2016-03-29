//using DomainService2.Hosts.Cap2;
//using Machine.Specifications;
//using X2DomainService.Interface.Cap2;

//namespace DomainService2.Specs.Hosts.Cap2
//{
//    [Subject(typeof(Cap2Host))]
//    public class When_instantiated_via_ioc : IOCHostInstantiationBase<ICap2>
//    {
//        Because of = () =>
//            {
//                host = loader.Get<ICap2>();
//            };

//        It should_have_returned_a_host = () =>
//        {
//            host.ShouldNotBeNull();
//        };
//    }
//}