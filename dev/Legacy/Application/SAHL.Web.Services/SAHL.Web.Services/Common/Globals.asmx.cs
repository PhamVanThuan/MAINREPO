using System.ComponentModel;
using System.Web.Services;
using SAHL.Common.Globals;

namespace SAHL.Web.Services
{
    /// <summary>
    /// Summary description for Globals
    /// </summary>
    [WebService(Namespace = "http://webservices.sahomeloans.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public partial class Globals : WebService
    {
        //STRING FORMATTING

        [WebMethod(Description = "Returns the Standard Currency format")]
        public string CurrencyFormat()
        {
            return Common.Constants.CurrencyFormat;
        }

        [WebMethod(Description = "Returns the Standard Currency format withour cents")]
        public string CurrencyFormatNoCents()
        {
            return Common.Constants.CurrencyFormatNoCents;
        }

        [WebMethod(Description = "Returns The Rate Format")]
        public string RateFormat()
        {
            return Common.Constants.RateFormat;
        }

        //SALARY TYPES
        [WebMethod(Description = "Returns EmploymentType Salaried")]
        public int Salaried()
        {
            return (int)EmploymentTypes.Salaried;
        }

        [WebMethod(Description = "Returns EmploymentType SelfEmployed")]
        public int SelfEmployed()
        {
            return (int)EmploymentTypes.SelfEmployed;
        }

        [WebMethod(Description = "Returns EmploymentType Subsidised")]
        public int Subsidised()
        {
            return (int)EmploymentTypes.SalariedwithDeduction;
        }

        [WebMethod(Description = "Returns EmploymentType Unemployed")]
        public int Unemployed()
        {
            return (int)EmploymentTypes.Unemployed;
        }

        [WebMethod(Description = "Returns EmploymentType Unknown")]
        public int Unknown()
        {
            return (int)EmploymentTypes.Unknown;
        }

        // RETURN THE DIFFERENT TYPES OF BASIC PRODUCT KEYS

        [WebMethod(Description = "ReturnsProduct Type Variable Loan")]
        public int VariableLoan()
        {
            return (int)Products.VariableLoan;
        }

        [WebMethod(Description = "ReturnsProduct Type VariFixLoan Loan")]
        public int VariFixLoan()
        {
            return (int)Products.VariFixLoan;
        }

        // RETURN THE DIFFERENT TYPES OF COMPOUND PRODUCT KEYS

        [WebMethod(Description = "Returns ProductsSwitchLoan.NewVariableLoan")]
        public int ProductsSwitchLoanNewVariableLoan()
        {
            return (int)ProductsSwitchLoan.NewVariableLoan;
        }

        [WebMethod(Description = "Returns ProductsSwitchLoan.VariFixLoan")]
        public int ProductsSwitchLoanVariFixLoan()
        {
            return (int)ProductsSwitchLoan.VariFixLoan;
        }

        [WebMethod(Description = "Returns ProductsSwitchLoan.SuperLo")]
        public int ProductsSwitchLoanSuperLo()
        {
            return (int)ProductsSwitchLoan.SuperLo;
        }

        [WebMethod(Description = "Returns ProductsNewPurchase.NewVariableLoan")]
        public int ProductsNewPurchaseNewVariableLoan()
        {
            return (int)ProductsNewPurchase.NewVariableLoan;
        }

        [WebMethod(Description = "Returns ProductsNewPurchase.VariFixLoan")]
        public int ProductsNewPurchaseVariFixLoan()
        {
            return (int)ProductsNewPurchase.VariFixLoan;
        }

        [WebMethod(Description = "Returns ProductsNewPurchase.SuperLo")]
        public int ProductsNewPurchaseSuperLo()
        {
            return (int)ProductsNewPurchase.SuperLo;
        }

        [WebMethod(Description = "Returns ProductsRefinance.NewVariableLoan")]
        public int ProductsRefinanceNewVariableLoan()
        {
            return (int)ProductsRefinance.NewVariableLoan;
        }

        [WebMethod(Description = "Returns ProductsRefinance.VariFixLoan")]
        public int ProductsRefinanceVariFixLoan()
        {
            return (int)ProductsRefinance.VariFixLoan;
        }

        [WebMethod(Description = "Returns ProductsRefinance.SuperLo")]
        public int ProductsRefinanceSuperLo()
        {
            return (int)ProductsRefinance.SuperLo;
        }

        // MORTGAGE LOAN PURPOSES

        [WebMethod(Description = "Returns MortgageLoanPurposes.Switchloan")]
        public int MortgageLoanPurposesSwitchloan()
        {
            return (int)MortgageLoanPurposes.Switchloan;
        }

        [WebMethod(Description = "Returns MortgageLoanPurposes.Newpurchase")]
        public int MortgageLoanPurposesNewpurchase()
        {
            return (int)MortgageLoanPurposes.Newpurchase;
        }

        [WebMethod(Description = "Returns MortgageLoanPurposes.Refinance")]
        public int MortgageLoanPurposesRefinance()
        {
            return (int)MortgageLoanPurposes.Refinance;
        }

        // ORIGINATION SOURCES

        [WebMethod(Description = "Returns the Origination Source for SA Home Loans")]
        public int OriginationSourcesSAHomeLoans()
        {
            return (int)OriginationSources.SAHomeLoans;
        }
    }
}