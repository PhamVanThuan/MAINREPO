using System;
using System.Collections.Generic;
using System.Data;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Rules.Cap;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Test;

namespace SAHL.Common.BusinessModel.Rules.Test.Cap
{
    [TestFixture]
    public class Cap : RuleBase
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        #region ApplicationCAP2QualifyStatus

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationCAP2QualifyStatusSuccess()
        {
            using (new SessionScope())
            {
                ApplicationCAP2QualifyStatus rule = new ApplicationCAP2QualifyStatus();

                // Setup the correct object to pass along
                ICapApplication capApplication = _mockery.StrictMock<ICapApplication>();

                // Setup capApplication.Account
                IAccount account = _mockery.StrictMock<IAccount>();
                SetupResult.For(capApplication.Account).Return(account);

                // Setup Account.account
                IAccountStatus accountStatus = _mockery.StrictMock<IAccountStatus>();
                SetupResult.For(account.AccountStatus).Return(accountStatus);
                SetupResult.For(account.Key).Return(1);
                SetupResult.For(accountStatus.Key).Return(Convert.ToInt32(AccountStatuses.Open));

                ExecuteRule(rule, 0, capApplication);
            }
        }

        /// <summary>
        /// Expects Messages.Count = 1.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationCAP2QualifyStatusFail()
        {
            using (new SessionScope())
            {
                ApplicationCAP2QualifyStatus rule = new ApplicationCAP2QualifyStatus();

                // Setup the correct object to pass along
                ICapApplication capApplication = _mockery.StrictMock<ICapApplication>();

                // Setup capApplication.Account
                IAccount account = _mockery.StrictMock<IAccount>();
                SetupResult.For(capApplication.Account).Return(account);

                // Setup Account.acco
                IAccountStatus accountStatus = _mockery.StrictMock<IAccountStatus>();
                SetupResult.For(account.AccountStatus).Return(accountStatus);
                SetupResult.For(accountStatus.Key).Return(Convert.ToInt32(AccountStatuses.Closed));
                SetupResult.For(accountStatus.Description).Return(AccountStatuses.Closed.ToString());
                SetupResult.For(account.Key).Return(1);

                ExecuteRule(rule, 1, capApplication);
            }
        }

        #endregion ApplicationCAP2QualifyStatus

        #region ApplicationCap2QualifyInterestOnly

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationCap2QualifyInterestOnlyPass()
        {
            using (new SessionScope())
            {
                ApplicationCap2QualifyInterestOnly rule = new ApplicationCap2QualifyInterestOnly();
                ICapApplication capApplication = _mockery.StrictMock<ICapApplication>();

                IMortgageLoanAccount mla = _mockery.StrictMock<IMortgageLoanAccount>();
                SetupResult.For(capApplication.Account).Return(mla);

                IMortgageLoan ml = _mockery.StrictMock<IMortgageLoan>();
                SetupResult.For(mla.SecuredMortgageLoan).Return(ml);

                IEventList<IFinancialAdjustment> financialAdjustments = new EventList<IFinancialAdjustment>();
                IFinancialAdjustment financialAdjustment = _mockery.StrictMock<IFinancialAdjustment>();
                financialAdjustments.Add(Messages, financialAdjustment);

                SetupResult.For(ml.FinancialAdjustments).Return(financialAdjustments);

                IFinancialAdjustmentTypeSource financialAdjustmentTypeSource = _mockery.StrictMock<IFinancialAdjustmentTypeSource>();
                SetupResult.For(financialAdjustment.FinancialAdjustmentTypeSource).Return(financialAdjustmentTypeSource);
                SetupResult.For(financialAdjustmentTypeSource.Key).Return(Convert.ToInt32(FinancialAdjustmentTypeSources.CAP));
                IFinancialAdjustmentStatus financialAdjustmentStatus = _mockery.StrictMock<IFinancialAdjustmentStatus>();
                SetupResult.For(financialAdjustment.FinancialAdjustmentStatus).Return(financialAdjustmentStatus);
                SetupResult.For(financialAdjustmentStatus.Key).Return(Convert.ToInt32(FinancialAdjustmentStatuses.Active));

                ExecuteRule(rule, 0, capApplication);
            }
        }

        /// <summary>
        /// Expects Messages.Count = 1.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationCap2QualifyInterestOnlyFail()
        {
            using (new SessionScope())
            {
                ApplicationCap2QualifyInterestOnly rule = new ApplicationCap2QualifyInterestOnly();
                ICapApplication capApplication = _mockery.StrictMock<ICapApplication>();

                IMortgageLoanAccount mla = _mockery.StrictMock<IMortgageLoanAccount>();
                SetupResult.For(capApplication.Account).Return(mla);

                IMortgageLoan ml = _mockery.StrictMock<IMortgageLoan>();
                SetupResult.For(mla.SecuredMortgageLoan).Return(ml);

                IEventList<IFinancialAdjustment> financialAdjustments = new EventList<IFinancialAdjustment>();
                IFinancialAdjustment financialAdjustment = _mockery.StrictMock<IFinancialAdjustment>();
                financialAdjustments.Add(Messages, financialAdjustment);

                SetupResult.For(ml.FinancialAdjustments).Return(financialAdjustments);

                IFinancialAdjustmentTypeSource financialAdjustmentTypeSource = _mockery.StrictMock<IFinancialAdjustmentTypeSource>();
                SetupResult.For(financialAdjustment.FinancialAdjustmentTypeSource).Return(financialAdjustmentTypeSource);
                SetupResult.For(financialAdjustmentTypeSource.Key).Return(Convert.ToInt32(FinancialAdjustmentTypeSources.InterestOnly));
                IFinancialAdjustmentStatus financialAdjustmentStatus = _mockery.StrictMock<IFinancialAdjustmentStatus>();
                SetupResult.For(financialAdjustment.FinancialAdjustmentStatus).Return(financialAdjustmentStatus);
                SetupResult.For(financialAdjustmentStatus.Key).Return(Convert.ToInt32(FinancialAdjustmentStatuses.Active));

                ExecuteRule(rule, 1, capApplication);
            }
        }

        #endregion ApplicationCap2QualifyInterestOnly

        #region ApplicationCap2QualifyProduct

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationCap2QualifyProductPass()
        {
            ApplicationCap2QualifyProduct rule = new ApplicationCap2QualifyProduct();

            ICapApplication capApplication = _mockery.StrictMock<ICapApplication>();
            IAccount account = _mockery.StrictMock<IAccount>();
            SetupResult.For(capApplication.Account).Return(account);
            IProduct product = _mockery.StrictMock<IProduct>();
            SetupResult.For(account.Product).Return(product);
            SetupResult.For(product.Key).Return(Convert.ToInt32(SAHL.Common.Globals.Products.VariableLoan));

            ExecuteRule(rule, 0, capApplication);
        }

        /// <summary>
        /// Expects Messages.Count = 1.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationCap2QualifyProductFail()
        {
            ApplicationCap2QualifyProduct rule = new ApplicationCap2QualifyProduct();

            ICapApplication capApplication = _mockery.StrictMock<ICapApplication>();
            IAccount account = _mockery.StrictMock<IAccount>();
            SetupResult.For(capApplication.Account).Return(account);
            IProduct product = _mockery.StrictMock<IProduct>();
            SetupResult.For(account.Product).Return(product);
            SetupResult.For(product.Key).Return(Convert.ToInt32(SAHL.Common.Globals.Products.HomeOwnersCover));

            ExecuteRule(rule, 1, capApplication);
        }

        #endregion ApplicationCap2QualifyProduct

        #region ApplicationCap2QualifySPVLTV

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationCap2QualifySPVLTVPass()
        {
            using (new SessionScope())
            {
                ApplicationCap2QualifySPVLTV rule = new ApplicationCap2QualifySPVLTV();

                ICapApplication capApplication = _mockery.StrictMock<ICapApplication>();
                IEventList<ICapApplicationDetail> capAppDetails = new EventList<ICapApplicationDetail>();
                SetupResult.For(capApplication.CapApplicationDetails).Return(capAppDetails);
                ICapApplicationDetail capDetail = _mockery.StrictMock<ICapApplicationDetail>();
                capAppDetails.Add(Messages, capDetail);
                SetupResult.For(capDetail.Fee).Return((double)(5000.00));
                ICapTypeConfigurationDetail capTypeConfigDetail = _mockery.StrictMock<ICapTypeConfigurationDetail>();
                SetupResult.For(capDetail.CapTypeConfigurationDetail).Return(capTypeConfigDetail);
                ICapType capType = _mockery.StrictMock<ICapType>();
                SetupResult.For(capTypeConfigDetail.CapType).Return(capType);
                SetupResult.For(capType.Description).Return("1%");

                ICapStatus capStatus = _mockery.StrictMock<ICapStatus>();
                SetupResult.For(capStatus.Key).Return((int)CapStatuses.ReadvanceRequired);
                SetupResult.For(capDetail.CapStatus).Return(capStatus);

                IMortgageLoanAccount mla = _mockery.StrictMock<IMortgageLoanAccount>();
                SetupResult.For(capApplication.Account).Return(mla);

                IMortgageLoan ml = _mockery.StrictMock<IMortgageLoan>();
                SetupResult.For(mla.SecuredMortgageLoan).Return(ml);
                SetupResult.For(mla.LoanCurrentBalance).Return((double)500000);
                SetupResult.For(ml.GetActiveValuationAmount()).Return((double)700000);

                IAccount acc = _mockery.StrictMock<IAccount>();
                SetupResult.For(ml.Account).Return(acc);

                ISPV spv = _mockery.StrictMock<ISPV>();
                SetupResult.For(acc.SPV).Return(spv);

                IEventList<ISPVMandate> mandates = new EventList<ISPVMandate>();
                SetupResult.For(spv.SPVMandates).Return(mandates);
                ISPVMandate spvMandate = _mockery.StrictMock<ISPVMandate>();
                SetupResult.For(spvMandate.MaxLTV).Return((double)0.8);

                mandates.Add(Messages, spvMandate);

                ExecuteRule(rule, 0, capApplication);
            }
        }

        /// <summary>
        /// Expects Messages.Count = 1.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationCap2QualifySPVLTVFail()
        {
            using (new SessionScope())
            {
                ApplicationCap2QualifySPVLTV rule = new ApplicationCap2QualifySPVLTV();

                ICapApplication capApplication = _mockery.StrictMock<ICapApplication>();
                IEventList<ICapApplicationDetail> capAppDetails = new EventList<ICapApplicationDetail>();
                SetupResult.For(capApplication.CapApplicationDetails).Return(capAppDetails);
                ICapApplicationDetail capDetail = _mockery.StrictMock<ICapApplicationDetail>();
                capAppDetails.Add(Messages, capDetail);
                SetupResult.For(capDetail.Fee).Return((double)(5000.00));
                ICapTypeConfigurationDetail capTypeConfigDetail = _mockery.StrictMock<ICapTypeConfigurationDetail>();
                SetupResult.For(capDetail.CapTypeConfigurationDetail).Return(capTypeConfigDetail);
                ICapType capType = _mockery.StrictMock<ICapType>();
                SetupResult.For(capTypeConfigDetail.CapType).Return(capType);
                SetupResult.For(capType.Description).Return("1%");

                ICapStatus capStatus = _mockery.StrictMock<ICapStatus>();
                SetupResult.For(capStatus.Key).Return((int)CapStatuses.ReadvanceRequired);
                SetupResult.For(capDetail.CapStatus).Return(capStatus);

                IMortgageLoanAccount mla = _mockery.StrictMock<IMortgageLoanAccount>();
                SetupResult.For(capApplication.Account).Return(mla);

                IMortgageLoan ml = _mockery.StrictMock<IMortgageLoan>();
                SetupResult.For(mla.SecuredMortgageLoan).Return(ml);
                SetupResult.For(mla.LoanCurrentBalance).Return((double)500000);
                SetupResult.For(ml.GetActiveValuationAmount()).Return((double)600000);

                IAccount acc = _mockery.StrictMock<IAccount>();
                SetupResult.For(ml.Account).Return(acc);

                ISPV spv = _mockery.StrictMock<ISPV>();
                SetupResult.For(acc.SPV).Return(spv);

                IEventList<ISPVMandate> mandates = new EventList<ISPVMandate>();
                SetupResult.For(spv.SPVMandates).Return(mandates);
                ISPVMandate spvMandate = _mockery.StrictMock<ISPVMandate>();
                SetupResult.For(spvMandate.MaxLTV).Return((double)0.8);

                mandates.Add(Messages, spvMandate);

                ExecuteRule(rule, 1, capApplication);
            }
        }

