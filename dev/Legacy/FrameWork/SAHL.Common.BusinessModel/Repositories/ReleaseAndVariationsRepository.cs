using System;
using System.Collections.Generic;
using System.Data;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Exceptions;
using SAHL.Common.CacheData;
using SAHL.Common.Security;

namespace SAHL.Common.BusinessModel.Repositories
{

    /// <summary>
    /// 
    /// </summary>
    [FactoryType(typeof(IReleaseAndVariationsRepository))]
    public class ReleaseAndVariationsRepository : AbstractRepositoryBase, IReleaseAndVariationsRepository
    {
        #region Local Variables

        /// <summary>
        /// Request Types contains a pre-defined list of Request Types.
        /// This array is a temporary measure to be replaced by a db table in the near future...
        /// </summary>
        private readonly string[] requestTypes = 
        {"-Select-", 
        "3rd Party Bond", 
        "Addition/Release Surety", 
        "Building Plans", 
        "Business Rights", 
        "Cancellation of Notarial Deed", 
        "Change of Condition", 
        "Change of Square M", 
        "Change of Marital Status", 
        "Change of Term", 
        "Consent of 2nd Dwelling", 
        "Correction of Title Deed", 
        "Consent Guest House", 
        "Consent to Sec Ext", 
        "Consent Subdivision", 
        "Consolidation", 
        "Consolidation & Sub", 
        "Expropriation", 
        "Extension of Scheme", 
        "Name Change", 
        "Notarial Deed", 
        "Real Right", 
        "Rectification of T/D", 
        "Release of Ptn", 
        "Release of Cession", 
        "Release of Units", 
        "Removal of Conditions", 
        "Rezoning", 
        "Rezoning and Removal", 
        "Right of Ext", 
        "Sectionalizing", 
        "Section 24 (7)", 
        "Sec 45 - Estate Late", 
        "Section 45 & 57", 
        "Sectional Register", 
        "Servitude", 
        "Sub & Consolidation", 
        "Sub & Rezoning", 
        "Subdivision and Release", 
        "Township Establishment", 
        "Interest Rate Review", 
        "Section 20 Record", 
        "Registration of Usufract"};

        /// <summary>
        /// Returns a value fromt he RequestTypes array
        /// </summary>
        public string[] RequestTypes
        {
            get { return requestTypes; }
        }

        #endregion

        /// <summary>
        /// Creates the ReleaseAndVariations Dataset 
        /// </summary>
        public DataSet CreateReleaseAndVariationsDataSet()
        {
            DataSet RVDS = new DataSet();

            /*      ConditionTypes  - 12
            *	0	No Tokens
            *	1	Tokenised String
            *	2	Run time Session Token
            *	3	Runtime executed SQL String
            *	4	User captured Condition
            *	5	Standard Condition edited by User
            *	6	Tokenised String edited by User
            */

            DataTable release = new DataTable();
            release.Columns.Add("MemoKey", typeof(int));
            release.Columns.Add("AccountNumber", typeof(int));
            release.Columns.Add("LinkedOffer", typeof(int));
            release.Columns.Add("AccountName", typeof(string));
            release.Columns.Add("RequestType", typeof(string));
            release.Columns.Add("ApplyChangeTo", typeof(string));
            release.Columns.Add("LoanBalance", typeof(double));
            release.Columns.Add("Arrears", typeof(double));
            release.Columns.Add("Notes", typeof(string));
            release.TableName = "Release";
            RVDS.Tables.Add(release);

            DataTable conditions = new DataTable();
            conditions.Columns.Add("ConditionKey", typeof(int));
            conditions.Columns.Add("MemoKey", typeof(int));
            conditions.Columns.Add("Condition", typeof(string));
            conditions.TableName = "Conditions";
            RVDS.Tables.Add(conditions);

            DataTable erf_existingsecurity = new DataTable();
            erf_existingsecurity.Columns.Add("ExistingKey", typeof(int));
            erf_existingsecurity.Columns.Add("MemoKey", typeof(int));
            erf_existingsecurity.Columns.Add("Notes", typeof(string));
            erf_existingsecurity.Columns.Add("EXT", typeof(string));
            erf_existingsecurity.Columns.Add("Valuation", typeof(double));
            erf_existingsecurity.TableName = "ErfExistingSecurity";
            RVDS.Tables.Add(erf_existingsecurity);

            DataTable erf_tobereleased = new DataTable();
            erf_tobereleased.Columns.Add("ReleaseKey", typeof(int));
            erf_tobereleased.Columns.Add("MemoKey", typeof(int));
            erf_tobereleased.Columns.Add("Notes", typeof(string));
            erf_tobereleased.Columns.Add("EXT", typeof(string));
            erf_tobereleased.Columns.Add("Valuation", typeof(double));
            erf_tobereleased.TableName = "ErfToBeReleased";
            RVDS.Tables.Add(erf_tobereleased);

            DataTable erf_remainingsecurity = new DataTable();
            erf_remainingsecurity.Columns.Add("RemainingKey", typeof(int));
            erf_remainingsecurity.Columns.Add("MemoKey", typeof(int));
            erf_remainingsecurity.Columns.Add("Notes", typeof(string));
            erf_remainingsecurity.Columns.Add("EXT", typeof(string));
            erf_remainingsecurity.Columns.Add("Valuation", typeof(double));
            erf_remainingsecurity.TableName = "ErfRemainingSecurity";
            RVDS.Tables.Add(erf_remainingsecurity);

            return RVDS;

        }

