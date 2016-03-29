using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Castle.ActiveRecord.Queries;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.DataTransferObjects;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.CacheData;
using SAHL.Common.Security;

namespace SAHL.Common.Service
{
    /// <summary>
    ///
    /// </summary>
    [FactoryType(typeof(ISPVService))]
    public class SPVService : ISPVService
    {
        private IControlRepository _ctrlRepo;

        private IControlRepository CtrlRepo
        {
            get
            {
                if (_ctrlRepo == null)
                    _ctrlRepo = SAHL.Common.Factories.RepositoryFactory.GetRepository<IControlRepository>();
                return _ctrlRepo;
            }
        }

        /// <summary>
        /// Return a list of SPV's that are valid for further lending to be disbursed into.
        /// </summary>
        /// <returns></returns>
        public IList<ISPV> GetSPVListForFurtherLending()
        {
            //string HQL = "from SPV_DAO spv where spv.Key = ? or spv.Key = " + (int)SPVs.BlueBannerAgencyAccount + " or spv.Key = " + (int)SPVs.MainStreet65PtyLtd + " and spv.AllowFurtherLending = 1";
            string HQL = "select spva.SPV from SPVAttribute_DAO spva where spva.SPVAttributeType.Key = ?";
            SimpleQuery<SPV_DAO> q = new SimpleQuery<SPV_DAO>(HQL, (int)SAHL.Common.Globals.SPVAttributeTypes.AllowFurtherLending);
            SPV_DAO[] res = q.Execute();

            IList<ISPV> retval = new List<ISPV>();
            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            for (int i = 0; i < res.Length; i++)
            {
                retval.Add(BMTM.GetMappedType<ISPV>(res[i]));
            }
            return retval;
        }

        public ISPV GetSPVDetails(int SPVKey)
        {
            string HQL = "from SPV_DAO spv where spv.Key = ?";

            SimpleQuery<SPV_DAO> q = new SimpleQuery<SPV_DAO>(HQL, SPVKey);
            q.SetQueryRange(1);
            SPV_DAO[] list = q.Execute();

            if (list.Length > 0)
            {
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return BMTM.GetMappedType<ISPV, SPV_DAO>(list[0]);
            }
            return null;
        }

        /// <summary>
        /// This is a tolerance method to allow Category 1 for LTV a little over the limit.
        /// Currently 80.01% is allowed in Cat 1
        /// </summary>
        /// <param name="LTV"></param>
        /// <returns></returns>
        private double CheckLTVForCategory1(double LTV)
        {
            double retLTV = LTV;

            IControl ctrl = CtrlRepo.GetControlByDescription("Maximum Category1 LTV");
            IControl ctrl1 = CtrlRepo.GetControlByDescription("Current Category1 LTV");

            if (ctrl != null && ctrl.ControlNumeric.HasValue)
            {
                if (LTV < ctrl.ControlNumeric.Value)
                {
                    if (ctrl1 != null && ctrl1.ControlNumeric.HasValue)
                        retLTV = ctrl1.ControlNumeric.Value;
                }
            }

            return retLTV;
        }

