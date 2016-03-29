using Castle.ActiveRecord;
using SAHL.Common.DomainMessages;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Queries;
using SAHL.Web.Views.AffordabilityAssessment.Interfaces;
using System;
using System.Collections.Generic;

namespace SAHL.Web.Views.AffordabilityAssessment.Presenters
{
    public class Add : IncomeContributorsBase
    {
        private int applicationKey;

        public Add(IIncomeContributors view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!View.ShouldRunPage)
            {
                return;
            }

            if (cboNode.GenericKeyTypeKey != (int)SAHL.Common.Globals.GenericKeyTypes.Offer)
            {
                throw new NotSupportedException(String.Format("The current generic key type key is not supported: {0}", cboNode.GenericKeyTypeKey));
            }

            _view.OnSubmitButtonClicked += new EventHandler(OnSubmitButtonClicked);
            applicationKey = cboNode.GenericKey;

            GetApplicantsWithRoleForApplicationQuery getApplicantsWithRoleForApplicationQuery = new GetApplicantsWithRoleForApplicationQuery(applicationKey);
            ISystemMessageCollection systemMessageCollection = applicationDomainService.PerformQuery(getApplicantsWithRoleForApplicationQuery);
            if (!systemMessageCollection.HasErrors)
            {
                if (getApplicantsWithRoleForApplicationQuery.Result != null)
                {
                    _view.BindApplicantContributors(getApplicantsWithRoleForApplicationQuery.Result.Results, new List<int>());

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
                    _view.BindNonApplicantContributors(getNonApplicantsWithRelationshipForApplicationQuery.Result.Results, new List<int>());
                }
            }
            else
            {
                v3ServiceManager.HandleSystemMessages(systemMessageCollection);
            }
        }

        private void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            ValidateUserInput();

            if (_view.IsValid)
            {
                TransactionScope txn = new TransactionScope();
                try
                {
                    AffordabilityAssessmentModel affordabilityAssessmentModel = new AffordabilityAssessmentModel();
                    affordabilityAssessmentModel.GenericKey = applicationKey;
                    affordabilityAssessmentModel.ContributingApplicantLegalEntities = _view.GetContributorsList;
                    affordabilityAssessmentModel.NumberOfContributingApplicants = _view.NumberOfContributingApplicants;
                    affordabilityAssessmentModel.NumberOfHouseholdDependants = _view.NumberOfHouseholdDependants;
                    if (applicationDomainService.AddAffordabilityAssessment(affordabilityAssessmentModel))
                    {
                        txn.VoteCommit();
                        CBOManager.RefreshInstanceNode(_view.CurrentPrincipal);
                        _view.Navigator.Navigate("Submit");
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
    }
}