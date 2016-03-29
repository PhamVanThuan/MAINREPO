using SAHL.Core.Data;

namespace SAHL.Services.FinanceDomain.Managers.Statements
{
    public class GetFinancialTransactionKeyByReferenceStatement : ISqlStatement<int>
    {
        public string Reference { get; protected set; }

        public GetFinancialTransactionKeyByReferenceStatement(string reference)
        {
            this.Reference = reference;
        }

        public string GetStatement()
        {
            return @"SELECT 
                        TOP 1 FinancialTransactionKey 
                    FROM [2AM].fin.FinancialTransaction 
                    WHERE 
                        Reference = convert(varchar(80),@Reference) 
                    AND IsRolledBack = 0";
        }
    }
}
