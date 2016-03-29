using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using WatiN.Core.Logging;

namespace BuildingBlocks.Assertions
{
    public static class CorrespondenceAssertions
    {
        private static IClientEmailService clientEmailService;
        private static ILegalEntityService legalEntityService;
        private static IReasonService reasonService;
        private static IAssignmentService assignmentService;
        private static IAccountService accountService;
        private static IDebtCounsellingService debtCounsellingService;
        private static IExternalRoleService externalRoleService;

        static CorrespondenceAssertions()
        {
            clientEmailService = ServiceLocator.Instance.GetService<IClientEmailService>();
            legalEntityService = ServiceLocator.Instance.GetService<ILegalEntityService>();
            reasonService = ServiceLocator.Instance.GetService<IReasonService>();
            assignmentService = ServiceLocator.Instance.GetService<IAssignmentService>();
            accountService = ServiceLocator.Instance.GetService<IAccountService>();
            debtCounsellingService = ServiceLocator.Instance.GetService<IDebtCounsellingService>();
            externalRoleService = ServiceLocator.Instance.GetService<IExternalRoleService>();
        }

        /// <summary>
        /// This assertion will check that a correspondence record exists for a give report statement and a send method used by a test
        /// </summary>
        /// <param name="genericKey">GenericKey</param>
        /// <param name="reportName">The Report Sent i.e. CAP 2 Letter</param>
        /// <param name="sendMethod">Fax/Post/Email</param>
        public static void AssertCorrespondenceRecordAdded(int genericKey, string reportName, string sendMethod, int accountKey = 0, List<int> legalEntityKeys = null, bool checkDataSTOR = false, string emailAddress = "")
        {
            if (legalEntityKeys != null)
            {
                foreach (var key in legalEntityKeys)
                {
                    var results = clientEmailService.GetLatestCorrespondenceReportForLegalEntityByGenericKeyAndReportStatement(genericKey, reportName, sendMethod, key);

                    if (!String.IsNullOrEmpty(emailAddress))
                        results = results.Where(x => x.DestinationValue == emailAddress);

                    Logger.LogAction("Asserting that the Correspondence record for the {0} report has been added for LegalEntityKey={1}", reportName, key);
                    Assert.True(results != null, "No Correspondence Record Found");
                    if (checkDataSTOR)
                    {
                        foreach (var correspondence in results)
                            AssertImageIndex(correspondence.IDMGuid, accountKey, genericKey);
                    }
                }
            }
            else
            {
                var results = clientEmailService.GetLatestCorrespondenceReportByGenericKeyAndReportStatement(genericKey, reportName, sendMethod);
                Logger.LogAction("Asserting that the Correspondence record for the " + reportName + " has been added.");
                Assert.True(results != null, "No Correspondence Record Found");
                if (checkDataSTOR)
                    AssertImageIndex(results.IDMGuid, accountKey, genericKey);
            }
        }

        public static void AssertCorrespondenceRecordNotAdded(int genericKey, string reportName, string sendMethod, List<int> legalEntityKeys = null)
        {
            if (legalEntityKeys != null)
            {
                foreach (var key in legalEntityKeys)
                {
                    var correspondence = clientEmailService.GetLatestCorrespondenceReportForLegalEntityByGenericKeyAndReportStatement(genericKey, reportName, sendMethod, key);
                    Logger.LogAction("Asserting no Correspondence record for the {0} report was added for LegalEntityKey={1}", reportName, key);
                    Assert.AreEqual(correspondence.Count(), 0, "Expected no correspondence for legal entity");
                }
            }
            else
            {
                var results = clientEmailService.GetLatestCorrespondenceReportByGenericKeyAndReportStatement(genericKey, reportName, sendMethod);
                Logger.LogAction("Asserting that no Correspondence record for the {0} has been added.", reportName);
                Assert.True(results == null, "Expected on correspondence Record");
            }
        }

        /// <summary>
        /// Finds the latest report statement and strips out its guid. The guid is then used to check the ImageIndex..Data table to ensure
        /// that an IDM record has been added.
        /// </summary>
        /// <param name="genericKey">Correspondence.GenericKey</param>
        /// <param name="reportName">ReportStatement.Description</param>
        /// <param name="sendMethod">CorrspondenceMedium</param>
        /// <param name="key1">ImageIndex..Data.key1. For debt counselling related documents this is the accountKey</param>
        /// <param name="key3">ImageIndex..Data.key3. For debt counselling related documents this is the debtCounsellingKey</param>
        public static void AssertImageIndex(string genericKey, string reportName, string sendMethod, int key1, int key3)
        {
            string guid = clientEmailService.GetCorrespondenceGUID(genericKey, reportName, sendMethod);
            var results = clientEmailService.GetImageIndexData(guid);
            if (!results.HasResults)
                Assert.Fail(string.Format(@"No IDM Image Index Record Found for GUID: {0}", guid));
            var rows = from row in results select row;
            if (key3 != 0 && key1 != 0)
            {
                rows = from r in rows
                       where r.Column("key1").GetValueAs<string>() == key1.ToString()
                           && r.Column("key3").GetValueAs<string>() == key3.ToString()
                       select r;
            }
            else if (key1 != 0)
            {
                rows = from r in rows where r.Column("key1").GetValueAs<string>() == key1.ToString() select r;
            }
            Assert.That(rows.Count() > 0, String.Format(@"No IDM Image Index Record Found for GUID: {0}", guid));
            results.Dispose();
        }

