using SAHL.Core.Data.Models._2AM;
using System;
using System.Linq;

namespace SAHL.Services.BankAccountDomain.Utils.ExceptionRoutines
{
    public class ExceptionRoutineG : IExceptionRoutine
    {
        private string accountNumber;

        public ExceptionRoutineG(string accountNumber)
        {
            this.accountNumber = accountNumber;
        }

        public int Execute(CDVExceptionsDataModel cdvExceptionModel)
        {
            if (accountNumber.Length != 13)
            {
                return 1;
            }

            string shortAccountNumber = accountNumber.Substring(0, 8);
            string newAccountNumber = shortAccountNumber;

            int remainder = ValidationUtilities.CalculateRemainder(cdvExceptionModel.Weightings, cdvExceptionModel.FudgeFactor, cdvExceptionModel.Modulus, newAccountNumber);
            if (remainder == 0)
            {
                return 0;
            }

            if (shortAccountNumber[6] == shortAccountNumber[7])
            {
                remainder = ValidationUtilities.CalculateRemainder("0101010T0N0J0H0D070300", 10, 11, newAccountNumber);
                if (remainder == 0)
                {
                    return 0;
                }
            }
            return 1;
        }
    }
}