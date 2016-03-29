using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.BankAccountDomain.Managers.Statements
{
    public class SelectAccountTypeRecognitionForACBBankStatement : ISqlStatement<AccountTypeRecognitionDataModel>
    {
        public int ACBBankCode { get; protected set; }

        public int ACBTypeNumber { get; protected set; }

        public SelectAccountTypeRecognitionForACBBankStatement(int acbBankCode, int acbTypeNumber)
        {
            this.ACBBankCode = acbBankCode;
            this.ACBTypeNumber = acbTypeNumber;
        }

        public string GetStatement()
        {
            return @"SELECT [AccountTypeRecognitionKey]
                      ,[ACBBankCode]
                      ,[ACBTypeNumber]
                      ,[RangeStart]
                      ,[RangeEnd]
                      ,[NoOfDigits1]
                      ,[NoOfDigits2]
                      ,[DigitNo1]
                      ,[MustEqual1]
                      ,[DigitNo2]
                      ,[MustEqual2]
                      ,[DropDigits]
                      ,[StartDropDigits]
                      ,[EndDropDigits]
                      ,[UserID]
                      ,[DateChange]
                  FROM [2AM].[dbo].[AccountTypeRecognition]
                  WHERE ACBBankCode = @ACBBankCode
                  AND ACBTypeNumber = @ACBTypeNumber";
        }
    }
}