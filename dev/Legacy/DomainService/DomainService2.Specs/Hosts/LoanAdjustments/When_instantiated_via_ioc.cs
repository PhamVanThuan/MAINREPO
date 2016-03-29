//using DomainService2.Hosts.LoanAdjustments;
//using Machine.Specifications;
//using X2DomainService.Interface.LoanAdjustments;

//namespace DomainService2.Specs.Hosts.LoanAdjustments
//{
//    [Subject(typeof(LoanAdjustmentsHost))]
//    public class When_instantiated_via_ioc : IOCHostInstantiationBase<ILoanAdjustments>
//    {
//        Because of = () =>
//            {
//                host = loader.Get<ILoanAdjustments>();
//            };

//        It should_have_returned_a_host = () =>
//        {
//            host.ShouldNotBeNull();
//        };
//    }
//}