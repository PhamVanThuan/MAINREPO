using Automation.DataAccess;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WatiN.Core;
using WatiN.Core.Logging;

namespace BuildingBlocks.Assertions
{
    public static class LifeAssertions
    {
        private static readonly ILifeService lifeService;
        private static readonly IX2WorkflowService x2Service;
        private static readonly ILegalEntityService legalEntityService;
        private static readonly IAccountService accountService;

        static LifeAssertions()
        {
            lifeService = ServiceLocator.Instance.GetService<ILifeService>();
            x2Service = ServiceLocator.Instance.GetService<IX2WorkflowService>();
            legalEntityService = ServiceLocator.Instance.GetService<ILegalEntityService>();
            accountService = ServiceLocator.Instance.GetService<IAccountService>();
        }

        /// <summary>
        /// This assertion will fetch the current state of an life instance and then compare the current
        /// state with an expected state provided by the test
        /// </summary>
        /// <param name="offerkey">OfferKey</param>
        /// <param name="expectedState">The Expected State of the Application in life</param>
        public static bool AssertCurrentLifeX2State(string offerkey, string expectedState)
        {
            var creationDate = DateTime.Now.Subtract(TimeSpan.FromMinutes(5));
            var lifeLead = x2Service.GetLifeInstanceDetails(Int32.Parse(offerkey), -1)
                                .Where(x => x.ParentInstanceID == null).FirstOrDefault();

            BuildingBlocks.Timers.GeneralTimer.BlockWaitFor(1000, 10, () =>
                {
                    lifeLead = x2Service.GetLifeInstanceDetails(Int32.Parse(offerkey), -1)
                                .Where(x => x.ParentInstanceID == null).FirstOrDefault();
                    return lifeLead != null;
                });

            //the current state should match the expected state
            Logger.LogAction("Asserting that Application " + offerkey + " is at the " + expectedState + " workflow state");
            StringAssert.AreEqualIgnoringCase(expectedState, lifeLead.StateName);
            if (lifeLead.StateName == expectedState)
                return true;
            return false;
        }

        /// <summary>
        /// This assertion will assert the life policy type of a life offer
        /// </summary>
        /// <param name="offerKey">Life offerkey of a life policy lead</param>
        /// <param name="expectedLifePolicyType">Expected Life Policy</param>
        public static void AssertLifeOfferPolicyType(int offerKey, string expectedLifePolicyType)
        {
            QueryResults results = lifeService.GetLifePolicyType(offerKey);
            string type = results.Rows(0).Column("PolicyType").Value;
            Logger.LogAction("Asserting that the life policy type for offer: " + offerKey + ", is of type: " + expectedLifePolicyType);
            StringAssert.AreEqualIgnoringCase(expectedLifePolicyType, type);
            results.Dispose();
        }

        /// <summary>
        /// Assert that the provided Postal Address exist for the given offer.
        /// </summary>
        /// <param name="offerKey">Life OfferKey</param>
        /// <param name="postalBoxAddress">LegalEntityAddressDetails.PostalBoxAddress</param>
        public static void AssertLifeAddressExist(int offerKey, Automation.DataModels.DebtCounselling postalBoxAddress)
        {
            bool addressExist = false;
            List<bool> addressCheck = new List<bool>();
            var results = lifeService.GetLifeLegalEntityAddresses(offerKey);
            foreach (QueryResultsRow row in results.RowList)
            {
                addressCheck.Clear();
                foreach (FieldInfo field in postalBoxAddress.GetType().GetFields())
                {
                    string fieldValue = field.GetValue(postalBoxAddress).ToString();
                    string rowValue = row.Column(field.Name).Value;
                    if (fieldValue.ToLower() == rowValue.ToLower())
                        addressCheck.Add(true);
                    if (fieldValue.ToLower() != rowValue.ToLower())
                        addressCheck.Add(false);
                }
                //if no false then it means all the field values matched those in the database.
                if (!addressCheck.Contains(false))
                {
                    addressExist = true;
                    break;
                }
            }
            Logger.LogAction("Asserting that the given postal address exist on at least one legal entity linked to an offer");
            Assert.True(addressExist, "PostalBox address doesn't exist");
        }

