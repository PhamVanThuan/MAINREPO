using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Attributes;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel.Repositories
{
    // not all Exception routines use all the parameters. Done like this for simplicity
    delegate int ExceptionRoutine(CDVExceptions_DAO r, CDVExceptions_DAO rnext, string p_BranchNo, string p_AccountNo, int p_Remainder, int p_AccountType, string p_UnstrippedAccountNumber);

    [FactoryType(typeof(ICDVRepository))]
    public class CDVValidator : AbstractRepositoryBase, ICDVRepository
    {
        string _errorMessage; // populated by an error message if false is returned.
        string _exceptionRoutine; // used to try and estimate code coverage in the test.

        public string ErrorMessage
        {
            get { return _errorMessage; }
        }

        public string ExceptionRoutine
        {
            get { return _exceptionRoutine; }
        }

        public bool ValidateAccountNo(string p_BranchNo, int p_AccType, String p_AccountNo)
        {
            int BankNumber = 0;
            
            // test the branch provide and get the BankNumber.
            IBankAccountRepository bar = RepositoryFactory.GetRepository<IBankAccountRepository>();
            IACBBranch p_Table = bar.GetACBBranchByKey(p_BranchNo);

            BankNumber = p_Table.ACBBank.Key;

            _exceptionRoutine = "";
            bool returnValue = PerformCDVCheck(p_AccountNo, p_BranchNo, BankNumber, p_AccType);
            return returnValue;
        }

        private bool PerformCDVCheck(String p_AccountNo, string p_BranchNo, int p_BankNumber, int p_AccType)
        {
            string HQL = "from CDV_DAO cdv where cdv.ACBBank.Key = ? and cdv.ACBBranch.Key = ? and cdv.ACBTypeNumber = ?";
            SimpleQuery<CDV_DAO> q = new SimpleQuery<CDV_DAO>(HQL, p_BankNumber, p_BranchNo, p_AccType);
            CDV_DAO[] cdv_Table = q.Execute();

            if (cdv_Table.Length == 0)
            {
                _errorMessage = "The account number not found in the bank file";
                return false;
            }
            foreach (CDV_DAO cdv in cdv_Table)
            {
                string UnstrippedAccountNumber = p_AccountNo;

                IList<AccountIndication_DAO> ait_Table = AccountIndication_DAO.FindAllByProperty("AccountIndicator", cdv.AccountIndicator);

                
                if (ait_Table.Count == 0)
                    throw new Exception("No entries found in the AccountTypeRecognition table");

                int PassCount = 0;
                foreach (AccountIndication_DAO ait in ait_Table)
                {
                    if (ait.AccountIndicator != cdv.AccountIndicator)
                        continue;

                    if (ait.Indicator == 'Y')
                    {
                        switch ((SAHL.Common.Globals.AccountIndicationTypes)ait.AccountIndicationType.Key)
                        {
                            case SAHL.Common.Globals.AccountIndicationTypes.AccountNumberRequired:
                                {
                                    if (AcctNoValidation(ref p_AccountNo, p_BankNumber, p_BranchNo, p_AccType))
                                        PassCount += 1;
                                }
                                break;
                            case SAHL.Common.Globals.AccountIndicationTypes.CDVRequired:
                                {
                                    if (CDVValidation(cdv, p_AccType, p_BranchNo, p_BankNumber, p_AccountNo, UnstrippedAccountNumber))
                                        PassCount += 1;
                                }
                                break;
                            case SAHL.Common.Globals.AccountIndicationTypes.ZeroAccountNumberAllowed:
                                {
                                    PassCount += 1;
                                }
                                break;

                        }

                    }
                    else
                    {
                        PassCount += 1;
                    }
                }
                if (PassCount == ait_Table.Count)
                {
                    return true;
                }
            }
            return false;
        }

        private bool AcctNoValidation(ref string p_AccountNo, int p_BankNumber, string p_BranchNo, int p_AccType)
        {
            string AccountNo = p_AccountNo;

            string HQL = "from AccountTypeRecognition_DAO atr where atr.ACBBank.Key = ? and atr.ACBType.Key = ?";
            SimpleQuery<AccountTypeRecognition_DAO> q = new SimpleQuery<AccountTypeRecognition_DAO>(HQL, p_BankNumber, p_AccType);
            AccountTypeRecognition_DAO[] atr_Table = q.Execute();

            if (atr_Table.Length < 1)
            {
                _errorMessage = "The account number not define in AccountTypeRecognition";
                return false;
            }
            // Need to see if we can match succesfully to any one of these rows
            foreach (AccountTypeRecognition_DAO atr in atr_Table)
            {
                AccountNo = p_AccountNo;
                _errorMessage = "";

                if (!(atr.NoOfDigits1 == null) && !(atr.NoOfDigits2 == null) && (AccountNo.Length != atr.NoOfDigits1 && AccountNo.Length != atr.NoOfDigits2))
                {
                    _errorMessage = "The account length is invalid";
                    continue;
                }

                if ((!(atr.NoOfDigits1 == null)) && (atr.NoOfDigits2 == null) && AccountNo.Length != atr.NoOfDigits1)
                {
                    _errorMessage = "The account length is invalid";
                    continue;
                }

                if (!(atr.RangeStart == null) && !(atr.RangeEnd == null) && (Int64.Parse(AccountNo) < atr.RangeStart || Int64.Parse(AccountNo) > atr.RangeEnd))
                {
                    _errorMessage = "The account no does not fall in a valid range";
                    continue;
                }

                AccountNo = MirrorStr(AccountNo);
                if (atr.DropDigits == 'Y')
                {

                    // check the values are rational here
                    if ((atr.EndDropDigits == null) == true || (atr.StartDropDigits == null) == true)
                        throw new Exception(string.Format("For Branch:{0} Accno:{1} DropDigits is true but Null values exist.. Key={2}", p_AccountNo, p_BranchNo, p_BranchNo, atr.Key));

                    if (atr.EndDropDigits - atr.StartDropDigits <= 0)
                        throw new Exception(string.Format("For Branch:{0} Accno:{1} DropDigits values not sensible.. Key={2}", p_AccountNo, p_BranchNo, p_BranchNo, atr.Key));

                    if (AccountNo.Length < atr.StartDropDigits)
                        throw new Exception(string.Format("For Branch:{0} Accno:{1} AccountNo.Length < atr.StartDropDigits test failed.. Key={2}", p_AccountNo, p_BranchNo, p_BranchNo, atr.Key));

                    if (AccountNo.Length < atr.EndDropDigits)
                        throw new Exception(string.Format("For Branch:{0} Accno:{1} AccountNo.Length < atr.EndDropDigits.. Key={2}", p_AccountNo, p_BranchNo, p_BranchNo, atr.Key));

                    // drop digits from(and including the StartDropDigits'th digit up to and excluding the EndDropDigits
                    // the digits are numbered from 1 the lhs of the reversed string

                    int startDropDigits = Convert.ToInt32(atr.StartDropDigits); // converting to int32 as it checks if value is null and returns 0 if so.

                    string RangeL = "";// = AccountNo.Substring(0, atr.StartDropDigits - 1);
                    string RangeR = "";// AccountNo.Substring(atr.EndDropDigits, atr.EndDropDigits - atr.StartDropDigits);
                    if (AccountNo.Length >= startDropDigits - 1)
                        RangeL = AccountNo.Substring(0, startDropDigits - 1);
                    else
                        RangeL = AccountNo;

                    int EndDropDigits = Convert.ToInt32(atr.EndDropDigits); // converting to int32 as it checks if value is null and returns 0 if so.
                    int Offset = EndDropDigits - 1;
                    int Numchar = AccountNo.Length - (EndDropDigits - 1);
                    if (AccountNo.Length >= Offset + Numchar)
                    {
                        RangeR = AccountNo.Substring(Offset, Numchar);
                    }
                    else
                    {
                        RangeR = "";
                    }

                    AccountNo = RangeL + RangeR;

                    //AccountNo = AccountNo.Substring(0, atr.StartDropDigits - 1) + AccountNo.Substring(atr.EndDropDigits, atr.EndDropDigits - atr.StartDropDigits);


                }

                int DigitNo1 = Convert.ToInt32(atr.DigitNo1); // converting to int32 as it checks if value is null and returns 0 if so.
                int DigitNo2 = Convert.ToInt32(atr.DigitNo2); // converting to int32 as it checks if value is null and returns 0 if so.

                if (!(atr.DigitNo1 == null) && (atr.MustEqual1 == null))
                {
                    throw new Exception(string.Format("For Branch:{0} Accno:{1} IsDigitNo1Null check failed.. Key={2}", p_AccountNo, p_BranchNo, atr.Key));
                }

                if (!(atr.DigitNo1 == null) && !(atr.MustEqual1 == null) && (atr.DigitNo1 > AccountNo.Length))
                {
                    throw new Exception(string.Format("For Branch:{0} Accno:{1} DigitNo1 less than length.. Key={2}", p_AccountNo, p_BranchNo, atr.Key));
                }

                if (!(atr.DigitNo2 == null) && (atr.MustEqual2 == null))
                {
                    throw new Exception(string.Format("For Branch:{0} Accno:{1} IsDigitNo2Null check failed.. Key={2}", p_AccountNo, p_BranchNo, atr.Key));
                }

                if (!(atr.DigitNo2 == null) && !(atr.MustEqual2 == null) && (atr.DigitNo2 > AccountNo.Length))
                {
                    throw new Exception(string.Format("For Branch:{0} Accno:{1} DigitNo2 less than length.. Key={2}", p_AccountNo, p_BranchNo, atr.Key));
                }

                if (!(atr.DigitNo1 == null) && (AccountNo[DigitNo1 - 1] != atr.MustEqual1.ToString()[0]))
                {
                    _errorMessage = "The account number starting digit/s are invalid";
                    continue;
                }

                if (!(atr.DigitNo2 == null) && (AccountNo[DigitNo2 - 1] != atr.MustEqual2.ToString()[0]))
                {
                    _errorMessage = "The account number starting digit/s are invalid";
                    continue;
                }

                //if we get here we have passed all the tests
                _errorMessage = "";
                p_AccountNo = MirrorStr(AccountNo);
                return true;

            }
            return false;
        }

        private bool CDVValidation(CDV_DAO cdv, int p_AccType, string p_BranchNo, int p_BankNumber, string p_AccountNo, string p_UnstrippedAccountNumber)
        {

            ExceptionRoutine pExceptionRoutine = null;

            String Weightings = cdv.Weightings.Trim();
            
            int Remainder = CalculateRemainder(cdv.Weightings, cdv.FudgeFactor, cdv.Modulus, p_AccountNo);
            
            if (Remainder == 0)
            {
                if (cdv.ExceptionCode == null)
                    return true;

                switch (cdv.ExceptionCode[0])
                {
                    case 'A':
                        pExceptionRoutine = A_ExceptionRoutine;
                        break;
                    case 'E':
                        if (E_ExceptionRoutine(p_BranchNo, p_AccountNo, Remainder, p_AccType) == 0)
                            return true;
                        break;
                    case 'D':
                        if (D_ExceptionRoutine(p_BranchNo, p_AccountNo, Remainder, p_AccType) == 0)
                            return true;
                        break;
                    case 'I':
                        if (I_ExceptionRoutine(p_BranchNo, p_AccountNo, Remainder, p_AccType) == 0)
                            return true;
                        break;
                    default:
                        return true;

                }


            }
            else
            {

                if (cdv.ExceptionCode == null)
                {
                    _errorMessage = "The CDV check for the account number failed. ";
                    //System.Console.WriteLine("CDVValidation Returning FALSE... " + ErrorMessage);

                    return false;
                }

                char te = Convert.ToChar(cdv.ExceptionCode[0]);
                switch (te)
                {
                    case 'A':
                        pExceptionRoutine = A_ExceptionRoutine;
                        break;
                    case 'B':
                        pExceptionRoutine = B_ExceptionRoutine;
                        break;
                    case 'C':
                        pExceptionRoutine = C_ExceptionRoutine;
                        break;
                    case 'D':
                        if (D_ExceptionRoutine(p_BranchNo, p_AccountNo, Remainder, p_AccType) == 0)
                            return true;
                        break;
                    case 'E':
                        _errorMessage = "The CDV check for the account number failed";
                        return false;
                    case 'F':
                        pExceptionRoutine = F_ExceptionRoutine;
                        break;
                    case 'G':
                        pExceptionRoutine = G_ExceptionRoutine;
                        break;
                    case 'H':
                        pExceptionRoutine = H_ExceptionRoutine;
                        break;
                    case 'I':
                        if (I_ExceptionRoutine(p_BranchNo, p_AccountNo, Remainder, p_AccType) == 0)
                            return true;
                        break;

                }
            }
            if (pExceptionRoutine != null)
            {
                //BankAccount.CDVExceptionsDataTable p_Table = new BankAccount.CDVExceptionsDataTable();
                //BankAccountWorker.GetCDVExceptionsByACBCodes(p_context, p_Table, p_BankNumber, cdv.ExceptionCode.Trim(), p_AccType);

                string HQL = "from CDVExceptions_DAO cdve where cdve.ACBBank.Key = ? and cdve.ExceptionCode = ? and cdve.ACBType.Key = ?";
                SimpleQuery<CDVExceptions_DAO> q = new SimpleQuery<CDVExceptions_DAO>(HQL, p_BankNumber, cdv.ExceptionCode.Trim(), p_AccType);
                CDVExceptions_DAO[] p_Table = q.Execute();


                if (p_Table.Length == 0)
                {
                    throw new Exception("CDVExceptionsByACBCodes returns nothing");
                }
                else
                {
                    foreach (CDVExceptions_DAO r in p_Table)
                    {
                        //int indx = p_Table.Rows.IndexOf(r);
                        int indx = Array.IndexOf(p_Table, r);
                        //int indx = p_Table;
                        CDVExceptions_DAO rnext = null;
                        if (indx + 1 < p_Table.Length)
                            rnext = p_Table[indx + 1];

                        //System.Console.WriteLine("CDVValidation CDVValidation pExceptionRoutine........." + indx + " " + rnext);


                        if (pExceptionRoutine(r, rnext, p_BranchNo, p_AccountNo, Remainder, p_AccType, p_UnstrippedAccountNumber) == 0)
                        {
                            return true;
                        }

                    }
                }
            }
            _errorMessage = "The CDV check for the account number failed";
            return false;

        }

        private int CalculateRemainder(string p_Weightings, int? p_FudgeFactor, int? p_Modulus, string p_AccountNo)
        {
            int cnt = 0, position = 0, sWeight = 0, Total = 0, sAccNo = 0;
            string AccountNumber = p_AccountNo;
            int FudgeFactor = Convert.ToInt32(p_FudgeFactor); // converting to int32 as it checks if value is null and returns 0 if so.
            int Modulus = Convert.ToInt32(p_Modulus); // converting to int32 as it checks if value is null and returns 0 if so.
            string Weightings = p_Weightings;
            int WeightLen = Weightings.Length / 2;

            while (WeightLen > AccountNumber.Length)
                AccountNumber = "0" + AccountNumber;

            
            for (cnt = 0; cnt < WeightLen; cnt++, position += 2)
            {
                if (AccountNumber[cnt] == ' ')
                    break;

                switch (Weightings[position + 1])
                {
                    case 'A':
                        sWeight = 10;
                        break;
                    case 'D':
                        sWeight = 13;
                        break;
                    case 'H':
                        sWeight = 17;
                        break;
                    case 'J':
                        sWeight = 19;
                        break;
                    case 'N':
                        sWeight = 23;
                        break;
                    case 'T':
                        sWeight = 29;
                        break;
                    default:
                        sWeight = int.Parse(Weightings.Substring(position, 2));
                        break;
                }
                sAccNo = int.Parse(AccountNumber.Substring(cnt, 1));
                Total += sWeight * sAccNo;

                
            }
            
            Total += FudgeFactor;
            if (Modulus == 0)
                return Total;
            return Total % Modulus;

        }

        private int A_ExceptionRoutine(CDVExceptions_DAO r, CDVExceptions_DAO rnext, string p_BranchNo, string p_AccountNo, int p_Remainder, int p_AccountType, string p_UnstrippedAccountNumber)
        {
            string AccountNumber = p_AccountNo;
            int Remainder = 0;
            //_exceptionRoutine = "A";
            if (AccountNumber.Length == 10)
            {
                //  code coverage - this segment of code not tested
                Remainder = CalculateRemainder("01020102010201020102010201020102", 0, 10, p_BranchNo + p_AccountNo);
                if (Remainder == 0)
                    return 0;
                if (AccountNumber[0] == '1' && Remainder == 1)
                    return 0;
            }
            else if (AccountNumber.Length == 11)
            {
                Remainder = CalculateRemainder(r.Weightings, r.FudgeFactor, r.Modulus, p_AccountNo);
                if (Remainder == 0)
                    return 0;
            }
            return 1;
        }

        private int B_ExceptionRoutine(CDVExceptions_DAO r, CDVExceptions_DAO rnext, string p_BranchNo, string p_AccountNo, int p_Remainder, int p_AccountType, string p_UnstrippedAccountNumber)
        {
            //_exceptionRoutine = "B";
            string AccountNumber = p_AccountNo;
            int Remainder = 0;
            Remainder = p_Remainder;// CalculateRemainder(p_context, r.Weightings, r.FudgeFactor, r.Modulus, p_AccountNo);

            if (Remainder == 0)
                return 0;
            string lsb = AccountNumber.Substring(AccountNumber.Length - 1, 1);
            if (Remainder == 1 && (lsb == "0" || lsb == "1"))
                return 0;
            return 1;
        }

        private int C_ExceptionRoutine(CDVExceptions_DAO r, CDVExceptions_DAO rnext, string p_BranchNo, string p_AccountNo, int p_Remainder, int p_AccountType, string p_UnstrippedAccountNumber)
        {

            //_exceptionRoutine = "C";
            string AccountNumber = p_AccountNo;
            int Remainder = 0;


            if (AccountNumber.Length != 11)
                return 1;

            Remainder = CalculateRemainder(r.Weightings, r.FudgeFactor, r.Modulus, p_AccountNo);
            if (Remainder == 0)
                return 0;

            return 1;
        }

        private int D_ExceptionRoutine(string p_BranchNo, string p_AccountNo, int p_Remainder, int p_AccountType)
        {
            string AccountNumber = p_AccountNo;
            int Remainder = p_Remainder;

            //_exceptionRoutine = "D";
            if (Remainder == 0 && (AccountNumber.Length == 8 || AccountNumber.Length == 10))
                return 0;

            if (Remainder != 0 && (AccountNumber.Length == 8 || AccountNumber.Length == 10) && AccountNumber[0] == '0')
                return 0;

            if (Remainder != 0 && (AccountNumber.Length == 8 || AccountNumber.Length == 10) && AccountNumber[0] != '0')
                return 1;

            if (AccountNumber.Length == 11 && p_AccountType == 1)
            {
                if (AccountNumber.Substring(0, 2) == "11")
                    return 0;
                else
                    return 1;
            }

            if (AccountNumber.Length == 11 && p_AccountType == 2)
            {
                if (AccountNumber.Substring(0, 2) == "13")
                    return 0;
                else if (Remainder == 1 && AccountNumber.Substring(10, 1) == "1")
                    return 0;
                else if (Remainder == 1 && AccountNumber.Substring(10, 1) == "0")
                    return 0;
            }
            return 1;
        }

        private int E_ExceptionRoutine(string p_BranchNo, string p_AccountNo, int p_Remainder, int p_AccountType)
        {
            //_exceptionRoutine = "E";
            if (p_AccountNo.Length >= 11)
            {
                // code coverage - this segment of code not tested. 
                // There is a contradiction in the bank serv document
                // It states the account number should be 10 long but this routine requires
                // testing of the 11 digit.
                int digit1 = int.Parse(p_AccountNo.Substring(p_AccountNo.Length - 1, 1));
                int digit2 = int.Parse(p_AccountNo.Substring(p_AccountNo.Length - 2, 1));
                int digit10 = int.Parse(p_AccountNo.Substring(p_AccountNo.Length - 10, 1));
                int digit11 = int.Parse(p_AccountNo.Substring(p_AccountNo.Length - 11, 1));

                if ((digit1 > 0 || digit2 > 0) && digit11 == 0 && digit10 > 0)
                    return 0;
            }

            // the following does not match the document
            if (p_AccountNo.Length == 10)
            {

                //  code coverage - this segment of code tested with the proviso above

                int digit1 = int.Parse(p_AccountNo.Substring(p_AccountNo.Length - 1, 1));
                int digit2 = int.Parse(p_AccountNo.Substring(p_AccountNo.Length - 2, 1));
                int digit10 = int.Parse(p_AccountNo.Substring(p_AccountNo.Length - 10, 1));

                if ((digit1 > 0 || digit2 > 0) && digit10 > 0)
                    return 0;
            }

            return 1;
        }

        private int F_ExceptionRoutine(CDVExceptions_DAO r, CDVExceptions_DAO rnext, string p_BranchNo, string p_AccountNo, int p_Remainder, int p_AccountType, string p_UnstrippedAccountNumber)
        {
            int Remainder = 0;
            string AccountNumber = p_AccountNo.Trim();

            //_exceptionRoutine = "F";
            if (AccountNumber.Length == 11 && AccountNumber.Substring(0, 2) == "53")
            {
                // moreton and maree think this is dumb
                Remainder = CalculateRemainder("0000000000000000000000", 0, 0, AccountNumber);
                if (Remainder == 0)
                    return 0;

            }

            if (r.Weightings == "0107030209080704030201")
            {
                Remainder = CalculateRemainder(r.Weightings, r.FudgeFactor, r.Modulus, AccountNumber);
                if (Remainder == 0)
                    return 0;
            }

            if (r.Weightings == "0104030207060504030201")
            {
                Remainder = CalculateRemainder(r.Weightings, r.FudgeFactor, r.Modulus, AccountNumber);
                if (Remainder == 0)
                    return 0;
            }

            if (r.Weightings == "0504030207060504030201")
            {
                Remainder = CalculateRemainder(r.Weightings, r.FudgeFactor, r.Modulus, AccountNumber);
                if (Remainder == 0)
                    return 0;

                if (Remainder == 1 && AccountNumber.Length == 10 && (AccountNumber[9] == '0' || AccountNumber[9] == '1'))
                    return 0;

                if (Remainder == 1 && AccountNumber.Length == 11 && (AccountNumber[10] == '0' || AccountNumber[10] == '1'))
                    return 0;

            }

            if (r.Weightings == "0101030207060504030201")
            {
                Remainder = CalculateRemainder(r.Weightings, r.FudgeFactor, r.Modulus, AccountNumber);
                if (Remainder == 0)
                    return 0;
                if (Remainder != 0 && AccountNumber.Length < 10)
                {
                    int LastDigit = int.Parse(AccountNumber.Substring(AccountNumber.Length - 1, 1));
                    LastDigit = (LastDigit + 6) % 10;
                    string NewDigit = LastDigit.ToString();
                    string NewAccountNumber = AccountNumber.Substring(0, AccountNumber.Length - 1) + NewDigit;
                    Remainder = CalculateRemainder(r.Weightings, r.FudgeFactor, r.Modulus, NewAccountNumber);
                    if (Remainder == 0)
                        return 0;
                }
            }

            if (r.Weightings == "0104030209080704030201")
            {
                Remainder = CalculateRemainder(r.Weightings, r.FudgeFactor, r.Modulus, AccountNumber);
                if (Remainder == 0)
                    return 0;
            }

            return 1;
        }

        private int G_ExceptionRoutine(CDVExceptions_DAO r, CDVExceptions_DAO rnext, string p_BranchNo, string p_AccountNo, int p_Remainder, int p_AccountType, string p_UnstrippedAccountNumber)
        {

            p_AccountNo = p_UnstrippedAccountNumber;


            // code coverage - this segment of code not tested. 

            //_exceptionRoutine = "G";
            
            if (p_AccountNo.Length != 13)
                return 1;

            string ShortAccountNumber = p_AccountNo.Substring(0, 8);
            string NewAccountNumber = ShortAccountNumber;

            int Remainder = CalculateRemainder(r.Weightings, r.FudgeFactor, r.Modulus, NewAccountNumber);
            if (Remainder == 0)
                return 0;

            if (ShortAccountNumber[6] == ShortAccountNumber[7])
            {
                Remainder = CalculateRemainder("0101010T0N0J0H0D070300", 10, 11, NewAccountNumber);
                if (Remainder == 0)
                    return 0;
            }
            return 1;
        }

        private int H_ExceptionRoutine(CDVExceptions_DAO r, CDVExceptions_DAO rnext, string p_BranchNo, string p_AccountNo, int p_Remainder, int p_AccountType, string p_UnstrippedAccountNumber)
        {

            p_AccountNo = p_UnstrippedAccountNumber;
            //_exceptionRoutine = "H";
            
            if (p_AccountNo.Length != 13)
                return 1;

            string ShortAccountNumber = p_AccountNo.Substring(0, 8);
            string NewAccountNumber = "000" + ShortAccountNumber;

            int Remainder = CalculateRemainder(r.Weightings, r.FudgeFactor, r.Modulus, NewAccountNumber);
            if (Remainder == 0)
                return 0;

            if (ShortAccountNumber[6] == ShortAccountNumber[7])
            {
                // asume using NewAccount here??
                Remainder = CalculateRemainder("0101010T0N0J0H0D070300", 10, 11, NewAccountNumber);
                if (Remainder == 0)
                    return 0;
            }
            return 1;
        }

        private int I_ExceptionRoutine(string p_BranchNo, string p_AccountNo, int p_Remainder, int p_AccountType)
        {

            //_exceptionRoutine = "I";
            // Needs a bit of massaging form the Bankserv doc
            string AccountNumber = p_AccountNo;
            if (AccountNumber.Length < 11)
                return 1;

            AccountNumber = AccountNumber.Substring(AccountNumber.Length - 11, 11);

            if (AccountNumber[0] == '2' || AccountNumber[0] == '4')
                return 0;

            return 1;
        }

        private string MirrorStr(string InAccNo)
        {
            char[] inchars = InAccNo.ToCharArray();
            char[] outchars = InAccNo.ToCharArray();
            for (int counter = 0; counter < inchars.GetLength(0); counter++)
                outchars[inchars.GetLength(0) - counter - 1] = inchars[counter];
            return new string(outchars);
        }

}
}
