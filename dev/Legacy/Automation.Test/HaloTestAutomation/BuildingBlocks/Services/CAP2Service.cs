using Automation.DataAccess;
using Automation.DataAccess.DataHelper;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using NUnit.Framework;
using System;
using System.Linq;

namespace BuildingBlocks.Services
{
    public class CAP2Service : _2AM_CAP2, ICAP2Service
    {
        /// <summary>
        /// Returns the current X2 state for a CAP offer
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        public string GetCAP2WorkflowCurrentState(int accountKey)
        {
            QueryResults results = base.GetLatestCapOfferByAccountKey(accountKey);
            string capOfferKey = results.Rows(0).Column("CapOfferKey").Value;
            results.Dispose();
            results = base.GetLatestCap2X2DataByAccountKeyAndCapOfferKey(accountKey, capOfferKey);
            return results.Rows(0).Column("StateName").Value;
        }

        /// <summary>
        /// Returns the previous state for a CAP offer
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        /// <param name="prevState">Previous State of Instance</param>
        /// <param name="prevStatus">CapStatus</param>
        public void GetCAP2PreviousStateAndStatus(int accountKey, out string prevState, out string prevStatus)
        {
            QueryResults results = base.GetCap2X2Data(accountKey);
            prevState = results.Rows(0).Column("Last_State").Value;
            results.Dispose();
            prevStatus = String.Empty;
            switch (prevState)
            {
                case WorkflowStates.CAP2OffersWF.AwaitingForms:
                    prevStatus = CAPStatus.AwaitingDocuments;
                    break;

                case WorkflowStates.CAP2OffersWF.AwaitingLA:
                    prevStatus = CAPStatus.AwaitingLA;
                    break;

                case WorkflowStates.CAP2OffersWF.OpenCAP2Offer:
                    prevStatus = CAPStatus.Open;
                    break;
            }
        }

        /// <summary>
        /// Returns the account key from the CAP2Tests.csv file when provided with the test identifier
        /// </summary>
        /// <param name="identifier">TestIdentifier from CSV</param>
        public Automation.DataModels.CapOffer GetTestCapOffer(string identifier)
        {
            var capOffer = base.GetTestCapOffer(identifier).FirstOrDefault();
            Assert.That(capOffer.AccountKey > 0, string.Format(@"No CAP test account found for {0}", identifier));
            return capOffer;
        }

        /// <summary>
        /// Retrieves the Expiry Date for a CAP Offer
        /// </summary>
        /// <param name="accountKey">AccountDate</param>
        /// <returns>Expiry Date</returns>
        public string GetCapExpiryDate(int accountKey)
        {
            QueryResults results = base.GetCapTypeConfigurationEndDateForCapOffer(accountKey);
            string expiryDate = results.Rows(0).Column("offerenddate").Value;
            return expiryDate;
        }
    }
}