//using DomainService2.Hosts.Common;
//using Machine.Specifications;
//using X2DomainService.Interface.Common;

//namespace DomainService2.Specs.Hosts.Common
//{
//    [Subject(typeof(CommonHost))]
//    public class When_instantiated_via_ioc : IOCHostInstantiationBase<ICommon>
//    {
//        Because of = () =>
//            {
//                host = loader.Get<ICommon>();
//            };

//        It should_have_returned_a_host = () =>
//        {
//            host.ShouldNotBeNull();
//        };
//    }
//}