        #endregion ApplicationCap2QualifySPVLTV

        #region ApplicationCap2QualifySPVPTI

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationCap2QualifySPVPTIPass()
        {
            ApplicationCap2QualifySPVPTI rule = new ApplicationCap2QualifySPVPTI();

            ICapApplication capApplication = _mockery.StrictMock<ICapApplication>();
            IEventList<ICapApplicationDetail> capAppDetails = new EventList<ICapApplicationDetail>();
            SetupResult.For(capApplication.CapApplicationDetails).Return(capAppDetails);
            ICapApplicationDetail capDetail = _mockery.StrictMock<ICapApplicationDetail>();
            capAppDetails.Add(Messages, capDetail);
            SetupResult.For(capDetail.Payment).Return((double)(1000.00));
            ICapTypeConfigurationDetail capTypeConfigDetail = _mockery.StrictMock<ICapTypeConfigurationDetail>();
            SetupResult.For(capDetail.CapTypeConfigurationDetail).Return(capTypeConfigDetail);
            ICapType capType = _mockery.StrictMock<ICapType>();
            SetupResult.For(capTypeConfigDetail.CapType).Return(capType);
            SetupResult.For(capType.Description).Return("1%");

            ICapStatus capStatus = _mockery.StrictMock<ICapStatus>();
            SetupResult.For(capStatus.Key).Return((int)CapStatuses.ReadvanceRequired);
            SetupResult.For(capDetail.CapStatus).Return(capStatus);

            IMortgageLoanAccount mla = _mockery.StrictMock<IMortgageLoanAccount>();
            SetupResult.For(capApplication.Account).Return(mla);

            IMortgageLoan ml = _mockery.StrictMock<IMortgageLoan>();
            SetupResult.For(mla.SecuredMortgageLoan).Return(ml);
            SetupResult.For(mla.FixedPayment).Return((double)2000.00);
            SetupResult.For(mla.GetHouseholdIncome()).Return((double)10001);

            IAccount acc = _mockery.StrictMock<IAccount>();
            SetupResult.For(ml.Account).Return(acc);

            ISPV spv = _mockery.StrictMock<ISPV>();
            SetupResult.For(acc.SPV).Return(spv);

            IEventList<ISPVMandate> mandates = new EventList<ISPVMandate>();
            SetupResult.For(spv.SPVMandates).Return(mandates);
            ISPVMandate spvMandate = _mockery.StrictMock<ISPVMandate>();
            SetupResult.For(spvMandate.MaxPTI).Return((double)0.3);

            mandates.Add(Messages, spvMandate);

            ExecuteRule(rule, 0, capApplication);
        }

        /// <summary>
        /// Expects Messages.Count = 1.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationCap2QualifySPVPTIFail()
        {
            ApplicationCap2QualifySPVPTI rule = new ApplicationCap2QualifySPVPTI();

            ICapApplication capApplication = _mockery.StrictMock<ICapApplication>();
            IEventList<ICapApplicationDetail> capAppDetails = new EventList<ICapApplicationDetail>();
            SetupResult.For(capApplication.CapApplicationDetails).Return(capAppDetails);
            ICapApplicationDetail capDetail = _mockery.StrictMock<ICapApplicationDetail>();
            capAppDetails.Add(Messages, capDetail);
            SetupResult.For(capDetail.Payment).Return((double)(2000.00));
            ICapTypeConfigurationDetail capTypeConfigDetail = _mockery.StrictMock<ICapTypeConfigurationDetail>();
            SetupResult.For(capDetail.CapTypeConfigurationDetail).Return(capTypeConfigDetail);
            ICapType capType = _mockery.StrictMock<ICapType>();
            SetupResult.For(capTypeConfigDetail.CapType).Return(capType);
            SetupResult.For(capType.Description).Return("1%");

            ICapStatus capStatus = _mockery.StrictMock<ICapStatus>();
            SetupResult.For(capStatus.Key).Return((int)CapStatuses.ReadvanceRequired);
            SetupResult.For(capDetail.CapStatus).Return(capStatus);

            IMortgageLoanAccount mla = _mockery.StrictMock<IMortgageLoanAccount>();
            SetupResult.For(capApplication.Account).Return(mla);

            IMortgageLoan ml = _mockery.StrictMock<IMortgageLoan>();
            SetupResult.For(mla.SecuredMortgageLoan).Return(ml);
            SetupResult.For(mla.FixedPayment).Return((double)5000.00);
            SetupResult.For(mla.GetHouseholdIncome()).Return((double)10000);

            IAccount acc = _mockery.StrictMock<IAccount>();
            SetupResult.For(ml.Account).Return(acc);

            ISPV spv = _mockery.StrictMock<ISPV>();
            SetupResult.For(acc.SPV).Return(spv);

            IEventList<ISPVMandate> mandates = new EventList<ISPVMandate>();
            SetupResult.For(spv.SPVMandates).Return(mandates);
            ISPVMandate spvMandate = _mockery.StrictMock<ISPVMandate>();
            SetupResult.For(spvMandate.MaxPTI).Return((double)0.1);

            mandates.Add(Messages, spvMandate);

            ExecuteRule(rule, 1, capApplication);
        }

        #endregion ApplicationCap2QualifySPVPTI

        #region ApplicationCap2QualifySPVBondAmount

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationCap2QualifySPVBondAmountPass()
        {
            using (new SessionScope())
            {
                ApplicationCap2QualifySPVBondAmount rule = new ApplicationCap2QualifySPVBondAmount();

                ICapApplication capApplication = _mockery.StrictMock<ICapApplication>();
                IEventList<ICapApplicationDetail> capAppDetails = new EventList<ICapApplicationDetail>();
                SetupResult.For(capApplication.CapApplicationDetails).Return(capAppDetails);
                ICapApplicationDetail capDetail = _mockery.StrictMock<ICapApplicationDetail>();
                capAppDetails.Add(Messages, capDetail);
                SetupResult.For(capDetail.Fee).Return((double)(5000));
                ICapTypeConfigurationDetail capTypeConfigDetail = _mockery.StrictMock<ICapTypeConfigurationDetail>();
                SetupResult.For(capDetail.CapTypeConfigurationDetail).Return(capTypeConfigDetail);
                ICapType capType = _mockery.StrictMock<ICapType>();
                SetupResult.For(capTypeConfigDetail.CapType).Return(capType);
                SetupResult.For(capType.Description).Return("1%");

                IMortgageLoanAccount mla = _mockery.StrictMock<IMortgageLoanAccount>();
                SetupResult.For(capApplication.Account).Return(mla);

                IMortgageLoan ml = _mockery.StrictMock<IMortgageLoan>();
                SetupResult.For(mla.SecuredMortgageLoan).Return(ml);
                SetupResult.For(mla.LoanCurrentBalance).Return((double)500000);
                IBond bond = _mockery.StrictMock<IBond>();
                SetupResult.For(ml.GetLatestRegisteredBond()).Return(bond);
                SetupResult.For(bond.BondRegistrationAmount).Return((double)505001);

                ISPV spv = _mockery.StrictMock<ISPV>();
                SetupResult.For(mla.SPV).Return(spv);

                IEventList<ISPVMandate> mandates = new EventList<ISPVMandate>();
                SetupResult.For(spv.SPVMandates).Return(mandates);
                ISPVMandate spvMandate = _mockery.StrictMock<ISPVMandate>();
                SetupResult.For(spvMandate.ExceedBondAmount).Return((double)0);

                mandates.Add(Messages, spvMandate);

                ExecuteRule(rule, 0, capApplication);
            }
        }

        /// <summary>
        /// Expects Messages.Count = 1.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationCap2QualifySPVBondAmountFail()
        {
            using (new SessionScope())
            {
                ApplicationCap2QualifySPVBondAmount rule = new ApplicationCap2QualifySPVBondAmount();

                ICapApplication capApplication = _mockery.StrictMock<ICapApplication>();
                IEventList<ICapApplicationDetail> capAppDetails = new EventList<ICapApplicationDetail>();
                SetupResult.For(capApplication.CapApplicationDetails).Return(capAppDetails);
                ICapApplicationDetail capDetail = _mockery.StrictMock<ICapApplicationDetail>();
                capAppDetails.Add(Messages, capDetail);
                SetupResult.For(capDetail.Fee).Return((double)(5000));
                ICapTypeConfigurationDetail capTypeConfigDetail = _mockery.StrictMock<ICapTypeConfigurationDetail>();
                SetupResult.For(capDetail.CapTypeConfigurationDetail).Return(capTypeConfigDetail);
                ICapType capType = _mockery.StrictMock<ICapType>();
                SetupResult.For(capTypeConfigDetail.CapType).Return(capType);
                SetupResult.For(capType.Description).Return("1%");

                IMortgageLoanAccount mla = _mockery.StrictMock<IMortgageLoanAccount>();
                SetupResult.For(capApplication.Account).Return(mla);

                IMortgageLoan ml = _mockery.StrictMock<IMortgageLoan>();
                SetupResult.For(mla.SecuredMortgageLoan).Return(ml);
                SetupResult.For(mla.LoanCurrentBalance).Return((double)500000);
                IBond bond = _mockery.StrictMock<IBond>();
                SetupResult.For(ml.GetLatestRegisteredBond()).Return(bond);
                SetupResult.For(bond.BondRegistrationAmount).Return((double)501000);

                ISPV spv = _mockery.StrictMock<ISPV>();
                SetupResult.For(mla.SPV).Return(spv);

                IEventList<ISPVMandate> mandates = new EventList<ISPVMandate>();
                SetupResult.For(spv.SPVMandates).Return(mandates);
                ISPVMandate spvMandate = _mockery.StrictMock<ISPVMandate>();
                SetupResult.For(spvMandate.ExceedBondAmount).Return((double)0);

                mandates.Add(Messages, spvMandate);

                ExecuteRule(rule, 1, capApplication);
            }
        }

        #endregion ApplicationCap2QualifySPVBondAmount

