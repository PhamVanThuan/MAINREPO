using SAHL.Core.Data;
using SAHL.Core.Data.Models.EventProjection;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedLastMonth.Statements
{
    public class UpdateAttorneyInvoicesNotProcessedLastMonthStatement : ISqlStatement<AttorneyInvoicesNotProcessedLastMonthDataModel>
    {
        public int Count { get; protected set; }
        public decimal Value { get; protected set; }

        public UpdateAttorneyInvoicesNotProcessedLastMonthStatement(int Count, decimal Value)
        {
            this.Count = Count;
            this.Value = Value;
        }

        public string GetStatement()
        {
            return @"IF EXISTS (select * from [EventProjection].[projection].[AttorneyInvoicesNotProcessedLastMonth]) 
                        UPDATE [EventProjection].[projection].[AttorneyInvoicesNotProcessedLastMonth] SET Count = @Count, Value = @Value 
                     ELSE
                    	INSERT INTO [EventProjection].[projection].[AttorneyInvoicesNotProcessedLastMonth] ([Count], [Value]) VALUES (@Count, @Value)";
        }
    }
}