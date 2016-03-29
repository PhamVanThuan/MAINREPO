using SAHL.Core.BusinessModel.Enums;
using System;
using System.Collections.Generic;

namespace SAHL.Services.Interfaces.ApplicationDomain.Models
{
    public class AffordabilityAssessmentModel
    {
        public AffordabilityAssessmentModel()
        {
        }

        public AffordabilityAssessmentModel(int key,
                                            int genericKey,
                                            AffordabilityAssessmentStatus affordabilityAssessmentStatus,
                                            DateTime dateLastAmended,
                                            int numberOfHouseholdDependants,
                                            int numberOfContributingApplicants,
                                            int stressFactorKey,
                                            IEnumerable<int> contributingApplicantLegalEntities,
                                            AffordabilityAssessmentDetailModel affordabilityAssessmentDetail,
                                            DateTime? confirmedDate)
        {
            this.Key = key;
            this.GenericKey = genericKey;
            this.StressFactorKey = stressFactorKey;
            this.AffordabilityAssessmentStatus = affordabilityAssessmentStatus;
            this.DateLastAmended = dateLastAmended;
            this.ContributingApplicantLegalEntities = contributingApplicantLegalEntities;
            this.NumberOfHouseholdDependants = numberOfHouseholdDependants;
            this.NumberOfContributingApplicants = numberOfContributingApplicants;
            this.AffordabilityAssessmentDetail = affordabilityAssessmentDetail;
            this.ConfirmedDate = confirmedDate;
        }

        public int Key { get; set; }

        public int GenericKey { get; set; }

        public AffordabilityAssessmentStatus AffordabilityAssessmentStatus { get; set; }

        public DateTime DateLastAmended { get; set; }

        public IEnumerable<int> ContributingApplicantLegalEntities { get; set; }

        public int NumberOfContributingApplicants { get; set; }

        public int NumberOfHouseholdDependants { get; set; }

        public int StressFactorKey { get; set; }

        public AffordabilityAssessmentDetailModel AffordabilityAssessmentDetail { get; set; }

        public DateTime? ConfirmedDate { get; set; }
    }
}