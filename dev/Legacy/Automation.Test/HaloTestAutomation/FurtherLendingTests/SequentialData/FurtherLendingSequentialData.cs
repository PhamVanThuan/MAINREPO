using Automation.DataAccess;
using Automation.DataAccess.DataHelper;
using System;
using System.Collections.Generic;
using WatiN.Core.Exceptions;

namespace FurtherLendingTests
{
    /// <summary>
    /// Builds up the lists for the Further Lending data setup tests
    /// </summary>
    public class FurtherLendingSequentialData
    {
        /// <summary>
        /// Holds the identifier from the test.AutomationFLTestCases table
        /// </summary>
        public List<string> Identifier = new List<string>();

        /// <summary>
        /// Holds the TestGroup from the test.AutomationFLTestCases table
        /// </summary>
        public List<string> TestGroup = new List<string>();

        /// <summary>
        /// Holds the AccountKey from the test.AutomationFLTestCases table
        /// </summary>
        public List<int> AccountKey = new List<int>();

        /// <summary>
        /// Constructor for FurtherLendingSequentialData, uses the test.AutomationFLTestCases table to build up lists.
        /// </summary>
        public FurtherLendingSequentialData()
        {
            try
            {
                var db = new _2AMDataHelper();
                QueryResults results = db.GetFLAutomationOffers();
                for (int i = 0; i < results.RowList.Count; i++)
                {
                    Identifier.Add(results.Rows(i).Column("TestIdentifier").Value);
                    TestGroup.Add(results.Rows(i).Column("TestGroup").Value);
                    AccountKey.Add(results.Rows(i).Column("AccountKey").GetValueAs<int>());
                }
            }
            catch (Exception ex)
            {
                throw new WatiNException(ex.ToString());
            }
        }
    }
}