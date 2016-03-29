using Automation.DataAccess;
using Automation.DataAccess.DataHelper;
using BuildingBlocks.Services.Contracts;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BuildingBlocks.Services
{
    public class ClientEmailService : _2AMDataHelper, IClientEmailService
    {
        /// <summary>
        /// Will wait for email x no tries in intervals of 10 sec, if it finds it it will return.
        /// </summary>
        /// <param name="emailTo"></param>
        /// <param name="noTries"></param>
        public void WaitForClientEmail(string emailTo, int noTries)
        {
            var email = default(QueryResults);
            for (int count = 0; count < noTries; count++)
            {
                var timeSpan = TimeSpan.FromSeconds(10);
                SpinWait.SpinUntil(() => { return false; }, timeSpan);
                email = base.GetClientEmail(emailTo);
                if (email.HasResults)
                    return;
            }
            Assert.That(email.HasResults, "Could not locate email in the client email table. EmailAddress: {0}", emailTo);
        }

        public IEnumerable<Automation.DataModels.Correspondence> GetLatestCorrespondenceReportForLegalEntityByGenericKeyAndReportStatement(int genericKey, string reportName, string sendMethod, int legalEntityKey)
        {
            var records = base.GetCorrespondenceRecordsByGenericKeyAndReportStatement(genericKey, reportName, sendMethod);
            return records.Where(x => x.LegalEntityKey == legalEntityKey);
        }

        public Automation.DataModels.Correspondence GetLatestCorrespondenceReportByGenericKeyAndReportStatement(int genericKey, string reportName, string sendMethod)
        {
            var records = base.GetCorrespondenceRecordsByGenericKeyAndReportStatement(genericKey, reportName, sendMethod);
            return records
                .OrderByDescending(x => x.ChangeDate)
                .FirstOrDefault();
        }
        
        public Automation.DataModels.Correspondence GetCorrespondenceReportByGenericKeyReportStatementAndGreaterThanDate(int genericKey, string reportName, string sendMethod, DateTime date)
        {
            var records = base.GetCorrespondenceRecordsByGenericKeyAndReportStatement(genericKey, reportName, sendMethod);
            return records.Where(x => x.ChangeDate > date).FirstOrDefault();
        }
    }
}