using Castle.ActiveRecord;
using SAHL.Common.CacheData;
using SAHL.Common.DomainMessages;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ApplicationDomain.Queries;
using SAHL.V3.Framework;
using SAHL.V3.Framework.DomainServices;
using SAHL.Web.Views.AffordabilityAssessment.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using SAHL.Services.Interfaces.ApplicationDomain.Models;

namespace SAHL.Web.Views.AffordabilityAssessment.Presenters
{
    public class Link : AddUnlinkLegalEntityBase
    {
        private int affordabilityAssessmentKey;

        public Link(IAddUnlinkLegalEntity view, SAHLCommonBaseController controller)
            : base(view, controller)
        { }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!View.ShouldRunPage)
                return;

            v3ServiceManager = V3ServiceManager.Instance;
            applicationDomainService = v3ServiceManager.Get<IApplicationDomainService>();

            _view.OnAddContributorButtonClicked += new EventHandler(OnAddContributorButtonClicked);
            _view.OnCancelButtonClicked += new EventHandler(OnCancelButtonClicked);
            _view.OnSubmitButtonClicked += new EventHandler(OnSubmitButtonClicked);

            if (GlobalCacheData.ContainsKey(ViewConstants.AffordabilityAssessmentKey))
            {
                GlobalCacheData.Remove(ViewConstants.AffordabilityAssessmentKey);
            }

            CBOMenuNode cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (cboNode != null)
            {
                affordabilityAssessmentKey = cboNode.GenericKey;

                GetAffordabilityAssessmentByKeyQuery getAssessmentQuery = new GetAffordabilityAssessmentByKeyQuery(affordabilityAssessmentKey);
                ISystemMessageCollection systemMessageCollection = applicationDomainService.PerformQuery(getAssessmentQuery);

                AffordabilityAssessmentModel assessment = getAssessmentQuery.Result.Results.FirstOrDefault();
                if (systemMessageCollection.HasErrors)
                {
                    v3ServiceManager.HandleSystemMessages(systemMessageCollection);
                }

                int applicationKey = assessment.GenericKey;

                _view.NumberOfContributingApplicants = assessment.NumberOfContributingApplicants;
                _view.NumberOfHouseholdDependants = assessment.NumberOfhouseholdDependants;

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
        }

        private void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            if (_view.IsValid == false)
                return;

            if (ValidateUserInput())
            {
                TransactionScope txn = new TransactionScope();
                try
                {

                   if (applicationDomainService.AmendAffordabilityAssessmentApplicants(affordabilityAssessmentKey, _view.GetContributorsList, _view.NumberOfContributingApplicants, _view.NumberOfHouseholdDependants))
                    {
                        txn.VoteCommit();
                        CBOManager.RefreshInstanceNode(_view.CurrentPrincipal);
                        _view.Navigator.Navigate("Submit");
                    }
                    else
                    {
                        txn.VoteRollBack();
                        errorMessage = "Link/Unlink assessment applicant failed.";
                        _view.Messages.Add(new Error(errorMessage, errorMessage));
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