        /// <summary>
        /// Assert that the provided Residential Address exist for the given offer.
        /// </summary>
        /// <param name="offerKey">Life OfferKey</param>
        /// <param name="residentialStreetAddress">Residential address detail</param>
        public static void AssertLifeAddressExist(int offerKey, Automation.DataModels.Address residentialStreetAddress)
        {
            bool addressExist = false;
            List<bool> addressCheck = new List<bool>();
            var results = lifeService.GetLifeLegalEntityAddresses(offerKey);
            foreach (QueryResultsRow row in results.RowList)
            {
                addressCheck.Clear();
                foreach (FieldInfo field in residentialStreetAddress.GetType().GetFields())
                {
                    string fieldValue = field.GetValue(residentialStreetAddress).ToString();
                    string rowValue = row.Column(field.Name).Value;
                    if (fieldValue.ToLower() == rowValue.ToLower())
                        addressCheck.Add(true);
                    if (fieldValue.ToLower() != rowValue.ToLower())
                        addressCheck.Add(false);
                }
                //if no false then it means all the field values matched those in the database.
                if (!addressCheck.Contains(false))
                {
                    addressExist = true;
                    break;
                }
            }
            Logger.LogAction("Asserting that the given residential address exist on at least one legal entity linked to an offer");
            Assert.True(addressExist, "Residential address doesn't exist");
        }

        /// <summary>
        /// This assertion will check if the specified legal entity plays a role in the account.
        /// </summary>
        /// <param name="idnumber">legal entity idnumber</param>
        /// <param name="offerkey">life lead offerkey</param>
        public static void AssertRoleExistForLifeAccount(string idnumber, int offerkey)
        {
            bool assertRoleExist = false;
            QueryResults results = lifeService.GetLegalEntitiesFromLifeAccountRoles(offerkey);
            foreach (QueryResultsRow row in results.RowList)
            {
                if (row.Column("IDNumber").Value == idnumber)
                    assertRoleExist = true;
            }
            Logger.LogAction("Asserting that the given legalentity does play a role in the life account.");
            Assert.True(assertRoleExist, "Role for legalentity ({1}), exist on life offer: {0}", offerkey, idnumber);
        }

        /// <summary>
        /// This method will assert that the contact details on a legal entity, match the ones provided.
        /// </summary>
        /// <param name="TestBrowser"></param>
        /// <param name="offerKey"></param>
        /// <param name="leidNumber"></param>
        /// <param name="homePhoneCode"></param>
        /// <param name="homePhoneNumber"></param>
        /// <param name="workPhoneCode"></param>
        /// <param name="workPhoneNumber"></param>
        /// <param name="faxCode"></param>
        /// <param name="faxNumber"></param>
        /// <param name="cellPhoneNumber"></param>
        /// <param name="emailAddress"></param>
        public static void AssertLegalEntityContactDetail(TestBrowser TestBrowser, string offerKey, string leidNumber, string homePhoneCode,
                    string homePhoneNumber, string workPhoneCode, string workPhoneNumber, string faxCode, string faxNumber, string cellPhoneNumber,
                    string emailAddress)
        {
            bool contactDetailsExist = false;
            QueryResults results = lifeService.GetLegalEntitiesFromLifeAccountRoles(int.Parse(offerKey));
            List<bool> contactDetailCheck = new List<bool>();
            foreach (QueryResultsRow row in results.RowList)
            {
                contactDetailCheck.Clear();
                //HomePhone
                if (row.Column("HomePhoneCode").Value == homePhoneCode)
                    contactDetailCheck.Add(true);
                else
                    contactDetailCheck.Add(false);
                if (row.Column("HomePhoneNumber").Value == homePhoneNumber)
                    contactDetailCheck.Add(true);
                else
                    contactDetailCheck.Add(false);
                //WorkPhone
                if (row.Column("WorkPhoneCode").Value == workPhoneCode)
                    contactDetailCheck.Add(true);
                else
                    contactDetailCheck.Add(false);

                if (row.Column("WorkPhoneNumber").Value == workPhoneNumber)
                    contactDetailCheck.Add(true);
                else
                    contactDetailCheck.Add(false);

                //Fax
                if (row.Column("FaxCode").Value == faxCode)
                    contactDetailCheck.Add(true);
                else
                    contactDetailCheck.Add(false);

                if (row.Column("FaxNumber").Value == faxNumber)
                    contactDetailCheck.Add(true);
                else
                    contactDetailCheck.Add(false);

                //Cellphone
                if (row.Column("CellPhoneNumber").Value == cellPhoneNumber)
                    contactDetailCheck.Add(true);
                else
                    contactDetailCheck.Add(false);

                //EmailAddress
                if (row.Column("EmailAddress").Value == emailAddress)
                    contactDetailCheck.Add(true);
                else
                    contactDetailCheck.Add(false);
                if (!contactDetailCheck.Contains(false))
                {
                    contactDetailsExist = true;
                }
            }
            Logger.LogAction("Asserting that legal entity (ID:{0}) was udpated on offer {1}.", leidNumber, offerKey);
            Assert.True(contactDetailsExist, "Contact details for legal entity (ID:{0}) was not updated on offer: {1}", leidNumber, offerKey);
        }

