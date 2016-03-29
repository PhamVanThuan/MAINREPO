using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Fakes;
using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Resubmit_to_Credit.OnComplete
{
    [Subject("Activity => Resubmit_to_Credit => OnComplete")]
    internal class when_resub_not_perform_before_not_require_val : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static ICommon commonClient;
        private static IApplicationManagement applicationManagementClient;

        Establish context = () =>
        {
            commonClient = An<ICommon>();
            applicationManagementClient = An<IApplicationManagement>();
            domainServiceLoader.RegisterMockForType<ICommon>(commonClient);
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(applicationManagementClient);

            workflowData.RequireValuation = false;
            workflowData.IsFL = false;
            workflowData.IsResub = false;

            //Mock ValidateApplication to return true
            applicationManagementClient.WhenToldTo(x => x.ValidateApplication((IDomainMessageCollection)messages, workflowData.ApplicationKey,
                    paramsData.IgnoreWarning)).Return(true);

            //Mock RefreshApplicationDocumentCheckList to return true
            applicationManagementClient.WhenToldTo(x => x.RefreshApplicationDocumentCheckList(
                                                         messages, workflowData.ApplicationKey)).Return(true);

            //Mock ValidateApplicationApplicants to return true
            applicationManagementClient.WhenToldTo(x => x.ValidateApplicationApplicants((IDomainMessageCollection)messages,
                                                                          workflowData.ApplicationKey,
                                                                                paramsData.IgnoreWarning)).Return(true);

            //Mock CheckApplicationInOrderRules to return true
            applicationManagementClient.WhenToldTo(x => x.CheckApplicationInOrderRules((IDomainMessageCollection)messages,
                                                                          workflowData.ApplicationKey,
                                                                                paramsData.IgnoreWarning)).Return(true);
        };

        Because of = () =>
        {
            result = workflow.OnCompleteActivity_Resubmit_to_Credit(
                messages, workflowData, instanceData, paramsData, ref message);
        };

        It should_not_check_valuation_required_rules = () =>
        {
            applicationManagementClient.WasNotToldTo(
               x => x.CheckValuationRequiredRules((IDomainMessageCollection)messages, workflowData.ApplicationKey, paramsData.IgnoreWarning));
        };
    }
}