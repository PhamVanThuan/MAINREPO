using SAHL.Core.Data;
using System;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown.Statements
{
    public class MergeAttorneyMonthlyBreakdownRecordForAttorneyStatement : ISqlStatement<object>
    {
        public Guid AttorneyId { get; protected set; }

        public string AttorneyName { get; protected set; }

        public MergeAttorneyMonthlyBreakdownRecordForAttorneyStatement(Guid attorneyId, string attorneyName)
        {
            this.AttorneyId = attorneyId;
            this.AttorneyName = attorneyName;
        }

        public string GetStatement()
        {
            return @"MERGE [EventProjection].[projection].AttorneyInvoiceMonthlyBreakdown WITH(HOLDLOCK) AS target
                    USING (SELECT @AttorneyId, @AttorneyName)
                    AS source (AttorneyId, AttorneyName)
                    ON (target.AttorneyId = source.AttorneyId)
                    WHEN MATCHED THEN
                        UPDATE SET
                        [AttorneyName] = source.AttorneyName
                    WHEN NOT MATCHED THEN
                        INSERT ([AttorneyId] ,[AttorneyName],[Unprocessed])
                        VALUES (source.AttorneyId, source.AttorneyName, 0);";
        }
    }
}