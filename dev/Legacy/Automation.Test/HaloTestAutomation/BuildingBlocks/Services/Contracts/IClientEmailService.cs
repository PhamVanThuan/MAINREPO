using Automation.DataAccess;
using System;
using System.Collections.Generic;


namespace BuildingBlocks.Services.Contracts
{
    public interface IClientEmailService
    {
        IEnumerable<Automation.DataModels.Account> GetUnusedMortgageAccountForCorrespondenceTests();

        IEnumerable<Automation.DataModels.Account> GetUnusedUnsecuredLendingAccountForCorrespondenceTests();

        void WaitForClientEmail(string emailTo, int noTries);

        IEnumerable<Automation.DataModels.Correspondence> GetCorrespondenceRecordsByGenericKeyAndReportStatement(int genericKey, string reportName, string sendMethod);

        string GetCorrespondenceGUID(string genericKey, string reportName, string sendMethod);

        QueryResults GetImageIndexData(string guid);

        IEnumerable<Automation.DataModels.CorrespondenceTemplate> GetCorrespondenceTemplate();

        QueryResults GetClientEmail(string emailTo, string emailSubject, string date, int genericKey, string emailBody);

        QueryResults GetClientEmailByToAddressAndSubject(string emailTo, string emailSubject, string date);

        QueryResults GetClientCommunication(Common.Enums.ContentTypeEnum contentType, int accountkey);

        QueryResults GetClientEmailSMS(string emailBody, string cellPhone, int genericKey);

        void UpdateLegalEntityEmailAddress(int leKey, string p);

        IEnumerable<Automation.DataModels.Correspondence> GetLatestCorrespondenceReportForLegalEntityByGenericKeyAndReportStatement(int genericKey, string reportName, string sendMethod, int legalEntityKey);

        Automation.DataModels.Correspondence GetLatestCorrespondenceReportByGenericKeyAndReportStatement(int genericKey, string reportName, string sendMethod);

        Automation.DataModels.Correspondence GetCorrespondenceReportByGenericKeyReportStatementAndGreaterThanDate(int genericKey, string reportName, string sendMethod, DateTime date);
    }
}