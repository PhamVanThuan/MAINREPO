using Automation.DataAccess;
using BuildingBlocks.Services.Contracts;
using Common.Enums;
using NUnit.Framework;
using WatiN.Core.Logging;

namespace BuildingBlocks.Assertions
{
    /// <summary>
    /// Contains assertions for application declarations
    /// </summary>
    public static class ApplicationDeclarationsAssertions
    {
        private static readonly IApplicationService applicationService;

        static ApplicationDeclarationsAssertions()
        {
            applicationService = ServiceLocator.Instance.GetService<IApplicationService>();
        }

        /// <summary>
        /// Checks that the captured application declaration answers are saved correctly for the given legal entity.
        /// </summary>
        /// <param name="app">Application Declaration Answer</param>
        /// <param name="offerKey">offerKey</param>
        /// <param name="legalEntityKey">legalEntityKey</param>
        public static void AssertDeclarationsExist(Automation.DataModels.ApplicationDeclaration app, int offerKey, int legalEntityKey)
        {
            int i = 0;
            var r = applicationService.GetApplicationDeclarations(offerKey, legalEntityKey);
            //check if we have results
            if (r.HasResults)
            {
                foreach (QueryResultsRow row in r.RowList)
                {
                    if (row.Column("OfferDeclarationQuestionKey").GetValueAs<int>() == (int)OfferDeclarationQuestionEnum.Insolvency)
                    {
                        Logger.LogAction(string.Format(@"Asserting that the answer for OfferDeclarationQuestionKey = {0} for Offer {1} is set to {2}", (int)OfferDeclarationQuestionEnum.Insolvency, offerKey, app.InsolvencyAnswer));
                        Assert.AreEqual(app.InsolvencyAnswer, row.Column("Description").Value, "Insolvency answer is incorrect");
                        i++;
                    }
                    if (row.Column("OfferDeclarationQuestionKey").GetValueAs<int>() == (int)OfferDeclarationQuestionEnum.Rehabilitation)
                    {
                        StringAssert.AreEqualIgnoringCase(app.DateRehabilitatedAnswer, row.Column("OfferDeclarationDate").Value, "Rehabilitation Date is incorrect");
                        i++;
                    }
                    if (row.Column("OfferDeclarationQuestionKey").GetValueAs<int>() == (int)OfferDeclarationQuestionEnum.AdminOrder)
                    {
                        Logger.LogAction(string.Format(@"Asserting that the answer for OfferDeclarationQuestionKey = {0} for Offer {1} is set to {2}", (int)OfferDeclarationQuestionEnum.AdminOrder, offerKey, app.AdministrationOrderAnswer));
                        Assert.AreEqual(app.AdministrationOrderAnswer, row.Column("Description").Value, "Admin Order answer is incorrect");
                        i++;
                    }
                    if (row.Column("OfferDeclarationQuestionKey").GetValueAs<int>() == (int)OfferDeclarationQuestionEnum.AdminOrderRescinded)
                    {
                        Logger.LogAction(string.Format(@"Asserting that the answer for OfferDeclarationQuestionKey = {0} for Offer {1} is set to {2}", (int)OfferDeclarationQuestionEnum.AdminOrderRescinded, offerKey, app.DateRescindedAnswer));
                        Assert.AreEqual(app.DateRescindedAnswer, row.Column("OfferDeclarationDate").Value, "Rescinded Date is incorrect");
                        i++;
                    }
                    if (row.Column("OfferDeclarationQuestionKey").GetValueAs<int>() == (int)OfferDeclarationQuestionEnum.CurrentlyUnderDebtCounselling)
                    {
                        Logger.LogAction(string.Format(@"Asserting that the answer for OfferDeclarationQuestionKey = {0} for Offer {1} is set to {2}", (int)OfferDeclarationQuestionEnum.CurrentlyUnderDebtCounselling, offerKey, app.CurrentUnderDebtCounsellingAnswer));
                        Assert.AreEqual(app.CurrentUnderDebtCounsellingAnswer, row.Column("Description").Value, "Debt Counselling answer is incorrect");
                        i++;
                    }
                    if (row.Column("OfferDeclarationQuestionKey").GetValueAs<int>() == (int)OfferDeclarationQuestionEnum.DebtRearrangement)
                    {
                        Logger.LogAction(string.Format(@"Asserting that the answer for OfferDeclarationQuestionKey = {0} for Offer {1} is set to {2}", (int)OfferDeclarationQuestionEnum.DebtRearrangement, offerKey, app.CurrentDebtRearrangementAnswer));
                        Assert.AreEqual(app.CurrentDebtRearrangementAnswer, row.Column("Description").Value, "Debt Rearrangement answer is incorrect");
                        i++;
                    }
                    if (row.Column("OfferDeclarationQuestionKey").GetValueAs<int>() == (int)OfferDeclarationQuestionEnum.CreditCheck)
                    {
                        Logger.LogAction(string.Format(@"Asserting that the answer for OfferDeclarationQuestionKey = {0} for Offer {1} is set to {2}", (int)OfferDeclarationQuestionEnum.CreditCheck, offerKey, app.ConductCreditCheckAnswer));
                        Assert.AreEqual(app.ConductCreditCheckAnswer, row.Column("Description").Value, "Credit Check answer is incorrect");
                        i++;
                    }
                }
                //ensures that all 7 questions have been answered correctly.
                Assert.AreEqual(7, i, "Not all the questions were saved to the database.");
            }
            else
            {
                Assert.Fail("No Application Declaration Records exist.");
            }
        }

