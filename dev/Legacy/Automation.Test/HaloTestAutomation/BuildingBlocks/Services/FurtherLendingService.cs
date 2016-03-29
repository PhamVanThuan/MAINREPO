using Automation.DataAccess;
using Automation.DataAccess.DataHelper;
using BuildingBlocks.Services.Contracts;
using Common.Enums;
using System;
using WatiN.Core.Exceptions;

namespace BuildingBlocks.Services
{
    public class FurtherLendingService : _2AMDataHelper, IFurtherLendingService
    {
        /// <summary>
        /// Returns an OfferKey based on the TestGroup for the Readvances/FurtherAdvances and FurtherLoans test groups.
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        public int ReturnFurtherLendingOfferKeyByTestGroup(QueryResults results)
        {
            int offerKey = 0;
            switch (results.Rows(0).Column("TestGroup").Value)
            {
                case "Readvances":
                    offerKey = results.Rows(0).Column("ReadvOfferKey").GetValueAs<int>();
                    break;

                case "FurtherAdvances":
                case "Existing - Further Advance":
                    offerKey = results.Rows(0).Column("FAdvOfferKey").GetValueAs<int>();
                    break;

                case "FurtherLoans":
                case "Existing - Further Loan":
                    offerKey = results.Rows(0).Column("FLOfferKey").GetValueAs<int>();
                    break;
            }
            return offerKey;
        }

        /// <summary>
        /// Loops throught the Credit Offer Roles to find the assigned Credit User. Once found it will update the
        /// FL Automation table with the assigned user and return the CreditADUser to the test for any assertions.
        /// </summary>
        /// <param name="offerKey">OfferKey</param>
        /// <param name="identifier">Test Identifier</param>
        /// <param name="creditADUser">The ADUserName of the user who was assigned the case in Credit</param>
        /// <param name="creditOfferRoleType">The OfferRoleType of the ADUser who was assigned the case in Credit</param>
        public void UpdateAssignedCreditUser(int offerKey, string identifier, out string creditADUser, out OfferRoleTypeEnum creditOfferRoleType)
        {
            OfferRoleTypeEnum[] creditRoles = new OfferRoleTypeEnum[] { OfferRoleTypeEnum.CreditUnderwriterD, OfferRoleTypeEnum.CreditSupervisorD, OfferRoleTypeEnum.CreditManagerD,
                OfferRoleTypeEnum.CreditExceptionsD };
            creditADUser = String.Empty;
            creditOfferRoleType = OfferRoleTypeEnum.Unknown; //String.Empty;

            foreach (OfferRoleTypeEnum roleType in creditRoles)
            {
                var creditUsers = GetActiveOfferRolesByOfferRoleType(offerKey, roleType);
                if (creditUsers.HasResults)
                {
                    creditADUser = creditUsers.Rows(0).Column("adusername").Value;
                    creditOfferRoleType = roleType;
                    creditUsers.Dispose();
                    break;
                }
            }

            if (identifier != null) UpdateFLAutomation("AssignedCreditUser", identifier, creditADUser);
        }

        /// <summary>
        /// This runs the test.GetNewBalanceLAADiffInclTolerance stored procedure which will calculate by how much the further advance
        /// currently exceeds the LAA including the 2% tolerance applied to the LAA's against the Mortgage Loan
        /// </summary>
        /// <param name="offerKey">OfferKey</param>
        /// <returns>Difference</returns>
        public double GetLAAExceededAmountForFurtherAdvance(int offerKey)
        {
            QueryResults r = base.GetNewBalanceLAADiffInclTolerance(offerKey);
            return r.Rows(0).Column("Diff").GetValueAs<double>();
        }

        /// <summary>
        /// returns a further lending offer that is related to a property that has a lightstone property ID.
        /// </summary>
        /// <returns>OfferKey</returns>
        public int GetFurtherLendingOfferWithLightstonePropertyID(bool lightstoneValOlderThan2Months)
        {
            try
            {
                int offerKey = base.GetFurtherLendingOfferWithLightstonePropertyID(lightstoneValOlderThan2Months);
                //check if we got a test case back.
                if (offerKey == 0)
                {
                    throw new WatiNException("No further lending application found for test.");
                }
                return offerKey;
            }
            catch (Exception ex)
            {
                throw new WatiNException(ex.ToString());
            }
        }
    }
}