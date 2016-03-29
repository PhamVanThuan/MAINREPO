using SAHL.Core.Data;
using SAHL.Core.Data.Models.EventProjection;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoicesPaidLastMonth.Statements
{
    public class UpdateAttorneyInvoicesPaidLastMonthStatement : ISqlStatement<AttorneyInvoicesPaidLastMonthDataModel>
    {
        public int Count { get; protected set; }
        public decimal Value { get; protected set; }

        public UpdateAttorneyInvoicesPaidLastMonthStatement(int Count, decimal Value)
        {
            this.Count = Count;
            this.Value = Value;
        }

        public string GetStatement()
        {
            return @"IF EXISTS (select * from [EventProjection].[projection].[AttorneyInvoicesPaidLastMonth]) 
                        UPDATE [EventProjection].[projection].[AttorneyInvoicesPaidLastMonth] SET Count = @Count, Value = @Value 
                     ELSE
                    	INSERT INTO [EventProjection].[projection].[AttorneyInvoicesPaidLastMonth] ([Count], [Value]) VALUES (@Count, @Value)";
        }
    }
}