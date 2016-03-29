using SAHL.Core.Data.Models._2AM;
using System;
using System.Linq;

namespace SAHL.Services.BankAccountDomain.Utils.ExceptionRoutines
{
    public class ExceptionRoutineC : IExceptionRoutine
    {
        private string accountNumber;

        public ExceptionRoutineC(string accountNumber)
        {
            this.accountNumber = accountNumber;
        }

        public int Execute(CDVExceptionsDataModel cdvExceptionModel)
        {
            if (accountNumber.Length != 11)
            {
                return 1;
            }

            int remainder = ValidationUtilities.CalculateRemainder(cdvExceptionModel.Weightings, cdvExceptionModel.FudgeFactor, cdvExceptionModel.Modulus, accountNumber);
            if (remainder == 0)
            {
                return 0;
            }

            return 1;
        }
    }
}