        /// <summary>
        /// Creates an Empty Conditions Table
        /// </summary>
        /// <returns></returns>
        public DataTable CreateConditionsTable()
        {
            DataTable Conditions = new DataTable("Conditions");
            Conditions.Columns.Add("ConditionKey", Type.GetType("System.String")); //0
            Conditions.Columns.Add("Condition", Type.GetType("System.String")); //0
            return Conditions;
        }


        /// <summary>
        /// Creates an empty bond details table
        /// </summary>
        /// <returns></returns>
        public DataTable CreateBondDetailsTable()
        {
            DataTable BondDetails = new DataTable("BondDetails");
            BondDetails.Columns.Add("InFavourOf", Type.GetType("System.String")); //0
            BondDetails.Columns.Add("BondRegistrationAmount", Type.GetType("System.String")); //0
            BondDetails.Columns.Add("CoverAmount", Type.GetType("System.String")); //0
            BondDetails.Columns.Add("BondRegistrationDate", Type.GetType("System.String")); //0

            // Format the BondDetails Data
            //if (e.Row.RowIndex >= 0)
            //{
            //    Bond.BondRow brow = m_Controller.m_BondDS._Bond[e.Row.RowIndex];
            //    e.Row.Cells[0].Text = "Bank";
            //    e.Row.Cells[1].Text = String.Format("{0:C}", brow.BondRegistrationAmount);
            //    e.Row.Cells[3].Text = String.Format("{0:d}", brow.BondRegistrationDate);
            //}

            return BondDetails;
        }


        /// <summary>
        /// Creates a request Types Table
        /// </summary>
        /// <returns></returns>
        public DataTable CreateRequestTypesTable()
        {
            DataTable RequestTypesDT = new DataTable("RequestTypes");
            RequestTypesDT.Columns.Add("RequestType", Type.GetType("System.String")); //0
            RequestTypesDT.Columns.Add("Key", Type.GetType("System.String")); //1

            //RequestTypes[]
            for (int i = 0; i < RequestTypes.Length; i++)
            {
                DataRow valRow = RequestTypesDT.NewRow();
                valRow["Key"] = i.ToString();
                valRow["RequestType"] = RequestTypes[i];
                RequestTypesDT.Rows.Add(valRow);
            }

            return RequestTypesDT;
        }

        /// <summary>
        /// Creates a New ChangeTypes Table
        /// </summary>
        /// <returns></returns>
        public DataTable CreateGetChangeTypesTable()
        {
            DataTable ChangeTypesDT = new DataTable("ChangeTypes");
            ChangeTypesDT.Columns.Add("LoanType", Type.GetType("System.String")); //0
            ChangeTypesDT.Columns.Add("Key", Type.GetType("System.String")); //1

            const string CQL = "from ApplicationType_DAO v where v.Key IN (4,6,7,8)";
            SimpleQuery<ApplicationType_DAO> query = new SimpleQuery<ApplicationType_DAO>(CQL);
            ApplicationType_DAO[] AT = query.Execute();

            DataRow selectRow = ChangeTypesDT.NewRow();
            selectRow["Key"] = "0";
            selectRow["LoanType"] = "-Select-";
            ChangeTypesDT.Rows.Add(selectRow);

            for (int i = 0; i < AT.Length; i++)
            {
                selectRow = ChangeTypesDT.NewRow();
                selectRow["Key"] = AT[i].Key.ToString();
                selectRow["LoanType"] = AT[i].Description;
                ChangeTypesDT.Rows.Add(selectRow);
            }
            return ChangeTypesDT;
        }



