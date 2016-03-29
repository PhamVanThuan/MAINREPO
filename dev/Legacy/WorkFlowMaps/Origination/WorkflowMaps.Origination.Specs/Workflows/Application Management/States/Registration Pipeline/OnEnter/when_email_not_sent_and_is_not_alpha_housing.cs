using Machine.Fakes;
using Machine.Specifications;
using Rhino.Mocks;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.ApplicationManagement.Specs.States.Registration_Pipeline.OnEnter
{
    [Subject("State => Registration_Pipeline => OnEnter")]
    internal class when_email_not_sent_and_is_not_alpha_housing : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static bool isAlpha;
        private static IApplicationManagement appMan;

        private Establish context = () =>
        {
            result = false;
            workflowData.AlphaHousingSurveyEmailSent = false;
            
            appMan = An<IApplicationManagement>();
            appMan.Expect(x => x.SendAlphaHousingSurveyEmail((IDomainMessageCollection)messages, workflowData.ApplicationKey, paramsData.ADUserName, out isAlpha)).OutRef(false);
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(appMan);
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Registration_Pipeline(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };

        private It should_set_email_sent_flag_true = () =>
        {
            workflowData.AlphaHousingSurveyEmailSent.ShouldBeFalse();
        };

        private It should_make_call_to_send_alpha_housing_survey = () =>
        {
            appMan.WasToldTo(x => x.SendAlphaHousingSurveyEmail((IDomainMessageCollection)messages, workflowData.ApplicationKey, paramsData.ADUserName, out isAlpha));
        };
    }
}