        #region ApplicationCap2QualifySPVBondPercent

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationCap2QualifySPVBondPercentPass()
        {
            using (new SessionScope())
            {
                ApplicationCap2QualifySPVBondPercent rule = new ApplicationCap2QualifySPVBondPercent();

                ICapApplication capApplication = _mockery.StrictMock<ICapApplication>();
                IEventList<ICapApplicationDetail> capAppDetails = new EventList<ICapApplicationDetail>();
                SetupResult.For(capApplication.CapApplicationDetails).Return(capAppDetails);
                ICapApplicationDetail capDetail = _mockery.StrictMock<ICapApplicationDetail>();
                capAppDetails.Add(Messages, capDetail);
                SetupResult.For(capDetail.Fee).Return((double)(5000));
                ICapTypeConfigurationDetail capTypeConfigDetail = _mockery.StrictMock<ICapTypeConfigurationDetail>();
                SetupResult.For(capDetail.CapTypeConfigurationDetail).Return(capTypeConfigDetail);
                ICapType capType = _mockery.StrictMock<ICapType>();
                SetupResult.For(capTypeConfigDetail.CapType).Return(capType);
                SetupResult.For(capType.Description).Return("1%");

                IMortgageLoanAccount mla = _mockery.StrictMock<IMortgageLoanAccount>();
                SetupResult.For(capApplication.Account).Return(mla);

                IMortgageLoan ml = _mockery.StrictMock<IMortgageLoan>();
                SetupResult.For(mla.SecuredMortgageLoan).Return(ml);
                SetupResult.For(mla.LoanCurrentBalance).Return((double)500000);
                IBond bond = _mockery.StrictMock<IBond>();
                SetupResult.For(ml.GetLatestRegisteredBond()).Return(bond);
                SetupResult.For(bond.BondRegistrationAmount).Return((double)505001);

                ISPV spv = _mockery.StrictMock<ISPV>();
                SetupResult.For(mla.SPV).Return(spv);

                IEventList<ISPVMandate> mandates = new EventList<ISPVMandate>();
                SetupResult.For(spv.SPVMandates).Return(mandates);
                ISPVMandate spvMandate = _mockery.StrictMock<ISPVMandate>();
                SetupResult.For(spvMandate.ExceedBondPercent).Return((double)0);

                mandates.Add(Messages, spvMandate);

                ExecuteRule(rule, 0, capApplication);
            }
        }

        /// <summary>
        /// Expects Messages.Count = 1.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationCap2QualifySPVBondPercentFail()
        {
            using (new SessionScope())
            {
                ApplicationCap2QualifySPVBondPercent rule = new ApplicationCap2QualifySPVBondPercent();

                ICapApplication capApplication = _mockery.StrictMock<ICapApplication>();
                IEventList<ICapApplicationDetail> capAppDetails = new EventList<ICapApplicationDetail>();
                SetupResult.For(capApplication.CapApplicationDetails).Return(capAppDetails);
                ICapApplicationDetail capDetail = _mockery.StrictMock<ICapApplicationDetail>();
                capAppDetails.Add(Messages, capDetail);
                SetupResult.For(capDetail.Fee).Return((double)(5000));
                ICapTypeConfigurationDetail capTypeConfigDetail = _mockery.StrictMock<ICapTypeConfigurationDetail>();
                SetupResult.For(capDetail.CapTypeConfigurationDetail).Return(capTypeConfigDetail);
                ICapType capType = _mockery.StrictMock<ICapType>();
                SetupResult.For(capTypeConfigDetail.CapType).Return(capType);
                SetupResult.For(capType.Description).Return("1%");

                IMortgageLoanAccount mla = _mockery.StrictMock<IMortgageLoanAccount>();
                SetupResult.For(capApplication.Account).Return(mla);

                IMortgageLoan ml = _mockery.StrictMock<IMortgageLoan>();
                SetupResult.For(mla.SecuredMortgageLoan).Return(ml);
                SetupResult.For(mla.LoanCurrentBalance).Return((double)500000);
                IBond bond = _mockery.StrictMock<IBond>();
                SetupResult.For(ml.GetLatestRegisteredBond()).Return(bond);
                SetupResult.For(bond.BondRegistrationAmount).Return((double)501000);

                ISPV spv = _mockery.StrictMock<ISPV>();
                SetupResult.For(mla.SPV).Return(spv);

                IEventList<ISPVMandate> mandates = new EventList<ISPVMandate>();
                SetupResult.For(spv.SPVMandates).Return(mandates);
                ISPVMandate spvMandate = _mockery.StrictMock<ISPVMandate>();
                SetupResult.For(spvMandate.ExceedBondPercent).Return((double)0);

                mandates.Add(Messages, spvMandate);

                ExecuteRule(rule, 1, capApplication);
            }
        }

        #endregion ApplicationCap2QualifySPVBondPercent

        #region ApplicationCap2QualifySPVLoanAgreementPercent

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationCap2QualifySPVLoanAgreementPercentPass()
        {
            using (new SessionScope())
            {
                ApplicationCap2QualifySPVLoanAgreementPercent rule = new ApplicationCap2QualifySPVLoanAgreementPercent();

                ICapApplication capApplication = _mockery.StrictMock<ICapApplication>();
                IEventList<ICapApplicationDetail> capAppDetails = new EventList<ICapApplicationDetail>();
                SetupResult.For(capApplication.CapApplicationDetails).Return(capAppDetails);
                ICapApplicationDetail capDetail = _mockery.StrictMock<ICapApplicationDetail>();
                capAppDetails.Add(Messages, capDetail);
                SetupResult.For(capDetail.Fee).Return((double)(5000));
                ICapTypeConfigurationDetail capTypeConfigDetail = _mockery.StrictMock<ICapTypeConfigurationDetail>();
                SetupResult.For(capDetail.CapTypeConfigurationDetail).Return(capTypeConfigDetail);
                ICapType capType = _mockery.StrictMock<ICapType>();
                SetupResult.For(capTypeConfigDetail.CapType).Return(capType);
                SetupResult.For(capType.Description).Return("1%");

                IMortgageLoanAccount mla = _mockery.StrictMock<IMortgageLoanAccount>();
                SetupResult.For(capApplication.Account).Return(mla);

                IMortgageLoan ml = _mockery.StrictMock<IMortgageLoan>();
                SetupResult.For(mla.SecuredMortgageLoan).Return(ml);
                SetupResult.For(mla.LoanCurrentBalance).Return((double)500000);
                IBond bond = _mockery.StrictMock<IBond>();
                SetupResult.For(ml.GetLatestRegisteredBond()).Return(bond);
                SetupResult.For(bond.BondLoanAgreementAmount).Return((double)505001);

                ISPV spv = _mockery.StrictMock<ISPV>();
                SetupResult.For(mla.SPV).Return(spv);

                IEventList<ISPVMandate> mandates = new EventList<ISPVMandate>();
                SetupResult.For(spv.SPVMandates).Return(mandates);
                ISPVMandate spvMandate = _mockery.StrictMock<ISPVMandate>();
                SetupResult.For(spvMandate.ExceedLoanAgreementPercent).Return((double)0);

                mandates.Add(Messages, spvMandate);

                ExecuteRule(rule, 0, capApplication);
            }
        }

        /// <summary>
        /// Expects Messages.Count = 1.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationCap2QualifySPVLoanAgreementPercentFail()
        {
            using (new SessionScope())
            {
                ApplicationCap2QualifySPVLoanAgreementPercent rule = new ApplicationCap2QualifySPVLoanAgreementPercent();

                ICapApplication capApplication = _mockery.StrictMock<ICapApplication>();
                IEventList<ICapApplicationDetail> capAppDetails = new EventList<ICapApplicationDetail>();
                SetupResult.For(capApplication.CapApplicationDetails).Return(capAppDetails);
                ICapApplicationDetail capDetail = _mockery.StrictMock<ICapApplicationDetail>();
                capAppDetails.Add(Messages, capDetail);
                SetupResult.For(capDetail.Fee).Return((double)(5000));
                ICapTypeConfigurationDetail capTypeConfigDetail = _mockery.StrictMock<ICapTypeConfigurationDetail>();
                SetupResult.For(capDetail.CapTypeConfigurationDetail).Return(capTypeConfigDetail);
                ICapType capType = _mockery.StrictMock<ICapType>();
                SetupResult.For(capTypeConfigDetail.CapType).Return(capType);
                SetupResult.For(capType.Description).Return("1%");

                IMortgageLoanAccount mla = _mockery.StrictMock<IMortgageLoanAccount>();
                SetupResult.For(capApplication.Account).Return(mla);

                IMortgageLoan ml = _mockery.StrictMock<IMortgageLoan>();
                SetupResult.For(mla.SecuredMortgageLoan).Return(ml);
                SetupResult.For(mla.LoanCurrentBalance).Return((double)500000);
                IBond bond = _mockery.StrictMock<IBond>();
                SetupResult.For(ml.GetLatestRegisteredBond()).Return(bond);
                SetupResult.For(bond.BondLoanAgreementAmount).Return((double)501000);

                ISPV spv = _mockery.StrictMock<ISPV>();
                SetupResult.For(mla.SPV).Return(spv);

                IEventList<ISPVMandate> mandates = new EventList<ISPVMandate>();
                SetupResult.For(spv.SPVMandates).Return(mandates);
                ISPVMandate spvMandate = _mockery.StrictMock<ISPVMandate>();
                SetupResult.For(spvMandate.ExceedLoanAgreementPercent).Return((double)0);

                mandates.Add(Messages, spvMandate);

                ExecuteRule(rule, 1, capApplication);
            }
        }

        #endregion ApplicationCap2QualifySPVLoanAgreementPercent

        #region ApplicationCap2QualifySPVMinBal

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationCap2QualifySPVMinBalPass()
        {
            ApplicationCap2QualifySPVMinBal rule = new ApplicationCap2QualifySPVMinBal();

            ICapApplication capApplication = _mockery.StrictMock<ICapApplication>();
            IEventList<ICapApplicationDetail> capAppDetails = new EventList<ICapApplicationDetail>();
            SetupResult.For(capApplication.CapApplicationDetails).Return(capAppDetails);
            ICapApplicationDetail capDetail = _mockery.StrictMock<ICapApplicationDetail>();
            capAppDetails.Add(Messages, capDetail);
            SetupResult.For(capDetail.Fee).Return((double)(5000));
            ICapTypeConfigurationDetail capTypeConfigDetail = _mockery.StrictMock<ICapTypeConfigurationDetail>();
            SetupResult.For(capDetail.CapTypeConfigurationDetail).Return(capTypeConfigDetail);
            ICapType capType = _mockery.StrictMock<ICapType>();
            SetupResult.For(capTypeConfigDetail.CapType).Return(capType);
            SetupResult.For(capType.Description).Return("1%");

            IMortgageLoanAccount mla = _mockery.StrictMock<IMortgageLoanAccount>();
            SetupResult.For(capApplication.Account).Return(mla);
            SetupResult.For(mla.LoanCurrentBalance).Return((double)70001);

            ExecuteRule(rule, 0, capApplication);
        }

        /// <summary>
        /// Expects Messages.Count = 1.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationCap2QualifySPVMinBalFail()
        {
            ApplicationCap2QualifySPVMinBal rule = new ApplicationCap2QualifySPVMinBal();

            ICapApplication capApplication = _mockery.StrictMock<ICapApplication>();
            IEventList<ICapApplicationDetail> capAppDetails = new EventList<ICapApplicationDetail>();
            SetupResult.For(capApplication.CapApplicationDetails).Return(capAppDetails);
            ICapApplicationDetail capDetail = _mockery.StrictMock<ICapApplicationDetail>();
            capAppDetails.Add(Messages, capDetail);
            SetupResult.For(capDetail.Fee).Return((double)(5000));
            ICapTypeConfigurationDetail capTypeConfigDetail = _mockery.StrictMock<ICapTypeConfigurationDetail>();
            SetupResult.For(capDetail.CapTypeConfigurationDetail).Return(capTypeConfigDetail);
            ICapType capType = _mockery.StrictMock<ICapType>();
            SetupResult.For(capTypeConfigDetail.CapType).Return(capType);
            SetupResult.For(capType.Description).Return("1%");

            IMortgageLoanAccount mla = _mockery.StrictMock<IMortgageLoanAccount>();
            SetupResult.For(capApplication.Account).Return(mla);
            SetupResult.For(mla.LoanCurrentBalance).Return((double)70000);

            ExecuteRule(rule, 1, capApplication);
        }

        #endregion ApplicationCap2QualifySPVMinBal