        /// <summary>
        /// This method will assert that the policy status in the given row match the LifePolicyStatus expected.
        /// </summary>
        /// <param name="row">Single row from the QueryResults set.</param>
        /// <param name="expectedStatus"></param>
        public static void AssertLifePolicyStatus(Automation.DataModels.LifeLead lead, LifePolicyStatusEnum expectedStatus)
        {
            try
            {
                int expectedStatusVal = (int)expectedStatus;
                QueryResults results = lifeService.GetLifePolicyStatus(lead.LifeAccountKey);
                if (!results.HasResults)
                    new NUnit.Framework.AssertionException("QueryResults for test.LifeLead didn't have a LifeAccountKey column or value.");
                QueryResultsRow _row = results.FindRowByExpression<int>("PolicyStatusKey", true, expectedStatusVal)[0];
                if (_row == null)
                {
                    string message = String.Format("PolicyStatus of \"{0}\" was expected, but was \"{1}\"", expectedStatus, results.Rows(0).Column("PolicyStatusKey").Value);
                    new NUnit.Framework.AssertionException(message);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// This assertion will assert that an offer is not created for an account that is in arrears for 3 months or more
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        /// <param name="testIdentifier">TestIdentifier in the test.LifeLeads </param>
        public static void AssertLifeOfferCreated3MonthsInArrears(int accountKey, string testIdentifier)
        {
            QueryResults results = lifeService.GetTestData("LifeLead");

            Logger.LogAction("Asserting that a life offer was NOT created for " + accountKey);
            bool offerNotCreated = false;
            foreach (QueryResultsRow row in results.RowList)
            {
                if (row.Column("TestIdentifier").Value == testIdentifier)
                {
                    if (string.IsNullOrEmpty(row.Column("OfferKey").Value))
                    {
                        offerNotCreated = true;
                    }
                }
            }
            Assert.True(offerNotCreated, "An Offer should not have been created for AccountKey: " + accountKey);
            results.Dispose();
        }

        /// <summary>
        /// This assertion will check if the specified legal entity doesn't play a role in the account.
        /// </summary>
        /// <param name="idnumber">legal entity idnumber</param>
        /// <param name="offerkey">life lead offerkey</param>
        public static void AssertRoleNotExistForLifeAccount(string idnumber, int offerkey)
        {
            bool assertRoleExist = false;
            QueryResults results = lifeService.GetLegalEntitiesFromLifeAccountRoles(offerkey);
            foreach (QueryResultsRow row in results.RowList)
            {
                if (row.Column("IDNumber").Value == idnumber)
                    assertRoleExist = true;
            }

            Logger.LogAction("Asserting that the given legalentity doesn't play a role in the life account.");
            Assert.False(assertRoleExist, "Role for legalentity ({1}), exist on life offer: {0}", offerkey, idnumber);
        }

        public static void AssertLifePolicyStatus(int lifeaccountkey, LifePolicyStatusEnum expectedStatus)
        {
            var results = lifeService.GetLifePolicyStatus(lifeaccountkey);
            var policystatuskey = results.Rows(0).Column("policystatuskey").GetValueAs<int>();
            Assert.AreEqual((int)expectedStatus, policystatuskey, "Expected policystatus {0}, but was {1}", (int)expectedStatus, policystatuskey);
        }

        public static void AssertMoreThanOneAssuredLifeRoles(string idnumber)
        {
            var legalentities = from r in legalEntityService.GetLegalEntityRoles(idnumber)
                                select r.LegalEntityKey;

            var lePlayTwoAssuredLifeRoles = false;
            foreach (var le in legalentities)
            {
                var leCount = (from l in legalentities
                               where l == le
                               select l).Count();
                if (leCount >= 2)
                    lePlayTwoAssuredLifeRoles = true;
            }
            Assert.True(lePlayTwoAssuredLifeRoles, "Expected one or more legalentities to play an assured life role on two or more different accounts.");
        }

        public static int AssertCreditProtectionPlanAccountExistsByRelatedAccount(int relatedAccountKey)
        {
            var account = accountService.GetOpenRelatedAccountsByProductKey(relatedAccountKey, ProductEnum.SAHLCreditProtectionPlan);
            Assert.That(account != null);
            return account.Rows(0).Column("RelatedAccountKey").GetValueAs<int>();
        }

        public static void AssertCreditLifePolicyClaim(Automation.DataModels.FinancialService creditLifePolicyFS, ClaimTypeEnum claimType, ClaimStatusEnum status, DateTime date)
        {
            var lifePolicyClaim = lifeService.GetCreditLifePolicyClaims(creditLifePolicyFS.FinancialServiceKey).Where(x => x.ClaimStatusKey == status
                                                                                                                      && x.ClaimTypeKey == claimType
                                                                                                                      && x.ClaimDate.ToString(Formats.DateFormat) == date.ToString(Formats.DateFormat)).FirstOrDefault();
            Assert.IsNotNull(lifePolicyClaim, "Expected no life policy claim of type:{0}, status:{1}, date:{2}", claimType, status, date.ToString(Formats.DateFormat));
        }

        public static void AssertNoCreditLifePolicyClaim(Automation.DataModels.FinancialService creditLifePolicyFS, ClaimTypeEnum claimType, ClaimStatusEnum status, DateTime date)
        {
            var lifePolicyClaim = lifeService.GetCreditLifePolicyClaims(creditLifePolicyFS.FinancialServiceKey).Where(x => x.ClaimStatusKey == status
                                                                                                                         && x.ClaimTypeKey == claimType
                                                                                                                         && x.ClaimDate.ToString(Formats.DateFormat) == date.ToString(Formats.DateFormat)).FirstOrDefault();
            Assert.IsNull(lifePolicyClaim, "Expected no life policy claim of type:{0}, status:{1}, date:{2}", claimType, status, date.ToString(Formats.DateFormat));
        }

        public static void AssertAccountExternalLifeCreated(int accountKey)
        {
            var accountExternalLife = lifeService.GetExternalLifePolicyByAccount(accountKey);
            Assert.IsNotNull(accountExternalLife, string.Format(@"Expected an ExternalLifePolicy record to exist for Account: {0}", accountKey));
        }

        public static List<string> AssertLifeLeadCreated(int mortgageAccountKey,DateTime timeCreated)
        {
            var assertions = new List<string>();
            var lifeLead = x2Service.GetLifeInstanceDetails(-1, mortgageAccountKey)
                                .Where(x=>x.CreationDate > timeCreated).FirstOrDefault();
            if (lifeLead == null)
                assertions.Add(String.Format("x2 instance for life was not created for account {0}", mortgageAccountKey));

            var lifeOffer =
                lifeService.GetLifeLeads(WorkflowStates.LifeOriginationWF.ContactClient
                                        , mortgageAccountKey
                                        , LifePolicyTypeEnum.StandardCover);
            if (lifeOffer == null)
                assertions.Add(String.Format("Life offer for account {0} was not created.", mortgageAccountKey));
            return assertions;
        }
    }
}