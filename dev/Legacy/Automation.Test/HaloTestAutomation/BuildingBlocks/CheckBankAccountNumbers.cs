using System;
using System.Collections.Generic;
using System.Text;
using SQLQuerying;
using SQLQuerying.DataHelper;

namespace BuildingBlocks
{
    public static class BankAccountNumbers
    {
        private static int EnumeratedRow = 0;

        /// <summary>
        /// Removes any IDNumbers from the test.IDNumbers table that exist in the dbo.LegalEntity table.  
        /// Cleans out any duplicate IDNumbers in the test.IDNumbers table.  Ensures the test.IDNumbers 
        /// table contains enough IDNumbers.  If the number of records in the test.IDNumbers table is 
        /// less than MinRecords then IDNumbers are inserted into the test.IDNumbers table from staging on sahls15, checked to 
        /// ensure they do not exist in the dbo.LegalEntity table and duplicate IDNumbers deleted, 
        /// until the number of records in the test.IDNumbers table equals MaxRecords
        /// </summary>
        public static void CleanIDNumbers()
        {
            _2AM sahls15 = new _2AM("sahls15");
            _2AM systest = new _2AM(Config.GetValue("SAHLDataBaseServer"));

            int MinRecords = 50;
            int MaxRecords = 200;
            int StartRow = 1;
            int RecordsPerPage = MaxRecords - systest.CountIDNumbers();
            int EndRow = StartRow + RecordsPerPage - 1;

            systest.CleanIDNumbers();

            if (systest.CountIDNumbers() < MinRecords)
            {
                while (systest.CountIDNumbers() < MaxRecords && sahls15.CountStagingIDNumbers() > StartRow)
                {
                    systest.InsertIDNumbers(sahls15.GetStagingIDNumbers(StartRow, EndRow));
                    systest.CleanIDNumbers();
                    RecordsPerPage = MaxRecords - systest.CountIDNumbers();
                    StartRow = EndRow + 1;
                    EndRow = StartRow + RecordsPerPage - 1;
                }
            }
            EnumeratedRow = 0;
        }

        /// <summary>
        /// Gets the next IDNumber in the list of cleaned ID numbers and incrememnts the EnumeratedRow
        /// </summary>
        /// <returns>Returns the next IDNumber in the list of cleaned ID numbers</returns>
        public static string GetNextIDNumber()
        {
            _2AM systest = new _2AM(Config.GetValue("SAHLDataBaseServer"));
            EnumeratedRow++;
            return systest.GetIDNumber(EnumeratedRow).Rows(0).Column(0).Value;
        }

        /// <summary>
        /// Resets the EnumeratedRow to its starting point
        /// </summary>
        public static void Reset()
        {
            EnumeratedRow = 0;
        }
    }
}