        #region ApplicationCap2QualifySPVMaxBal

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationCap2QualifySPVMaxBalPass()
        {
            ApplicationCap2QualifySPVMaxBal rule = new ApplicationCap2QualifySPVMaxBal();

            ICapApplication capApplication = _mockery.StrictMock<ICapApplication>();
            IEventList<ICapApplicationDetail> capAppDetails = new EventList<ICapApplicationDetail>();
            SetupResult.For(capApplication.CapApplicationDetails).Return(capAppDetails);
            ICapApplicationDetail capDetail = _mockery.StrictMock<ICapApplicationDetail>();
            capAppDetails.Add(Messages, capDetail);
            SetupResult.For(capDetail.Fee).Return((double)(5000));
            ICapTypeConfigurationDetail capTypeConfigDetail = _mockery.StrictMock<ICapTypeConfigurationDetail>();
            SetupResult.For(capDetail.CapTypeConfigurationDetail).Return(capTypeConfigDetail);
            ICapType capType = _mockery.StrictMock<ICapType>();
            SetupResult.For(capTypeConfigDetail.CapType).Return(capType);
            SetupResult.For(capType.Description).Return("1%");

            IMortgageLoanAccount mla = _mockery.StrictMock<IMortgageLoanAccount>();
            SetupResult.For(capApplication.Account).Return(mla);
            SetupResult.For(mla.LoanCurrentBalance).Return((double)1650000);

            ExecuteRule(rule, 0, capApplication);
        }

        /// <summary>
        /// Expects Messages.Count = 1.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationCap2QualifySPVMaxBalFail()
        {
            ApplicationCap2QualifySPVMaxBal rule = new ApplicationCap2QualifySPVMaxBal();

            ICapApplication capApplication = _mockery.StrictMock<ICapApplication>();
            IEventList<ICapApplicationDetail> capAppDetails = new EventList<ICapApplicationDetail>();
            SetupResult.For(capApplication.CapApplicationDetails).Return(capAppDetails);
            ICapApplicationDetail capDetail = _mockery.StrictMock<ICapApplicationDetail>();
            capAppDetails.Add(Messages, capDetail);
            SetupResult.For(capDetail.Fee).Return((double)(5000));
            ICapTypeConfigurationDetail capTypeConfigDetail = _mockery.StrictMock<ICapTypeConfigurationDetail>();
            SetupResult.For(capDetail.CapTypeConfigurationDetail).Return(capTypeConfigDetail);
            ICapType capType = _mockery.StrictMock<ICapType>();
            SetupResult.For(capTypeConfigDetail.CapType).Return(capType);
            SetupResult.For(capType.Description).Return("1%");

            IMortgageLoanAccount mla = _mockery.StrictMock<IMortgageLoanAccount>();
            SetupResult.For(capApplication.Account).Return(mla);
            SetupResult.For(mla.LoanCurrentBalance).Return((double)5000000);

            ExecuteRule(rule, 1, capApplication);
        }

        #endregion ApplicationCap2QualifySPVMaxBal

        #region ApplicationCap2QualifyDebtCounselling

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationCap2QualifyDebtCounsellingSuccess()
        {
            // 1 - Test in the context of a CAP Application
            ApplicationCap2QualifyDebtCounsellingHelper(0, 1, false);

            // 2 - Test in the context of the Loan Account
            ApplicationCap2QualifyDebtCounsellingHelper(0, 2, false);
        }

        /// <summary>
        /// Expects Messages.Count = 1.0
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationCap2QualifyDebtCounsellingFail()
        {
            // 1 - Test in the context of a CAP Application
            ApplicationCap2QualifyDebtCounsellingHelper(2, 1, true);

            // 2 - Test in the context of the Loan Account
            ApplicationCap2QualifyDebtCounsellingHelper(2, 2, true);
        }

