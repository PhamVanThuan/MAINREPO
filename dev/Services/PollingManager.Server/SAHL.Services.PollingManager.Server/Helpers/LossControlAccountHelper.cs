using System;
using System.Collections.Generic;

namespace SAHL.Services.PollingManager.Helpers
{
    public interface ILossControlAccountHelper
    {
        Tuple<int, List<string>> StripAccountNumber(char seperator, string inputString);
    }

    public class LossControlAccountHelper : ILossControlAccountHelper
    {
        private List<string> accountErrorMessages;

        public LossControlAccountHelper()
        {
            accountErrorMessages = new List<string>();
        }

        public Tuple<int, List<string>> StripAccountNumber(char seperator, string inputString)
        {
            CheckForEmptyInputString(inputString);
            CheckForSeparator(seperator, inputString);

            string[] inputItems = inputString.Split(seperator);
            var accountNumberText = inputItems[0].Trim();
            CheckAccountFormat(accountNumberText);

            int accountNumber;
            if (!Int32.TryParse(accountNumberText, out accountNumber))
            {
                accountNumber = 0;
            }
            return new Tuple<int, List<string>>(accountNumber, accountErrorMessages);
        }

        private void CheckAccountFormat(string accountNumber)
        {
            int validAccountNumber;
            if (!Int32.TryParse(accountNumber, out validAccountNumber))
            {
                accountErrorMessages.Add("Supplied account number is invalid.");
            }
        }

        private void CheckForSeparator(char seperator, string inputString)
        {
            if (!inputString.Contains(seperator.ToString()))
            {
                accountErrorMessages.Add("Email subject doesn't contain the correct separator.");
            }
        }

        private void CheckForEmptyInputString(string inputString)
        {
            if (inputString.Length == 0)
            {
                accountErrorMessages.Add("Email subject cannot be blank.");
            }
        }
    }
}