        /// <summary>
        /// Get SPV that doesn't rely on persisted data
        /// Pass the parameters in and it does the magic
        /// </summary>
        /// <param name="application"></param>
        public void DetermineSPVOnApplication(IApplication application)
        {
            IAccountRepository accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();
            IApplicationRepository applicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();
            IRow spvDetermineParameters = new Row();
            ISPV currentSPV = null;

            bool furtherLending = (application is IApplicationReAdvance ||
                             application is IApplicationFurtherAdvance ||
                             application is IApplicationFurtherLoan);

            // check if the application has application information
            // Check if the application is past the point that NO detail is allowed to be changed on this
            IApplicationInformation latestApplicationInformation = application.GetLatestApplicationInformation();
            if (!application.IsOpen
                || latestApplicationInformation == null
                || latestApplicationInformation.ApplicationInformationType.Key == (int)OfferInformationTypes.AcceptedOffer)
                return;

            // Get ApplicationInformationVariableLoan
            IApplicationProduct appProd = application.CurrentProduct;
            ISupportsVariableLoanApplicationInformation vli = application.CurrentProduct as ISupportsVariableLoanApplicationInformation;
            if (vli == null)
            {
                return;
            }

            IApplicationInformationVariableLoan aivl = vli.VariableLoanInformation;
            if (aivl == null)
            {
                return;
            }

            // Get LTV
            double ltv = 0D;
            if (furtherLending)
            {
                // For further lending pass the new LTV (Account + Application)
                ltv = DetermineLTVOnFurtherLendingApplication(application);
                currentSPV = application.Account.SPV;
            }
            else
            {
                ltv = aivl.LTV.HasValue ? aivl.LTV.Value : 0D;
            }

            spvDetermineParameters.LTV = decimal.Parse(ltv.ToString(), System.Globalization.NumberStyles.Float);

            // Get House Hold Income
            double houseHoldIncome = aivl.HouseholdIncome.HasValue ? aivl.HouseholdIncome.Value : 0D;

            // Get Employment Type
            int employmentTypeKey = aivl.EmploymentType == null ? -1 : aivl.EmploymentType.Key;

            bool isStaffLoan = application.HasAttribute(OfferAttributeTypes.StaffHomeLoan);
            bool isGEPF = application.HasAttribute(OfferAttributeTypes.GovernmentEmployeePensionFund);
            if (!furtherLending)
            {
                List<ApplicationAttributeToApply> applicationAttributesToApply = applicationRepository.DetermineApplicationAttributeTypes(application.Key, ltv, employmentTypeKey, houseHoldIncome, isStaffLoan, isGEPF);
                List<int> applicationAttributes = applicationAttributesToApply.Where(x => x.ToBeRemoved == false)
                                                                                .Select(x => x.ApplicationAttributeTypeKey).ToList();
                applicationAttributes = applicationAttributes.Union(application.ApplicationAttributes.Select(x => x.ApplicationAttributeType.Key)).ToList();
                var spvApplicationAttributes = String.Join(",", applicationAttributes);
                spvDetermineParameters.OfferAttributes = spvApplicationAttributes;
            }
            // Check HasApplicationBeenInCompany2
            // Assume this is only done for offers that have a account
            if (application.Account != null)
            {
                spvDetermineParameters.HasBeenInCompany2 = Convert.ToInt16(accountRepository.HasApplicationBeenInCompany2(application.Account.Key));
            }

            // Check if the context is Further Lending
            spvDetermineParameters.FLAllowed = Convert.ToInt16(furtherLending);

            // Check if the context is Term Change
            // It should not be on an application as Term Change is ONLY done on an Account
            spvDetermineParameters.TermChangeAllowed = Convert.ToInt16(false);

            spvDetermineParameters.IsGEPF = Convert.ToInt16(isGEPF);

            int genericKey = -1;

            SPVDetermineSources spvDetermineSource = SPVDetermineSources.Params;

            int currentSPVKey = -1;

            if (currentSPV != null)
            {
                currentSPVKey = furtherLending ? currentSPV.Key : -1;
            }

            var determinedSPV = GetSPVByParameters(spvDetermineParameters, genericKey, spvDetermineSource, currentSPVKey);
            if (determinedSPV != null)
            {
                aivl.SPV = determinedSPV;
            }
            else
            {
                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
                var message = new Error("Cannot determine SPV", "Cannot determine SPV");
                spc.DomainMessages.Add(message);
            }
        }

        public ISPV DetermineSPVForFurtherLending(int accountKey, double loanAmount, double valuation)
        {
            IAccountRepository accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();
            IAccount account = accountRepository.GetAccountByKey(accountKey);
            IRow spvDetermineParameters = new Row();
            ISPV currentSPV = account.SPV;

            spvDetermineParameters.HasBeenInCompany2 = Convert.ToInt16(accountRepository.HasApplicationBeenInCompany2(accountKey));

            spvDetermineParameters.LTV = decimal.Parse((loanAmount / valuation).ToString(), System.Globalization.NumberStyles.Float);

            // further lending
            spvDetermineParameters.FLAllowed = Convert.ToInt16(true);

            // not a term change
            spvDetermineParameters.TermChangeAllowed = Convert.ToInt16(false);

            spvDetermineParameters.LoanAmount = (decimal)loanAmount;

            spvDetermineParameters.IsGEPF = 0;

            SPVDetermineSources spvDetermineSource = SPVDetermineSources.Params;

            int genericKey = -1;

            ISPV determinedSPV = GetSPVByParameters(spvDetermineParameters, genericKey, spvDetermineSource, currentSPV.Key);
            if (determinedSPV != null)
                return determinedSPV;
            else
                return currentSPV;
        }