        /// <summary>
        /// Finds the latest report statement and strips out its guid. The guid is then used to check the ImageIndex..Data table to ensure
        /// that an IDM record has been added.
        /// </summary>
        /// <param name="key1">ImageIndex..Data.key1. For debt counselling related documents this is the accountKey</param>
        /// <param name="key3">ImageIndex..Data.key3. For debt counselling related documents this is the debtCounsellingKey</param>
        public static void AssertImageIndex(string guid, int key1, int key3)
        {
            var results = clientEmailService.GetImageIndexData(guid);
            if (!results.HasResults)
                Assert.Fail(string.Format(@"No IDM Image Index Record Found for GUID: {0}", guid));
            var rows = from row in results select row;
            if (key3 != 0 && key1 != 0)
            {
                rows = from r in rows
                       where r.Column("key1").GetValueAs<string>() == key1.ToString()
                           && r.Column("key3").GetValueAs<string>() == key3.ToString()
                       select r;
            }
            else if (key1 != 0)
            {
                rows = from r in rows where r.Column("key1").GetValueAs<string>() == key1.ToString() select r;
            }
            Assert.That(rows.Count() > 0, String.Format(@"No IDM Image Index Record Found for GUID: {0}", guid));
            results.Dispose();
        }

        /// <summary>
        /// We have correspondence records that get added using a predefined correspondence template. This assertion will take in the correspondence
        /// template that we are expecting to have in the correspondence table. However, an extra method is still required to be added in order to format
        /// the template with the correct token values. See FormatDeceasedNotification() as an example.
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="template"></param>
        /// <param name="date"></param>
        /// <param name="emailTo"></param>
        /// <param name="accountKey"></param>
        public static void AssertClientEmailByCorrespondenceTemplate(int genericKey, Common.Enums.CorrespondenceTemplateEnum template, DateTime date, string
            emailTo, int accountKey)
        {
            date = date.AddMinutes(-1);
            var correspondenceTemplate = clientEmailService.GetCorrespondenceTemplate();
            var subjectText = (from c in correspondenceTemplate
                               where c.CorrespondenceTemplateKey == (int)template
                               select c.Subject).FirstOrDefault();
            var templateText = (from c in correspondenceTemplate
                                where c.CorrespondenceTemplateKey == (int)template
                                select c.Template).FirstOrDefault();
            switch (template)
            {
                case Common.Enums.CorrespondenceTemplateEnum.MortgageLoanCancelledContinuePaying:
                case Common.Enums.CorrespondenceTemplateEnum.MortgageLoanCancelledDontContinuePaying:
                    string str = String.Format("{0}", accountKey);
                    subjectText = subjectText.Replace("{0}", str);
                    templateText = FormatMortgageLoanCancelledNotification(templateText, genericKey, accountKey);
                    break;

                case Common.Enums.CorrespondenceTemplateEnum.DeceasedNotificationNoLiving:
                case Common.Enums.CorrespondenceTemplateEnum.DeceasedNotificationLivingExists:
                    string str1 = String.Format("{0}", accountKey);
                    subjectText = subjectText.Replace("{0}", str1);
                    templateText = FormatDeceasedNotification(templateText, genericKey, accountKey);
                    break;

                case Common.Enums.CorrespondenceTemplateEnum.SequestrationNotificationNoOthers:
                case Common.Enums.CorrespondenceTemplateEnum.SequestrationNotificationOthersExist:
                    string str2 = String.Format("{0}", accountKey);
                    subjectText = subjectText.Replace("{0}", str2);
                    templateText = FormatSequestrationNotification(templateText, genericKey, accountKey);
                    break;

                default:
                    break;
            }
            clientEmailService.WaitForClientEmail(emailTo, 3);
            var r = clientEmailService.GetClientEmail(emailTo, subjectText, date.ToString(Formats.DateTimeFormatSQL), genericKey, templateText);
            Logger.LogAction(string.Format(@"Asserting that a client email record exists for EmailTo: {0}, Subject: {1}, Date: {2}, GenericKey: {3}, TemplateText={4}",
                emailTo, subjectText, date.ToString(Formats.DateTimeFormatSQL), genericKey, templateText));
            Assert.IsTrue(r.HasResults, string.Format(@"No client email record exists for EmailTo: {0}, Subject: {1}, Date: {2}, GenericKey: {3}, TemplateText={4}",
                emailTo, subjectText, date.ToString(Formats.DateTimeFormatSQL), genericKey, templateText));
        }

