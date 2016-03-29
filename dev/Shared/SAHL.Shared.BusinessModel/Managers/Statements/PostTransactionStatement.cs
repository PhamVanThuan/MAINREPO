using SAHL.Core.Data;
using System;

namespace SAHL.Shared.BusinessModel.Managers.Statements
{
    public class PostTransactionStatement : ISqlStatement<string>
    {
        public PostTransactionStatement(int financialServiceKey, int transactionTypeKey, DateTime effectiveDate, decimal amount, string reference, string userId)
        {
            this.FinancialServiceKey = financialServiceKey;
            this.TransactionTypeKey = transactionTypeKey;
            this.EffectiveDate = effectiveDate;
            this.Amount = amount;
            this.Reference = reference;
            this.UserID = userId;
        }

        public int FinancialServiceKey { get; protected set; }

        public int TransactionTypeKey { get; protected set; }

        public DateTime EffectiveDate { get; protected set; }

        public decimal Amount { get; protected set; }

        public string Reference { get; protected set; }

        public string UserID { get; protected set; }

        public string GetStatement()
        {
            var sql = @"
                     BEGIN TRY
	                    BEGIN TRANSACTION
		                    DECLARE @Msg varchar(1024)
		                    EXEC [Process].[halo].[pProcessTranWithEffectiveDate]
		                    @FinancialServiceKey
		                    ,@TransactionTypeKey
		                    ,@EffectiveDate
		                    ,@Amount
		                    ,@Reference
		                    ,@UserID
		                    ,@Msg OUTPUT
		                    SELECT @Msg
	                    COMMIT
                    END TRY
                    BEGIN CATCH
	                    ROLLBACK
                    END CATCH";

            return sql;
        }
    }
}