        private void ApplicationCap2QualifyDebtCounsellingHelper(int expectedMessageCount, int context, bool fail)
        {
            ApplicationCap2QualifyDebtCounselling rule = new ApplicationCap2QualifyDebtCounselling();
            ICapApplication capApp = _mockery.StrictMock<ICapApplication>();
            ICapStatus capStatus = _mockery.StrictMock<ICapStatus>();
            IAccount account = _mockery.StrictMock<IAccount>();
            SetupResult.For(account.UnderDebtCounselling).Return(fail);
            SetupResult.For(account.Key).Return(1);
            SetupResult.For(capApp.Account).Return(account);
            SetupResult.For(capStatus.Key).Return((int)CapStatuses.GrantedCap2Offer);
            SetupResult.For(capApp.CapStatus).Return(capStatus);

            IEventList<IRole> roles = new EventList<IRole>();
            IEventList<IDebtCounselling> debtCounsellingCases = new EventList<IDebtCounselling>();
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            IRoleType roleType = _mockery.StrictMock<IRoleType>();
            IRole role = _mockery.StrictMock<IRole>();
            IDebtCounselling debtCounselling = _mockery.StrictMock<IDebtCounselling>();

            SetupResult.For(roleType.Description).Return("Role Type Description");
            SetupResult.For(legalEntity.DisplayName).Return("Legal Entity Display Name");
            SetupResult.For(role.LegalEntity).Return(legalEntity);
            SetupResult.For(role.RoleType).Return(roleType);
            roles.Add(null, role);
            SetupResult.For(account.Roles).Return(roles);

            debtCounsellingCases.Add(null, debtCounselling);
            if (fail)
            {
                SetupResult.For(legalEntity.DebtCounsellingCases).Return(debtCounsellingCases);
            }
            else
            {
                SetupResult.For(legalEntity.DebtCounsellingCases).Return(null);
            }

            switch (context)
            {
                case 1:
                    {
                        ExecuteRule(rule, expectedMessageCount, capApp);
                        break;
                    }
                case 2:
                    {
                        ExecuteRule(rule, expectedMessageCount, account);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        #endregion ApplicationCap2QualifyDebtCounselling

        #region ApplicationCap2DebitOrderPaymentOption

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationCap2DebitOrderPaymentOptionSuccess()
        {
            ApplicationCap2DebitOrderPaymentOption rule = new ApplicationCap2DebitOrderPaymentOption();

            ICapApplication capApp = _mockery.StrictMock<ICapApplication>();
            ICapPaymentOption capPaymentOption = _mockery.StrictMock<ICapPaymentOption>();
            SetupResult.For(capPaymentOption.Key).Return((int)CAPPaymentOptions.DebitOrderBankAccount);
            SetupResult.For(capApp.CAPPaymentOption).Return(capPaymentOption);

            IAccount account = _mockery.StrictMock<IAccount>();
            IEventList<IFinancialService> finServices = new EventList<IFinancialService>();
            SetupResult.For(account.FinancialServices).Return(finServices);
            IFinancialService fs = _mockery.StrictMock<IFinancialService>();
            finServices.Add(Messages, fs);

            IEventList<IFinancialServiceBankAccount> finServiceBankAccounts = new EventList<IFinancialServiceBankAccount>();
            SetupResult.For(fs.FinancialServiceBankAccounts).Return(finServiceBankAccounts);
            IFinancialServiceBankAccount fsba = _mockery.StrictMock<IFinancialServiceBankAccount>();
            finServiceBankAccounts.Add(Messages, fsba);

            IFinancialServiceType finServiceType = _mockery.StrictMock<IFinancialServiceType>();
            SetupResult.For(finServiceType.Key).Return((int)FinancialServiceTypes.VariableLoan);
            SetupResult.For(fs.FinancialServiceType).Return(finServiceType);

            IGeneralStatus status = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(status.Key).Return((int)GeneralStatuses.Active);
            SetupResult.For(fsba.GeneralStatus).Return(status);

            IFinancialServicePaymentType fspt = _mockery.StrictMock<IFinancialServicePaymentType>();
            SetupResult.For(fspt.Key).Return((int)FinancialServicePaymentTypes.DebitOrderPayment);

            SetupResult.For(fsba.FinancialServicePaymentType).Return(fspt);

            SetupResult.For(capApp.Account).Return(account);

            // This rule succeeds for capPaymentOption.Key = 2 and FinancialServicePaymentType.Key = 1
            ExecuteRule(rule, 0, capApp);
        }

        /// <summary>
        /// Expects Messages.Count = 1.0
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationCap2DebitOrderPaymentOptionFail()
        {
            ApplicationCap2DebitOrderPaymentOption rule = new ApplicationCap2DebitOrderPaymentOption();

            ICapApplication capApp = _mockery.StrictMock<ICapApplication>();
            ICapPaymentOption capPaymentOption = _mockery.StrictMock<ICapPaymentOption>();
            SetupResult.For(capPaymentOption.Key).Return((int)CAPPaymentOptions.DebitOrderBankAccount);
            SetupResult.For(capApp.CAPPaymentOption).Return(capPaymentOption);

            IAccount account = _mockery.StrictMock<IAccount>();
            IEventList<IFinancialService> finServices = new EventList<IFinancialService>();
            SetupResult.For(account.FinancialServices).Return(finServices);
            IFinancialService fs = _mockery.StrictMock<IFinancialService>();
            finServices.Add(Messages, fs);

            IEventList<IFinancialServiceBankAccount> finServiceBankAccounts = new EventList<IFinancialServiceBankAccount>();
            SetupResult.For(fs.FinancialServiceBankAccounts).Return(finServiceBankAccounts);
            IFinancialServiceBankAccount fsba = _mockery.StrictMock<IFinancialServiceBankAccount>();
            finServiceBankAccounts.Add(Messages, fsba);

            IFinancialServiceType finServiceType = _mockery.StrictMock<IFinancialServiceType>();
            SetupResult.For(finServiceType.Key).Return((int)FinancialServiceTypes.VariableLoan);
            SetupResult.For(fs.FinancialServiceType).Return(finServiceType);

            IGeneralStatus status = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(status.Key).Return((int)GeneralStatuses.Active);
            SetupResult.For(fsba.GeneralStatus).Return(status);

            IFinancialServicePaymentType fspt = _mockery.StrictMock<IFinancialServicePaymentType>();
            SetupResult.For(fspt.Key).Return((int)FinancialServicePaymentTypes.DirectPayment);

            SetupResult.For(fsba.FinancialServicePaymentType).Return(fspt);

            SetupResult.For(capApp.Account).Return(account);

            // This rule fails for capPaymentOption.Key = 2 and FinancialServicePaymentType.Key = 2 or 3
            ExecuteRule(rule, 1, capApp);
        }

        /// <summary>
        /// Expects Messages.Count = 1.0
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationCap2DebitOrderPaymentOptionParametersFail()
        {
            using (new SessionScope())
            {
                ApplicationCap2DebitOrderPaymentOption rule = new ApplicationCap2DebitOrderPaymentOption();

                ICapApplication capApp = _mockery.StrictMock<ICapApplication>();

                //ICapPaymentOption capPaymentOption = _mockery.StrictMock<ICapPaymentOption>();
                //SetupResult.For(capPaymentOption.Key).Return((int)CAPPaymentOptions.DebitOrderBankAccount);
                SetupResult.For(capApp.CAPPaymentOption).Return(null);

                IAccount account = _mockery.StrictMock<IAccount>();
                IEventList<IFinancialService> finServices = new EventList<IFinancialService>();
                SetupResult.For(account.FinancialServices).Return(finServices);
                IFinancialService fs = _mockery.StrictMock<IFinancialService>();
                finServices.Add(Messages, fs);

                IEventList<IFinancialServiceBankAccount> finServiceBankAccounts = new EventList<IFinancialServiceBankAccount>();
                SetupResult.For(fs.FinancialServiceBankAccounts).Return(finServiceBankAccounts);
                IFinancialServiceBankAccount fsba = _mockery.StrictMock<IFinancialServiceBankAccount>();

                //finServiceBankAccounts.Add(Messages, fsba);
                // commenting out above line ensures there are no finservicebankaccounts so can check the correct error message is thrown.
                IGeneralStatus status = _mockery.StrictMock<IGeneralStatus>();
                SetupResult.For(status.Key).Return((int)GeneralStatuses.Active);
                SetupResult.For(fsba.GeneralStatus).Return(status);

                IFinancialServicePaymentType fspt = _mockery.StrictMock<IFinancialServicePaymentType>();
                SetupResult.For(fspt.Key).Return((int)FinancialServicePaymentTypes.DirectPayment);

                SetupResult.For(fsba.FinancialServicePaymentType).Return(fspt);

                SetupResult.For(capApp.Account).Return(account);

                // This rule fails for capPaymentOption.Key = 2 and FinancialServicePaymentType.Key = 2 or 3
                ExecuteRule(rule, 1, capApp);
            }
        }

        #endregion ApplicationCap2DebitOrderPaymentOption

        #region ApplicationCap2CheckReadvancePosted

        [Test]
        public void ApplicationCap2CheckReadvancePostedTestPass()
        {
            using (new SessionScope())
            {
                ApplicationCap2CheckReadvancePosted rule = new ApplicationCap2CheckReadvancePosted();

                string sql = String.Format(@"SELECT TOP 1 CO.*
            FROM [2am].[dbo].[CapOffer] CO
            INNER JOIN [2am].[dbo].[CapTypeConfiguration] CTC
            ON CO.CapTypeConfigurationKey = CTC.CapTypeConfigurationKey
            INNER JOIN [2am].[dbo].[FinancialService] FS
            ON FS.AccountKey = CO.AccountKey
            INNER JOIN  [2am].[fin].[FinancialTransaction] FT
            ON FS.FinancialServiceKey = FT.FinancialServiceKey
            WHERE (FT.InsertDate >= CTC.OfferStartDate AND FT.InsertDate <= CTC.OfferEndDate)
            AND FT.TransactionTypeKey = {0}
            ORDER BY CO.CapOfferKey DESC", (int)TransactionTypes.ReadvanceCAP);

                SimpleQuery<CapApplication_DAO> caQ = new SimpleQuery<CapApplication_DAO>(QueryLanguage.Sql, sql);
                caQ.AddSqlReturnDefinition(typeof(CapApplication_DAO), "CO");
                CapApplication_DAO[] res = caQ.Execute();

                if (res != null && res.Length > 0)
                {
                    IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    ICapApplication _capApp = bmtm.GetMappedType<ICapApplication, CapApplication_DAO>(res[0]);
                    ExecuteRule(rule, 0, _capApp);
                }
                else
                    Assert.Fail("No data");
            }
        }

        [Test]
        public void ApplicationCap2CheckReadvancePostedTestFail()
        {
            using (new SessionScope())
            {
                ApplicationCap2CheckReadvancePosted rule = new ApplicationCap2CheckReadvancePosted();

                string sql = String.Format(@"SELECT TOP 1 CO.*
                    FROM [2am].[dbo].[CapOffer] CO (nolock)
                    INNER JOIN [2am].[dbo].[CapTypeConfiguration] CTC (nolock) ON CO.CapTypeConfigurationKey = CTC.CapTypeConfigurationKey
                    INNER JOIN [2am].[dbo].[FinancialService] FS (nolock) ON FS.AccountKey = CO.AccountKey
                    WHERE FS.FinancialServiceKey not in (select distinct FinancialServiceKey from [2am].[fin].[FinancialTransaction] (nolock))");

                SimpleQuery<CapApplication_DAO> caQ = new SimpleQuery<CapApplication_DAO>(QueryLanguage.Sql, sql);
                caQ.AddSqlReturnDefinition(typeof(CapApplication_DAO), "CO");
                CapApplication_DAO[] res = caQ.Execute();

                if (res != null && res.Length > 0)
                {
                    IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    ICapApplication _capApp = bmtm.GetMappedType<ICapApplication, CapApplication_DAO>(res[0]);
                    ExecuteRule(rule, 1, _capApp);
                }
                else
                    Assert.Fail("No data");
            }
        }

        #endregion ApplicationCap2CheckReadvancePosted

        #region ApplicationCap2CheckReadvanceRequired

        [NUnit.Framework.Test]
        public void ApplicationCap2CheckReadvanceRequiredTestPass()
        {
            using (new SessionScope())
            {
                #region Old Testing Code - Method Changed

                //ApplicationCap2CheckReadvanceRequired rule = new ApplicationCap2CheckReadvanceRequired();

                //ICapApplication capApplication = _mockery.StrictMock<ICapApplication>();
                //IEventList<ICapApplicationDetail> capAppDetails = new EventList<ICapApplicationDetail>();
                //SetupResult.For(capApplication.CapApplicationDetails).Return(capAppDetails);
                //ICapApplicationDetail capDetail = _mockery.StrictMock<ICapApplicationDetail>();
                //capAppDetails.Add(Messages, capDetail);
                //SetupResult.For(capDetail.Fee).Return((double)(5000.00));
                //ICapTypeConfigurationDetail capTypeConfigDetail = _mockery.StrictMock<ICapTypeConfigurationDetail>();
                //SetupResult.For(capDetail.CapTypeConfigurationDetail).Return(capTypeConfigDetail);
                //ICapType capType = _mockery.StrictMock<ICapType>();
                //SetupResult.For(capTypeConfigDetail.CapType).Return(capType);
                //SetupResult.For(capType.Description).Return("1%");

                //ICapStatus capStatus = _mockery.StrictMock<ICapStatus>();
                //SetupResult.For(capStatus.Key).Return((int)CapStatuses.ReadvanceRequired);
                //SetupResult.For(capDetail.CapStatus).Return(capStatus);

                //IMortgageLoanAccount mla = _mockery.StrictMock<IMortgageLoanAccount>();
                //SetupResult.For(capApplication.Account).Return(mla);

                //IMortgageLoan ml = _mockery.StrictMock<IMortgageLoan>();

                //IBond bond = _mockery.StrictMock<IBond>();
                //SetupResult.For(bond.BondLoanAgreementAmount).Return((double)505000);

                //IEventList<IBond> bonds = new EventList<IBond>();
                //bonds.Add(Messages, bond);

                //SetupResult.For(ml.Bonds).Return(bonds);
                //SetupResult.For(ml.CurrentBalance).Return((double)500000);
                //SetupResult.For(mla.SecuredMortgageLoan).Return(ml);

                //ExecuteRule(rule, 0, capApplication);

                #endregion Old Testing Code - Method Changed

                SetRepositoryStrategy(TypeFactoryStrategy.Mock);
                ApplicationCap2CheckReadvanceRequired rule = new ApplicationCap2CheckReadvanceRequired();
                ICapApplication capApplication = _mockery.StrictMock<ICapApplication>();

                //
                ICapRepository capRepo = _mockery.StrictMock<ICapRepository>();
                MockCache.Add(typeof(ICapRepository).ToString(), capRepo);

                //
                bool result = true;
                SetupResult.For(capRepo.IsReAdvance(capApplication)).IgnoreArguments().Return(result);
                ExecuteRule(rule, 0, capApplication);
            }
        }

        [NUnit.Framework.Test]
        public void ApplicationCap2CheckReadvanceRequiredTestFail()
        {
            using (new SessionScope())
            {
                #region Old Testing Code

                //using (new SessionScope())
                //{
                //    ApplicationCap2CheckReadvanceRequired rule = new ApplicationCap2CheckReadvanceRequired();

                //    ICapApplication capApplication = _mockery.StrictMock<ICapApplication>();
                //    IEventList<ICapApplicationDetail> capAppDetails = new EventList<ICapApplicationDetail>();
                //    SetupResult.For(capApplication.CapApplicationDetails).Return(capAppDetails);
                //    ICapApplicationDetail capDetail = _mockery.StrictMock<ICapApplicationDetail>();
                //    capAppDetails.Add(Messages, capDetail);
                //    SetupResult.For(capDetail.Fee).Return((double)(5000.00));
                //    ICapTypeConfigurationDetail capTypeConfigDetail = _mockery.StrictMock<ICapTypeConfigurationDetail>();
                //    SetupResult.For(capDetail.CapTypeConfigurationDetail).Return(capTypeConfigDetail);
                //    ICapType capType = _mockery.StrictMock<ICapType>();
                //    SetupResult.For(capTypeConfigDetail.CapType).Return(capType);
                //    SetupResult.For(capType.Description).Return("1%");

                //    ICapStatus capStatus = _mockery.StrictMock<ICapStatus>();
                //    SetupResult.For(capStatus.Key).Return((int)CapStatuses.ReadvanceRequired);
                //    SetupResult.For(capDetail.CapStatus).Return(capStatus);

                //    IMortgageLoanAccount mla = _mockery.StrictMock<IMortgageLoanAccount>();
                //    SetupResult.For(capApplication.Account).Return(mla);

                //    IMortgageLoan ml = _mockery.StrictMock<IMortgageLoan>();

                //    IBond bond = _mockery.StrictMock<IBond>();
                //    SetupResult.For(bond.BondLoanAgreementAmount).Return((double)500000);

                //    IEventList<IBond> bonds = new EventList<IBond>();
                //    bonds.Add(Messages, bond);

                //    SetupResult.For(ml.Bonds).Return(bonds);
                //    SetupResult.For(ml.CurrentBalance).Return((double)500000);
                //    SetupResult.For(mla.SecuredMortgageLoan).Return(ml);

                //    ExecuteRule(rule, 1, capApplication);
                //}

                #endregion Old Testing Code

                SetRepositoryStrategy(TypeFactoryStrategy.Mock);
                ApplicationCap2CheckReadvanceRequired rule = new ApplicationCap2CheckReadvanceRequired();
                ICapApplication capApplication = _mockery.StrictMock<ICapApplication>();

                //
                ICapRepository capRepo = _mockery.StrictMock<ICapRepository>();
                MockCache.Add(typeof(ICapRepository).ToString(), capRepo);

                //
                bool result = false;
                SetupResult.For(capRepo.IsReAdvance(capApplication)).IgnoreArguments().Return(result);
                ExecuteRule(rule, 1, capApplication);
            }
        }

        #endregion ApplicationCap2CheckReadvanceRequired

        #region ApplicationCap2AllowFurtherLendingSPVTest

        /// <summary>
        /// This checks if a SPV has been discontinued. If it has it throws an error as cannot disburse.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationCap2AllowFurtherLendingSPVTest()
        {
            // if OfferType is ReAdvance then it succeeds.
            ApplicationCap2AllowFurtherLendingSPVHelper(0, 2);

            // if offertype is FAdvance then it fails - rule returns zero when the accounts SPV allows furtherlending
            ApplicationCap2AllowFurtherLendingSPVHelper(0, 3);
        }

        /// <summary>
        /// Helper method to set up the expectations for the CATSDisbursementSPVCheckTest Test.
        /// </summary>
        /// <param name=""></param>
        private void ApplicationCap2AllowFurtherLendingSPVHelper(int expectedMessageCount, int offerTypeKey)
        {
            ApplicationCap2AllowFurtherLendingSPV rule = new ApplicationCap2AllowFurtherLendingSPV(RepositoryFactory.GetRepository<ICastleTransactionsService>());

            string sql = @"select top 1 fs.AccountKey from [2am].[dbo].Financialservice fs (nolock)
					join [2am].[dbo].Account a (nolock) on fs.AccountKey = a.AccountKey
                    join [2am].[spv].SPV (nolock) on a.SPVKey = SPV.SPVKey
                    join [2am].[dbo].Offer o (nolock) on o.AccountKey = fs.AccountKey
                    join [2am].[dbo].Disbursement d (nolock) on d.AccountKey = fs.AccountKey
                    where SPV.SPVKey = 126
                    and fs.AccountStatusKey = 1
                    and fs.FinancialServiceTypeKey = 1
                    and o.OfferTypeKey = 2
                    and d.DisbursementTransactionTypeKey = 9 order by a.opendate desc";
            DataTable dt = base.GetQueryResults(sql);

            if (dt.Rows.Count > 0)
            {
                int accKey = Convert.ToInt32(dt.Rows[0][0]);

                //int offerTypeKey = 2;
                int disTransTypeKey = 9;

                ExecuteRule(rule, expectedMessageCount, accKey, disTransTypeKey, offerTypeKey);
            }
            else
                Assert.Inconclusive("No data");
        }

        #endregion ApplicationCap2AllowFurtherLendingSPVTest

        #region ApplicationCap2AccountResetDateCheck Account Search Test

        [Test]
        public void SearchApplicationCap2AccountResetDateCheckTestFail()
        {
            ApplicationCap2AccountResetDateCheck rule = new ApplicationCap2AccountResetDateCheck(RepositoryFactory.GetRepository<ICastleTransactionsService>());
            IAccount acc = _mockery.StrictMock<IAccount>();
            ICapApplication cap = _mockery.StrictMock<ICapApplication>();

            using (new SessionScope())
            {
                string query = @"select top 1 a.*
                from account a
                join financialservice fs on a.accountkey=fs.accountkey
                where datediff(day, a.opendate,fs.nextresetdate) <= 93 and fs.accountstatuskey=1 order by a.opendate desc ";

                SimpleQuery<Account_DAO> q = new SimpleQuery<Account_DAO>(QueryLanguage.Sql, query);
                q.AddSqlReturnDefinition(typeof(Account_DAO), "a");
                Account_DAO[] res = q.Execute();

                if (res != null && res.Length > 0)
                {
                    SetupResult.For(acc.Key).Return(res[0].Key);
                    SetupResult.For(cap.Account).Return(acc);
                    ExecuteRule(rule, 1, cap);
                }
                else
                    Assert.Inconclusive("No data");
            }
        }

        [Test]
        public void SearchApplicationCap2AccountResetDateCheckTestPass()
        {
            ApplicationCap2AccountResetDateCheck rule = new ApplicationCap2AccountResetDateCheck(RepositoryFactory.GetRepository<ICastleTransactionsService>());
            IAccount acc = _mockery.StrictMock<IAccount>();
            ICapApplication cap = _mockery.StrictMock<ICapApplication>();

            using (new SessionScope())
            {
                string query = @"select top 1 a.*
                from account a
                join financialservice fs on a.accountkey=fs.accountkey
                where datediff(month, a.opendate,fs.nextresetdate) > 3 and fs.accountstatuskey=1 order by a.opendate desc ";

                SimpleQuery<Account_DAO> q = new SimpleQuery<Account_DAO>(QueryLanguage.Sql, query);
                q.AddSqlReturnDefinition(typeof(Account_DAO), "a");
                Account_DAO[] res = q.Execute();

                if (res != null && res.Length > 0)
                {
                    SetupResult.For(acc.Key).Return(res[0].Key);
                    SetupResult.For(cap.Account).Return(acc);
                    ExecuteRule(rule, 0, cap);
                }
                else
                    Assert.Fail("No data");
            }
        }

        #endregion ApplicationCap2AccountResetDateCheck Account Search Test

        #region ApplicationCap2AccountResetDateCheck Account Import Test

        [Test]
        public void ImportApplicationCap2AccountResetDateCheckTestFail()
        {
            ApplicationCap2AccountResetDateCheck rule = new ApplicationCap2AccountResetDateCheck(RepositoryFactory.GetRepository<ICastleTransactionsService>());
            IAccount acc = _mockery.StrictMock<IAccount>();

            using (new SessionScope())
            {
                string query = @"select top 1 a.*
                from account a
                join financialservice fs on a.accountkey=fs.accountkey
                where datediff(day, a.opendate,fs.nextresetdate) <= 93 and fs.accountstatuskey=1 order by a.opendate desc ";

                SimpleQuery<Account_DAO> q = new SimpleQuery<Account_DAO>(QueryLanguage.Sql, query);
                q.AddSqlReturnDefinition(typeof(Account_DAO), "a");
                Account_DAO[] res = q.Execute();

                if (res != null && res.Length > 0)
                {
                    SetupResult.For(acc.Key).Return(res[0].Key);
                    ExecuteRule(rule, 1, acc);
                }
                else
                    Assert.Inconclusive("No data");
            }
        }

        [Test]
        public void ImportApplicationCap2AccountResetDateCheckTestPass()
        {
            ApplicationCap2AccountResetDateCheck rule = new ApplicationCap2AccountResetDateCheck(RepositoryFactory.GetRepository<ICastleTransactionsService>());
            IAccount acc = _mockery.StrictMock<IAccount>();

            using (new SessionScope())
            {
                string query = @"select a.*
                from account a
                join financialservice fs on a.accountkey=fs.accountkey
                where datediff(month, a.opendate,fs.nextresetdate) > 3 and fs.accountstatuskey=1 order by a.opendate desc";

                SimpleQuery<Account_DAO> q = new SimpleQuery<Account_DAO>(QueryLanguage.Sql, query);
                q.SetQueryRange(1);
                q.AddSqlReturnDefinition(typeof(Account_DAO), "a");
                Account_DAO[] res = q.Execute();

                if (res != null && res.Length > 0)
                {
                    SetupResult.For(acc.Key).Return(res[0].Key);
                    ExecuteRule(rule, 0, acc);
                }
                else
                    Assert.Fail("No data");
            }
        }

        #endregion ApplicationCap2AccountResetDateCheck Account Import Test

        #region ApplicationCAP2VerifyCapBroker Test

        [Test]
        public void ApplicationCAP2VerifyCapBrokerTestPass()
        {
            using (new SessionScope())
            {
                ApplicationCAP2VerifyCapBroker rule = new ApplicationCAP2VerifyCapBroker();
                string sql = @"select top 1 ad.aduserKey
                from [2am].[dbo].broker b (nolock)
                inner join [2am].[dbo].aduser ad (nolock)
                    on b.aduserKey = ad.ADUserKey";
                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(ADUser_DAO), new ParameterCollection());
                int adUserKey = Convert.ToInt32(o);
                IOrganisationStructureRepository orgRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IADUser aduser = orgRepo.GetADUserByKey(adUserKey);
                ExecuteRule(rule, 0, null, aduser);
            }
        }

        [Test]
        public void ApplicationCAP2VerifyCapBrokerTestFail()
        {
            using (new SessionScope())
            {
                ApplicationCAP2VerifyCapBroker rule = new ApplicationCAP2VerifyCapBroker();
                string sql = @"select top 1 ad.aduserKey
                from [2am].[dbo].aduser ad  (nolock)
                left join [2am].[dbo].broker b (nolock)
	                on b.aduserKey = ad.ADUserKey
                where b.BrokerKey is null";
                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(ADUser_DAO), new ParameterCollection());
                int adUserKey = Convert.ToInt32(o);
                IOrganisationStructureRepository orgRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IADUser aduser = orgRepo.GetADUserByKey(adUserKey);
                ExecuteRule(rule, 1, null, aduser);
            }
        }

        #endregion ApplicationCAP2VerifyCapBroker Test

        #region ApplicationCAP2QualifyCAPOfferDetail Test

        [Test]
        public void ApplicationCAP2QualifyCAPOfferDetailTestPass()
        {
            ApplicationCAP2QualifyCAPOfferDetail rule = new ApplicationCAP2QualifyCAPOfferDetail();
            ICapApplication capOffer = _mockery.StrictMock<ICapApplication>();
            ICapApplicationDetail capOfferDetail = _mockery.StrictMock<ICapApplicationDetail>();
            IMortgageLoanAccount mla = _mockery.StrictMock<IMortgageLoanAccount>();
            IMortgageLoan ml = _mockery.StrictMock<IMortgageLoan>();
            IBond bond = _mockery.StrictMock<IBond>();
            List<IBond> bondLst = new List<IBond>();
            IEventList<IBond> bondEventLst = null;

            //
            double bondRegistrationAmount = 10.00;
            double capAmt = 1.00;

            //
            SetupResult.For(bond.BondRegistrationAmount).Return(bondRegistrationAmount);
            bondLst.Add(bond);
            bondEventLst = new EventList<IBond>(bondLst);

            //
            SetupResult.For(ml.Bonds).Return(bondEventLst);

            //
            SetupResult.For(mla.SecuredMortgageLoan).Return(ml);

            //
            SetupResult.For(capOffer.CurrentBalance).Return(capAmt);
            SetupResult.For(capOffer.Account).Return(mla);

            //
            SetupResult.For(capOfferDetail.Fee).Return(capAmt);
            SetupResult.For(capOfferDetail.CapApplication).Return(capOffer);

            //
            ExecuteRule(rule, 0, capOfferDetail);
        }

        [Test]
        public void ApplicationCAP2QualifyCAPOfferDetailTestFail()
        {
            ApplicationCAP2QualifyCAPOfferDetail rule = new ApplicationCAP2QualifyCAPOfferDetail();
            ICapTypeConfigurationDetail capTypeConfigurationDetail = _mockery.StrictMock<ICapTypeConfigurationDetail>();
            ICapType capType = _mockery.StrictMock<ICapType>();
            ICapApplication capOffer = _mockery.StrictMock<ICapApplication>();
            ICapApplicationDetail capOfferDetail = _mockery.StrictMock<ICapApplicationDetail>();
            IMortgageLoanAccount mla = _mockery.StrictMock<IMortgageLoanAccount>();
            IMortgageLoan ml = _mockery.StrictMock<IMortgageLoan>();
            IBond bond = _mockery.StrictMock<IBond>();
            List<IBond> bondLst = new List<IBond>();
            IEventList<IBond> bondEventLst = null;

            //
            double bondRegistrationAmount = 1.00;
            double capAmt = 10.00;
            string desc = "test";

            //
            SetupResult.For(capType.Description).Return(desc);
            SetupResult.For(capTypeConfigurationDetail.CapType).Return(capType);
            SetupResult.For(capOfferDetail.CapTypeConfigurationDetail).Return(capTypeConfigurationDetail);

            //
            SetupResult.For(bond.BondRegistrationAmount).Return(bondRegistrationAmount);
            bondLst.Add(bond);
            bondEventLst = new EventList<IBond>(bondLst);

            //
            SetupResult.For(ml.Bonds).Return(bondEventLst);

            //
            SetupResult.For(mla.SecuredMortgageLoan).Return(ml);

            //
            SetupResult.For(capOffer.CurrentBalance).Return(capAmt);
            SetupResult.For(capOffer.Account).Return(mla);

            //
            SetupResult.For(capOfferDetail.Fee).Return(capAmt);
            SetupResult.For(capOfferDetail.CapApplication).Return(capOffer);

            //
            ExecuteRule(rule, 1, capOfferDetail);
        }

        #endregion ApplicationCAP2QualifyCAPOfferDetail Test

        #region ApplicationCAP2NextQuarter Test

        [Test]
        public void ApplicationCAP2NextQuarterTestPass()
        {
            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            ICapRepository capRepo = _mockery.StrictMock<ICapRepository>();
            MockCache.Add(typeof(ICapRepository).ToString(), capRepo);

            //
            ApplicationCAP2NextQuarter rule = new ApplicationCAP2NextQuarter();
            IAccount account = _mockery.StrictMock<IAccount>();
            ICapApplication capOffer = _mockery.StrictMock<ICapApplication>();
            ICapApplication cap = _mockery.StrictMock<ICapApplication>();
            IList<ICapApplication> capOfferLst = new List<ICapApplication>();
            ICapStatus capStatus = _mockery.StrictMock<ICapStatus>();

            //
            int Key = 1;
            int capOfferKey = 1;

            //
            SetupResult.For(account.Key).Return(Key);

            //
            SetupResult.For(capStatus.Key).Return(Key);

            //
            SetupResult.For(capOffer.Key).Return(capOfferKey);
            SetupResult.For(capOffer.Account).Return(account);

            //
            SetupResult.For(cap.Key).Return(Key);
            SetupResult.For(cap.CapStatus).Return(capStatus);
            capOfferLst.Add(cap);

            //
            SetupResult.For(capRepo.GetCapOfferByAccountKey(Key)).IgnoreArguments().Return(capOfferLst);

            //
            ExecuteRule(rule, 0, capOffer);
        }

        [Test]
        public void ApplicationCAP2NextQuarterTestFail()
        {
            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            ICapRepository capRepo = _mockery.StrictMock<ICapRepository>();
            MockCache.Add(typeof(ICapRepository).ToString(), capRepo);

            //
            ApplicationCAP2NextQuarter rule = new ApplicationCAP2NextQuarter();
            IAccount account = _mockery.StrictMock<IAccount>();
            ICapApplication capOffer = _mockery.StrictMock<ICapApplication>();
            ICapApplication cap = _mockery.StrictMock<ICapApplication>();
            IList<ICapApplication> capOfferLst = new List<ICapApplication>();
            ICapStatus capStatus = _mockery.StrictMock<ICapStatus>();

            //
            int Key = 1;
            int capOfferKey = 0;

            //
            SetupResult.For(account.Key).Return(Key);

            //
            SetupResult.For(capStatus.Key).Return(Key);

            //
            SetupResult.For(capOffer.Key).Return(capOfferKey);
            SetupResult.For(capOffer.Account).Return(account);

            //
            SetupResult.For(cap.Key).Return(Key);
            SetupResult.For(cap.CapStatus).Return(capStatus);
            capOfferLst.Add(cap);

            //
            SetupResult.For(capRepo.GetCapOfferByAccountKey(Key)).IgnoreArguments().Return(capOfferLst);

            //
            ExecuteRule(rule, 1, capOffer);
        }

        #endregion ApplicationCAP2NextQuarter Test

        #region ApplicationCAP2SPVBack2Back Test

        [Test]
        public void ApplicationCAP2SPVBack2BackTestPass()
        {
            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            ICapRepository capRepo = _mockery.StrictMock<ICapRepository>();
            MockCache.Add(typeof(ICapRepository).ToString(), capRepo);

            //
            ApplicationCAP2SPVBack2Back rule = new ApplicationCAP2SPVBack2Back();
            IAccount account = _mockery.StrictMock<IAccount>();
            ICapApplication capOffer = _mockery.StrictMock<ICapApplication>();
            ICapApplication cap = _mockery.StrictMock<ICapApplication>();
            IList<ICapApplication> capOfferLst = new List<ICapApplication>();
            ICapStatus capStatus = _mockery.StrictMock<ICapStatus>();
            ICapTypeConfiguration ctcEffDT1 = _mockery.StrictMock<ICapTypeConfiguration>();
            ICapTypeConfiguration ctcEffDT2 = _mockery.StrictMock<ICapTypeConfiguration>();

            //
            int Key = 2;
            int capOfferKey = 2;
            DateTime existingCapDate = new DateTime(2009, 1, 18);
            DateTime newCapDate = new DateTime(2011, 1, 18);

            //
            SetupResult.For(ctcEffDT1.CapEffectiveDate).Return(existingCapDate);
            SetupResult.For(ctcEffDT1.CapClosureDate).Return(existingCapDate.AddMonths(24).AddDays(7));
            SetupResult.For(ctcEffDT2.CapEffectiveDate).Return(newCapDate);
            SetupResult.For(ctcEffDT2.CapClosureDate).Return(newCapDate.AddMonths(24));

            //
            SetupResult.For(account.Key).Return(Key);

            //
            SetupResult.For(capStatus.Key).Return(Key);

            //
            SetupResult.For(capOffer.CapTypeConfiguration).Return(ctcEffDT2);
            SetupResult.For(capOffer.Key).Return(capOfferKey);
            SetupResult.For(capOffer.Account).Return(account);

            //
            SetupResult.For(cap.CapTypeConfiguration).Return(ctcEffDT1);
            SetupResult.For(cap.Key).Return(Key);
            SetupResult.For(cap.CapStatus).Return(capStatus);
            capOfferLst.Add(cap);

            //
            SetupResult.For(capRepo.GetCapOfferByAccountKey(Key)).IgnoreArguments().Return(capOfferLst);

            //
            ExecuteRule(rule, 0, capOffer);
        }

        [Test]
        public void ApplicationCAP2SPVBack2BackTestFail()
        {
            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            ICapRepository capRepo = _mockery.StrictMock<ICapRepository>();
            MockCache.Add(typeof(ICapRepository).ToString(), capRepo);

            //
            ApplicationCAP2SPVBack2Back rule = new ApplicationCAP2SPVBack2Back();
            IAccount account = _mockery.StrictMock<IAccount>();
            ICapApplication capOffer = _mockery.StrictMock<ICapApplication>();
            ICapApplication cap = _mockery.StrictMock<ICapApplication>();
            IList<ICapApplication> capOfferLst = new List<ICapApplication>();
            ICapStatus capStatus = _mockery.StrictMock<ICapStatus>();
            ICapTypeConfiguration ctcEffDT1 = _mockery.StrictMock<ICapTypeConfiguration>();
            ICapTypeConfiguration ctcEffDT2 = _mockery.StrictMock<ICapTypeConfiguration>();

            //
            int Key = 2;
            int capOfferKey = 0;
            DateTime existingCapDate = new DateTime(2009, 1, 18);
            DateTime newCapDate = new DateTime(2011, 1, 18);

            //
            SetupResult.For(ctcEffDT1.CapEffectiveDate).Return(existingCapDate);
            SetupResult.For(ctcEffDT1.CapClosureDate).Return(existingCapDate.AddMonths(24).AddDays(8));
            SetupResult.For(ctcEffDT2.CapEffectiveDate).Return(newCapDate);
            SetupResult.For(ctcEffDT2.CapClosureDate).Return(newCapDate.AddMonths(24));

            //
            SetupResult.For(account.Key).Return(Key);

            //
            SetupResult.For(capStatus.Key).Return(Key);

            //
            SetupResult.For(capOffer.CapTypeConfiguration).Return(ctcEffDT2);
            SetupResult.For(capOffer.Key).Return(capOfferKey);
            SetupResult.For(capOffer.Account).Return(account);

            //
            SetupResult.For(cap.CapTypeConfiguration).Return(ctcEffDT1);
            SetupResult.For(cap.Key).Return(Key);
            SetupResult.For(cap.CapStatus).Return(capStatus);
            capOfferLst.Add(cap);

            //
            SetupResult.For(capRepo.GetCapOfferByAccountKey(Key)).IgnoreArguments().Return(capOfferLst);

            //
            ExecuteRule(rule, 1, capOffer);
        }

        #endregion ApplicationCAP2SPVBack2Back Test

        #region ApplicationCap2QualifyActiveCap Test

        [Test]
        public void ApplicationCap2QualifyActiveCapTestPass()
        {
            ApplicationCap2QualifyActiveCap rule = new ApplicationCap2QualifyActiveCap();
            ICapApplication capOffer = _mockery.StrictMock<ICapApplication>();
            ICapTypeConfiguration capTypeConfiguration = _mockery.StrictMock<ICapTypeConfiguration>();
            IMortgageLoanAccount mla = _mockery.StrictMock<IMortgageLoanAccount>();
            IMortgageLoan ml = _mockery.StrictMock<IMortgageLoan>();
            IFinancialAdjustmentStatus financialAdjustmentStatus = _mockery.StrictMock<IFinancialAdjustmentStatus>();
            IFinancialAdjustment financialAdjustment = _mockery.StrictMock<IFinancialAdjustment>();
            IFinancialAdjustmentTypeSource financialAdjustmentTypeSource = _mockery.StrictMock<IFinancialAdjustmentTypeSource>();
            List<IFinancialAdjustment> financialAdjustmentLst = new List<IFinancialAdjustment>();
            IEventList<IFinancialAdjustment> financialAdjustmentEventLst = null;

            //
            int financialAdjustmentTypeSourceKey = (int)FinancialAdjustmentTypeSources.CAP2;
            int financialAdjustmentStatusKey = (int)FinancialAdjustmentStatuses.Active;

            //int term = 0;
            //
            SetupResult.For(financialAdjustmentTypeSource.Key).Return(financialAdjustmentTypeSourceKey);
            SetupResult.For(financialAdjustment.FinancialAdjustmentTypeSource).Return(financialAdjustmentTypeSource);
            SetupResult.For(financialAdjustment.FromDate).Return(DateTime.Now.AddDays(1));
            SetupResult.For(financialAdjustment.EndDate).Return(DateTime.Now.AddDays(2));

            //SetupResult.For(financialAdjustment.Term).Return(term);
            financialAdjustmentLst.Add(financialAdjustment);
            financialAdjustmentEventLst = new EventList<IFinancialAdjustment>(financialAdjustmentLst);

            //
            SetupResult.For(financialAdjustmentStatus.Key).Return(financialAdjustmentStatusKey);
            SetupResult.For(financialAdjustment.FinancialAdjustmentStatus).Return(financialAdjustmentStatus);

            //
            SetupResult.For(ml.FinancialAdjustments).Return(financialAdjustmentEventLst);

            //
            SetupResult.For(mla.SecuredMortgageLoan).Return(ml);

            //
            SetupResult.For(capTypeConfiguration.CapEffectiveDate).Return(DateTime.Now.AddDays(4));

            //
            SetupResult.For(capOffer.Account).Return(mla);
            SetupResult.For(capOffer.CapTypeConfiguration).Return(capTypeConfiguration);

            //
            ExecuteRule(rule, 0, capOffer);
        }

        [Test]
        public void ApplicationCap2QualifyActiveCapTestFail()
        {
            ApplicationCap2QualifyActiveCap rule = new ApplicationCap2QualifyActiveCap();
            ICapApplication capOffer = _mockery.StrictMock<ICapApplication>();
            ICapTypeConfiguration capTypeConfiguration = _mockery.StrictMock<ICapTypeConfiguration>();
            IMortgageLoanAccount mla = _mockery.StrictMock<IMortgageLoanAccount>();
            IMortgageLoan ml = _mockery.StrictMock<IMortgageLoan>();
            IFinancialAdjustmentStatus financialAdjustmentStatus = _mockery.StrictMock<IFinancialAdjustmentStatus>();
            IFinancialAdjustment financialAdjustment = _mockery.StrictMock<IFinancialAdjustment>();
            IFinancialAdjustmentTypeSource financialAdjustmentTypeSource = _mockery.StrictMock<IFinancialAdjustmentTypeSource>();
            List<IFinancialAdjustment> financialAdjustmentLst = new List<IFinancialAdjustment>();
            IEventList<IFinancialAdjustment> financialAdjustmentEventLst = null;

            //
            int financialAdjustmentTypeSourceKey = (int)FinancialAdjustmentTypeSources.CAP2;
            int financialAdjustmentStatusKey = (int)FinancialAdjustmentStatuses.Active;

            //int term = 5;
            //
            SetupResult.For(financialAdjustmentTypeSource.Key).Return(financialAdjustmentTypeSourceKey);
            SetupResult.For(financialAdjustment.FinancialAdjustmentTypeSource).Return(financialAdjustmentTypeSource);
            SetupResult.For(financialAdjustment.FromDate).Return(DateTime.Now.AddDays(1));
            SetupResult.For(financialAdjustment.EndDate).Return(DateTime.Now.AddDays(20));

            //SetupResult.For(financialAdjustment.Term).Return(term);
            financialAdjustmentLst.Add(financialAdjustment);
            financialAdjustmentEventLst = new EventList<IFinancialAdjustment>(financialAdjustmentLst);

            //
            SetupResult.For(financialAdjustmentStatus.Key).Return(financialAdjustmentStatusKey);
            SetupResult.For(financialAdjustment.FinancialAdjustmentStatus).Return(financialAdjustmentStatus);

            //
            SetupResult.For(ml.FinancialAdjustments).Return(financialAdjustmentEventLst);

            //
            SetupResult.For(mla.SecuredMortgageLoan).Return(ml);

            //
            SetupResult.For(capTypeConfiguration.CapEffectiveDate).Return(DateTime.Now.AddDays(5));

            //
            SetupResult.For(capOffer.Account).Return(mla);
            SetupResult.For(capOffer.CapTypeConfiguration).Return(capTypeConfiguration);

            //
            ExecuteRule(rule, 1, capOffer);
        }

        #endregion ApplicationCap2QualifyActiveCap Test

        #region ApplicationCap2QualifyUnderCancel Test

        [Test]
        public void ApplicationCap2QualifyUnderCancelTestPass()
        {
            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            IAccountRepository accRepo = _mockery.StrictMock<IAccountRepository>();
            MockCache.Add(typeof(IAccountRepository).ToString(), accRepo);

            //
            ApplicationCap2QualifyUnderCancel rule = new ApplicationCap2QualifyUnderCancel();
            ICapApplication capOffer = _mockery.StrictMock<ICapApplication>();
            IAccount account = _mockery.StrictMock<IAccount>();
            IDetail detail = _mockery.StrictMock<IDetail>();
            IList<IDetail> detailLst = new List<IDetail>();
            IReadOnlyEventList<IDetail> detailEventLst = null;

            //
            int accountKey = 1;
            int detailTypeKey = (int)DetailTypes.UnderCancellation;

            //
            detailEventLst = new ReadOnlyEventList<IDetail>(detailLst);

            //
            SetupResult.For(accRepo.GetDetailByAccountKeyAndDetailType(accountKey, detailTypeKey)).IgnoreArguments().Return(detailEventLst);

            //
            SetupResult.For(account.Key).Return(accountKey);

            //
            SetupResult.For(capOffer.Account).Return(account);

            //
            ExecuteRule(rule, 0, capOffer);
        }

        [Test]
        public void ApplicationCap2QualifyUnderCancelTestFail()
        {
            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            IAccountRepository accRepo = _mockery.StrictMock<IAccountRepository>();
            MockCache.Add(typeof(IAccountRepository).ToString(), accRepo);

            //
            ApplicationCap2QualifyUnderCancel rule = new ApplicationCap2QualifyUnderCancel();
            ICapApplication capOffer = _mockery.StrictMock<ICapApplication>();
            IAccount account = _mockery.StrictMock<IAccount>();
            IDetail detail = _mockery.StrictMock<IDetail>();
            IList<IDetail> detailLst = new List<IDetail>();
            IReadOnlyEventList<IDetail> detailEventLst = null;

            //
            int accountKey = 1;
            int detailTypeKey = (int)DetailTypes.UnderCancellation;

            //
            detailLst.Add(detail);
            detailEventLst = new ReadOnlyEventList<IDetail>(detailLst);

            //
            SetupResult.For(accRepo.GetDetailByAccountKeyAndDetailType(accountKey, detailTypeKey)).IgnoreArguments().Return(detailEventLst);

            //
            SetupResult.For(account.Key).Return(accountKey);

            //
            SetupResult.For(capOffer.Account).Return(account);

            //
            ExecuteRule(rule, 1, capOffer);
        }

        #endregion ApplicationCap2QualifyUnderCancel Test

        #region ApplicationCap2CurrentBalance Test

        [Test]
        public void ApplicationCap2CurrentBalanceTestPass()
        {
            ApplicationCap2CurrentBalance rule = new ApplicationCap2CurrentBalance();
            IMortgageLoanAccount mla = _mockery.StrictMock<IMortgageLoanAccount>();
            IMortgageLoan ml = _mockery.StrictMock<IMortgageLoan>();
            IAccountStatus ac = _mockery.StrictMock<IAccountStatus>();

            //
            double currentBalance = 1.00;
            int accountStatusKey = (int)AccountStatuses.Open;
            int accountKey = 1;

            //
            SetupResult.For(ml.CurrentBalance).Return(currentBalance);
            SetupResult.For(mla.SecuredMortgageLoan).Return(ml);
            SetupResult.For(mla.Key).Return(accountKey);
            SetupResult.For(ac.Key).Return(accountStatusKey);
            SetupResult.For(mla.AccountStatus).Return(ac);

            //
            ExecuteRule(rule, 0, mla);
        }

        [Test]
        public void ApplicationCap2CurrentBalanceTestFail()
        {
            ApplicationCap2CurrentBalance rule = new ApplicationCap2CurrentBalance();
            IMortgageLoanAccount mla = _mockery.StrictMock<IMortgageLoanAccount>();
            IMortgageLoan ml = _mockery.StrictMock<IMortgageLoan>();
            IAccountStatus ac = _mockery.StrictMock<IAccountStatus>();

            //
            double currentBalance = 0.00;
            int accountStatusKey = (int)AccountStatuses.Open;
            int accountKey = 1;

            //
            SetupResult.For(ml.CurrentBalance).Return(currentBalance);
            SetupResult.For(mla.SecuredMortgageLoan).Return(ml);
            SetupResult.For(mla.Key).Return(accountKey);
            SetupResult.For(ac.Key).Return(accountStatusKey);
            SetupResult.For(mla.AccountStatus).Return(ac);

            //
            ExecuteRule(rule, 1, mla);
        }

        #endregion ApplicationCap2CurrentBalance Test

        #region ApplicationCap2CapTypeConfig Test

        [Test]
        public void ApplicationCap2CapTypeConfigTestPass()
        {
            ApplicationCap2CapTypeConfig rule = new ApplicationCap2CapTypeConfig();

            //
            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            ICapRepository capRepo = _mockery.StrictMock<ICapRepository>();
            MockCache.Add(typeof(ICapRepository).ToString(), capRepo);

            //
            IMortgageLoanAccount mla = _mockery.StrictMock<IMortgageLoanAccount>();
            IMortgageLoan ml = _mockery.StrictMock<IMortgageLoan>();
            ICapTypeConfiguration ctc = _mockery.StrictMock<ICapTypeConfiguration>();
            IResetConfiguration rs = _mockery.StrictMock<IResetConfiguration>();
            IAccountStatus ac = _mockery.StrictMock<IAccountStatus>();

            //
            int Key = 1;
            int accountStatusKey = (int)AccountStatuses.Open;
            string desc = "test";

            //
            SetupResult.For(rs.Key).Return(Key);
            SetupResult.For(rs.Description).Return(desc);

            //
            SetupResult.For(ml.ResetConfiguration).Return(rs);

            //
            SetupResult.For(ac.Key).Return(accountStatusKey);

            //
            SetupResult.For(mla.SecuredMortgageLoan).Return(ml);
            SetupResult.For(mla.Key).Return(Key);
            SetupResult.For(mla.AccountStatus).Return(ac);

            //
            SetupResult.For(capRepo.GetCurrentCapTypeConfigByResetConfigKey(Key)).IgnoreArguments().Return(ctc);

            //
            ExecuteRule(rule, 0, mla);
        }

        [Test]
        public void ApplicationCap2CapTypeConfigTestFail()
        {
            ApplicationCap2CapTypeConfig rule = new ApplicationCap2CapTypeConfig();

            //
            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            ICapRepository capRepo = _mockery.StrictMock<ICapRepository>();
            MockCache.Add(typeof(ICapRepository).ToString(), capRepo);

            //
            IMortgageLoanAccount mla = _mockery.StrictMock<IMortgageLoanAccount>();
            IMortgageLoan ml = _mockery.StrictMock<IMortgageLoan>();
            ICapTypeConfiguration ctc = null;
            IResetConfiguration rs = _mockery.StrictMock<IResetConfiguration>();
            IAccountStatus ac = _mockery.StrictMock<IAccountStatus>();

            //
            int Key = 1;
            int accountStatusKey = (int)AccountStatuses.Open;
            string desc = "test";

            //
            SetupResult.For(rs.Key).Return(Key);
            SetupResult.For(rs.Description).Return(desc);

            //
            SetupResult.For(ml.ResetConfiguration).Return(rs);

            //
            SetupResult.For(ac.Key).Return(accountStatusKey);

            //
            SetupResult.For(mla.SecuredMortgageLoan).Return(ml);
            SetupResult.For(mla.Key).Return(Key);
            SetupResult.For(mla.AccountStatus).Return(ac);

            //
            SetupResult.For(capRepo.GetCurrentCapTypeConfigByResetConfigKey(Key)).IgnoreArguments().Return(ctc);

            //
            ExecuteRule(rule, 1, mla);
        }

        #endregion ApplicationCap2CapTypeConfig Test

        #region ApplicationCap2CheckLTVThreshold

        [NUnit.Framework.Test]
        public void ApplicationCap2CheckLTVThresholdTestPass()
        {
            using (new SessionScope())
            {
                SetRepositoryStrategy(TypeFactoryStrategy.Mock);
                ApplicationCap2CheckLTVThreshold rule = new ApplicationCap2CheckLTVThreshold();
                ICapApplication capApplication = _mockery.StrictMock<ICapApplication>();

                //
                ICapRepository capRepo = _mockery.StrictMock<ICapRepository>();
                MockCache.Add(typeof(ICapRepository).ToString(), capRepo);

                //
                bool result = true;
                SetupResult.For(capRepo.CheckLTVThreshold(capApplication)).IgnoreArguments().Return(result);
                ExecuteRule(rule, 0, capApplication);
            }
        }

        [NUnit.Framework.Test]
        public void ApplicationCap2CheckLTVThresholdTestFail()
        {
            using (new SessionScope())
            {
                SetRepositoryStrategy(TypeFactoryStrategy.Mock);
                ApplicationCap2CheckLTVThreshold rule = new ApplicationCap2CheckLTVThreshold();
                ICapApplication capApplication = _mockery.StrictMock<ICapApplication>();
                IControl control = _mockery.StrictMock<IControl>();

                //
                ICapRepository capRepo = _mockery.StrictMock<ICapRepository>();
                IControlRepository ctrlRepo = _mockery.StrictMock<IControlRepository>();
                MockCache.Add(typeof(ICapRepository).ToString(), capRepo);
                MockCache.Add(typeof(IControlRepository).ToString(), ctrlRepo);

                //
                bool result = false;
                double LTV = 0.0;
                SetupResult.For(capRepo.CheckLTVThreshold(capApplication)).IgnoreArguments().Return(result);
                SetupResult.For(control.ControlNumeric).Return(LTV);
                SetupResult.For(ctrlRepo.GetControlByDescription("CapLTVThreshold")).IgnoreArguments().Return(control);
                ExecuteRule(rule, 1, capApplication);
            }
        }

        #endregion ApplicationCap2CheckLTVThreshold

        [Test]
        public void RuleTestApplicationCap2QualifyInterestOnly()
        {
            ApplicationCap2QualifyInterestOnly rule = new ApplicationCap2QualifyInterestOnly();
            SetRepositoryStrategy(TypeFactoryStrategy.Default);
            using (new SessionScope())
            {
                ICapRepository capRepo = RepositoryFactory.GetRepository<ICapRepository>();
                ICapApplication capApp = capRepo.GetCapOfferByKey(214266);
                ExecuteRule(rule, 0, capApp);
            }
        }
    }
}