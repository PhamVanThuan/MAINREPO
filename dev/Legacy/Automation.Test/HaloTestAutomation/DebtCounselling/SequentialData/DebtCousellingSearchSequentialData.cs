using Automation.DataAccess;
using Automation.DataAccess.DataHelper;
using System;
using System.Collections.Generic;
using WatiN.Core.Exceptions;

namespace DebtCounsellingTests
{
    public class DebtCousellingSearchSequentialData
    {
        /// <summary>
        /// Holds the test identifier from the test.DebtCounsellingTestCases table.
        /// </summary>
        public List<string> TestIdentifier = new List<string>();

        /// <summary>
        /// Constructor for DebtCounsellingSequentialData.
        /// </summary>
        public DebtCousellingSearchSequentialData()
        {
            try
            {
                var db = new _2AMDataHelper();
                QueryResults results = db.GetDebtCounsellingSearchCases();
                for (int i = 0; i < results.RowList.Count; i++)
                {
                    TestIdentifier.Add(results.Rows(i).Column("TestIdentifier").Value);
                }
            }
            catch (Exception ex)
            {
                throw new WatiNException(ex.ToString());
            }
        }
    }
}