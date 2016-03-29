using SAHL.Core.Data.Models._2AM;
using System;
using System.Linq;

namespace SAHL.Services.BankAccountDomain.Utils.ExceptionRoutines
{
    public class ExceptionRoutineF : IExceptionRoutine
    {
        private string accountNumber;

        public ExceptionRoutineF(string accountNumber)
        {
            this.accountNumber = accountNumber;
        }

        public int Execute(CDVExceptionsDataModel cdvExceptionModel)
        {
            int remainder = 0;

            if (accountNumber.Length == 11 && accountNumber.Substring(0, 2) == "53")
            {
                remainder = ValidationUtilities.CalculateRemainder("0000000000000000000000", 0, 0, accountNumber);
                if (remainder == 0)
                {
                    return 0;
                }
            }

            if (cdvExceptionModel.Weightings == "0107030209080704030201")
            {
                remainder = ValidationUtilities.CalculateRemainder(cdvExceptionModel.Weightings, cdvExceptionModel.FudgeFactor, cdvExceptionModel.Modulus, accountNumber);
                if (remainder == 0)
                {
                    return 0;
                }
            }

            if (cdvExceptionModel.Weightings == "0104030207060504030201")
            {
                remainder = ValidationUtilities.CalculateRemainder(cdvExceptionModel.Weightings, cdvExceptionModel.FudgeFactor, cdvExceptionModel.Modulus, accountNumber);
                if (remainder == 0)
                {
                    return 0;
                }
            }

            if (cdvExceptionModel.Weightings == "0504030207060504030201")
            {
                remainder = ValidationUtilities.CalculateRemainder(cdvExceptionModel.Weightings, cdvExceptionModel.FudgeFactor, cdvExceptionModel.Modulus, accountNumber);
                if (remainder == 0)
                {
                    return 0;
                }

                if (remainder == 1 && accountNumber.Length == 10 && (accountNumber[9] == '0' || accountNumber[9] == '1'))
                {
                    return 0;
                }

                if (remainder == 1 && accountNumber.Length == 11 && (accountNumber[10] == '0' || accountNumber[10] == '1'))
                {
                    return 0;
                }
            }

            if (cdvExceptionModel.Weightings == "0101030207060504030201")
            {
                remainder = ValidationUtilities.CalculateRemainder(cdvExceptionModel.Weightings, cdvExceptionModel.FudgeFactor, cdvExceptionModel.Modulus, accountNumber);
                if (remainder == 0)
                {
                    return 0;
                }
                if (remainder != 0 && accountNumber.Length < 10)
                {
                    int lastDigit = int.Parse(accountNumber.Substring(accountNumber.Length - 1, 1));
                    lastDigit = (lastDigit + 6) % 10;
                    string newDigit = lastDigit.ToString();
                    string newAccountNumber = accountNumber.Substring(0, accountNumber.Length - 1) + newDigit;
                    remainder = ValidationUtilities.CalculateRemainder(cdvExceptionModel.Weightings, cdvExceptionModel.FudgeFactor, cdvExceptionModel.Modulus, newAccountNumber);
                    if (remainder == 0)
                    {
                        return 0;
                    }
                }
            }

            if (cdvExceptionModel.Weightings == "0104030209080704030201")
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