        /// <summary>
        /// Checks that the captured external application declaration answers are saved correctly for the given legal entity.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="offerKey"></param>
        /// <param name="legalEntityKey"></param>
        public static void AssertExternalDeclarationsExist(Automation.DataModels.ApplicationDeclaration app, int offerKey, int legalEntityKey)
        {
            int i = 0;
            var r = applicationService.GetExternalApplicationDeclarations(offerKey, legalEntityKey);
            //check if we have results
            if (r.HasResults)
            {
                foreach (QueryResultsRow row in r.RowList)
                {
                    if (row.Column("OfferDeclarationQuestionKey").GetValueAs<int>() == (int)OfferDeclarationQuestionEnum.Insolvency)
                    {
                        Logger.LogAction(string.Format(
                        @"Asserting that the answer for OfferDeclarationQuestionKey = {0} for Offer {1} is set to {2}",
                        (int)OfferDeclarationQuestionEnum.Insolvency, offerKey, app.InsolvencyAnswer));
                        Assert.AreEqual(app.InsolvencyAnswer, row.Column("Description").Value, "Insolvency answer is incorrect");
                        i++;
                    }
                    if (row.Column("OfferDeclarationQuestionKey").GetValueAs<int>() == (int)OfferDeclarationQuestionEnum.Rehabilitation)
                    {
                        StringAssert.AreEqualIgnoringCase(string.IsNullOrEmpty(app.DateRehabilitatedAnswer) == true ? string.Empty : app.DateRehabilitatedAnswer, row.Column("ExternalRoleDeclarationDate").Value,
                            "Rehabilitation Date is incorrect");
                        i++;
                    }
                    if (row.Column("OfferDeclarationQuestionKey").GetValueAs<int>() == (int)OfferDeclarationQuestionEnum.AdminOrder)
                    {
                        Logger.LogAction(string.Format(@"Asserting that the answer for OfferDeclarationQuestionKey = {0} for Offer {1} is set to {2}", (int)OfferDeclarationQuestionEnum.AdminOrder, offerKey, app.AdministrationOrderAnswer));
                        Assert.AreEqual(app.AdministrationOrderAnswer, row.Column("Description").Value, "Admin Order answer is incorrect");
                        i++;
                    }
                    if (row.Column("OfferDeclarationQuestionKey").GetValueAs<int>() == (int)OfferDeclarationQuestionEnum.AdminOrderRescinded)
                    {
                        Logger.LogAction(string.Format(@"Asserting that the answer for OfferDeclarationQuestionKey = {0} for Offer {1} is set to {2}", (int)OfferDeclarationQuestionEnum.AdminOrderRescinded, offerKey, app.DateRescindedAnswer));
                        Assert.AreEqual(string.IsNullOrEmpty(app.DateRescindedAnswer) == true ? string.Empty : app.DateRescindedAnswer, row.Column("ExternalRoleDeclarationDate").Value, "Rescinded Date is incorrect");
                        i++;
                    }
                    if (row.Column("OfferDeclarationQuestionKey").GetValueAs<int>() == (int)OfferDeclarationQuestionEnum.CurrentlyUnderDebtCounselling)
                    {
                        Logger.LogAction(string.Format(
                        @"Asserting that the answer for OfferDeclarationQuestionKey = {0} for Offer {1} is set to {2}",
                        (int)OfferDeclarationQuestionEnum.CurrentlyUnderDebtCounselling, offerKey, app.CurrentUnderDebtCounsellingAnswer));
                        Assert.AreEqual(app.CurrentUnderDebtCounsellingAnswer, row.Column("Description").Value, "Debt Counselling answer is incorrect");
                        i++;
                    }
                    if (row.Column("OfferDeclarationQuestionKey").GetValueAs<int>() == (int)OfferDeclarationQuestionEnum.DebtRearrangement)
                    {
                        Logger.LogAction(string.Format(
                        @"Asserting that the answer for OfferDeclarationQuestionKey = {0} for Offer {1} is set to {2}",
                        (int)OfferDeclarationQuestionEnum.DebtRearrangement, offerKey, app.CurrentDebtRearrangementAnswer));
                        Assert.AreEqual(app.CurrentDebtRearrangementAnswer, row.Column("Description").Value, "Debt Rearrangement answer is incorrect");
                        i++;
                    }
                    if (row.Column("OfferDeclarationQuestionKey").GetValueAs<int>() == (int)OfferDeclarationQuestionEnum.CreditCheck)
                    {
                        Logger.LogAction(string.Format(
                        @"Asserting that the answer for OfferDeclarationQuestionKey = {0} for Offer {1} is set to {2}",
                        (int)OfferDeclarationQuestionEnum.CreditCheck, offerKey, app.ConductCreditCheckAnswer));
                        Assert.AreEqual(app.ConductCreditCheckAnswer, row.Column("Description").Value, "Credit Check answer is incorrect");
                        i++;
                    }
                }
                //ensures that all 7 questions have been answered correctly.
                Assert.AreEqual(7, i, "Not all the questions were saved to the database.");
            }
            else
            {
                Assert.Fail("No Application Declaration Records exist.");
            }
        }

        /// <summary>
        /// Checks that no application declaration answers exist for the given legal entity.
        /// </summary>
        /// <param name="offerKey">offerKey</param>
        /// <param name="legalEntityKey">legalEntityKey</param>
        public static void AssertApplicationDeclarationsDoNotExist(int offerKey, int legalEntityKey)
        {
            var results = applicationService.GetApplicationDeclarations(offerKey, legalEntityKey);
            //check if we have results
            if (results.HasResults)
            {
                Assert.Fail("Application Declaration Records exist when we expect them to not exist.");
            }
        }

        public static void AssertExternalApplicationDeclarationsDoNotExist(int offerKey, int legalEntityKey)
        {
            var results = applicationService.GetExternalApplicationDeclarations(offerKey, legalEntityKey);
            //check if we have results
            if (results.HasResults)
            {
                Assert.Fail("Application Declaration Records exist when we expect them to not exist.");
            }
        }
    }
}