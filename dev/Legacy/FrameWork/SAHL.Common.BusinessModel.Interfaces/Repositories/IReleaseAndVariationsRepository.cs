using System.Collections.Generic;
using System.Data;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    /// <summary>
    ///
    /// </summary>
    public interface IReleaseAndVariationsRepository
    {
        /// <summary>
        /// Returns a value fromt he RequestTypes array
        /// </summary>
        string[] RequestTypes { get; }

        /// <summary>
        /// Creates the ReleaseAndVariations Dataset
        /// </summary>
        DataSet CreateReleaseAndVariationsDataSet();

        /// <summary>
        /// Creates an Empty Conditions Table
        /// </summary>
        /// <returns></returns>
        DataTable CreateConditionsTable();

        /// <summary>
        /// Creates an empty bond details table
        /// </summary>
        /// <returns></returns>
        DataTable CreateBondDetailsTable();

        /// <summary>
        /// Creates a populated request Types Table
        /// </summary>
        /// <returns></returns>
        DataTable CreateRequestTypesTable();

        /// <summary>
        /// Creates a New populated ChangeTypes Table
        /// </summary>
        /// <returns></returns>
        DataTable CreateGetChangeTypesTable();

        /// <summary>
        /// Populate a Dataset from XML string
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="DS"></param>
        DataSet PopulateDataSet(string xml, DataSet DS);

        /// <summary>
        /// Parse the Dataset to an XML string
        /// </summary>
        /// <param name="DS"></param>
        /// <returns></returns>
        string ParseDatasetToXMLstring(DataSet DS);

        /// <summary>
        /// Get the bond information by Financial Services key
        /// </summary>
        /// <param name="financialServiceKey"></param>
        List<IBond> GetBondByFinancialServiceKey(int financialServiceKey);

        /// <summary>
        ///  Gets the Household income for an account key
        /// </summary>
        /// <param name="accountkey"></param>
        /// <returns></returns>
        double GetHouseholdIncomeByAccountKey(int accountkey);

        /// <summary>
        ///  Gets the Household address for an account key
        /// </summary>
        /// <param name="accountkey"></param>
        /// <returns></returns>
        string GetAddressGivenAccountKey(int accountkey);

        /// <summary>
        /// Get the Current bond installment for the account
        /// </summary>
        /// <param name="accountkey"></param>
        /// <returns></returns>
        double GetCurrentInstallment(int accountkey);

        /// <summary>
        ///  Get the Latest Valuation by property Key
        /// </summary>
        /// <param name="propertykey"></param>
        /// <returns></returns>
        double GetLatestValuationByPropertyKey(int propertykey);

        /// <summary>
        /// Get the Account's Special Purpose Vehicle
        /// </summary>
        /// <param name="accountkey"></param>
        /// <returns></returns>
        string FindSPVNameByFinancialServicesKey(int accountkey);

        /// <summary>
        /// Get an existing Release and Variations Dataset from the memo table
        /// </summary>
        /// <param name="memokey"></param>
        DataSet GetExistingReleaseAndVariationsByMemoKey(int memokey);

        /// <summary>
        /// Get an existing Release and Variations Dataset from the memo table
        /// </summary>
        /// <param name="generickey"></param>
        DataSet GetExistingReleaseAndVariationsByGenericKey(int generickey);

        /// <summary>
        /// Gets the Memo key
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <returns></returns>
        int GetMemoKey(int AccountKey);

        /// <summary>
        /// Adds a New Memo record containing a populated dataset. Return the New Key
        /// </summary>
        /// <param name="RVDS"></param>
        /// <param name="accountkey"></param>
        /// <param name="ADUser"></param>
        /// <param name="messages"></param>
        /// <returns></returns>
        int AddNewMemo(DataSet RVDS, int accountkey, IADUser ADUser, IDomainMessageCollection messages);

        /// <summary>
        /// Saves the Existing dataset as XML in the Memo table
        /// </summary>
        /// <param name="memokey"></param>
        /// <param name="RVDS"></param>
        /// /// <returns>true if successful</returns>
        bool UpdateMemo(int memokey, DataSet RVDS);

        /// <summary>
        /// Get the Accounts current loan balance
        /// </summary>
        /// <param name="accountkey"></param>
        /// <returns></returns>
        double GetLoanCurrentBalance(int accountkey);

        /// <summary>
        /// Get the current arrears balance for the account
        /// </summary>
        /// <param name="accountkey"></param>
        /// <returns></returns>
        double GetArrearBalanceByAccountKey(int accountkey);

        /// <summary>
        /// Returns the Main Applicants for the account
        /// </summary>
        /// <param name="accountkey"></param>
        /// <param name="messages"></param>
        /// <returns></returns>
        string[] GetLegalEntities(int accountkey, IDomainMessageCollection messages);
    }
}