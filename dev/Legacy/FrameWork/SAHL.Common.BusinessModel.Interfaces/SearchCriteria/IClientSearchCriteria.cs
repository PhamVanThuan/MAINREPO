namespace SAHL.Common.BusinessModel.Interfaces.SearchCriteria
{
    /// <summary>
    /// Indicates the type of client search being performed.
    /// </summary>
    public enum ClientSearchType
    {
        /// <summary>
        /// Search by account number, all other fields are ignored.
        /// </summary>
        AccountOnly,
        /// <summary>
        /// Natural person search.
        /// </summary>
        NaturalPerson,
        /// <summary>
        /// Company search.
        /// </summary>
        Company
    }

    public enum ClientSearchLoanType
    {
        MortgageLoan = 1,
        Life = 2,
        HOC = 3
    }

    public interface IClientSearchCriteria
    {
        bool IsEmpty { get; }

        //ClientSearchType SearchType { get;set;}
        //ClientSearchLoanType AccountType { get;set;}
        string AccountNumber { get; set; }

        string FirstNames { get; set; }

        //string Initials { get;set;}
        //string PreferredName { get;set;}
        string Surname { get; set; }

        string IDNumber { get; set; }

        //string PassportNumber { get;set;}
        //string HomePhoneCode { get;set;}
        //string HomePhone { get;set;}
        //string WorkPhoneCode { get;set;}
        //string WorkPhone { get;set;}
        string SalaryNumber { get; set; }

        //string CompanyRegisteredName { get;set;}
        //string CompanyTradingName { get;set;}
        //string CompanyNumber { get;set;}
    }
}