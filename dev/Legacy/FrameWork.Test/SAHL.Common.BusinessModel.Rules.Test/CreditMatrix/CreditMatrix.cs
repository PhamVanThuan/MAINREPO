using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Test;
using NUnit.Framework;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using Rhino.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Rules.CreditMatrix;

namespace SAHL.Common.BusinessModel.Rules.Test.CreditMatrix
{
    [TestFixture]
    public class CreditMatrix : RuleBase
    {
        /// <summary>
        /// This interface is created for mocking purposes only, for rules that case IApplicationProduct objects 
        /// to ISupportsVariableLoanApplicationInformation objects.
        /// </summary>
        public interface IApplicationProductSupportsVariableLoanApplicationInformation : IApplicationProduct, ISupportsVariableLoanApplicationInformation
        {
        }

        /// <summary>
        /// This interface is created for mocking purposes only, for rules that cast IApplication objects 
        /// to IApplicationMortgageLoanWithCashOut objects.
        /// </summary>
        public interface IApplicationApplicationMortgageLoanWithCashOut : IApplication, IApplicationMortgageLoanWithCashOut
        {
        }

        /// <summary>
        /// This interface is created for mocking purposes only, for rules that cast IAccount objects 
        /// to IMortgageLoanAccount objects.
        /// </summary>
        public interface IAccountIMortgageLoanAccount : IAccount, IMortgageLoanAccount
        {
        }

        /// <summary>
        /// This interface is created for mocking purposes only, for rules that cast IAccount objects 
        /// to IMortgageLoanAccount objects and IAccountVariFixLoan.
        /// </summary>
        public interface IAccountIMortgageLoanAccountIAccountVariFixLoan : IAccount, IMortgageLoanAccount, IAccountVariFixLoan
        {
        }


