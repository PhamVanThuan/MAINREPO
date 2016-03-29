using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Strings;
using System;
using System.Linq;

namespace SAHL.Core.BusinessModel.Validation
{
    public class ValidationUtils : IValidationUtils
    {
        /// <summary>
        /// Use to Validate a South African ID Number
        /// </summary>
        public bool ValidateIDNumber(string idNumber)
        {
            var evenArray = new string[7];
            var newNumber = "0" + idNumber;

            if (idNumber.Length != 13)
            {
                return false;
            }

            if (!idNumber.IsDigitsOnly())
            {
                return false;
            }

            int total = GetOdd(newNumber) + GetEven(newNumber, evenArray);

            var totalString = total.ToString();

            var checkDigit = GetCheckDigit(totalString);

            return checkDigit == Int32.Parse(idNumber[12].ToString());
        }

        private static int GetCheckDigit(string totalString)
        {
            var checkDigitString = totalString[totalString.Length - 1].ToString();

            int checkDigit = int.Parse(checkDigitString);

            return checkDigit > 0 ? 10 - checkDigit : 0;
        }

        private static int GetEven(string newNumber, string[] evenArray)
        {
            var evenControl = GetEvenControl(newNumber, evenArray);

            int even = 0;
            for (int i = 0; i < evenControl; i++)
            {
                for (int j = 0; j < evenArray[i].Length; j++)
                {
                    even += Int32.Parse(evenArray[i][j].ToString());
                }
            }
            return even;
        }

        private static int GetEvenControl(string newNumber, string[] evenArray)
        {
            var evenctr = 0;
            for (int i = 0; i < 13; i++)
            {
                if (i % 2 != 0)
                {
                    continue;
                }
                int tmp = Int32.Parse(newNumber[i].ToString()) * 2;
                if (tmp > 0)
                {
                    evenArray[evenctr++] = tmp.ToString();
                }
            }
            return evenctr;
        }

        private static int GetOdd(string newnum)
        {
            int odd = 0;
            for (int i = 0; i < 13; i++)
            {
                if (i % 2 != 0)
                {
                    odd += Int32.Parse(newnum[i].ToString());
                }
            }
            return odd;
        }

        public int GetAgeFromDateOfBirth(DateTime dateOfBirth)
        {
            int age = DateTime.Today.Year - dateOfBirth.Year;
            if (dateOfBirth > DateTime.Today.AddYears(-age))
            {
                age--;
            }

            return age;
        }

        public bool ValidatePassportNumber(string passportNumber)
        {
            return passportNumber.Trim().Length >= 6;
        }

        public T ParseEnum<T>(string value)
        {
            string result = value.Trim().Replace(" ", "");
            result = result.Replace("/", "_");
            result = result.Replace("-", "_");
            result = result.Replace("–", "_");
            result = result.Replace("(", "_");
            result = result.Replace(")", "");
            result = result.Replace("'", "");
            result = result.Replace("+", "");
            result = result.Replace("&", "_and_");
            result = result.Replace(".", "");
            result = result.Replace(",", "_");
            if (result.StartsWith("_"))
            {
               result = result.Remove(0, 1);
            }

            return (T)Enum.Parse(typeof(T), result, ignoreCase: true);
        }

        public string MapComcorpToSAHLProvince(string comcorpProvince)
        {
            if (String.IsNullOrWhiteSpace(comcorpProvince))
            {
                return null;
            }

            string sahlProvince;

            switch (comcorpProvince.ToLower())
            {
                case "northwest":
                    sahlProvince = "North West";
                    break;
                case "kwazulu natal":
                    sahlProvince = "Kwazulu-natal";
                    break;
                default:
                    sahlProvince = comcorpProvince;
                    break;
            }
            return sahlProvince;
        }

        public bool CheckIfAffordabilityRequiresDescription(AffordabilityType affordabilityType)
        {
            bool requiresDescription = false;

            switch (affordabilityType)
            {
                case AffordabilityType.IncomefromInvestments:
                case AffordabilityType.OtherIncome1:
                case AffordabilityType.OtherIncome2:
                case AffordabilityType.OtherInstalments:
                case AffordabilityType.Other:
                case AffordabilityType.Otherdebtrepayment:
                    requiresDescription = true;
                    break;
                default:
                    break;
            }

            return requiresDescription;
        }

    }
}