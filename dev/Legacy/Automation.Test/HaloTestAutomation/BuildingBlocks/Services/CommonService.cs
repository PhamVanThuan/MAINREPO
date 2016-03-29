using Automation.DataAccess;
using Automation.DataAccess.DataHelper;
using BuildingBlocks.Services.Contracts;
using Common.Enums;
using Common.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BuildingBlocks.Services
{
    public class CommonService : _2AMDataHelper, ICommonService
    {
        /// <summary>
        /// Deletes the data in the TestMethod and TestMethodParameter tables for a given Test Method and Test Identifier
        /// </summary>
        /// <param name="testMethod"></param>
        /// <param name="testIdentifier"></param>
        public void DeleteTestMethodData(string testMethod, string testIdentifier)
        {
            //we need the TestMethodKey
            string testMethodKey = base.GetTestMethodKey(testMethod, testIdentifier);
            base.DeleteTestMethodData(testMethodKey);
        }

        /// <summary>
        /// Retrieves the parameters for a give test method and test identifier
        /// </summary>
        /// <param name="testMethod">TestMethod</param>
        /// <param name="testIdentifier">TestIdentifier</param>
        /// <param name="typeToFetch"></param>
        /// <returns>ParameterValue</returns>
        public ParamType GetTestMethodParameters<ParamType>(string testMethod, string testIdentifier, ParameterTypeEnum typeToFetch)
        {
            object paramValue = new object();
            var results = base.GetTestMethodParameters(testMethod, testIdentifier);
            Assert.That(results.HasResults, "No test method found. TestIdentifier:{0}", testIdentifier);
            foreach (var row in results.RowList)
            {
                if (row.Column("ParameterTypeKey").GetValueAs<int>() == (int)typeToFetch)
                {
                    paramValue = row.Column("ParameterValue").GetValueAs<ParamType>();
                    break;
                }
            }
            results.Dispose();

            return (ParamType)paramValue;
        }

        /// <summary>
        /// Takes a string in the format #####.## and returns the rand value and cents value.
        /// </summary>
        /// <param name="randValue">Rand Value</param>
        /// <param name="centsValue">Cents Value</param>
        /// <param name="valueString">String to be splitted</param>
        public void SplitRandsCents(out string randValue, out string centsValue, string valueString)
        {
            centsValue = "00";
            if (valueString.IndexOf('.') > 0)
            {
                randValue = valueString.Substring(0, valueString.IndexOf('.'));
                centsValue = valueString.Substring(valueString.IndexOf('.') + 1, valueString.Length - valueString.IndexOf('.') - 1);
                if (centsValue.Length > 2) centsValue = centsValue.Substring(0, 2);
            }
            else
            {
                randValue = valueString;
            }
        }

        /// <summary>
        /// Takes a string in the format #####.## and returns the rand value and cents value.
        /// </summary>
        /// <param name="randValue">Rand Value</param>
        /// <param name="centsValue">Cents Value</param>
        /// <param name="amount"></param>
        public void SplitRandsCents(out int randValue, out int centsValue, double amount)
        {
            string amountString = Math.Round(amount, 2).ToString();
            string sRandValue; string sCentsValue;
            SplitRandsCents(out sRandValue, out sCentsValue, amountString);
            randValue = int.Parse(sRandValue);
            centsValue = int.Parse(sCentsValue);
        }

        ///<summary>
        ///</summary>
        ///<param name="whenTimeIsOver"></param>
        ///<returns></returns>
        public bool IsTimeOver(TimeSpan whenTimeIsOver)
        {
            return DateTime.Now.TimeOfDay > whenTimeIsOver;
        }

        /// <summary>
        /// Gets the DOB from an ID Number
        /// </summary>
        /// <param name="idNumber">ID Number</param>
        /// <returns>DOB</returns>
        public string GetDateOfBirthFromIDNumber(string idNumber)
        {
            string dob = string.Empty;
            if (idNumber != null)
            {
                for (int index = 4; index >= 0; index = (index - 2))
                {
                    if (index == 0)
                    {
                        if (Convert.ToInt32(idNumber.Substring(index, 2)) > 20) dob = dob + "19" + idNumber.Substring(index, 2);
                        else dob = dob + "20" + idNumber.Substring(index, 2);
                    }
                    else dob = dob + idNumber.Substring(index, 2) + "/";
                }
            }
            return dob;
        }

        /// <summary>
        /// Returns the next day after today that is a business day or not.
        /// </summary>
        /// <param name="isBusinessDay">TRUE = next business day after today, FALSE = next non-business day after today.</param>
        /// <param name="prevDayBusinessDay">TRUE = the previous day must also be a business day, FALSE = prev day must be a non-business day</param>
        /// <returns></returns>
        public DateTime GetDateWithBusinessDayCheck(bool isBusinessDay, bool prevDayBusinessDay)
        {
            DateTime date;
            if (isBusinessDay)
            {
                date = base.GetNextBusinessDay();
                if (prevDayBusinessDay)
                {
                    if (date.AddDays(-1).DayOfWeek == DayOfWeek.Saturday || date.AddDays(-1).DayOfWeek == DayOfWeek.Sunday)
                    {
                        date = base.GetNextBusinessDay(date);
                        return date;
                    }
                }
                else
                {
                    //we need to make sure that the prev day is not a business day, so we get the next monday as sunday is never a business day
                    return GetNextDateForDay(DateTime.Now, DayOfWeek.Monday);
                }
                return date;
            }
            if (!isBusinessDay)
                return base.GetNextNonBusinessDay();
            return default(DateTime);
        }

        private static DateTime GetNextDateForDay(DateTime startDate, DayOfWeek desiredDay)
        {
            // Given a date and day of week,
            // find the next date whose day of the week equals the specified day of the week.
            return startDate.AddDays(DaysToAdd(startDate.DayOfWeek, desiredDay));
        }

        /// <summary>
        /// Calculates the number of days to add to the given day of
        /// the week in order to return the next occurrence of the
        /// desired day of the week.
        /// </summary>
        /// <param name="current">The starting day of the week.</param>
        /// <param name="desired">The desired day of the week.</param>
        /// <returns>
        ///     The number of days to add to <var>current</var> day of week
        ///     in order to achieve the next <var>desired</var> day of week.
        /// </returns>
        private static int DaysToAdd(DayOfWeek current, DayOfWeek desired)
        {
            // f( c, d ) = g( c, d ) mod 7, g( c, d ) > 7
            //           = g( c, d ), g( c, d ) < = 7
            //   where 0 <= c < 7 and 0 <= d < 7
            int c = (int)current;
            int d = (int)desired;
            int n = (7 - c + d);
            return (n > 7) ? n % 7 : n;
        }

        /// <summary>
        /// Returns the OfferKey from the OffersAtApplicationCapture table when provided with the TestIdentifier
        /// </summary>
        /// <param name="testIdentifier">Identifier</param>
        /// <returns>OffersAtApplicationCapture.OfferKey</returns>
        public int GetOfferKeyByTestIdentifier(string testIdentifier)
        {
            return base.GetTestDataByTestIdentifier(testIdentifier).OfferKey;
        }

        /// <summary>
        /// Updates the OfferKey column in the OffersAtApplicationCapture table for a given TestIdentifier
        /// </summary>
        /// <param name="testIdentifier">Identifier</param>
        /// <param name="offerKey">New OfferKey value</param>
        public void CommitOfferKeyForTestIdentifier(string testIdentifier, int offerKey)
        {
            base.CommitTestDataForTestIdentifier("TestIdentifier", testIdentifier, "OfferKey", offerKey.ToString());
        }

        /// <summary>
        /// Updates the OfferKey column in the OffersAtApplicationCapture table for a given TestIdentifier
        /// </summary>
        /// <param name="testIdentifier">Identifier</param>
        /// <param name="offerKey">New OfferKey value</param>
        public void CommitOfferKeyForTestIdentifier(string identifierColumn, string testIdentifier, int offerKey)
        {
            base.CommitTestDataForTestIdentifier(identifierColumn, testIdentifier, "OfferKey", offerKey.ToString());
        }

        /// <summary>
        /// Updates the OfferKey column in the AutomationLeads table for a given TestIdentifier
        /// </summary>
        /// <param name="testIdentifier">Identifier</param>
        /// <param name="offerKey">New OfferKey value</param>
        /// <param name="tableName">Test table to commit data to. AutomationLeads or InternetLeads</param>
        public void CommitOfferKeyForAutomationLeads(string testIdentifier, int offerKey, string tableName)
        {
            base.CommitTestDataAutomationLeads(testIdentifier, "OfferKey", offerKey.ToString(), tableName);
        }

        public int GetOfferKeyFromTestSchemaTable(string tableName, string conditionColumnName, string conditionColumnValue)
        {
            var queryResults = base.GetTestData(tableName, conditionColumnName, conditionColumnValue);
            return queryResults.Rows(0).Column("OfferKey").GetValueAs<int>();
        }

        ///<summary>
        ///</summary>
        ///<param name="startDate"></param>
        ///<param name="endDate"></param>
        ///<returns></returns>
        public int MonthDifference(DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Math.Abs(monthsApart);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="InputString"></param>
        /// <returns></returns>
        public int ConvertCurrencyStringToInt(string InputString)
        {
            string cleanedString = InputString.CleanCurrencyString(true);
            return Convert.ToInt32(cleanedString);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        public string GetConcatenatedPropertyNameValues<T>(object instance)
        {
            var concatenatedPropertyNameValues = String.Empty;
            var properties = instance.GetType().GetProperties();

            foreach (var property in properties)
                concatenatedPropertyNameValues += String.Format("Property Name:\"{0}:{1}\" \r\n ", property.Name, property.GetValue(instance, null));
            return concatenatedPropertyNameValues;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="enumKeyExclusion"></param>
        /// <param name="enumType"></param>
        /// <param name="constantType"></param>
        /// <returns></returns>
        public string GetRandomTypeDescription(int enumKeyExclusion, Type enumType, Type constantType)
        {
            var nameToExclude = Enum.GetName(enumType, enumKeyExclusion);
            //Get a description where the name of the constant field is not equal to the name to exclude
            foreach (FieldInfo constantField in constantType.GetFields())
                if (!constantField.Name.Equals(nameToExclude)
                        && !constantField.Name.Equals("Unknown"))
                    return (string)constantField.GetValue(constantType);
            return String.Empty;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nameToExclude"></param>
        /// <param name="constantType"></param>
        /// <returns></returns>
        public string GetRandomTypeDescription(string nameToExclude, Type constantType)
        {
            //Get a description where the name of the constant field is not equal to the name to exclude
            foreach (FieldInfo constantField in constantType.GetFields())
                if (!constantField.Name.Equals(nameToExclude)
                        && !constantField.Name.Equals("Unknown"))
                    return (string)constantField.GetValue(constantType);
            return String.Empty;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="columnValue"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public int GetOfferKeyByTestIdentifier(string columnValue, string columnName)
        {
            var results = base.GetTestDataByTestIdentifier(columnName, columnValue);
            return results.Rows(0).Column("OfferKey").GetValueAs<int>();
        }

        /// <summary>
        /// Gets a row of data from the OffersAtApplicationCapture table for a given identifier
        /// </summary>
        /// <param name="identifier">Identifier</param>
        /// <param name="columnName"></param>
        /// <returns>OffersAtApplicationCapture.*</returns>
        public QueryResults OffersAtApplicationCaptureRow(string identifier, string columnName)
        {
            QueryResults results = base.GetTestDataByTestIdentifier(columnName, identifier);
            return results;
        }

        /// <summary>
        /// Gets a row of data from the OffersAtApplicationCapture table for a given identifier
        /// </summary>
        /// <param name="identifier">Identifier</param>
        /// <returns>OffersAtApplicationCapture.*</returns>
        public QueryResults OffersAtApplicationCaptureRow(string identifier)
        {
            return OffersAtApplicationCaptureRow(identifier, "ApplicationManagementTestID");
        }

        /// <summary>
        /// checks if an offer is an automation test case.
        /// </summary>
        /// <param name="offerKey"></param>
        /// <returns></returns>
        public bool IsAutomationTestCase(int offerKey)
        {
            var row = (from r in base.GetTestDataByTestIdentifier("OfferKey", offerKey.ToString())
                       select r).FirstOrDefault();
            var exists = row != null ? true : false;
            return exists;
        }

        public QueryResultsRow GetRandomOfferRow(OfferTypeEnum offertype, string workflow, string stateName)
        {
            QueryResults results = base.GetApplicationsByStateAndAppType(stateName, workflow, "", (int)offertype);
            return (from r in results select r).SelectRandom();
        }

        public IEnumerable<Automation.DataModels.OriginationTestCase> GetOriginationTestCases()
        {
            return base.GetOriginationTestCases();
        }

        public IEnumerable<Automation.DataModels.LeadTestCase> GetAutomationTestLeads()
        {
            return base.GetAutomationTestLeads();
        }
    }
}