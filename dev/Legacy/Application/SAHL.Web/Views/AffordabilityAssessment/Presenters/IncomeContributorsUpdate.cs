using Castle.ActiveRecord;
using SAHL.Common.DomainMessages;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Queries;
using SAHL.Web.Views.AffordabilityAssessment.Interfaces;
using System;
using System.Linq;

namespace SAHL.Web.Views.AffordabilityAssessment.Presenters
{
    public class IncomeContributorsUpdate : IncomeContributorsBase
    {
        private int affordabilityAssessmentKey;
        private AffordabilityAssessmentModel assessment;

        public IncomeContributorsUpdate(IIncomeContributors view, SAHLCommonBaseController controller)
            : base(view, controller)
        { }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!View.ShouldRunPage)
            {
                return;
            }

            if (cboNode.GenericKeyTypeKey != (int)SAHL.Common.Globals.GenericKeyTypes.AffordabilityAssessment)
            {
                throw new NotSupportedException(String.Format("The current generic key type key is not supported: {0}", cboNode.GenericKeyTypeKey));
            }

            _view.OnSubmitButtonClicked += new EventHandler(OnSubmitButtonClicked);

            affordabilityAssessmentKey = cboNode.GenericKey;

            GetAffordabilityAssessmentByKeyQuery getAssessmentQuery = new GetAffordabilityAssessmentByKeyQuery(affordabilityAssessmentKey);
            ISystemMessageCollection systemMessageCollection = applicationDomainService.PerformQuery(getAssessmentQuery);

            assessment = getAssessmentQuery.Result.Results.FirstOrDefault();
            if (systemMessageCollection.HasErrors)
            {
                v3ServiceManager.HandleSystemMessages(systemMessageCollection);
            }

            int applicationKey = assessment.GenericKey;

            _view.NumberOfContributingApplicants = assessment.NumberOfContributingApplicants;
            _view.NumberOfHouseholdDependants = assessment.NumberOfHouseholdDependants;

            GetApplicantsWithRoleForApplicationQuery getApplicantsWithRoleForApplicationQuery = new GetApplicantsWithRoleForApplicationQuery(applicationKey);
            systemMessageCollection = applicationDomainService.PerformQuery(getApplicantsWithRoleForApplicationQuery);
            if (!systemMessageCollection.HasErrors)
            {
                if (getApplicantsWithRoleForApplicationQuery.Result != null)
                {
                    _view.BindApplicantContributors(getApplicantsWithRoleForApplicationQuery.Result.Results, assessment.ContributingApplicantLegalEntities);
                    _view.BindLegalEntityDropdownList(getApplicantsWithRoleForApplicationQuery.Result.Results);
                }
            }
            else
            {
                v3ServiceManager.HandleSystemMessages(systemMessageCollection);
            }

            GetNonApplicantsWithRelationshipForApplicationQuery getNonApplicantsWithRelationshipForApplicationQuery = new GetNonApplicantsWithRelationshipForApplicationQuery(applicationKey);
            systemMessageCollection = applicationDomainService.PerformQuery(getNonApplicantsWithRelationshipForApplicationQuery);
            if (!systemMessageCollection.HasErrors)
            {
                if (getNonApplicantsWithRelationshipForApplicationQuery.Result != null)
                {
                    _view.BindNonApplicantContributors(getNonApplicantsWithRelationshipForApplicationQuery.Result.Results, assessment.ContributingApplicantLegalEntities);
                }
            }
            else
            {
                v3ServiceManager.HandleSystemMessages(systemMessageCollection);
            }
        }

        private void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            if (_view.IsValid == false)
                return;

            ValidateUserInput();
            ValidateAffordabilityAssessmentStatus();

            if (View.IsValid)
            {
                TransactionScope txn = new TransactionScope();
                try
                {
                    assessment.ContributingApplicantLegalEntities = _view.GetContributorsList;
                    assessment.NumberOfContributingApplicants = _view.NumberOfContributingApplicants;
                    assessment.NumberOfHouseholdDependants = _view.NumberOfHouseholdDependants;
                    if (applicationDomainService.AmendAffordabilityAssessmentIncomeContributors(assessment))
                    {
                        txn.VoteCommit();
                        CBOManager.RefreshInstanceNode(_view.CurrentPrincipal);

                        if (assessment.AffordabilityAssessmentStatus == AffordabilityAssessmentStatus.Confirmed)
                        {
                            CBOManager.SetSelectedNodeByKeyAndDescription(_view.CurrentPrincipal, SAHL.Common.CBONodeSetType.X2, assessment.GenericKey, "Affordability Assessments");
                            Navigator.Navigate("AffordabilityAssessmentSummary");
                        }
                        else
                        {
                            Navigator.Navigate("Submit");
                        }
                    }
                    else
                    {
                        txn.VoteRollBack();
                    }
                }
                catch (Exception)
                {
                    txn.VoteRollBack();
                    if (_view.IsValid)
                        throw;
                }
                finally
                {
                    txn.Dispose();
                }
            }
        }

        private bool ValidateAffordabilityAssessmentStatus()
        {
            // warn user if they are saving a confirmed assessment that a copy will be made.
            if (assessment.AffordabilityAssessmentStatus == AffordabilityAssessmentStatus.Confirmed)
            {
                string errorMessage = "Confirmed Assessments cannot be updated. If you continue, a new Unconfirmed copy will be made and this Assessment will be Archived.";
                _view.Messages.Add(new Warning(errorMessage, errorMessage));
                return false;
            }

            return true;
        }
    }
}