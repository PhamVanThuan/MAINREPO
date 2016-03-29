using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.FinancialDomain.Managers.Models;
using SAHL.Services.FinancialDomain.Managers.Statements;
using SAHL.Services.Interfaces.FinancialDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.FinancialDomain.Managers
{
    public class FinancialDataManager : IFinancialDataManager
    {
        private IDbFactory dbFactory;

        public FinancialDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public MortgageLoanApplicationInformationModel GetApplicationInformationMortgageLoan(int applicationInformationKey)
        {
            var query = new GetMortgageLoanApplicationInformationStatement(applicationInformationKey);

            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOne<MortgageLoanApplicationInformationModel>(query);
            }
        }

        public FeeApplicationAttributesModel GetFeeApplicationAttributes(int ApplicationNumber)
        {
            var query = new GetFeeApplicationAttributesStatement(ApplicationNumber);

            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOne<FeeApplicationAttributesModel>(query);
            }
        }

        public OriginationFeesModel CalculateOriginationFees(decimal loanAmount, decimal bondRequired, Core.BusinessModel.Enums.OfferType offerType, decimal cashOut, decimal overRideCancelFee,
            bool capitaliseFees, bool isQuickPay, decimal householdIncome, Core.BusinessModel.Enums.EmploymentType employmentType, decimal ltv, bool isStaffLoan, bool isDiscountedInitiationFee,
            DateTime offerStartDate, bool capitaliseInitiationFee, bool isGEPF)
        {
            var query = new CalculateOriginationFeesStatement(loanAmount, bondRequired, offerType, cashOut, overRideCancelFee, capitaliseFees, false, false, isQuickPay, householdIncome,
                employmentType, ltv, 0, isStaffLoan, isDiscountedInitiationFee, offerStartDate, capitaliseInitiationFee, isGEPF);

            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOne<OriginationFeesModel>(query);
            }
        }

        public void RemoveAllApplicationFees(int ApplicationNumber)
        {
            var statement = new RemoveAllApplicationFeesStatement(ApplicationNumber);

            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Delete<int>(statement);
                db.Complete();
            }
        }

        public void AddApplicationExpense(int ApplicationNumber, ExpenseType expenseType, decimal amount)
        {
            var offerExpense = new OfferExpenseDataModel(ApplicationNumber, null, (int)expenseType, null, null, null, Convert.ToDouble(amount), 0, false, false);

            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<OfferExpenseDataModel>(offerExpense);
                db.Complete();
            }
        }

        public void SetApplicationExpenses(int applicationNumber, OriginationFeesModel fees)
        {
            RemoveAllApplicationFees(applicationNumber);

            AddApplicationExpense(applicationNumber, ExpenseType.CancellationFee, fees.CancellationFee);
            AddApplicationExpense(applicationNumber, ExpenseType.InitiationFee_BondPreparationFee, fees.InitiationFee);
            AddApplicationExpense(applicationNumber, ExpenseType.RegistrationFee, fees.RegistrationFee);
        }

        public OfferInformationVariableLoanDataModel GetApplicationInformationVariableLoan(int applicationInformationKey)
        {
            var query = new GetOfferInformationVariableLoanStatement(applicationInformationKey);

            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOne<OfferInformationVariableLoanDataModel>(query);
            }
        }

        public void SaveOfferInformationVariableLoan(OfferInformationVariableLoanDataModel latestOfferInformationVariableLoan)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Update(latestOfferInformationVariableLoan);
                db.Complete();
            }
        }

        public CreditCriteriaDataModel DetermineCreditCriteria(MortgageLoanPurpose mortgageLoanPurpose, EmploymentType employmentType, decimal totalLoanAmount, decimal ltv,
            OriginationSource originationSource, Product product, decimal householdIncome)
        {
            var query = new GetCreditCriteriaStatement(mortgageLoanPurpose, employmentType, totalLoanAmount, ltv, originationSource, product, householdIncome,
                CreditCriteriaAttributeType.NewBusiness);

            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOne<CreditCriteriaDataModel>(query);
            }
        }

        public RateConfigurationValuesModel GetRateConfigurationValues(int marginKey, int marketRateKey)
        {
            var query = new GetRateConfigurationValuesStatement(marginKey, marketRateKey);

            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOne<RateConfigurationValuesModel>(query);
            }
        }

        public GetValidSPVResultModel GetValidSPV(decimal LTV, string offerAttributesCSV)
        {
            var query = new GetValidSPVStatement(LTV, offerAttributesCSV);

            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOne<GetValidSPVResultModel>(query);
            }
        }

        public OfferInformationType GetLatestOfferInformationType(int ApplicationNumber)
        {
            var query = new GetLatestOfferInformationTypeStatement(ApplicationNumber);

            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return (OfferInformationType)db.SelectOne<int>(query);
            }
        }

        public OfferDataModel GetApplication(int ApplicationNumber)
        {
            var query = new GetApplicationStatement(ApplicationNumber);

            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOne<OfferDataModel>(query);
            }
        }

        public OfferInformationDataModel GetLatestApplicationOfferInformation(int applicationNumber)
        {
            var statement = new GetLatestApplicationOfferInformationStatement(applicationNumber);
            OfferInformationDataModel offerInformationDataModel = null;

            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                offerInformationDataModel = db.SelectOne<OfferInformationDataModel>(statement);
            }

            return offerInformationDataModel;
        }

        public void SetApplicationResetConfiguration(int ApplicationNumber, int SPVKey, int ProductKey)
        {
            var query = new SetApplicationResetConfigurationStatement(ApplicationNumber, SPVKey, ProductKey);

            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.ExecuteNonQuery(query);
                db.Complete();
            }
        }

        public IEnumerable<GetOfferAttributesModel> DetermineApplicationAttributes(int applicationNumber, decimal LTV, EmploymentType employmentType, decimal householdIncome, bool isStaffLoan, bool isGEPF)
        {
            var query = new DetermineApplicationAttributesStatement(applicationNumber, LTV, employmentType, householdIncome, isStaffLoan, isGEPF);

            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.Select<GetOfferAttributesModel>(query);
            }
        }

        public bool ApplicationHasAttribute(int applicationNumber, int offerAttributeTypeKey)
        {
            var query = new GetApplicationAttributeByAttributeTypeStatement(applicationNumber, offerAttributeTypeKey);
            IEnumerable<OfferAttributeDataModel> applicationAttributes = null;

            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                applicationAttributes = db.Select<OfferAttributeDataModel>(query);
            }
            return applicationAttributes.Count() > 0;
        }

        public void RemoveApplicationAttribute(int applicationNumber, int offerAttributeTypeKey)
        {
            var statement = new RemoveApplicationAttributeStatement(applicationNumber, offerAttributeTypeKey);

            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Delete<int>(statement);
                db.Complete();
            }
        }

        public void AddApplicationAttribute(int applicationNumber, int offerAttributeTypeKey)
        {
            var applicationAttribute = new OfferAttributeDataModel(applicationNumber, offerAttributeTypeKey);

            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<OfferAttributeDataModel>(applicationAttribute);
                db.Complete();
            }
        }
    }
}