using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Origination.Workflow.PreCredit.QA
{
    [RequiresSTA]
    public class QARequestResolvedTests : Origination.OriginationTestBase<BasePage>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new BuildingBlocks.TestBrowser(TestUsers.BranchConsultant);
        }
        protected override void OnTestStart()
        {
        }
        /// <summary>
        /// The branch consultant user returns the case the QA Admin user after resolving the QA Query by performing the Request Resolved action at the QA Query state.
        /// This test will ensure that the case is moved back to the QA state and that the correct QA Admin role is assigned to the application. If the current QA admin on the
        /// application is no longer an active AD User we will fetch the next QA Admin who is due to be assigned the case via round robin.
        /// </summary>
        [Test, Description("This ensures that a requested QAQuery can be resolved")]
        public void QARequestResolved()
        {
            base.GetOfferKeyAtStateAndSearchForCase(WorkflowStates.ApplicationManagementWF.RequestatQA, OfferTypeEnum.NewPurchase);
            string date = DateTime.Now.ToString(Formats.DateTimeFormatSQL);
            //Update ITC details for all legalentities
            Service<ILegalEntityService>().InsertITC(base.TestCase.OfferKey, 999, 600);
            //give them all domicilium addresses
            Service<IApplicationService>().CleanUpOfferDomicilium(base.TestCase.OfferKey);
            //get the QA user
            string adUserName = base.Service<IApplicationService>().GetADUserByActiveOfferRoles(base.TestCase.OfferKey, OfferRoleTypeEnum.QAAdministratorD);
            if (!base.Service<IADUserService>().IsADUserActive(adUserName))
            {
                adUserName = Service<IAssignmentService>().GetNextRoundRobinAssignmentByOfferRoleTypeKey(OfferRoleTypeEnum.QAAdministratorD,
                    RoundRobinPointerEnum.QAAdministrator);
            }
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.RequestResolved);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            base.Browser.Page<BasePage>().DomainWarningClickYes();
            Service<IX2WorkflowService>().WaitForX2State(base.TestCase.OfferKey, Workflows.ApplicationManagement, WorkflowStates.ApplicationManagementWF.QA);
            X2Assertions.AssertCurrentAppManX2State(base.TestCase.OfferKey, WorkflowStates.ApplicationManagementWF.QA);
            AssignmentAssertions.AssertWorkflowAssignment(adUserName, base.TestCase.OfferKey, OfferRoleTypeEnum.QAAdministratorD);
        }
    }
}