using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Managers.Applicant
{
    public class ApplicantManager : IApplicantManager
    {
        private IApplicantDataManager applicantDataManager;

        public ApplicantManager(IApplicantDataManager applicationDataManager)
        {
            this.applicantDataManager = applicationDataManager;
        }

        public void AddIncomeContributorOfferRoleAttribute(int offerRoleKey)
        {
            var offerRoleAttribute = new OfferRoleAttributeDataModel(offerRoleKey, (int)OfferRoleAttributeType.IncomeContributor);
            applicantDataManager.AddOfferRoleAttribute(offerRoleAttribute);
        }

        public bool IsApplicantAnIncomeContributor(int applicationRoleKey)
        {
            var applicantAttributes = applicantDataManager.GetApplicantAttributes(applicationRoleKey);
            var isIncomeContributor = applicantAttributes.Where(x => x.OfferRoleAttributeTypeKey == (int)OfferRoleAttributeType.IncomeContributor).Any();
            return isIncomeContributor;
        }
    }
}