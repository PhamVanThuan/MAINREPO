using BuildingBlocks.Services.Contracts;
using Common.Constants;
using NUnit.Framework;
using ObjectMaps.Pages;
using System;

namespace BuildingBlocks.Presenters.PersonalLoans
{
    public class CreditProtectionSummary : CreditProtectionSummaryControls
    {
        public CreditProtectionSummary()
        {
        }
        
        public void AssertFieldValues(Automation.DataModels.Account lifePolicy, double PersonalLoanSumInsured)
        {
            int policyNumber = Convert.ToInt32(base.PolicyNumber.InnerHtml);
            var openDate = base.OpenDate.InnerHtml;
            string accountStatus = base.Status.InnerHtml.ToString();
            double lifePolicyPremium = Convert.ToDouble(Common.Extensions.StringExtensions.CleanCurrencyString(base.LifePolicyPremium.InnerHtml, false));
            double sumInsured = Convert.ToDouble(Common.Extensions.StringExtensions.CleanCurrencyString(base.SumInsured.InnerHtml, false));

            Assert.AreEqual(lifePolicy.AccountKey, policyNumber);
            Assert.AreEqual(lifePolicy.OpenDate.ToString(Formats.DateFormat), openDate);
            Assert.AreEqual(lifePolicy.AccountStatusKey.ToString(), accountStatus);
            Assert.AreEqual(lifePolicy.TotalInstalment, lifePolicyPremium);
            Assert.AreEqual(PersonalLoanSumInsured, sumInsured);
        }
    }
}