using Automation.DataAccess;
using Common.Enums;
using System;
using System.Collections.Generic;

namespace BuildingBlocks.Services.Contracts
{
    public interface ICommonService
    {
        void SaveTestMethodParameters(string testMethodName, string testIdentifier, ParameterTypeEnum parameterDescription, string parameterValue);

        void DeleteTestMethodData(string testMethod, string testIdentifier);

        void DeleteTestMethodDataForFixture(string testFixture);

        ParamType GetTestMethodParameters<ParamType>(string testMethod, string testIdentifier, ParameterTypeEnum typeToFetch);

        void InsertTestMethod(string testMethodName, string testIdentifier, string testFixture);

        void UpdateRuleParameter(string RuleName, string RuleParameter, string NewValue);

        string GetDateOfBirthFromIDNumber(string idNumber);

        bool IsTimeOver(TimeSpan whenTimeIsOver);

        void SplitRandsCents(out int randValue, out int centsValue, double amount);

        void SplitRandsCents(out string randValue, out string centsValue, string valueString);

        DateTime GetDateWithBusinessDayCheck(bool isBusinessDay, bool prevDayBusinessDay);

        Automation.DataModels.OriginationTestCase GetTestDataAutomationLeads(string testIdentifier);

        Automation.DataModels.OriginationTestCase GetTestDataByTestIdentifier(string testIdentifier);

        void CommitOfferKeyForAutomationLeads(string testIdentifier, int offerKey, string tableName);

        void CommitOfferKeyForTestIdentifier(string testIdentifier, int offerKey);

        void CommitOfferKeyForTestIdentifier(string identifierColumn, string testIdentifier, int offerKey);

        int GetOfferKeyByTestIdentifier(string testIdentifier);

        string GetRandomTypeDescription(string nameToExclude, Type constantType);

        string GetRandomTypeDescription(int enumKeyExclusion, Type enumType, Type constantType);

        string GetConcatenatedPropertyNameValues<T>(object instance);

        int ConvertCurrencyStringToInt(string InputString);

        int MonthDifference(DateTime startDate, DateTime endDate);

        int GetOfferKeyFromTestSchemaTable(string tableName, string conditionColumnName, string conditionColumnValue);

        double GetMarketRate(MarketRateEnum mRate);

        QueryResults GetTestData(string testIdentifier, string tableName);

        bool UpdateTermChangeTest(string testIdentifier, bool openOffer);

        void CreateTermChangeTestCases();

        int GetOfferKeyByTestIdentifier(string columnValue, string columnName);

        QueryResults OffersAtApplicationCaptureRow(string identifier, string columnName);

        QueryResults OffersAtApplicationCaptureRow(string identifier);

        bool IsAutomationTestCase(int offerKey);

        QueryResultsRow GetRandomOfferRow(OfferTypeEnum offertype, string workflow, string stateName);

        DateTime GetNextBusinessDay(DateTime? date = null, bool includeDate = false);

        DateTime GetNextBusinessDay(DateTime startDate, DateTime endDate);

        void CreatePersonalLoanTestCases();

        void SyncMarketRates();

        Automation.DataModels.MarketingSource GetMarketingSourceByDescriptionAndStatus(string description, GeneralStatusEnum generalStatus);

        Automation.DataModels.MarketingSource GetMarketingSourceByStatus(GeneralStatusEnum generalStatus);

        IEnumerable<Automation.DataModels.OriginationTestCase> GetOriginationTestCases();

        IEnumerable<Automation.DataModels.LeadTestCase> GetAutomationTestLeads();

        void InsertIDNumberIntoTestTable(string idNumber);
    }
}