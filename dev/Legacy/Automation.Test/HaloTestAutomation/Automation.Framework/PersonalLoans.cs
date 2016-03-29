using Automation.Framework.DataAccess;
using System.Collections.Generic;
using SAHL.Core.BusinessModel.Enums;

namespace Automation.Framework
{
    public class PersonalLoanWF : WorkflowBase
    {
        /// <summary>
        /// cleans up the application data prior to submitting into credit
        /// </summary>
        /// <param name="keyValue"></param>
        public void PriorToApplicationInOrder(int keyValue)
        {
            base.InsertDeclarations(keyValue, Common.Enums.GenericKeyTypeEnum.ExternalRoleType_ExternalRoleTypeKey, Common.Enums.OriginationSourceProductEnum.SAHomeLoans_PersonalLoan);
            base.InsertITC(keyValue);
            base.InsertOfferDebitOrder(keyValue);
            base.InsertOfferMailingAddress(keyValue);
            base.InsertEmploymentRecords(keyValue);
            base.InsertLegalEntityAffordability(keyValue);
            base.InsertExternalRoleDomicilium(keyValue);
            base.CleanUpLegalEntityData(keyValue);
            base.InsertAffordabilityAssessment((int)AffordabilityAssessmentStatus.Confirmed, keyValue);
        }

        /// <summary>
        /// Inserts a decline reason prior to declining.
        /// </summary>
        /// <param name="keyValue"></param>
        public void PriorToDecline(int keyValue)
        {
            var offerInformationKey = base.GetLatestOfferInformationKey(keyValue);
            base.InsertReason(offerInformationKey, 586);
        }

        public void PriorToNTU(int keyValue)
        {
            base.InsertReason(keyValue, 594);
        }

        /// <summary>
        /// Adds the offer information records to the application for a personal loan
        /// </summary>
        /// <param name="keyValue"></param>
        public void PriorToCreateApplication(int keyValue)
        {
            var parameters = new Dictionary<string, string>() { { "@offerKey", keyValue.ToString() } };
            DataHelper.ExecuteProcedure("test.ConvertPersonalLoanLeadToApplication", parameters);
        }

        /// <summary>
        /// Adds the offer information records to the application for a personal loan
        /// </summary>
        /// <param name="keyValue"></param>
        public void PriorToCreateApplicationNoLifeCover(int keyValue)
        {
            var parameters = new Dictionary<string, string>() { { "@offerKey", keyValue.ToString() }, { "@hasSAHLLife", "0" }, { "@hasExternalLife", "0" } };
            DataHelper.ExecuteProcedure("test.ConvertPersonalLoanLeadToApplication", parameters);
        }
    }
}