        public ISPV GetSPVByParameters(IRow spvDetermineParameters, int genericKey, SPVDetermineSources spvDetermineSource, int currentSPVKey)
        {
            //spvDetermineParameters.LTV = Math.Round(spvDetermineParameters.LTV, 6);
            // Serialize the DTO to a string to be passed to the API
            string rowXml;
            using (MemoryStream mem = new MemoryStream())
            {
                new XmlSerializer(typeof(Row)).Serialize(mem, spvDetermineParameters);
                using (var sr = new StreamReader(mem))
                {
                    mem.Position = 0;
                    rowXml = sr.ReadToEnd();
                }
            }

            // Call the API to get a valid SPV by using a parameter collection
            var prms = new ParameterCollection();

            //Has to be set to -1 when calling get SPV with parameters
            var genericKeyParam = new SqlParameter("@GenericKey", genericKey);
            genericKeyParam.IsNullable = true;
            prms.Add(genericKeyParam);

            var sourceKeyParam = new SqlParameter("@SourceKey", (int)spvDetermineSource);
            prms.Add(sourceKeyParam);

            object currentSPV = System.DBNull.Value;
            if (currentSPVKey > 0)
                currentSPV = currentSPVKey;

            var existingSPVKeyParam = new SqlParameter("@ExistingSPVKey", currentSPV);
            existingSPVKeyParam.IsNullable = true;
            prms.Add(existingSPVKeyParam);

            SqlParameter xmlParameter = new SqlParameter("@XML", rowXml);
            prms.Add(xmlParameter);

            var validSPVkeyParam = new SqlParameter("@ValidSPVKey", SqlDbType.Int);
            validSPVkeyParam.Direction = ParameterDirection.Output;
            prms.Add(validSPVkeyParam);

            CastleTransactionsServiceHelper.ExecuteHaloAPI_uiS_OnCastleTranForRead("COMMON", "GetValidSPV", prms);
            int validSPVKey = Convert.ToInt32(validSPVkeyParam.Value);

            // return the determined SPV
            return GetSPVDetails(validSPVKey);
        }

        private double DetermineLTVOnFurtherLendingApplication(IApplication application)
        {
            IAccount account = application.Account;
            IMortgageLoanAccount mla = account as IMortgageLoanAccount;
            IMortgageLoan vML = mla.SecuredMortgageLoan;
            double latestValuationAmount = vML.GetActiveValuationAmount();
            double accCurrBalance = vML.CurrentBalance;
            double LTV = 0D;

            //Get the Fixed ML
            if ((account as IAccountVariFixLoan) != null)
            {
                IAccountVariFixLoan fAccount = account as IAccountVariFixLoan;
                IMortgageLoan fML = fAccount.FixedSecuredMortgageLoan;
                accCurrBalance += fML.CurrentBalance;
            }

            double valuation = latestValuationAmount;
            double furtherLendingAmount = 0D;

            IApplicationReAdvance ra = application as IApplicationReAdvance;
            IApplicationFurtherAdvance fa = application as IApplicationFurtherAdvance;
            IApplicationFurtherLoan fl = application as IApplicationFurtherLoan;

            if (ra != null)
            {
                ISupportsVariableLoanApplicationInformation svli = ra.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                IApplicationInformationVariableLoan vli = svli.VariableLoanInformation;
                furtherLendingAmount = vli.LoanAmountNoFees.HasValue ? vli.LoanAmountNoFees.Value : 0;
                furtherLendingAmount += (vli.FeesTotal.HasValue ? vli.FeesTotal.Value : 0D);

                if (ra.ClientEstimatePropertyValuation.HasValue && ra.ClientEstimatePropertyValuation.Value > valuation)
                    valuation = ra.ClientEstimatePropertyValuation.Value;
            }

            if (fa != null)
            {
                ISupportsVariableLoanApplicationInformation svli = fa.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                IApplicationInformationVariableLoan vli = svli.VariableLoanInformation;
                furtherLendingAmount = vli.LoanAmountNoFees.HasValue ? vli.LoanAmountNoFees.Value : 0;
                furtherLendingAmount += (vli.FeesTotal.HasValue ? vli.FeesTotal.Value : 0D);

                if (fa.ClientEstimatePropertyValuation.HasValue && fa.ClientEstimatePropertyValuation.Value > valuation)
                    valuation = fa.ClientEstimatePropertyValuation.Value;
            }

            if (fl != null)
            {
                ISupportsVariableLoanApplicationInformation svli = fl.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                IApplicationInformationVariableLoan vli = svli.VariableLoanInformation;
                furtherLendingAmount = vli.LoanAmountNoFees.HasValue ? vli.LoanAmountNoFees.Value : 0;
                furtherLendingAmount += (vli.FeesTotal.HasValue ? vli.FeesTotal.Value : 0D);

                if (fl.ClientEstimatePropertyValuation.HasValue && fl.ClientEstimatePropertyValuation.Value > valuation)
                    valuation = fl.ClientEstimatePropertyValuation.Value;
            }

            double ttlDisbursedAmount = accCurrBalance + furtherLendingAmount;
            LTV = ttlDisbursedAmount / valuation;
            return LTV;
        }
    }
}