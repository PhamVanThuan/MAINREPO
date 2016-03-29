//using DomainService2.Hosts.DebtCounselling;
//using Machine.Specifications;
//using X2DomainService.Interface.DebtCounselling;

//namespace DomainService2.Specs.Hosts.DebtCounselling
//{
//    [Subject(typeof(DebtCounsellingHost))]
//    public class When_instantiated_via_ioc : IOCHostInstantiationBase<IDebtCounselling>
//    {
//        Because of = () =>
//            {
//                host = loader.Get<IDebtCounselling>();
//            };

//        It should_have_returned_a_host = () =>
//        {
//            host.ShouldNotBeNull();
//        };
//    }
//}