using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.BankAccountDomain.Managers.Statements
{
    public class SelectCDVExceptionsForCDVStatement : ISqlStatement<CDVExceptionsDataModel>
    {
        public int ACBBankCode { get; protected set; }

        public string ExceptionCode { get; protected set; }

        public int ACBTypeNumber { get; protected set; }

        public SelectCDVExceptionsForCDVStatement(int acbBankCode, string exceptionCode, int acbTypeNumber)
        {
            this.ACBBankCode = acbBankCode;
            this.ExceptionCode = exceptionCode;
            this.ACBTypeNumber = acbTypeNumber;
        }

        public string GetStatement()
        {
            return @"SELECT [CDVExceptionsKey]
                      ,[ACBBankCode]
                      ,[ACBTypeNumber]
                      ,[NoOfDigits]
                      ,[Weightings]
                      ,[Modulus]
                      ,[FudgeFactor]
                      ,[ExceptionCode]
                      ,[UserID]
                      ,[DateChange]
                  FROM [2AM].[dbo].[CDVExceptions]
                  WHERE ACBBankCode = @ACBBankCode
                  AND ExceptionCode = @ExceptionCode
                  AND ACBTypeNumber = @ACBTypeNumber";
        }
    }
}