        // XML MEMO HANDLERS ***************************************************


        #region XML Memo Handlers


        //-------------------------------------------------------------------------------

        /// <summary>
        /// Populate a Dataset from XML string
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="DS"></param>
        public DataSet PopulateDataSet(string xml, DataSet DS)
        {
            if (xml == null) throw new ArgumentNullException("xml");
            //XmlWriteMode XMLMode = XmlWriteMode.WriteSchema;
            System.IO.StringReader TextReader = new System.IO.StringReader(xml);
            DS.ReadXml(TextReader, XmlReadMode.Auto);

            return DS;
        }

        /// <summary>
        /// Parse the Dataset to an XML string
        /// </summary>
        /// <param name="DS"></param>
        /// <returns></returns>
        public string ParseDatasetToXMLstring(DataSet DS)
        {
            if (DS == null) { return ""; }
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            DS.WriteXml(stream, XmlWriteMode.IgnoreSchema);
            string ReturnVal = System.Text.Encoding.Default.GetString(stream.ToArray());
            return ReturnVal;
        }

        #endregion

        // *********************************************************************


        #region Public Methods

        /// <summary>
        /// Get the bond information by Financial Services key
        /// </summary>
        /// <param name="accountkey"></param>
        public List<IBond> GetBondByFinancialServiceKey(int accountkey)
        {

                       
            //string HQL = "from FinancialService_DAO fs where fs.Account.Key = ?";
            //SimpleQuery<FinancialService_DAO> f = new SimpleQuery<FinancialService_DAO>(HQL, accountkey);
            //FinancialService_DAO[] fs = f.Execute();
            //int fskey = fs[0].Key;

            



            // populate a bond list by finaincial services key...
            const string HQL = "select b from Bond_DAO b join b.MortgageLoans ml where ml.Account.Key = ?";
            //HQL = "from Bond_DAO b where b.Application.Account.Key = ?";
            SimpleQuery<Bond_DAO> q = new SimpleQuery<Bond_DAO>(HQL, accountkey);
            Bond_DAO[] res = q.Execute();
            //   bonds.Clear;
            List<IBond> bonds = new List<IBond>();
            for (int i = 0; i < res.Length; i++)
                bonds.Add(new Bond(res[i]));

            return bonds;
        }


        /// <summary>
        ///  Gets the Household income for an account key
        /// </summary>
        /// <param name="accountkey"></param>
        /// <returns></returns>
        public double GetHouseholdIncomeByAccountKey(int accountkey)
        {
            double Amount = 0d;
            IAccountRepository accrep = RepositoryFactory.GetRepository<IAccountRepository>();
            IMortgageLoanAccount mlacc = accrep.GetAccountByKey(accountkey) as IMortgageLoanAccount;
            if (mlacc != null) Amount = mlacc.SecuredMortgageLoan.Account.GetHouseholdIncome();
            return Amount;
        }


        /// <summary>
        ///  Gets the Household address for an account key
        /// </summary>
        /// <param name="accountkey"></param>
        /// <returns></returns>
        public string GetAddressGivenAccountKey(int accountkey)
        {
            IAddress address = null;
            IAccountRepository accrep = RepositoryFactory.GetRepository<IAccountRepository>();
            IMortgageLoanAccount mlacc = accrep.GetAccountByKey(accountkey) as IMortgageLoanAccount;

            if (mlacc != null)
            {
                IProperty property = mlacc.SecuredMortgageLoan.Property;
                address = property.Address;
            }

            if (address != null)
            {
                string retval = address.GetFormattedDescription(AddressDelimiters.Space);
                return retval;
            }

            return "";
        }


        /// <summary>
        /// Get the Current bond installment for the account
        /// </summary>
        /// <param name="accountkey"></param>
        /// <returns></returns>
        public double GetCurrentInstallment(int accountkey)
        {
            double Amount = 0d;
            IAccountRepository accrep = RepositoryFactory.GetRepository<IAccountRepository>();
            IMortgageLoanAccount mlacc = accrep.GetAccountByKey(accountkey) as IMortgageLoanAccount;
            if (mlacc != null) Amount = mlacc.SecuredMortgageLoan.Account.InstallmentSummary.TotalLoanInstallment;
            return Amount;
        }


