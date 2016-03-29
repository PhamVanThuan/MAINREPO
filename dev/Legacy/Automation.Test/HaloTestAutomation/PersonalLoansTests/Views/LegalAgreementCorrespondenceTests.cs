using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System.Linq;

namespace PersonalLoansTests.Views
{
    [RequiresSTA]
    public class LegalAgreementCorrespondenceTests : PersonalLoansWorkflowTestBase<CorrespondenceProcessing>
    {
        private int legalEntityKey;

        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.FindCaseAtState(WorkflowStates.PersonalLoansWF.LegalAgreements, WorkflowRoleTypeEnum.PLConsultantD);
            this.legalEntityKey = (from le in base.Service<IExternalRoleService>().GetActiveExternalRoleList(base.GenericKey, GenericKeyTypeEnum.Offer_OfferKey,
                                                                         ExternalRoleTypeEnum.Client)
                                   select le.LegalEntityKey).FirstOrDefault();
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.SendDocuments);
        }

        [Test]
        public void when_send_legal_agreement_should_display_correspondence_screen()
        {
            base.View.AssertPostAvailable();
            base.View.AssertFaxAvailable();
            base.View.AssertEmailAvailable();
            base.View.AssertLegalEntityExistsOnCorrespondence(legalEntityKey);
        }

        [Test]
        public void when_legal_agreement_with_email_option_should_send_using_email_address()
        {
            base.View.SendCorrespondence(CorrespondenceSendMethodEnum.Email);
            BuildingBlocks.Assertions.CorrespondenceAssertions.AssertCorrespondenceRecordAdded(base.GenericKey, CorrespondenceReports.PersonalLoanLegalAgreement, CorrespondenceMedium.Email);
            CorrespondenceAssertions.AssertImageIndex
                (base.GenericKey.ToString(), CorrespondenceReports.PersonalLoanLegalAgreement, CorrespondenceMedium.Email, 0, 0);
        }

        [Test]
        public void when_send_legal_agreement_with_fax_option_should_send_using_fax_number()
        {
            base.View.SendCorrespondence(CorrespondenceSendMethodEnum.Fax);
            BuildingBlocks.Assertions.CorrespondenceAssertions.AssertCorrespondenceRecordAdded(base.GenericKey, CorrespondenceReports.PersonalLoanLegalAgreement, CorrespondenceMedium.Fax);
            CorrespondenceAssertions.AssertImageIndex
                  (base.GenericKey.ToString(), CorrespondenceReports.PersonalLoanLegalAgreement, CorrespondenceMedium.Fax, 0, 0);
        }

        [Test]
        public void when_send_legal_agreement_with_post_option_should_send_using_mail_address()
        {
            base.View.SendCorrespondence(CorrespondenceSendMethodEnum.Post);
            CorrespondenceAssertions.AssertCorrespondenceRecordAdded(base.GenericKey, CorrespondenceReports.PersonalLoanLegalAgreement, CorrespondenceMedium.Post);
            CorrespondenceAssertions.AssertImageIndex
                 (base.GenericKey.ToString(), CorrespondenceReports.PersonalLoanLegalAgreement, CorrespondenceMedium.Post, 0, 0);
        }
    }
}