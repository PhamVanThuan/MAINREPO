using SAHL.Core.Data.Models._2AM;
using System;
using System.Linq;

namespace SAHL.Services.BankAccountDomain.Utils.ExceptionRoutines
{
    public class ExceptionRoutineA : IExceptionRoutine
    {
        private string accountNumber;
        private string branchNumber;

        public ExceptionRoutineA(string branchNumber, string accountNumber)
        {
            this.branchNumber = branchNumber;
            this.accountNumber = accountNumber;
        }

        public int Execute(CDVExceptionsDataModel cdvExceptionModel)
        {
            int remainder = 0;
            if (accountNumber.Length == 10)
            {
                remainder = ValidationUtilities.CalculateRemainder("01020102010201020102010201020102", 0, 10, branchNumber + accountNumber);
                if (remainder == 0)
                {
                    return 0;
                }
                if (accountNumber[0] == '1' && remainder == 1)
                {
                    return 0;
                }
            }
            else if (accountNumber.Length == 11)
            {
                remainder = ValidationUtilities.CalculateRemainder(cdvExceptionModel.Weightings, cdvExceptionModel.FudgeFactor, cdvExceptionModel.Modulus, accountNumber);
                if (remainder == 0)
                {
                    return 0;
                }
            }
            return 1;
        }
    }
}