        [NUnit.Framework.SetUp()]
        public override void Setup()
        {
            base.Setup();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        //ApplicationReasonTypeAdd TESTS

        [NUnit.Framework.Test]
        public void CreditMatrixInvestmentPropertySecondary_Pass()
        {
            CreditMatrixInvestmentPropertySecondary rule = new CreditMatrixInvestmentPropertySecondary();

            IApplicationMortgageLoan app = _mockery.StrictMock<IApplicationMortgageLoan>();
            IApplicationProductSupportsVariableLoanApplicationInformation appProduct = _mockery.StrictMock<IApplicationProductSupportsVariableLoanApplicationInformation>();
            IProperty prop = _mockery.StrictMock<IProperty>();
            IOccupancyType ocType = _mockery.StrictMock<IOccupancyType>();
            IApplicationInformationVariableLoan appInfoVarLoan = _mockery.StrictMock<IApplicationInformationVariableLoan>();

            // use occupancy type of owner occupied - this will pass immediately
            SetupResult.For(app.CurrentProduct).Return(appProduct);
            SetupResult.For(app.Property).Return(prop);
            SetupResult.For(prop.OccupancyType).Return(ocType);
            SetupResult.For(ocType.Key).Return((int)OccupancyTypes.OwnerOccupied);
            ExecuteRule(rule, 0, app);

            // now use a maximum LTV and make the variable loan return a lower value, this should result in a pass
            // (note that we're mocking the max value so we don't really care that the max value passed in is accurate, 
            // just that the test compares the values correctly)
            double maxLTV = 0.75D;
            SetupResult.For(app.CurrentProduct).Return(appProduct);
            SetupResult.For(app.Property).Return(prop);
            SetupResult.For(prop.OccupancyType).Return(ocType);
            SetupResult.For(ocType.Key).Return((int)OccupancyTypes.InvestmentProperty);

            SetupResult.For(appProduct.VariableLoanInformation).Return(appInfoVarLoan);
            SetupResult.For(appInfoVarLoan.LTV).Return(new double?(0.74D));

            ExecuteRule(rule, 0, app, maxLTV);
        }

        [NUnit.Framework.Test]
        public void CreditMatrixInvestmentPropertySecondary_Fail()
        {
            CreditMatrixInvestmentPropertySecondary rule = new CreditMatrixInvestmentPropertySecondary();

            IApplicationMortgageLoan app = _mockery.StrictMock<IApplicationMortgageLoan>();
            IApplicationProductSupportsVariableLoanApplicationInformation appProduct = _mockery.StrictMock<IApplicationProductSupportsVariableLoanApplicationInformation>();
            IProperty prop = _mockery.StrictMock<IProperty>();
            IOccupancyType ocType = _mockery.StrictMock<IOccupancyType>();
            IApplicationInformationVariableLoan appInfoVarLoan = _mockery.StrictMock<IApplicationInformationVariableLoan>();

            // use occupancy type of owner occupied - this will pass immediately
            SetupResult.For(app.CurrentProduct).Return(appProduct);
            SetupResult.For(app.Property).Return(prop);
            SetupResult.For(prop.OccupancyType).Return(ocType);
            SetupResult.For(ocType.Key).Return((int)OccupancyTypes.OwnerOccupied);
            ExecuteRule(rule, 0, app);

            // now use a maximum LTV and make the variable loan return a higher value, this should result in a pass
            // (note that we're mocking the max value so we don't really care that the max value passed in is accurate, 
            // just that the test compares the values correctly)
            double maxLTV = 0.75D;
            SetupResult.For(app.CurrentProduct).Return(appProduct);
            SetupResult.For(app.Property).Return(prop);
            SetupResult.For(prop.OccupancyType).Return(ocType);
            SetupResult.For(ocType.Key).Return((int)OccupancyTypes.InvestmentProperty);

            SetupResult.For(appProduct.VariableLoanInformation).Return(appInfoVarLoan);
            SetupResult.For(appInfoVarLoan.LTV).Return(new double?(0.76D));

            ExecuteRule(rule, 1, app, maxLTV);
        }

        [NUnit.Framework.Test]
        public void CreditMatrixInvestmentPropertySecondary_Test()
        {
            using (new Castle.ActiveRecord.SessionScope())
            {
                CreditMatrixInvestmentPropertySecondary rule = new CreditMatrixInvestmentPropertySecondary();

                IApplicationRepository appRepo = SAHL.Common.Factories.RepositoryFactory.GetRepository<IApplicationRepository>();
                IApplication app = appRepo.GetApplicationByKey(701923);

                ExecuteRule(rule, 0, app);
            }

            //IApplicationMortgageLoan app = _mockery.StrictMock<IApplicationMortgageLoan>();
            //IApplicationProductSupportsVariableLoanApplicationInformation appProduct = _mockery.StrictMock<IApplicationProductSupportsVariableLoanApplicationInformation>();
            //IProperty prop = _mockery.StrictMock<IProperty>();
            //IOccupancyType ocType = _mockery.StrictMock<IOccupancyType>();
            //IApplicationInformationVariableLoan appInfoVarLoan = _mockery.StrictMock<IApplicationInformationVariableLoan>();

            //// use occupancy type of owner occupied - this will pass immediately
            //SetupResult.For(app.CurrentProduct).Return(appProduct);
            //SetupResult.For(app.Property).Return(prop);
            //SetupResult.For(prop.OccupancyType).Return(ocType);
            //SetupResult.For(ocType.Key).Return((int)OccupancyTypes.OwnerOccupied);
            //ExecuteRule(rule, 0, app);

            //// now use a maximum LTV and make the variable loan return a higher value, this should result in a pass
            //// (note that we're mocking the max value so we don't really care that the max value passed in is accurate, 
            //// just that the test compares the values correctly)
            //double maxLTV = 0.75D;
            //SetupResult.For(app.CurrentProduct).Return(appProduct);
            //SetupResult.For(app.Property).Return(prop);
            //SetupResult.For(prop.OccupancyType).Return(ocType);
            //SetupResult.For(ocType.Key).Return((int)OccupancyTypes.InvestmentProperty);

            //SetupResult.For(appProduct.VariableLoanInformation).Return(appInfoVarLoan);
            //SetupResult.For(appInfoVarLoan.LTV).Return(new double?(0.76D));

            //ExecuteRule(rule, 1, app, maxLTV);
        }

        // CreditMatrixRefinanceLoans <- Revisit to see if there are any useful tests that can be written
        [NUnit.Framework.Test]
        public void CreditMatrixRefinanceLoans_Pass()
        {
            CreditMatrixRefinanceLoans rule = new CreditMatrixRefinanceLoans();

            IApplicationApplicationMortgageLoanWithCashOut mlco = _mockery.StrictMock<IApplicationApplicationMortgageLoanWithCashOut>();
            IApplicationProductSupportsVariableLoanApplicationInformation vlai = _mockery.StrictMock<IApplicationProductSupportsVariableLoanApplicationInformation>();
            IApplicationInformationVariableLoan vli = _mockery.StrictMock<IApplicationInformationVariableLoan>();
            IApplicationType apptype = _mockery.StrictMock<IApplicationType>();

            SetupResult.For(apptype.Key).Return((int)OfferTypes.SwitchLoan);
            SetupResult.For(mlco.ApplicationType).Return(apptype);
            SetupResult.For(mlco.RequestedCashAmount).Return(new double?(49D));
            SetupResult.For(vli.PropertyValuation).Return(new double?(100D));
            SetupResult.For(vlai.VariableLoanInformation).Return(vli);
            SetupResult.For(mlco.CurrentProduct).Return(vlai);

            ExecuteRule(rule, 0, mlco);
        }

        [NUnit.Framework.Test]
        public void CreditMatrixRefinanceLoans_Fail()
        {
            CreditMatrixRefinanceLoans rule = new CreditMatrixRefinanceLoans();

            IApplicationApplicationMortgageLoanWithCashOut mlco = _mockery.StrictMock<IApplicationApplicationMortgageLoanWithCashOut>();
            IApplicationProductSupportsVariableLoanApplicationInformation vlai = _mockery.StrictMock<IApplicationProductSupportsVariableLoanApplicationInformation>();
            IApplicationInformationVariableLoan vli = _mockery.StrictMock<IApplicationInformationVariableLoan>();
            IApplicationType apptype = _mockery.StrictMock<IApplicationType>();

            SetupResult.For(apptype.Key).Return((int)OfferTypes.SwitchLoan);
            SetupResult.For(mlco.ApplicationType).Return(apptype);
            SetupResult.For(mlco.RequestedCashAmount).Return(new double?(50D));
            SetupResult.For(vli.PropertyValuation).Return(new double?(100D));
            SetupResult.For(vlai.VariableLoanInformation).Return(vli);
            SetupResult.For(mlco.CurrentProduct).Return(vlai);

            ExecuteRule(rule, 1, mlco);
        }


        //CreditMatrixRiskCategory5
        //"Max 80% LTV and Max 25% PTI for property risk category 5."
        [NUnit.Framework.Test]
        public void CreditMatrixRiskCategory5_PassNew()
        {
            CreditMatrixRiskCategory5 rule = new CreditMatrixRiskCategory5();
            //Setup mock objects
            IApplicationMortgageLoan appML = _mockery.StrictMock<IApplicationMortgageLoan>();
            IProperty prop = _mockery.StrictMock<IProperty>();
            IAreaClassification areaClass = _mockery.StrictMock<IAreaClassification>();
            IApplicationType appType = _mockery.StrictMock<IApplicationType>();
            IApplicationProductSupportsVariableLoanApplicationInformation appProd = _mockery.StrictMock<IApplicationProductSupportsVariableLoanApplicationInformation>();
            IApplicationInformationVariableLoan vli = _mockery.StrictMock<IApplicationInformationVariableLoan>();

            //Setup results
            SetupResult.For(vli.LTV).Return(new double?(0.50D));
            SetupResult.For(vli.PTI).Return(new double?(0.24D));
            SetupResult.For(appProd.VariableLoanInformation).Return(vli);
            SetupResult.For(appML.CurrentProduct).Return(appProd);
            SetupResult.For(appType.Key).Return((int)OfferTypes.NewPurchaseLoan);
            SetupResult.For(appML.ApplicationType).Return(appType);
            SetupResult.For(areaClass.Key).Return((int)AreaClassifications.Class5);
            SetupResult.For(prop.AreaClassification).Return(areaClass);
            SetupResult.For(appML.Property).Return(prop);
            //setup rule parameters, dont care what these values are, so long as they are being evaluated
            //correctly against the values set above
            double maxLTV = 0.80D;
            double maxPTI = 0.25D;
            //run the rule using the paramters set up
            ExecuteRule(rule, 0, appML, maxLTV, maxPTI);
        }

        [NUnit.Framework.Test]
        public void CreditMatrixRiskCategory5_FailNewLTV()
        {
            CreditMatrixRiskCategory5 rule = new CreditMatrixRiskCategory5();
            //Setup mock objects
            IApplicationMortgageLoan appML = _mockery.StrictMock<IApplicationMortgageLoan>();
            IProperty prop = _mockery.StrictMock<IProperty>();
            IAreaClassification areaClass = _mockery.StrictMock<IAreaClassification>();
            IApplicationType appType = _mockery.StrictMock<IApplicationType>();
            IApplicationProductSupportsVariableLoanApplicationInformation appProd = _mockery.StrictMock<IApplicationProductSupportsVariableLoanApplicationInformation>();
            IApplicationInformationVariableLoan vli = _mockery.StrictMock<IApplicationInformationVariableLoan>();

            //Setup results
            SetupResult.For(vli.LTV).Return(new double?(0.81D));
            SetupResult.For(vli.PTI).Return(new double?(0.24D));
            SetupResult.For(appProd.VariableLoanInformation).Return(vli);
            SetupResult.For(appML.CurrentProduct).Return(appProd);
            SetupResult.For(appType.Key).Return((int)OfferTypes.NewPurchaseLoan);
            SetupResult.For(appML.ApplicationType).Return(appType);
            SetupResult.For(areaClass.Key).Return((int)AreaClassifications.Class5);
            SetupResult.For(areaClass.Description).Return("Class5");
            SetupResult.For(prop.AreaClassification).Return(areaClass);
            SetupResult.For(appML.Property).Return(prop);
            //setup rule parameters, dont care what these values are, so long as they are being evaluated
            //correctly against the values set above
            double maxLTV = 0.80D;
            double maxPTI = 0.25D;
            //run the rule using the paramters set up
            ExecuteRule(rule, 1, appML, maxLTV, maxPTI);
        }

        [NUnit.Framework.Test]
        public void CreditMatrixRiskCategory5_FailNewPTI()
        {
            CreditMatrixRiskCategory5 rule = new CreditMatrixRiskCategory5();
            //Setup mock objects
            IApplicationMortgageLoan appML = _mockery.StrictMock<IApplicationMortgageLoan>();
            IProperty prop = _mockery.StrictMock<IProperty>();
            IAreaClassification areaClass = _mockery.StrictMock<IAreaClassification>();
            IApplicationType appType = _mockery.StrictMock<IApplicationType>();
            IApplicationProductSupportsVariableLoanApplicationInformation appProd = _mockery.StrictMock<IApplicationProductSupportsVariableLoanApplicationInformation>();
            IApplicationInformationVariableLoan vli = _mockery.StrictMock<IApplicationInformationVariableLoan>();

            //Setup results
            SetupResult.For(vli.LTV).Return(new double?(0.50D));
            SetupResult.For(vli.PTI).Return(new double?(0.26D));
            SetupResult.For(appProd.VariableLoanInformation).Return(vli);
            SetupResult.For(appML.CurrentProduct).Return(appProd);
            SetupResult.For(appType.Key).Return((int)OfferTypes.NewPurchaseLoan);
            SetupResult.For(appML.ApplicationType).Return(appType);
            SetupResult.For(areaClass.Key).Return((int)AreaClassifications.Class5);
            SetupResult.For(areaClass.Description).Return("Class5");
            SetupResult.For(prop.AreaClassification).Return(areaClass);
            SetupResult.For(appML.Property).Return(prop);
            //setup rule parameters, dont care what these values are, so long as they are being evaluated
            //correctly against the values set above
            double maxLTV = 0.80D;
            double maxPTI = 0.25D;
            //run the rule using the paramters set up
            ExecuteRule(rule, 1, appML, maxLTV, maxPTI);
        }

        [NUnit.Framework.Test]
        public void CreditMatrixRiskCategory5_PassFL()
        {
            CreditMatrixRiskCategory5 rule = new CreditMatrixRiskCategory5();
            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            //Setup mock objects
            IApplicationMortgageLoan appML = _mockery.StrictMock<IApplicationMortgageLoan>();
            IProperty prop = _mockery.StrictMock<IProperty>();
            IAreaClassification areaClass = _mockery.StrictMock<IAreaClassification>();
            IApplicationType appType = _mockery.StrictMock<IApplicationType>();
            IApplicationProductSupportsVariableLoanApplicationInformation appProd = _mockery.StrictMock<IApplicationProductSupportsVariableLoanApplicationInformation>();
            IApplicationInformationVariableLoan vli = _mockery.StrictMock<IApplicationInformationVariableLoan>();
            IRateConfiguration rc = _mockery.StrictMock<IRateConfiguration>();
            IMargin margin = _mockery.StrictMock<IMargin>();

            //Account
            IAccountRepository accRepo = _mockery.StrictMock<IAccountRepository>();
            MockCache.Add(typeof(IAccountRepository).ToString(), accRepo);
            IProduct prod = _mockery.StrictMock<IProduct>();
            IAccountSequence accSeq = _mockery.StrictMock<IAccountSequence>();
            IAccountIMortgageLoanAccount acc = _mockery.StrictMock<IAccountIMortgageLoanAccount>();
            IMortgageLoan vml = _mockery.StrictMock<IMortgageLoan>();
            int accountKey = 1;

            double marginValue = 0.021D;
            double? propVal = 10000; //use a high valuation to get low LTV
            double? income = 10000; //use a high income to get low PTI
            int? term = 240;

            double baseRate = 0.100D;
            double varDiscount = 0D;
            double varCurrBalance = 1000;
            double furtherAmount = 1000;
            //Setup results

            SetupResult.For(vli.LoanAgreementAmount).Return(furtherAmount);
            SetupResult.For(vml.CurrentBalance).Return(varCurrBalance);
            SetupResult.For(vml.RateAdjustment).Return(varDiscount);
            SetupResult.For(vml.ActiveMarketRate).Return(baseRate);
            SetupResult.For(acc.SecuredMortgageLoan).Return(vml);

            SetupResult.For(appML.HasFinancialAdjustment(FinancialAdjustmentTypeSources.InterestOnly)).Return(false);
            SetupResult.For(vli.Term).Return(term);
            SetupResult.For(vli.HouseholdIncome).Return(income);
            SetupResult.For(vli.PropertyValuation).Return(propVal);
            SetupResult.For(margin.Value).Return(marginValue);
            SetupResult.For(rc.Margin).Return(margin);
            SetupResult.For(vli.RateConfiguration).Return(rc);

            SetupResult.For(accSeq.Key).Return(accountKey);
            SetupResult.For(appML.ReservedAccount).Return(accSeq);
            SetupResult.For(prod.Key).Return((int)SAHL.Common.Globals.Products.NewVariableLoan);
            SetupResult.For(acc.Product).Return(prod);
            SetupResult.For(accRepo.GetAccountByKey(accountKey)).IgnoreArguments().Return(acc);
            SetupResult.For(appProd.VariableLoanInformation).Return(vli);
            SetupResult.For(appML.CurrentProduct).Return(appProd);
            SetupResult.For(appType.Key).Return((int)OfferTypes.FurtherLoan);
            SetupResult.For(appML.ApplicationType).Return(appType);
            SetupResult.For(areaClass.Key).Return((int)AreaClassifications.Class5);
            SetupResult.For(areaClass.Description).Return("Class5");
            SetupResult.For(prop.AreaClassification).Return(areaClass);
            SetupResult.For(appML.Property).Return(prop);
            
            //setup rule parameters, dont care what these values are, so long as they are being evaluated
            //correctly against the values set above
            double maxLTV = 0.80D;
            double maxPTI = 0.25D;
            //run the rule using the paramters set up
            ExecuteRule(rule, 0, appML, maxLTV, maxPTI);
        }

        [NUnit.Framework.Test]
        public void CreditMatrixRiskCategory5_FailFLPTI()
        {
            CreditMatrixRiskCategory5 rule = new CreditMatrixRiskCategory5();
            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            //Setup mock objects
            IApplicationMortgageLoan appML = _mockery.StrictMock<IApplicationMortgageLoan>();
            IProperty prop = _mockery.StrictMock<IProperty>();
            IAreaClassification areaClass = _mockery.StrictMock<IAreaClassification>();
            IApplicationType appType = _mockery.StrictMock<IApplicationType>();
            IApplicationProductSupportsVariableLoanApplicationInformation appProd = _mockery.StrictMock<IApplicationProductSupportsVariableLoanApplicationInformation>();
            IApplicationInformationVariableLoan vli = _mockery.StrictMock<IApplicationInformationVariableLoan>();
            IRateConfiguration rc = _mockery.StrictMock<IRateConfiguration>();
            IMargin margin = _mockery.StrictMock<IMargin>();

            //Account
            IAccountRepository accRepo = _mockery.StrictMock<IAccountRepository>();
            MockCache.Add(typeof(IAccountRepository).ToString(), accRepo);
            IProduct prod = _mockery.StrictMock<IProduct>();
            IAccountSequence accSeq = _mockery.StrictMock<IAccountSequence>();
            IAccountIMortgageLoanAccount acc = _mockery.StrictMock<IAccountIMortgageLoanAccount>();
            IMortgageLoan vml = _mockery.StrictMock<IMortgageLoan>();
            int accountKey = 1;

            double marginValue = 0.021D;
            double? propVal = 10000; //use a high valuation to get low LTV
            double? income = 1; //use a low income to get high PTI
            int? term = 240;

            double baseRate = 0.100D;
            double varDiscount = 0D;
            double varCurrBalance = 1000;
            double furtherAmount = 1000;
            //Setup results

            SetupResult.For(vli.LoanAgreementAmount).Return(furtherAmount);
            SetupResult.For(vml.CurrentBalance).Return(varCurrBalance);
            SetupResult.For(vml.RateAdjustment).Return(varDiscount);
            SetupResult.For(vml.ActiveMarketRate).Return(baseRate);
            SetupResult.For(acc.SecuredMortgageLoan).Return(vml);

            SetupResult.For(appML.HasFinancialAdjustment(FinancialAdjustmentTypeSources.InterestOnly)).Return(false);
            SetupResult.For(vli.Term).Return(term);
            SetupResult.For(vli.HouseholdIncome).Return(income);
            SetupResult.For(vli.PropertyValuation).Return(propVal);
            SetupResult.For(margin.Value).Return(marginValue);
            SetupResult.For(rc.Margin).Return(margin);
            SetupResult.For(vli.RateConfiguration).Return(rc);

            SetupResult.For(accSeq.Key).Return(accountKey);
            SetupResult.For(appML.ReservedAccount).Return(accSeq);
            SetupResult.For(prod.Key).Return((int)SAHL.Common.Globals.Products.NewVariableLoan);
            SetupResult.For(acc.Product).Return(prod);
            SetupResult.For(accRepo.GetAccountByKey(accountKey)).IgnoreArguments().Return(acc);
            SetupResult.For(appProd.VariableLoanInformation).Return(vli);
            SetupResult.For(appML.CurrentProduct).Return(appProd);
            SetupResult.For(appType.Key).Return((int)OfferTypes.FurtherLoan);
            SetupResult.For(appML.ApplicationType).Return(appType);
            SetupResult.For(areaClass.Key).Return((int)AreaClassifications.Class5);
            SetupResult.For(areaClass.Description).Return("Class5");
            SetupResult.For(prop.AreaClassification).Return(areaClass);
            SetupResult.For(appML.Property).Return(prop);

            //setup rule parameters, dont care what these values are, so long as they are being evaluated
            //correctly against the values set above
            double maxLTV = 0.80D;
            double maxPTI = 0.25D;
            //run the rule using the paramters set up
            ExecuteRule(rule, 1, appML, maxLTV, maxPTI);
        }

        [NUnit.Framework.Test]
        public void CreditMatrixRiskCategory5_FailFLLTV()
        {
            CreditMatrixRiskCategory5 rule = new CreditMatrixRiskCategory5();
            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            //Setup mock objects
            IApplicationMortgageLoan appML = _mockery.StrictMock<IApplicationMortgageLoan>();
            IProperty prop = _mockery.StrictMock<IProperty>();
            IAreaClassification areaClass = _mockery.StrictMock<IAreaClassification>();
            IApplicationType appType = _mockery.StrictMock<IApplicationType>();
            IApplicationProductSupportsVariableLoanApplicationInformation appProd = _mockery.StrictMock<IApplicationProductSupportsVariableLoanApplicationInformation>();
            IApplicationInformationVariableLoan vli = _mockery.StrictMock<IApplicationInformationVariableLoan>();
            IRateConfiguration rc = _mockery.StrictMock<IRateConfiguration>();
            IMargin margin = _mockery.StrictMock<IMargin>();

            //Account
            IAccountRepository accRepo = _mockery.StrictMock<IAccountRepository>();
            MockCache.Add(typeof(IAccountRepository).ToString(), accRepo);
            IProduct prod = _mockery.StrictMock<IProduct>();
            IAccountSequence accSeq = _mockery.StrictMock<IAccountSequence>();
            IAccountIMortgageLoanAccount acc = _mockery.StrictMock<IAccountIMortgageLoanAccount>();
            IMortgageLoan vml = _mockery.StrictMock<IMortgageLoan>();
            int accountKey = 1;

            double marginValue = 0.021D;
            double? propVal = 1; //use a low valuation to get high LTV
            double? income = 10000; //use a high income to get low PTI
            int? term = 240;

            double baseRate = 0.100D;
            double varDiscount = 0D;
            double varCurrBalance = 1000;
            double furtherAmount = 1000;
            //Setup results

            SetupResult.For(vli.LoanAgreementAmount).Return(furtherAmount);
            SetupResult.For(vml.CurrentBalance).Return(varCurrBalance);
            SetupResult.For(vml.RateAdjustment).Return(varDiscount);
            SetupResult.For(vml.ActiveMarketRate).Return(baseRate);
            SetupResult.For(acc.SecuredMortgageLoan).Return(vml);

            SetupResult.For(appML.HasFinancialAdjustment(FinancialAdjustmentTypeSources.InterestOnly)).Return(false);
            SetupResult.For(vli.Term).Return(term);
            SetupResult.For(vli.HouseholdIncome).Return(income);
            SetupResult.For(vli.PropertyValuation).Return(propVal);
            SetupResult.For(margin.Value).Return(marginValue);
            SetupResult.For(rc.Margin).Return(margin);
            SetupResult.For(vli.RateConfiguration).Return(rc);

            SetupResult.For(accSeq.Key).Return(accountKey);
            SetupResult.For(appML.ReservedAccount).Return(accSeq);
            SetupResult.For(prod.Key).Return((int)SAHL.Common.Globals.Products.NewVariableLoan);
            SetupResult.For(acc.Product).Return(prod);
            SetupResult.For(accRepo.GetAccountByKey(accountKey)).IgnoreArguments().Return(acc);
            SetupResult.For(appProd.VariableLoanInformation).Return(vli);
            SetupResult.For(appML.CurrentProduct).Return(appProd);
            SetupResult.For(appType.Key).Return((int)OfferTypes.FurtherLoan);
            SetupResult.For(appML.ApplicationType).Return(appType);
            SetupResult.For(areaClass.Key).Return((int)AreaClassifications.Class5);
            SetupResult.For(areaClass.Description).Return("Class5");
            SetupResult.For(prop.AreaClassification).Return(areaClass);
            SetupResult.For(appML.Property).Return(prop);

            //setup rule parameters, dont care what these values are, so long as they are being evaluated
            //correctly against the values set above
            double maxLTV = 0.80D;
            double maxPTI = 0.25D;
            //run the rule using the paramters set up
            ExecuteRule(rule, 1, appML, maxLTV, maxPTI);
        }
    }
}