        /// <summary>
        ///  Get the Latest Valuation by property Key
        /// </summary>
        /// <param name="propertykey"></param>
        /// <returns></returns>
        public double GetLatestValuationByPropertyKey(int propertykey)
        {
            // Get the Memo Record and convert from the XML format to the dataset
            IPropertyRepository proprep = RepositoryFactory.GetRepository<IPropertyRepository>();
            IReadOnlyEventList<IValuation> valuations = proprep.GetValuationByPropertyKey(propertykey);
            return (double)valuations[valuations.Count - 1].ValuationAmount;
        }

        /// <summary>
        /// Get the Account's Special Purpose Vehicle
        /// </summary>
        /// <param name="accountkey"></param>
        /// <returns></returns>
        public string FindSPVNameByFinancialServicesKey(int accountkey)
        {
            const string HQL = "from MortgageLoan_DAO ml where ml.Account.Key = ?";
            SimpleQuery<MortgageLoan_DAO> q = new SimpleQuery<MortgageLoan_DAO>(HQL, accountkey);
            MortgageLoan_DAO[] res = q.Execute();
            string ReturnVal = res[res.Length - 1].Account.SPV.Description;
            return ReturnVal;
        }


        /// <summary>
        /// Get an existing Release and Variations Dataset from the memo table
        /// </summary>
        /// <param name="memokey"></param>
        public DataSet GetExistingReleaseAndVariationsByMemoKey(int memokey)
        {
            DataSet RVDS = CreateReleaseAndVariationsDataSet();
            // Get the Memo Record and convert from the XML format to the dataset
            IMemoRepository memorep = RepositoryFactory.GetRepository<IMemoRepository>();
            IMemo memo = memorep.GetMemoByKey(memokey);
            PopulateDataSet(memo.Description, RVDS);
            return RVDS;
        }

        /// <summary>
        /// Get an existing Release and Variations Dataset from the memo table
        /// </summary>
        /// <param name="generickey"></param>
        public DataSet GetExistingReleaseAndVariationsByGenericKey(int generickey)
        {
            DataSet RVDS = CreateReleaseAndVariationsDataSet();
            const string HQL = "from Memo_DAO m where m.GenericKey = ? and m.GenericKeyType.Key = 9 order by m.Key desc";
            SimpleQuery<Memo_DAO> q = new SimpleQuery<Memo_DAO>(HQL, generickey);
            q.SetQueryRange(1);
            Memo_DAO[] res = q.Execute();
            PopulateDataSet(res[0].Description, RVDS);
            return RVDS;
        }

        /// <summary>
        /// Gets the Memo key
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <returns></returns>
        public int GetMemoKey(int AccountKey)
        {
            const string HQL = "from Memo_DAO m where m.GenericKey = ? and m.GenericKeyType.Key = 9 order by m.Key desc";
            SimpleQuery<Memo_DAO> q = new SimpleQuery<Memo_DAO>(HQL, AccountKey);
            q.SetQueryRange(1);
            Memo_DAO[] res = q.Execute();
            return res[0].Key;
        }



        /// <summary>
        /// Adds a New Memo record containing a populated dataset. Return the New Key
        /// </summary>
        /// <param name="RVDS"></param>
        /// <param name="accountkey"></param>
        /// <param name="ADUser"></param>
        /// <param name="messages"></param>
        /// <returns>New Memo Key</returns>
        public int AddNewMemo(DataSet RVDS, int accountkey, IADUser ADUser, IDomainMessageCollection messages)
        {
            IMemoRepository memorep = RepositoryFactory.GetRepository<IMemoRepository>();
            ILookupRepository lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
            IGenericKeyType GKT = lookupRepository.GenericKeyType[6];
            IGeneralStatus GS = lookupRepository.GeneralStatuses[0];
            //Common.Security.SAHLPrincipal currentPrincipal = new Common.Security.SAHLPrincipal(System.Security.Principal.WindowsIdentity.GetCurrent());
            //Common.SAHLPrincipalCache spc = SAHL.Common.SAHLPrincipalCache.GetPrincipalCache(currentPrincipal);
            //IADUser ADUser = spc.GetADUser(currentPrincipal);

            IMemo memo = memorep.CreateMemo();
            memo.GenericKey = accountkey;
            memo.GenericKeyType = GKT;
            memo.InsertedDate = DateTime.Now;
            memo.ADUser = ADUser;
            memo.Description = "Watch this space....";
            memo.GeneralStatus = GS;

            // Thismemo record is being used for data - ignore
            //memo.ExcludedRules.Add("MemoAddUpdateMemoReminderDate");
            //memo.ExcludedRules.Add("MemoAddUpdateMemoExpiryDate");

            if (messages == null)
                throw new ArgumentNullException(StaticMessages.NullDomainCollection);
            Memo_DAO dao = (Memo_DAO)((IDAOObject)memo).GetDAOObject();

            //memorep.SaveMemo(memo);
            dao.SaveAndFlush();

            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();

            return memo.Key;

        }

