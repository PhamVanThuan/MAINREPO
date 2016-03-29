using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.Capitec.Common
{
    public class ApplicationPurposeEnums
    {
        public const string SwitchLoan = "Switch";
        public const string NewPurchaseLoan = "New Purchase";
    }

    public class CalculatorConstants
    {
        public const int CalculatorTermInMonths = 240;
    }

    public class DeclarationDefinitions
    {
        public const string AllowCreditBureauCheck = "Allow Credit Bureau Check";
        public const string HasCapitecTransactionAccount = "Has Capitec Transaction Account";
        public const string IncomeContributor = "Income Contributor";
        public const string MarriedInCommunityOfProperty = "Married In Community Of Property";
		public const string ExcludeFromDirectMarketing = "Do you want to be excluded from direct marketing by Capitec bank?";
		public const string GivePermissionToShareInformation = "Do you give permission for Capitec Bank and SA Home Loans to share you personal information with each other?";
    }

    public class Notifications
    {              
        public static string UpdateApplicationRecievedNotification(int applicationId)
        {
            return string.Format("Thank you for your application with SA Home Loans. A consultant will be in contact shortly. Application number: {0}", applicationId);
        }
    }
}
