using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Providers;
using System;
using SAHL.Core.Services;

namespace SAHL.X2Engine2.Specs.X2RequestInterrogatorSpecs
{
    public class when_getting_a_workflow_for_an_orphaned_security_recalc_instance : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2RequestInterrogator> autoMocker;

        private static X2Workflow requestWorkflow;
        private static X2RequestForSecurityRecalc requestForSecurityRecalc;
        private static InstanceDataModel nullInstanceDataModel;
        private static IServiceRequestMetadata metadata;
        private Establish context = () =>
        {
            metadata = An<IServiceRequestMetadata>();
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2RequestInterrogator>();
            nullInstanceDataModel = null;
            requestForSecurityRecalc = new X2RequestForSecurityRecalc(Guid.NewGuid(), Param.IsAny<long>(), metadata);

            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetInstanceDataModel(Param.IsAny<long>())).Return(nullInstanceDataModel);
        };

        private Because of = () =>
        {
            requestWorkflow = autoMocker.ClassUnderTest.GetRequestWorkflow(requestForSecurityRecalc);
        };

        private It should_not_find_the_instance_in_the_database = () =>
        {
            autoMocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetInstanceDataModel(requestForSecurityRecalc.InstanceId));
        };

        private It should_return_a_workflow_with_a_blank_workflow_name_for_the_given_request = () =>
        {
            requestWorkflow.WorkflowName.ShouldEqual("");
        };

        private It should_return_a_workflow_with_a_blank_process_name_for_the_given_request = () =>
        {
            requestWorkflow.ProcessName.ShouldEqual("");
        };
    }
}