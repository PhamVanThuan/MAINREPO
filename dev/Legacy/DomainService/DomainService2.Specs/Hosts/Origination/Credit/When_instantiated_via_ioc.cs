//using DomainService2.Hosts.Origination.Credit;
//using Machine.Specifications;
//using X2DomainService.Interface.Origination;

//namespace DomainService2.Specs.Hosts.Origination.Credit
//{
//    [Subject(typeof(CreditHost))]
//    public class When_instantiated_via_ioc : IOCHostInstantiationBase<ICredit>
//    {
//        Because of = () =>
//            {
//                host = loader.Get<ICredit>();
//            };

//        It should_have_returned_a_host = () =>
//        {
//            host.ShouldNotBeNull();
//        };
//    }
//}