//using DomainService2.Hosts.Life;
//using Machine.Specifications;
//using X2DomainService.Interface.Life;

//namespace DomainService2.Specs.Hosts.Life
//{
//    [Subject(typeof(LifeHost))]
//    public class When_instantiated_via_ioc : IOCHostInstantiationBase<ILife>
//    {
//        Because of = () =>
//            {
//                host = loader.Get<ILife>();
//            };

//        It should_have_returned_a_host = () =>
//        {
//            host.ShouldNotBeNull();
//        };
//    }
//}