        /// <summary>
        /// Saves the Existing dataset as XML in the Memo table
        /// </summary>
        /// <param name="memokey"></param>
        /// <param name="RVDS"></param>
        public bool UpdateMemo(int memokey, DataSet RVDS)
        {
            IMemoRepository memorep = RepositoryFactory.GetRepository<IMemoRepository>();
            ISecurityRepository secRepo = RepositoryFactory.GetRepository<ISecurityRepository>();

            IMemo memo = memorep.GetMemoByKey(memokey);
            memo.Description = ParseDatasetToXMLstring(RVDS);

            IADUser ADUser = secRepo.GetADUserByPrincipal(SAHLPrincipal.GetCurrent());
            memo.ADUser = ADUser;

            // Thismemo record is being used for data - ignore
            //memo.ExcludedRules.Add("MemoAddUpdateMemoReminderDate");
            //memo.ExcludedRules.Add("MemoAddUpdateMemoExpiryDate");

            Memo_DAO dao = (Memo_DAO)((IDAOObject)memo).GetDAOObject();
            dao.UpdateAndFlush();

            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();

            return true;

        }

        #endregion


        /// <summary>
        /// Get the Accounts current loan balance
        /// </summary>
        /// <param name="accountkey"></param>
        /// <returns></returns>
        public double GetLoanCurrentBalance(int accountkey)
        {
            double Amount = 0d;
            IAccountRepository accrep = RepositoryFactory.GetRepository<IAccountRepository>();
            IMortgageLoanAccount mlacc = accrep.GetAccountByKey(accountkey) as IMortgageLoanAccount;
            if (mlacc != null) Amount = mlacc.SecuredMortgageLoan.CurrentBalance;

            // Get Fixed leg of Mortgage Loan
            IAccountVariFixLoan varifixLoanAccount = mlacc as IAccountVariFixLoan;
            if (varifixLoanAccount != null)
                Amount += varifixLoanAccount.FixedSecuredMortgageLoan.CurrentBalance;

            return Amount;
        }


        /// <summary>
        /// Get the current arrears balance for the account
        /// </summary>
        /// <param name="accountkey"></param>
        /// <returns></returns>
        public double GetArrearBalanceByAccountKey(int accountkey)
        {
            double Amount = 0d;
            IAccountRepository accrep = RepositoryFactory.GetRepository<IAccountRepository>();
            IMortgageLoanAccount yy = accrep.GetAccountByKey(accountkey) as IMortgageLoanAccount;
            if (yy != null) Amount = yy.SecuredMortgageLoan.ArrearBalance;

            // Get Fixed leg of Mortgage Loan
            IAccountVariFixLoan varifixLoanAccount = yy as IAccountVariFixLoan;
            if (varifixLoanAccount != null)
                Amount += varifixLoanAccount.FixedSecuredMortgageLoan.ArrearBalance;

            return Amount;
        }


        /// <summary>
        /// Returns the Main Applicants for the account
        /// </summary>
        /// <param name="accountkey"></param>
        /// <param name="messages"></param>
        /// <returns></returns>
        public string[] GetLegalEntities(int accountkey, IDomainMessageCollection messages)
        {
            string[] resultset = null;
            int[] roletypes = { (int)RoleTypes.MainApplicant };
            IAccountRepository accrep = RepositoryFactory.GetRepository<IAccountRepository>();
            IAccount account = accrep.GetAccountByKey(accountkey);
            IReadOnlyEventList<ILegalEntity> le = account.GetLegalEntitiesByRoleType(messages, roletypes, GeneralStatusKey.All);
            for (int i = 0; i < le.Count; i++)
            {
                Array.Resize(ref resultset, i + 1);
                resultset.SetValue(le[i].DisplayName, i);
            }
            return resultset;
        }




    }
}
