using Automation.DataAccess.DataHelper;
using BuildingBlocks.Services.Contracts;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace BuildingBlocks.Services
{
    public class PersonalLoanService : _2AMDataHelper, IPersonalLoanService
    {
        public void RemoveCreditLifePolicyFromPersonalLoanOffer(ref Automation.DataModels.PersonalLoanApplication personalLoanApplication)
        {
            base.RemoveCreditLifePolicyFromPersonalLoanOffer(personalLoanApplication.OfferKey);
            personalLoanApplication.CreditLifeTakenUp = 0;
            personalLoanApplication.LifePremium = 0;
        }

        public List<string> WaitUntilAllBatchLeadsHaveBeenCreated(int expectedNumberOfCases)
        {
            var messages = base.GetLatestBatchServiceGenericMessages();
            List<string> successfulCases = new List<string>();
			bool atLeastOneFailed = false;
            while ((successfulCases.Count < expectedNumberOfCases) && !atLeastOneFailed)
            {
				messages = base.GetLatestBatchServiceGenericMessages();
                foreach (var item in messages)
                {
                    JToken token = JObject.Parse(item.MessageContent);
                    string idNumber = (string)token.SelectToken("IdNumber");
                    int failureCount = (int)token.SelectToken("FailureCount");
                    if (failureCount.Equals(0) && item.GenericStatusID == 2)
                    {
                        if (!successfulCases.Contains(idNumber))
                            successfulCases.Add(idNumber);
                    }
					else if (item.GenericStatusID == 3)
					{
						atLeastOneFailed = true;
						break;
					}
                    else if (item.GenericStatusID == 4)
                    {
                        atLeastOneFailed = true;
                        break;
                    }
                }
            }
            return successfulCases;
        }

        public bool CheckLegalEntityHasValidPersonalLoanApplication(string idNumber, string workflowState)
        {
            return base.GetPersonalLoanApplicationByApplicantsIdNumber(idNumber, workflowState).HasResults;
        }

    }
}