        private static string FormatMortgageLoanCancelledNotification(string templateText, int genericKey, int accountKey)
        {
            string accountLegalName = accountService.GetAccountByKey(accountKey).AccountLegalName;
            List<Automation.DataModels.ExternalRole> legalEntitiesUnderDebtCounselling = externalRoleService.GetActiveExternalRoleList(genericKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey, ExternalRoleTypeEnum.Client, true);
            string[] idNumbers = legalEntitiesUnderDebtCounselling.Select(x => x.IDNumber).ToArray();
            return string.Format(templateText, accountKey, accountLegalName, String.Join(" & ", idNumbers));
        }

        /// <summary>
        /// Formats the template for the sequestration notification
        /// </summary>
        /// <param name="templateText">template text with tokens to be replaced.</param>
        /// <param name="genericKey">debtCounsellingKey</param>
        /// <param name="accountKey">accountKey</param>
        /// <returns></returns>
        private static string FormatSequestrationNotification(string templateText, int genericKey, int accountKey)
        {
            List<string> list = GetLegalEntitiesAndConsultant(accountKey, genericKey, ReasonDescription.NotificationofSequestration, ReasonTypeEnum.DebtCounsellingNotification);
            return string.Format(templateText, list[0], accountKey, list[1], "Debt Counselling Consultant");
        }

        /// <summary>
        /// Formats the template for the deceased notification
        /// </summary>
        /// <param name="templateText">template text with tokens to be replaced.</param>
        /// <param name="genericKey">debtCounsellingKey</param>
        /// <param name="accountKey">accountKey</param>
        /// <returns></returns>
        private static string FormatDeceasedNotification(string templateText, int genericKey, int accountKey)
        {
            List<string> list = GetLegalEntitiesAndConsultant(accountKey, genericKey, ReasonDescription.NotificationofDeath, ReasonTypeEnum.DebtCounsellingNotification);
            return string.Format(templateText, list[0], accountKey,
                list[1], "Debt Counselling Consultant");
        }

        /// <summary>
        /// Returns a list consisting of the legal entities marked as dead or under sequestration and the consultant's name
        /// </summary>
        /// <param name="accountKey">accountKey</param>
        /// <param name="genericKey">genericKey</param>
        /// <returns></returns>
        private static List<string> GetLegalEntitiesAndConsultant(int accountKey, int genericKey, string reasonDescription, ReasonTypeEnum reasonType)
        {
            //we need to get the reason definition
            var reasonDefinition = reasonService.GetReasonDefinition(reasonType, reasonDescription);
            List<string> list = new List<string>();
            string legalEntities = string.Empty;
            bool exists = false;
            bool leLeft = false;

            //we need the LE's
            var results = reasonService.GetNotificationReasonsForLegalEntity(reasonDefinition.ReasonDefinitionKey, accountKey);

            //we need the consultant
            var aduser = assignmentService.GetActiveWorkflowRoleByTypeAndGenericKey(WorkflowRoleTypeEnum.DebtCounsellingConsultantD, genericKey);
            int legalentitykey = (from a in aduser select a.Key).FirstOrDefault();
            foreach (var row in results.RowList)
            {
                exists = Convert.ToBoolean(row.Column("NotificationExists").GetValueAs<int>());
                if (exists)
                    legalEntities += row.Column("Displayname").GetValueAs<string>() + ", ";
                else
                    leLeft = true;
            }

            if (!String.IsNullOrEmpty(legalEntities))
            {
                legalEntities = legalEntities.Trim();
                legalEntities = legalEntities.TrimEnd(',');
            }
            string consultantName = legalEntityService.GetLegalEntityLegalName(legalentitykey);
            list.Add(legalEntities);
            list.Add(consultantName);
            return list;
        }

        public static void AssertCorrespondenceRecordAddedAfterDate(int genericKey, string reportName, string sendMethod,DateTime date, int accountKey = 0, bool checkDataSTOR = false, string emailAddress = "")
        {
            //if (legalEntityKeys != null)
            //{
            //    foreach (var key in legalEntityKeys)
            //    {
            //        var results = clientEmailService.GetLatestCorrespondenceReportForLegalEntityByGenericKeyAndReportStatement(genericKey, reportName, sendMethod, key);

            //        if (!String.IsNullOrEmpty(emailAddress))
            //            results = results.Where(x => x.DestinationValue == emailAddress);

            //        Logger.LogAction("Asserting that the Correspondence record for the {0} report has been added for LegalEntityKey={1}", reportName, key);
            //        Assert.True(results != null, "No Correspondence Record Found");
            //        if (checkDataSTOR)
            //        {
            //            foreach (var correspondence in results)
            //                AssertImageIndex(correspondence.IDMGuid, accountKey, genericKey);
            //        }
            //    }
            //}
            //else
            //{
            var results = clientEmailService.GetCorrespondenceReportByGenericKeyReportStatementAndGreaterThanDate(genericKey, reportName, sendMethod, date);
                Logger.LogAction("Asserting that the Correspondence record for the " + reportName + " has been added.");
                Assert.True(results != null, "No Correspondence Record Found");
                if (checkDataSTOR)
                    AssertImageIndex(results.IDMGuid, accountKey, genericKey);
            //}
        }
    }
}