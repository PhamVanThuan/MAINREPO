using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using SAHL.Services.BankAccountDomain.Managers;
using SAHL.Services.BankAccountDomain.Utils.ExceptionRoutines;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.BankAccountDomain.Utils
{
    public class CdvValidationManager : ICdvValidationManager
    {
        private string branchCode;
        private int bankCode;
        private int accountType;
        private string accountNumber;
        private ISystemMessageCollection systemMessages;
        private ICDVDataManager dataManager;
        private List<AccountTypeRecognitionDataModel> accountTypeRecognitions;
        private List<AccountIndicationDataModel> accountIndications;

        public CdvValidationManager(ISystemMessageCollection systemMessages, ICDVDataManager dataManager)
        {
            this.systemMessages = systemMessages;
            this.dataManager = dataManager;
        }

        public bool ValidateAccountNumber(string branchCode, int accountType, String accountNumber)
        {
            this.accountNumber = accountNumber;
            this.accountType = accountType;
            this.branchCode = branchCode;
            var bank = this.dataManager.GetBankForACBBranch(branchCode).FirstOrDefault();
            if (bank == null)
            {
                this.systemMessages.AddMessage(new SystemMessage(string.Format("No Bank found for branch code {0}", branchCode), SystemMessageSeverityEnum.Error));
                return false;
            }
            this.bankCode = bank.ACBBankCode;
            this.accountTypeRecognitions = this.dataManager.GetAccountTypeRecognitions(bankCode, accountType).ToList();
            if (accountTypeRecognitions.Count < 1)
            {
                systemMessages.AddMessage(new SystemMessage("The account number is not defined in the AccountTypeRecognition table", SystemMessageSeverityEnum.Error));
                return false;
            }
            this.accountIndications = this.dataManager.GetAccountIndications().ToList();
            bool returnValue = PerformCDVCheck();
            return returnValue;
        }

        private bool PerformCDVCheck()
        {
            var cdvs = this.dataManager.GetCDVs(bankCode, branchCode, accountType).ToList();
            if (cdvs.Count == 0)
            {
                systemMessages.AddMessage(new SystemMessage("The account number not found in the bank file", SystemMessageSeverityEnum.Error));
                return false;
            }

            foreach (var cdv in cdvs)
            {
                var cdvAccountIndications = accountIndications.Where(x => x.AccountIndicator == cdv.AccountIndicator).ToList();
                var passCount = GetPassCountForAccountIndications(cdvAccountIndications, cdv);
                if (passCount == cdvAccountIndications.Count)
                {
                    return true;
                }
            }
            return false;
        }

        private int GetPassCountForAccountIndications(List<AccountIndicationDataModel> accountIndications, CDVDataModel cdv)
        {
            var unstrippedAccountNumber = accountNumber;
            int passCount = 0;

            foreach (var accountIndication in accountIndications)
            {
                if (accountIndication.Indicator == "Y")
                {
                    switch ((AccountIndicationType)accountIndication.AccountIndicationTypeKey)
                    {
                        case AccountIndicationType.AccountNumberRequired:
                            {
                                if (AccountNumberValidation())
                                {
                                    passCount += 1;
                                }
                            }
                            break;

                        case AccountIndicationType.CDVRequired:
                            {
                                if (CDVValidation(cdv, accountType, branchCode, bankCode, accountNumber, unstrippedAccountNumber))
                                {
                                    passCount += 1;
                                }
                            }
                            break;

                        case AccountIndicationType.ZeroAccountNumberAllowed:
                            {
                                passCount += 1;
                            }
                            break;
                    }
                }
                else
                {
                    passCount += 1;
                }
            }
            return passCount;
        }

        private bool AccountNumberValidation()
        {
            // Need to see if we can match succesfully to any one of these rows
            var errorMessage = "";
            foreach (var recognition in accountTypeRecognitions)
            {
                if ((recognition.NoOfDigits1 != null) && (recognition.NoOfDigits2 != null) && (accountNumber.Length != recognition.NoOfDigits1 && accountNumber.Length != recognition.NoOfDigits2))
                {
                    errorMessage = "The account number length is invalid";
                    continue;
                }

                if (((recognition.NoOfDigits1 != null)) && (recognition.NoOfDigits2 == null) && accountNumber.Length != recognition.NoOfDigits1)
                {
                    errorMessage = "The account number length is invalid";
                    continue;
                }

                if ((recognition.RangeStart != null) && (recognition.RangeEnd != null) && (Int64.Parse(accountNumber) < recognition.RangeStart || Int64.Parse(accountNumber) > recognition.RangeEnd))
                {
                    errorMessage = "The account no does not fall in a valid range";
                    continue;
                }

                var mirroredAccountNumber = ValidationUtilities.MirrorStr(accountNumber);
                if (recognition.DropDigits == "Y")
                {
                    ValidateAccountTypeRecognition(recognition, mirroredAccountNumber);

                    int startDropDigits = recognition.StartDropDigits ?? 0;
                    int EndDropDigits = recognition.EndDropDigits ?? 0;
                    string rangeLeft, rangeRight = String.Empty;
                    int Offset = EndDropDigits - 1;
                    int Numchar = mirroredAccountNumber.Length - (EndDropDigits - 1);

                    if (mirroredAccountNumber.Length >= startDropDigits - 1)
                    {
                        rangeLeft = mirroredAccountNumber.Substring(0, startDropDigits - 1);
                    }
                    else
                    {
                        rangeLeft = mirroredAccountNumber;
                    }

                    if (mirroredAccountNumber.Length >= Offset + Numchar)
                    {
                        rangeRight = mirroredAccountNumber.Substring(Offset, Numchar);
                    }
                    else
                    {
                        rangeRight = String.Empty;
                    }

                    mirroredAccountNumber = rangeLeft + rangeRight;
                }

                int DigitNo1 = recognition.DigitNo1 ?? 0;
                int DigitNo2 = recognition.DigitNo2 ?? 0;

                ValidateAccountTypeRecognitionDigitNumber(recognition, mirroredAccountNumber);

                if (!(recognition.DigitNo1 == null) && (mirroredAccountNumber[DigitNo1 - 1] != recognition.MustEqual1.ToString()[0]))
                {
                    errorMessage = "The account number starting digit/s are invalid";
                    continue;
                }

                if (!(recognition.DigitNo2 == null) && (mirroredAccountNumber[DigitNo2 - 1] != recognition.MustEqual2.ToString()[0]))
                {
                    errorMessage = "The account number starting digit/s are invalid";
                    continue;
                }
                return true;
            }
            if (!String.IsNullOrEmpty(errorMessage))
            {
                systemMessages.AddMessage(new SystemMessage(errorMessage, SystemMessageSeverityEnum.Error));
            }
            return false;
        }

        private bool CDVValidation(CDVDataModel cdv, int accountType, string p_BranchNo, int p_BankNumber, string p_AccountNo, string p_UnstrippedAccountNumber)
        {
            IExceptionRoutine exceptionRoutine = null;

            String Weightings = cdv.Weightings.Trim();

            int remainder = ValidationUtilities.CalculateRemainder(cdv.Weightings, cdv.FudgeFactor, cdv.Modulus, p_AccountNo);

            if (remainder == 0)
            {
                if (cdv.ExceptionCode == null)
                {
                    return true;
                }

                switch (cdv.ExceptionCode[0])
                {
                    case 'A':
                        exceptionRoutine = new ExceptionRoutineA(branchCode, accountNumber);
                        break;

                    case 'E':
                        if (ExceptionRoutineE(accountNumber) == 0)
                        {
                            return true;
                        }
                        break;

                    case 'D':
                        if (ExceptionRoutineD(accountNumber, remainder, accountType) == 0)
                        {
                            return true;
                        }
                        break;

                    case 'I':
                        if (ExceptionRoutineI(accountNumber) == 0)
                        {
                            return true;
                        }
                        break;

                    default:
                        return true;
                }
            }
            else
            {
                if (cdv.ExceptionCode == null)
                {
                    this.systemMessages.AddMessage(new SystemMessage("The CDV check for the account number failed.", SystemMessageSeverityEnum.Error));
                    return false;
                }

                char exceptionCode = cdv.ExceptionCode[0];
                switch (exceptionCode)
                {
                    case 'A':
                        exceptionRoutine = new ExceptionRoutineA(branchCode, accountNumber);
                        break;

                    case 'B':
                        if (ExceptionRoutineB(accountNumber, remainder) == 0)
                        {
                            return true;
                        }
                        break;

                    case 'C':
                        exceptionRoutine = new ExceptionRoutineC(accountNumber);
                        break;

                    case 'D':
                        if (ExceptionRoutineD(accountNumber, remainder, accountType) == 0)
                        {
                            return true;
                        }
                        break;

                    case 'E':
                        this.systemMessages.AddMessage(new SystemMessage("The CDV check for the account number failed", SystemMessageSeverityEnum.Error));
                        return false;

                    case 'F':
                        exceptionRoutine = new ExceptionRoutineF(accountNumber);
                        break;

                    case 'G':
                        exceptionRoutine = new ExceptionRoutineG(accountNumber);
                        break;

                    case 'H':
                        exceptionRoutine = new ExceptionRoutineH(accountNumber);
                        break;

                    case 'I':
                        if (ExceptionRoutineI(accountNumber) == 0)
                        {
                            return true;
                        }
                        break;
                    default:
                        break;
                }
            }
            if (exceptionRoutine != null)
            {
                var cdvExceptions = this.dataManager.GetCDVExceptions(bankCode, cdv.ExceptionCode, accountType).ToList();
                if (cdvExceptions.Count == 0)
                {
                    throw new Exception("CDVExceptionsByACBCodes returns nothing");
                }
                else
                {
                    foreach (var cdvException in cdvExceptions)
                    {
                        if (exceptionRoutine.Execute(cdvException) == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            this.systemMessages.AddMessage(new SystemMessage("The CDV check for the account number failed", SystemMessageSeverityEnum.Error));
            return false;
        }

        private void ValidateAccountTypeRecognitionDigitNumber(AccountTypeRecognitionDataModel recognition, string mirroredAccountNumber)
        {
            if (!(recognition.DigitNo1 == null) && (recognition.MustEqual1 == null))
            {
                throw new Exception(string.Format("For Branch:{0} Accno:{1} IsDigitNo1Null check failed. Key={2}", branchCode, accountNumber, recognition.AccountTypeRecognitionKey));
            }

            if (!(recognition.DigitNo1 == null) && !(recognition.MustEqual1 == null) && (recognition.DigitNo1 > mirroredAccountNumber.Length))
            {
                throw new Exception(string.Format("For Branch:{0} Accno:{1} DigitNo1 less than length. Key={2}", branchCode, accountNumber, recognition.AccountTypeRecognitionKey));
            }

            if (!(recognition.DigitNo2 == null) && (recognition.MustEqual2 == null))
            {
                throw new Exception(string.Format("For Branch:{0} Accno:{1} IsDigitNo2Null check failed. Key={2}", branchCode, accountNumber, recognition.AccountTypeRecognitionKey));
            }

            if (!(recognition.DigitNo2 == null) && !(recognition.MustEqual2 == null) && (recognition.DigitNo2 > mirroredAccountNumber.Length))
            {
                throw new Exception(string.Format("For Branch:{0} Accno:{1} DigitNo2 less than length. Key={2}", branchCode, accountNumber, recognition.AccountTypeRecognitionKey));
            }
        }

        private void ValidateAccountTypeRecognition(AccountTypeRecognitionDataModel recognition, string mirroredAccountNumber)
        {
            if ((recognition.EndDropDigits == null) || (recognition.StartDropDigits == null))
            {
                throw new Exception(string.Format("For Branch:{0} Accno:{1} DropDigits is true but Null values exist. Key={2}", branchCode, accountNumber, recognition.AccountTypeRecognitionKey));
            }
            if (recognition.EndDropDigits - recognition.StartDropDigits <= 0)
            {
                throw new Exception(string.Format("For Branch:{0} Accno:{1} DropDigits values are not sensible. Key={2}", branchCode, accountNumber, recognition.AccountTypeRecognitionKey));
            }
            if (mirroredAccountNumber.Length < recognition.StartDropDigits)
            {
                throw new Exception(string.Format("For Branch:{0} Accno:{1} AccountNo.Length < accountTypeRecognition.StartDropDigits test failed. Key={2}", branchCode, accountNumber,
                    recognition.AccountTypeRecognitionKey));
            }
            if (mirroredAccountNumber.Length < recognition.EndDropDigits)
            {
                throw new Exception(string.Format("For Branch:{0} Accno:{1} AccountNo.Length < accountTypeRecognition.EndDropDigits. Key={2}", branchCode, accountNumber,
                    recognition.AccountTypeRecognitionKey));
            }
        }

        private int ExceptionRoutineB(string accountNumber, int remainder)
        {
            if (remainder == 0)
            {
                return 0;
            }
            string lsb = accountNumber.Substring(accountNumber.Length - 1, 1);
            if (remainder == 1 && (lsb == "0" || lsb == "1"))
            {
                return 0;
            }
            return 1;
        }

        private int ExceptionRoutineE(string accountNumber)
        {
            if (accountNumber.Length >= 11)
            {
                // code coverage - this segment of code not tested.
                // There is a contradiction in the bank serv document
                // It states the account number should be 10 long but this routine requires
                // testing of the 11 digit.
                int digit1 = int.Parse(accountNumber.Substring(accountNumber.Length - 1, 1));
                int digit2 = int.Parse(accountNumber.Substring(accountNumber.Length - 2, 1));
                int digit10 = int.Parse(accountNumber.Substring(accountNumber.Length - 10, 1));
                int digit11 = int.Parse(accountNumber.Substring(accountNumber.Length - 11, 1));

                if ((digit1 > 0 || digit2 > 0) && digit11 == 0 && digit10 > 0)
                {
                    return 0;
                }
            }

            // the following does not match the document
            if (accountNumber.Length == 10)
            {
                int digit1 = int.Parse(accountNumber.Substring(accountNumber.Length - 1, 1));
                int digit2 = int.Parse(accountNumber.Substring(accountNumber.Length - 2, 1));
                int digit10 = int.Parse(accountNumber.Substring(accountNumber.Length - 10, 1));

                if ((digit1 > 0 || digit2 > 0) && digit10 > 0)
                {
                    return 0;
                }
            }
            return 1;
        }

        private int ExceptionRoutineD(string accountNumber, int remainder, int accountType)
        {
            if (remainder == 0 && (accountNumber.Length == 8 || accountNumber.Length == 10))
            {
                return 0;
            }

            if (remainder != 0 && (accountNumber.Length == 8 || accountNumber.Length == 10) && accountNumber[0] == '0')
            {
                return 0;
            }

            if (remainder != 0 && (accountNumber.Length == 8 || accountNumber.Length == 10) && accountNumber[0] != '0')
            {
                return 1;
            }

            if (accountNumber.Length == 11 && accountType == 1)
            {
                if (accountNumber.Substring(0, 2) == "11")
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }

            if (accountNumber.Length == 11 && accountType == 2)
            {
                if (accountNumber.Substring(0, 2) == "13")
                {
                    return 0;
                }
                else if (remainder == 1 && accountNumber.Substring(10, 1) == "1")
                {
                    return 0;
                }
                else if (remainder == 1 && accountNumber.Substring(10, 1) == "0")
                {
                    return 0;
                }
            }
            return 1;
        }

        private int ExceptionRoutineI(string accountNumber)
        {
            if (accountNumber.Length < 11)
            {
                return 1;
            }

            accountNumber = accountNumber.Substring(accountNumber.Length - 11, 11);

            if (accountNumber[0] == '2' || accountNumber[0] == '4')
            {
                return 0;
            }

            return 1;
        }
    }
}