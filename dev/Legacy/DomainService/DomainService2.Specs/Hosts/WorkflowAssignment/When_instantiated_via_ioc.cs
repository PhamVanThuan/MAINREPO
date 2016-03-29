//using DomainService2.Hosts.WorkflowAssignment;
//using Machine.Specifications;
//using X2DomainService.Interface.WorkflowAssignment;

//namespace DomainService2.Specs.Hosts.WorkflowAssignment
//{
//    [Subject(typeof(WorkflowAssignmentHost))]
//    public class When_instantiated_via_ioc : IOCHostInstantiationBase<IWorkflowAssignment>
//    {
//        Because of = () =>
//            {
//                host = loader.Get<IWorkflowAssignment>();
//            };

//        It should_have_returned_a_host = () =>
//        {
//            host.ShouldNotBeNull();
//        };
//    }
//}