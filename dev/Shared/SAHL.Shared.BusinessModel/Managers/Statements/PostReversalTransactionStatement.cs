using SAHL.Core.Data;

namespace SAHL.Shared.BusinessModel.Managers.Statements
{
    public class PostReversalTransactionStatement : ISqlStatement<string>
    {
        public PostReversalTransactionStatement(int financialTransactionKey, string userId)
        {
            this.FinancialTransactionKey = financialTransactionKey;
            this.UserID = userId;
        }

        public int FinancialTransactionKey { get; protected set; }

        public string UserID { get; protected set; }

        public string GetStatement()
        {
            var sql = @"
                BEGIN TRY
                    BEGIN TRANSACTION
                        DECLARE @Msg varchar(1024)
                        EXEC [Process].[halo].[pRollbackFinancialTransaction]
                        @FinancialTransactionKey
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