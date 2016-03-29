//using DomainService2.Hosts.Origination.ApplicationManagement;
//using Machine.Specifications;
//using X2DomainService.Interface.Origination;

//namespace DomainService2.Specs.Hosts.Origination.ApplicationManagement
//{
//    [Subject(typeof(ApplicationManagementHost))]
//    public class When_instantiated_via_ioc : IOCHostInstantiationBase<IApplicationManagement>
//    {
//        Because of = () =>
//            {
//                host = loader.Get<IApplicationManagement>();
//            };

//        It should_have_returned_a_host = () =>
//        {
//            host.ShouldNotBeNull();
//        };
//    }
//}