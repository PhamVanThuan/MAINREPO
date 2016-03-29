//using DomainService2.Hosts.Origination.ApplicationCapture;
//using Machine.Specifications;
//using X2DomainService.Interface.Origination;

//namespace DomainService2.Specs.Hosts.ApplicationCapture
//{
//    [Subject(typeof(ApplicationCaptureHost))]
//    public class When_instantiated_via_ioc : IOCHostInstantiationBase<IApplicationCapture>
//    {
//        Because of = () =>
//            {
//                host = loader.Get<IApplicationCapture>();
//            };

//        It should_have_returned_a_host = () =>
//        {
//            host.ShouldNotBeNull();
//        };
//    }
//}