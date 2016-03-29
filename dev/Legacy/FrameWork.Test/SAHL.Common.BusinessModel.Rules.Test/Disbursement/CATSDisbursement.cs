using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Castle.ActiveRecord;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Rules.Disbursement;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Test;

namespace SAHL.Common.BusinessModel.Rules.Test.Disbursement
{
    [TestFixture]
    public class CATSDisbursement : RuleBase
    {
        /// <summary>
        /// This interface is created for mocking purposes only, for rules that case IApplicationProduct objects
        /// to ISupportsVariableLoanApplicationInformation objects.
        /// </summary>
        public interface IApplicationProductSupportsVariableLoanApplicationInformation : IApplicationProduct, ISupportsVariableLoanApplicationInformation
        {
        }

        IDisbursement disbursement;

        [NUnit.Framework.SetUp]
        public new void Setup()
        {
            base.Setup();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [NUnit.Framework.Test]
        public void CATSDisbursementValidateUpdateRecordTestPass()
        {
            CATSDisbursementValidateUpdateRecord rule = new CATSDisbursementValidateUpdateRecord();

            disbursement = _mockery.StrictMock<IDisbursement>();
            IDisbursementStatus disbursementStatus = _mockery.StrictMock<IDisbursementStatus>();

            SetupResult.For(disbursement.DisbursementStatus).IgnoreArguments().Return(disbursementStatus);
            SetupResult.For(disbursementStatus.Key).Return((int)DisbursementStatuses.ReadyForDisbursement);

            IList<IDisbursement> dList = new List<IDisbursement>();
            dList.Add(disbursement);

            IReadOnlyEventList<IDisbursement> disburseList = new ReadOnlyEventList<IDisbursement>(dList);
            ExecuteRule(rule, 0, disburseList);
        }

        [NUnit.Framework.Test]
        public void CATSDisbursementValidateUpdateRecordTestFail()
        {
            CATSDisbursementValidateUpdateRecord rule = new CATSDisbursementValidateUpdateRecord();

            disbursement = _mockery.StrictMock<IDisbursement>();
            IDisbursementStatus disbursementStatus = _mockery.StrictMock<IDisbursementStatus>();

            SetupResult.For(disbursement.DisbursementStatus).IgnoreArguments().Return(disbursementStatus);
            SetupResult.For(disbursementStatus.Key).Return((int)DisbursementStatuses.Disbursed);

            IList<IDisbursement> dList = new List<IDisbursement>();
            dList.Add(disbursement);

            IReadOnlyEventList<IDisbursement> disburseList = new ReadOnlyEventList<IDisbursement>(dList);

            ExecuteRule(rule, 1, disburseList);
        }

        [NUnit.Framework.Test]
        public void CATSDisbursementValidateAmountTestMinFail()
        {
            CATSDisbursementValidateAmount rule = new CATSDisbursementValidateAmount();

            disbursement = _mockery.StrictMock<IDisbursement>();
            double min = 0;

            SetupResult.For(disbursement.Amount).IgnoreArguments().Return(min);

            IList<IDisbursement> dList = new List<IDisbursement>();
            dList.Add(disbursement);
            IReadOnlyEventList<IDisbursement> disburseList = new ReadOnlyEventList<IDisbursement>(dList);
            ExecuteRule(rule, 1, disburseList);
        }

        [NUnit.Framework.Test]
        public void CATSDisbursementValidateAmountTestMaxFail()
        {
            CATSDisbursementValidateAmount rule = new CATSDisbursementValidateAmount();

            disbursement = _mockery.StrictMock<IDisbursement>();
            double max = 11000000;

            SetupResult.For(disbursement.Amount).IgnoreArguments().Return(max);

            IList<IDisbursement> dList = new List<IDisbursement>();
            dList.Add(disbursement);
            IReadOnlyEventList<IDisbursement> disburseList = new ReadOnlyEventList<IDisbursement>(dList);

            ExecuteRule(rule, 1, disburseList);
        }

        [NUnit.Framework.Test]
        public void CATSDisbursementValidateAmountTestMinMaxPass()
        {
            CATSDisbursementValidateAmount rule = new CATSDisbursementValidateAmount();

            disbursement = _mockery.StrictMock<IDisbursement>();
            double amt = 5000;

            SetupResult.For(disbursement.Amount).IgnoreArguments().Return(amt);
            IList<IDisbursement> dList = new List<IDisbursement>();
            dList.Add(disbursement);
            IReadOnlyEventList<IDisbursement> disburseList = new ReadOnlyEventList<IDisbursement>(dList);
            ExecuteRule(rule, 0, disburseList);
        }

        [NUnit.Framework.Test]
        public void CATSDisbursementValidateTypeNotCancRefundCurrentBalanceTestPass()
        {
            CATSDisbursementValidateTypeCancRefundCurrentBalance rule = new CATSDisbursementValidateTypeCancRefundCurrentBalance();

            disbursement = _mockery.StrictMock<IDisbursement>();
            IDisbursementTransactionType disbursementTT = _mockery.StrictMock<IDisbursementTransactionType>();
            IMortgageLoanAccount mlAccount = _mockery.StrictMock<IMortgageLoanAccount>();
            double amt = 1000;
            double currBal = -10;

            SetupResult.For(disbursement.DisbursementTransactionType).IgnoreArguments().Return(disbursementTT);
            SetupResult.For(disbursementTT.Key).Return((int)DisbursementTransactionTypes.Refund);
            SetupResult.For(disbursement.Account).Return(mlAccount);
            SetupResult.For(mlAccount.LoanCurrentBalance).Return(currBal);
            SetupResult.For(disbursement.Amount).Return(amt);

            IList<IDisbursement> dList = new List<IDisbursement>();
            dList.Add(disbursement);
            IReadOnlyEventList<IDisbursement> disburseList = new ReadOnlyEventList<IDisbursement>(dList);

            ExecuteRule(rule, 0, disburseList);
        }

        [NUnit.Framework.Test]
        public void CATSDisbursementValidateTypeCancRefundCurrentBalanceLessThanZeroTestFail()
        {
            CATSDisbursementValidateTypeCancRefundCurrentBalance rule = new CATSDisbursementValidateTypeCancRefundCurrentBalance();

            disbursement = _mockery.StrictMock<IDisbursement>();
            IDisbursementTransactionType disbursementTT = _mockery.StrictMock<IDisbursementTransactionType>();
            IMortgageLoanAccount mlAccount = _mockery.StrictMock<IMortgageLoanAccount>();
            IDisbursementStatus disbStatus = _mockery.StrictMock<IDisbursementStatus>();

            double amt = 1000;
            double currBal = -10;

            SetupResult.For(disbStatus.Key).Return((int)DisbursementTransactionTypes.CancellationRefund);
            SetupResult.For(disbursement.DisbursementStatus).Return(disbStatus);
            SetupResult.For(disbursement.DisbursementTransactionType).IgnoreArguments().Return(disbursementTT);
            SetupResult.For(disbursementTT.Key).Return((int)DisbursementTransactionTypes.CancellationRefund);
            SetupResult.For(disbursement.Account).Return(mlAccount);
            SetupResult.For(mlAccount.LoanCurrentBalance).Return(currBal);
            SetupResult.For(disbursement.Amount).Return(amt);

            IList<IDisbursement> dList = new List<IDisbursement>();
            dList.Add(disbursement);
            IReadOnlyEventList<IDisbursement> disburseList = new ReadOnlyEventList<IDisbursement>(dList);

            ExecuteRule(rule, 1, disburseList);
        }

        [NUnit.Framework.Test]
        public void CATSDisbursementValidateTypeCancRefundCurrentBalanceGreaterThanZeroTestFail()
        {
            CATSDisbursementValidateTypeCancRefundCurrentBalance rule = new CATSDisbursementValidateTypeCancRefundCurrentBalance();

            disbursement = _mockery.StrictMock<IDisbursement>();
            IDisbursementTransactionType disbursementTT = _mockery.StrictMock<IDisbursementTransactionType>();
            IMortgageLoanAccount mlAccount = _mockery.StrictMock<IMortgageLoanAccount>();
            IDisbursementStatus disbStatus = _mockery.StrictMock<IDisbursementStatus>();

            double amt = 1000;
            double currBal = 1250;

            SetupResult.For(disbStatus.Key).Return((int)DisbursementTransactionTypes.CancellationRefund);
            SetupResult.For(disbursement.DisbursementStatus).Return(disbStatus);
            SetupResult.For(disbursement.DisbursementTransactionType).IgnoreArguments().Return(disbursementTT);
            SetupResult.For(disbursementTT.Key).Return((int)DisbursementTransactionTypes.CancellationRefund);
            SetupResult.For(disbursement.Account).Return(mlAccount);
            SetupResult.For(mlAccount.LoanCurrentBalance).Return(currBal);
            SetupResult.For(disbursement.Amount).Return(amt);

            IList<IDisbursement> dList = new List<IDisbursement>();
            dList.Add(disbursement);
            IReadOnlyEventList<IDisbursement> disburseList = new ReadOnlyEventList<IDisbursement>(dList);

            ExecuteRule(rule, 1, disburseList);
        }

        [NUnit.Framework.Test]
        public void CATSDisbursementValidateTypeCancRefundCurrentBalanceGreaterThanZeroTest_Fail()
        {
            CATSDisbursementValidateTypeCancRefundCurrentBalance rule = new CATSDisbursementValidateTypeCancRefundCurrentBalance();

            disbursement = _mockery.StrictMock<IDisbursement>();
            IDisbursementTransactionType disbursementTT = _mockery.StrictMock<IDisbursementTransactionType>();
            IMortgageLoanAccount mlAccount = _mockery.StrictMock<IMortgageLoanAccount>();
            IDisbursementStatus disbStatus = _mockery.StrictMock<IDisbursementStatus>();

            double amt = 1250;
            double currBal = 1250;

            SetupResult.For(disbStatus.Key).Return((int)DisbursementTransactionTypes.CancellationRefund);
            SetupResult.For(disbursement.DisbursementStatus).Return(disbStatus);
            SetupResult.For(disbursement.DisbursementTransactionType).IgnoreArguments().Return(disbursementTT);
            SetupResult.For(disbursementTT.Key).Return((int)DisbursementTransactionTypes.CancellationRefund);
            SetupResult.For(disbursement.Account).Return(mlAccount);
            SetupResult.For(mlAccount.LoanCurrentBalance).Return(currBal);
            SetupResult.For(disbursement.Amount).Return(amt);

            IList<IDisbursement> dList = new List<IDisbursement>();
            dList.Add(disbursement);
            IReadOnlyEventList<IDisbursement> disburseList = new ReadOnlyEventList<IDisbursement>(dList);
            ExecuteRule(rule, 1, disburseList);
        }

        [NUnit.Framework.Test]
        public void CATSDisbursementValidateReadvanceDebtCounsellingNoReadvanceTestPass()
        {
            CATSDisbursementValidateReadvanceDebtCounselling rule = new CATSDisbursementValidateReadvanceDebtCounselling();

            disbursement = _mockery.StrictMock<IDisbursement>();
            IDisbursementTransactionType disbursementTT = _mockery.StrictMock<IDisbursementTransactionType>();
            IMortgageLoanAccount mlAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            SetupResult.For(disbursement.DisbursementTransactionType).IgnoreArguments().Return(disbursementTT);
            SetupResult.For(disbursementTT.Key).Return((int)DisbursementTransactionTypes.CancellationRefund);
            SetupResult.For(disbursement.Account).Return(mlAccount);
            SetupResult.For(mlAccount.UnderDebtCounselling).Return(true);
            IList<IDisbursement> dList = new List<IDisbursement>();
            dList.Add(disbursement);
            IReadOnlyEventList<IDisbursement> disburseList = new ReadOnlyEventList<IDisbursement>(dList);

            ExecuteRule(rule, 0, disburseList);
        }

        [NUnit.Framework.Test]
        public void CATSDisbursementValidateReadvanceDebtCounsellingReadvanceTestFail()
        {
            CATSDisbursementValidateReadvanceDebtCounselling rule = new CATSDisbursementValidateReadvanceDebtCounselling();

            disbursement = _mockery.StrictMock<IDisbursement>();
            IDisbursementTransactionType disbursementTT = _mockery.StrictMock<IDisbursementTransactionType>();
            IMortgageLoanAccount mlAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            SetupResult.For(disbursement.DisbursementTransactionType).IgnoreArguments().Return(disbursementTT);
            SetupResult.For(disbursementTT.Key).Return((int)DisbursementTransactionTypes.ReAdvance);
            SetupResult.For(disbursement.Account).Return(mlAccount);
            SetupResult.For(mlAccount.UnderDebtCounselling).Return(true);

            IList<IDisbursement> dList = new List<IDisbursement>();
            dList.Add(disbursement);
            IReadOnlyEventList<IDisbursement> disburseList = new ReadOnlyEventList<IDisbursement>(dList);

            ExecuteRule(rule, 1, disburseList);
        }

        [NUnit.Framework.Test]
        public void CATSDisbursementValidateReadvanceDebtCounsellingCAP2ReadvanceTestFail()
        {
            CATSDisbursementValidateReadvanceDebtCounselling rule = new CATSDisbursementValidateReadvanceDebtCounselling();

            disbursement = _mockery.StrictMock<IDisbursement>();
            IDisbursementTransactionType disbursementTT = _mockery.StrictMock<IDisbursementTransactionType>();
            IMortgageLoanAccount mlAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            SetupResult.For(disbursement.DisbursementTransactionType).IgnoreArguments().Return(disbursementTT);
            SetupResult.For(disbursementTT.Key).Return((int)DisbursementTransactionTypes.CAP2ReAdvance);
            SetupResult.For(disbursement.Account).Return(mlAccount);
            SetupResult.For(mlAccount.UnderDebtCounselling).Return(true);
            IList<IDisbursement> dList = new List<IDisbursement>();
            dList.Add(disbursement);
            IReadOnlyEventList<IDisbursement> disburseList = new ReadOnlyEventList<IDisbursement>(dList);

            ExecuteRule(rule, 1, disburseList);
        }

        [NUnit.Framework.Test]
        public void CATSDisbursementValidateReadvanceDebtCounsellingReadvanceTestPass()
        {
            CATSDisbursementValidateReadvanceDebtCounselling rule = new CATSDisbursementValidateReadvanceDebtCounselling();

            disbursement = _mockery.StrictMock<IDisbursement>();
            IDisbursementTransactionType disbursementTT = _mockery.StrictMock<IDisbursementTransactionType>();
            IMortgageLoanAccount mlAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            SetupResult.For(disbursement.DisbursementTransactionType).IgnoreArguments().Return(disbursementTT);
            SetupResult.For(disbursementTT.Key).Return((int)DisbursementTransactionTypes.ReAdvance);
            SetupResult.For(disbursement.Account).Return(mlAccount);
            SetupResult.For(mlAccount.UnderDebtCounselling).Return(false);
            IList<IDisbursement> dList = new List<IDisbursement>();
            dList.Add(disbursement);
            IReadOnlyEventList<IDisbursement> disburseList = new ReadOnlyEventList<IDisbursement>(dList);

            ExecuteRule(rule, 0, disburseList);
        }

        [NUnit.Framework.Test]
        public void CATSDisbursementValidateReadvanceDebtCounsellingCAP2ReadvanceTestPass()
        {
            CATSDisbursementValidateReadvanceDebtCounselling rule = new CATSDisbursementValidateReadvanceDebtCounselling();

            disbursement = _mockery.StrictMock<IDisbursement>();
            IDisbursementTransactionType disbursementTT = _mockery.StrictMock<IDisbursementTransactionType>();
            IMortgageLoanAccount mlAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            SetupResult.For(disbursement.DisbursementTransactionType).IgnoreArguments().Return(disbursementTT);
            SetupResult.For(disbursementTT.Key).Return((int)DisbursementTransactionTypes.CAP2ReAdvance);
            SetupResult.For(disbursement.Account).Return(mlAccount);
            SetupResult.For(mlAccount.UnderDebtCounselling).Return(false);
            IList<IDisbursement> dList = new List<IDisbursement>();
            dList.Add(disbursement);
            IReadOnlyEventList<IDisbursement> disburseList = new ReadOnlyEventList<IDisbursement>(dList);

            ExecuteRule(rule, 0, disburseList);
        }

        [NUnit.Framework.Test]
        public void CATSDisbursementValidateTypeCAP2AddRecordNotCAP2ReadvanceTestPass()
        {
            CATSDisbursementValidateTypeCAP2AddRecord rule = new CATSDisbursementValidateTypeCAP2AddRecord();

            disbursement = _mockery.StrictMock<IDisbursement>();
            IDisbursementTransactionType dtt = _mockery.StrictMock<IDisbursementTransactionType>();
            SetupResult.For(disbursement.DisbursementTransactionType).Return(dtt);
            SetupResult.For(dtt.Key).Return((int)DisbursementTransactionTypes.Refund);
            IList<IDisbursement> dList = new List<IDisbursement>();
            dList.Add(disbursement);
            IReadOnlyEventList<IDisbursement> disburseList = new ReadOnlyEventList<IDisbursement>(dList);

            ExecuteRule(rule, 0, disburseList);
        }

        // mocking all of these cos there are none in the Cap2 Workflow
        [NUnit.Framework.Test]
        public void CATSDisbursementValidateTypeCAP2AddRecordCAP2ReadvanceTestFail()
        {
            CATSDisbursementValidateTypeCAP2AddRecord rule = new CATSDisbursementValidateTypeCAP2AddRecord();

            disbursement = _mockery.StrictMock<IDisbursement>();
            IDisbursementTransactionType dtt = _mockery.StrictMock<IDisbursementTransactionType>();
            SetupResult.For(disbursement.DisbursementTransactionType).Return(dtt);
            IAccount acc = _mockery.StrictMock<IAccount>();
            SetupResult.For(disbursement.Account).Return(acc);
            SetupResult.For(acc.Key).Return(1);

            SetupResult.For(dtt.Key).Return((int)DisbursementTransactionTypes.CAP2ReAdvance);
            IList<IDisbursement> dList = new List<IDisbursement>();
            dList.Add(disbursement);
            IReadOnlyEventList<IDisbursement> disburseList = new ReadOnlyEventList<IDisbursement>(dList);

            ExecuteRule(rule, 1, disburseList);
        }

        // mocking all of these cos there are no cases in the Cap2 Workflow
        [NUnit.Framework.Test]
        public void CATSDisbursementValidateTypeCAP2AddRecordCAP2ReadvanceTestPass()
        {
            CATSDisbursementValidateTypeCAP2AddRecord rule = new CATSDisbursementValidateTypeCAP2AddRecord();

            disbursement = _mockery.StrictMock<IDisbursement>();
            IDisbursementTransactionType dtt = _mockery.StrictMock<IDisbursementTransactionType>();
            SetupResult.For(disbursement.DisbursementTransactionType).Return(dtt);
            IAccount acc = _mockery.StrictMock<IAccount>();
            SetupResult.For(disbursement.Account).Return(acc);
            int accKey = 0;
            SetupResult.For(acc.Key).Return(accKey);

            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            ICapRepository capRepo = _mockery.StrictMock<ICapRepository>();
            MockCache.Add(typeof(ICapRepository).ToString(), capRepo);

            IList<ICapApplication> capApps = new List<ICapApplication>();
            ICapApplication capApp = _mockery.StrictMock<ICapApplication>();
            ICapStatus capStatus = _mockery.StrictMock<ICapStatus>();
            SetupResult.For(capApp.CapStatus).Return(capStatus);
            SetupResult.For(capStatus.Key).Return((int)CapStatuses.ReadvanceRequired);
            capApps.Add(capApp);

            SetupResult.For(capRepo.GetCapOfferByAccountKey(accKey)).IgnoreArguments().Return(capApps);

            SetupResult.For(dtt.Key).Return((int)DisbursementTransactionTypes.CAP2ReAdvance);

            IList<IDisbursement> dList = new List<IDisbursement>();
            dList.Add(disbursement);
            IReadOnlyEventList<IDisbursement> disburseList = new ReadOnlyEventList<IDisbursement>(dList);
            ExecuteRule(rule, 0, disburseList);
        }

        #region CATSDisbursementTransactionAmountValidate

        [NUnit.Framework.Test]
        public void CATSDisbursementTransactionAmountValidate()
        {
            CATSDisbursementTransactionAmountValidate rule = new CATSDisbursementTransactionAmountValidate();

            ExecuteRule(rule, 0, 1D, 1D);

            ExecuteRule(rule, 1, 1D, 2D);
        }

        #endregion CATSDisbursementTransactionAmountValidate

        #region CATSDisbursementLoanAgreementAmountValidate

        [NUnit.Framework.Test]
        public void CATSDisbursementLoanAgreementAmountValidate()
        {
            CATSDisbursementLoanAgreementAmountValidate rule = new CATSDisbursementLoanAgreementAmountValidate();

            IMortgageLoanAccount mla = _mockery.StrictMock<IMortgageLoanAccount>();
            IMortgageLoan ml = _mockery.StrictMock<IMortgageLoan>();
            IDisbursement disb = _mockery.StrictMock<IDisbursement>();
            IDisbursementStatus disStatus = _mockery.StrictMock<IDisbursementStatus>();
            IDisbursementTransactionType dtt = _mockery.StrictMock<IDisbursementTransactionType>();
            double? amount = 100D;

            //int? tranTypeNum = (int)DisbursementLoanTransactionTypes.ReAdvance;

            //SetupResult.For(dtt.TransactionTypeNumber).Return(tranTypeNum);
            SetupResult.For(dtt.Key).Return((int)DisbursementTransactionTypes.ReAdvance);
            SetupResult.For(disb.DisbursementTransactionType).Return(dtt);
            SetupResult.For(disb.Amount).Return(amount);
            SetupResult.For(disStatus.Key).Return((int)DisbursementStatuses.Pending);
            SetupResult.For(disb.DisbursementStatus).Return(disStatus);
            SetupResult.For(mla.LoanCurrentBalance).Return(1D);
            SetupResult.For(ml.SumBondLoanAgreementAmounts()).Return(1000D);
            SetupResult.For(ml.Key).Return(1);
            SetupResult.For(mla.SecuredMortgageLoan).Return(ml);
            IList<IDisbursement> dList = new List<IDisbursement>();
            dList.Add(disb);
            IReadOnlyEventList<IDisbursement> disburseList = new ReadOnlyEventList<IDisbursement>(dList);

            ExecuteRule(rule, 0, disburseList, mla);

            dList.Clear();

            //SetupResult.For(dtt.TransactionTypeNumber).Return(tranTypeNum);
            SetupResult.For(dtt.Key).Return((int)DisbursementTransactionTypes.ReAdvance);
            SetupResult.For(disb.DisbursementTransactionType).Return(dtt);
            SetupResult.For(disb.Amount).Return(amount);
            SetupResult.For(disStatus.Key).Return((int)DisbursementStatuses.Pending);
            SetupResult.For(disb.DisbursementStatus).Return(disStatus);
            SetupResult.For(mla.LoanCurrentBalance).Return(1000D);
            SetupResult.For(ml.SumBondLoanAgreementAmounts()).Return(1D);
            SetupResult.For(ml.Key).Return(1);
            SetupResult.For(mla.SecuredMortgageLoan).Return(ml);

            dList.Add(disb);

            IReadOnlyEventList<IDisbursement> disburseList1 = new ReadOnlyEventList<IDisbursement>(dList);
            ExecuteRule(rule, 1, disburseList1, mla);
        }

        #endregion CATSDisbursementLoanAgreementAmountValidate

        #region CATSDisbursementReAdvanceNotDisbursedTest

        /// <summary>
        /// Checks that the Total Readvance Amount is equal to the application readvance amount and
        /// that the readvance value has not already been disbursed.
        /// </summary>
        [NUnit.Framework.Test]
        public void CATSDisbursementReAdvanceNotDisbursedTest()
        {
            // if OfferType is not ReAdvance then it succeeds.
            CATSDisbursementReAdvanceNotDisbursedHelper(0, OfferTypes.FurtherAdvance, OfferStatuses.Open, OfferInformationTypes.AcceptedOffer, 100000.00, 100000.00, false, false);

            // if offer has already been disbursed then throw error
            CATSDisbursementReAdvanceNotDisbursedHelper(1, OfferTypes.ReAdvance, OfferStatuses.Accepted, OfferInformationTypes.AcceptedOffer, 100000.00, 100000.00, false, false);

            // if AppStatus != Open or AppInfoType != Accepted throw error message
            CATSDisbursementReAdvanceNotDisbursedHelper(1, OfferTypes.ReAdvance, OfferStatuses.Open, OfferInformationTypes.OriginalOffer, 100000.00, 100000.00, false, false);

            // Cannot disburse if Total Readvance Amount is not equal to the Application Readvance amount
            CATSDisbursementReAdvanceNotDisbursedHelper(1, OfferTypes.ReAdvance, OfferStatuses.Open, OfferInformationTypes.AcceptedOffer, 100000.00, 200000.00, false, false);

            // Total Readvance Amount equal to the Application Readvance amount - Success
            CATSDisbursementReAdvanceNotDisbursedHelper(0, OfferTypes.ReAdvance, OfferStatuses.Open, OfferInformationTypes.AcceptedOffer, 100000.00, 100000.00, false, false);

            // Total Readvance Amount equal to the Application Readvance amount, Loan transaction exists - Fail
            CATSDisbursementReAdvanceNotDisbursedHelper(1, OfferTypes.ReAdvance, OfferStatuses.Open, OfferInformationTypes.AcceptedOffer, 100000.00, 100000.00, true, true);
        }

        /// <summary>
        /// Helper method to set up the expectations for the CATSDisbursementReAdvanceNotDisbursed Test.
        /// </summary>
        /// <param name=""></param>
        private void CATSDisbursementReAdvanceNotDisbursedHelper(int expectedMessageCount, OfferTypes offType, OfferStatuses offStatus, OfferInformationTypes offInfoTypes, double totalReadvanceAmount, double LoanAgreementAmount, bool withTransaction, bool extRule)
        {
            SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);

            IAccountRepository accRepo = _mockery.StrictMock<IAccountRepository>();
            MockCache.Add(typeof(IAccountRepository).ToString(), accRepo);

            ILoanTransactionRepository ltRepo = _mockery.StrictMock<ILoanTransactionRepository>();
            MockCache.Add(typeof(ILoanTransactionRepository).ToString(), ltRepo);
            int accountKey = 1;
            int fsKey = 1;

            CATSDisbursementReAdvanceNotDisbursed rule = new CATSDisbursementReAdvanceNotDisbursed();
            CATSDisbursementReAdvanceNotDisbursedExt ruleExt = new CATSDisbursementReAdvanceNotDisbursedExt();

            IAccount acc = _mockery.StrictMock<IAccount>();
            IOriginationSource os = _mockery.StrictMock<IOriginationSource>();
            IEventList<IApplication> appList = new EventList<IApplication>();
            IApplication app = _mockery.StrictMock<IApplication>();

            IApplicationType appType = _mockery.StrictMock<IApplicationType>();
            IApplicationStatus appStatus = _mockery.StrictMock<IApplicationStatus>();
            IApplicationInformation appInfo = _mockery.StrictMock<IApplicationInformation>();
            IApplicationInformationType appInfoType = _mockery.StrictMock<IApplicationInformationType>();
            IApplicationProductSupportsVariableLoanApplicationInformation vlai = _mockery.StrictMock<IApplicationProductSupportsVariableLoanApplicationInformation>();
            IApplicationInformationVariableLoan vli = _mockery.StrictMock<IApplicationInformationVariableLoan>();

            IFinancialService fs = _mockery.StrictMock<IFinancialService>();
            IFinancialServiceType fsType = _mockery.StrictMock<IFinancialServiceType>();
            IAccountStatus accStatus = _mockery.StrictMock<IAccountStatus>();
            IFinancialTransaction lt = _mockery.StrictMock<IFinancialTransaction>();
            IEventList<IFinancialService> fsList = new EventList<IFinancialService>();

            SetupResult.For(fsType.Key).Return(1);
            SetupResult.For(fs.Key).Return(fsKey);
            SetupResult.For(fs.FinancialServiceType).Return(fsType);
            SetupResult.For(accStatus.Key).Return(1);
            SetupResult.For(fs.AccountStatus).Return(accStatus);

            SetupResult.For(lt.Amount).Return(totalReadvanceAmount);
            if (withTransaction)
                SetupResult.For(lt.InsertDate).Return(DateTime.Now);
            else
                SetupResult.For(lt.InsertDate).Return(DateTime.Now.AddDays(-4));

            SetupResult.For(ltRepo.GetLastLoanTransactionByTransactionTypeAndFinancialServiceKey(TransactionTypes.Readvance, fsKey, false)).IgnoreArguments().Return(lt);

            SetupResult.For(vli.LoanAgreementAmount).Return(LoanAgreementAmount);
            SetupResult.For(vlai.VariableLoanInformation).Return(vli);
            SetupResult.For(app.CurrentProduct).Return(vlai);

            SetupResult.For(appInfoType.Key).Return((int)offInfoTypes);
            SetupResult.For(appInfo.ApplicationInformationType).Return(appInfoType);
            SetupResult.For(app.GetLatestApplicationInformation()).Return(appInfo);
            SetupResult.For(app.Key).Return(1);

            SetupResult.For(appStatus.Key).Return((int)offStatus);
            SetupResult.For(app.ApplicationStatus).Return(appStatus);

            SetupResult.For(appType.Key).Return((int)offType);
            SetupResult.For(app.ApplicationType).Return(appType);

            SetupResult.For(os.Key).Return((int)OriginationSources.SAHomeLoans);

            SetupResult.For(acc.Key).Return(accountKey);
            SetupResult.For(acc.OriginationSource).Return(os);
            SetupResult.For(accRepo.GetAccountByKey(accountKey)).IgnoreArguments().Return(acc);

            appList.Add(null, app);
            SetupResult.For(acc.Applications).Return(appList);
            fsList.Add(null, fs);
            SetupResult.For(acc.FinancialServices).Return(fsList);

            object paramObj = totalReadvanceAmount;

            if (extRule)
                ExecuteRule(ruleExt, expectedMessageCount, acc, paramObj);
            else
                ExecuteRule(rule, expectedMessageCount, acc, paramObj);
        }

        #endregion CATSDisbursementReAdvanceNotDisbursedTest

        #region CATSDisbursementSPVCheckTest

        /// <summary>
        /// This checks if a SPV has been discontinued. If it has it throws an error as cannot disburse.
        /// </summary>
        [NUnit.Framework.Test]
        public void CATSDisbursementSPVCheckTest()
        {
            // if OfferType is not ReAdvance then it succeeds.
            CATSDisbursementSPVCheckHelper(0);
        }

        /// <summary>
        /// Helper method to set up the expectations for the CATSDisbursementSPVCheckTest Test.
        /// </summary>
        /// <param name=""></param>
        private void CATSDisbursementSPVCheckHelper(int expectedMessageCount)
        {
            CATSDisbursementSPVCheck rule = new CATSDisbursementSPVCheck(RepositoryFactory.GetRepository<ICastleTransactionsService>());
            disbursement = _mockery.StrictMock<IDisbursement>();

            IDisbursementTransactionType dtt = _mockery.StrictMock<IDisbursementTransactionType>();
            SetupResult.For(disbursement.DisbursementTransactionType).Return(dtt);
            IAccount acc = _mockery.StrictMock<IAccount>();
            SetupResult.For(disbursement.Account).Return(acc);

            string sql = @"select fs.AccountKey from financialservice fs
					join account act on fs.accountkey = act.accountkey
                    join offer o on o.AccountKey = fs.accountkey
                    join disbursement d on d.accountkey = fs.accountkey
                    where act.SPVKey = 25
                    and fs.AccountStatusKey = 1
                    and fs.FinancialServiceTypeKey = 1
                    and o.offertypekey = 2
                    and d.disbursementtransactiontypekey = 9";
            DataTable dt = base.GetQueryResults(sql);

            if (dt.Rows.Count > 0)
            {
                int accKey = Convert.ToInt32(dt.Rows[0][0]);

                SetupResult.For(acc.Key).Return(accKey);

                SetRepositoryStrategy(TypeFactoryStrategy.Mock);
                ICapRepository capRepo = _mockery.StrictMock<ICapRepository>();
                MockCache.Add(typeof(ICapRepository).ToString(), capRepo);

                IList<ICapApplication> capApps = new List<ICapApplication>();
                ICapApplication capApp = _mockery.StrictMock<ICapApplication>();
                ICapStatus capStatus = _mockery.StrictMock<ICapStatus>();
                SetupResult.For(capApp.CapStatus).Return(capStatus);
                SetupResult.For(capStatus.Key).Return((int)CapStatuses.ReadvanceRequired);
                capApps.Add(capApp);

                SetupResult.For(capRepo.GetCapOfferByAccountKey(accKey)).IgnoreArguments().Return(capApps);

                SetupResult.For(dtt.Key).Return((int)DisbursementTransactionTypes.CAP2ReAdvance);

                IList<IDisbursement> dList = new List<IDisbursement>();
                dList.Add(disbursement);
                IReadOnlyEventList<IDisbursement> disburseList = new ReadOnlyEventList<IDisbursement>(dList);

                ExecuteRule(rule, expectedMessageCount, disburseList);
            }
            else
                Assert.Inconclusive("No data");
        }

        #endregion CATSDisbursementSPVCheckTest

        #region CATSDisbursementDebitOrderSuspendedCapReAdvanceTest

        /// <summary>
        /// This checks if a SPV has been discontinued. If it has it throws an error as cannot disburse.
        /// </summary>
        [NUnit.Framework.Test]
        public void CATSDisbursementDebitOrderSuspendedCapReAdvanceTest()
        {
            IDetail detail = _mockery.StrictMock<IDetail>();
            SetRepositoryStrategy(TypeFactoryStrategy.Mock);

            // FAIL - CAP2READVANCE and DEBITORDERSUSPENDED
            CATSDisbursementDebitOrderSuspendedCapReAdvanceHelper(1, (int)DisbursementTransactionTypes.CAP2ReAdvance, detail, (int)DetailTypes.DebitOrderSuspended);

            // FAIL - READVANCE and DEBITORDERSUSPENDED
            CATSDisbursementDebitOrderSuspendedCapReAdvanceHelper(1, (int)DisbursementTransactionTypes.ReAdvance, detail, (int)DetailTypes.DebitOrderSuspended);

            // PASS - READVANCE and NOT DEBITORDERSUSPENDED
            CATSDisbursementDebitOrderSuspendedCapReAdvanceHelper(0, (int)DisbursementTransactionTypes.ReAdvance, detail, (int)DetailTypes.CancellationRegistered);

            // PASS - NOT READVANCE or NOT CAP2READVANCE and NOT DEBITORDERSUSPENDED
            CATSDisbursementDebitOrderSuspendedCapReAdvanceHelper(0, (int)DisbursementTransactionTypes.QuickCash, detail, (int)DetailTypes.CancellationRegistered);
        }

        /// <summary>
        /// Helper method to set up the expectations for the CATSDisbursementSPVCheckTest Test.
        /// </summary>
        /// <param name=""></param>
        private void CATSDisbursementDebitOrderSuspendedCapReAdvanceHelper(int expectedMessageCount, int disTranType, IDetail detail, int detailType)
        {
            IAccountRepository accRepo = _mockery.StrictMock<IAccountRepository>();
            MockCache.Add(typeof(IAccountRepository).ToString(), accRepo);

            CATSDisbursementDebitOrderSuspendedCapReAdvance rule = new CATSDisbursementDebitOrderSuspendedCapReAdvance();
            disbursement = _mockery.StrictMock<IDisbursement>();

            IDisbursementTransactionType dtt = _mockery.StrictMock<IDisbursementTransactionType>();
            SetupResult.For(dtt.Key).Return(disTranType);
            SetupResult.For(disbursement.DisbursementTransactionType).Return(dtt);

            IAccount acc = _mockery.StrictMock<IAccount>();
            SetupResult.For(acc.Key).Return(1);

            SetupResult.For(disbursement.Account).Return(acc);

            IEventList<IDetail> details = new EventList<IDetail>();
            if (detail != null)
            {
                details.Add(new DomainMessageCollection(), detail);
                IDetailType dType = _mockery.StrictMock<IDetailType>();
                SetupResult.For(dType.Key).Return(detailType);
                SetupResult.For(detail.DetailType).Return(dType);
            }
            SetupResult.For(acc.Details).Return(details);

            IList<IDisbursement> dList = new List<IDisbursement>();
            dList.Add(disbursement);
            IReadOnlyEventList<IDisbursement> disburseList = new ReadOnlyEventList<IDisbursement>(dList);

            SetupResult.For(accRepo.GetAccountByKey(1)).IgnoreArguments().Return(acc);
            ExecuteRule(rule, expectedMessageCount, disburseList);
        }

        #endregion CATSDisbursementDebitOrderSuspendedCapReAdvanceTest

        #region Rollback Disbursements

        [NUnit.Framework.Test]
        public void CATSDisbursementRollback()
        {
            TimeSpan ts = new TimeSpan(12, 30, 0);
            using (new SessionScope())
            {
                string testQuery = string.Format(@"select rp.[Value] from RuleParameter rp (NOLOCK)
                            join RuleItem ri (NOLOCK) on rp.RuleItemKey = ri.RuleItemKey
                            where ri.[Name] = 'CATSDisbursementRollback' and rp.[Name] = '@CutOffTime'");

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(testQuery, typeof(Account_DAO), new ParameterCollection());
                if (o != null)
                    ts = TimeSpan.Parse(o.ToString());
            }

            int msgCount = 0;
            if (DateTime.Now.TimeOfDay > ts)
                msgCount = 1;

            DisbursementRollbackHelper(DisbursementStatuses.Disbursed, 1);
            DisbursementRollbackHelper(DisbursementStatuses.Pending, msgCount);
            DisbursementRollbackHelper(DisbursementStatuses.ReadyForDisbursement, msgCount);
            DisbursementRollbackHelper(DisbursementStatuses.RolledBack, msgCount);
        }

        private void DisbursementRollbackHelper(DisbursementStatuses status, int msgCount)
        {
            CATSDisbursementRollback rule = new CATSDisbursementRollback();

            disbursement = _mockery.StrictMock<IDisbursement>();
            SetupResult.For(disbursement.Key).Return(1);

            IDisbursementStatus ds = _mockery.StrictMock<IDisbursementStatus>();
            SetupResult.For(ds.Key).Return((int)status);

            SetupResult.For(disbursement.DisbursementStatus).Return(ds);

            IList<IDisbursement> dList = new List<IDisbursement>();
            dList.Add(disbursement);
            IReadOnlyEventList<IDisbursement> disburseList = new ReadOnlyEventList<IDisbursement>(dList);

            ExecuteRule(rule, msgCount, disburseList);
        }

        #endregion Rollback Disbursements

        #region CATSDisbursementQuickCashDisbursementValidate

        [Test]
        public void CATSDisbursementQuickCashDisbursementValidate()
        {
            CATSDisbursementQuickCashDisbursementValidateHelper(true, true, 0);
            CATSDisbursementQuickCashDisbursementValidateHelper(false, true, 0);
            CATSDisbursementQuickCashDisbursementValidateHelper(true, false, 1);
        }

        private void CATSDisbursementQuickCashDisbursementValidateHelper(bool isDisbursement, bool isValidDisbursement, int msgCount)
        {
            CATSDisbursementQuickCashDisbursementValidate rule = new CATSDisbursementQuickCashDisbursementValidate();

            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            IQuickCashRepository qcRepo = _mockery.StrictMock<IQuickCashRepository>();
            MockCache.Add(typeof(IQuickCashRepository).ToString(), qcRepo);

            disbursement = _mockery.StrictMock<IDisbursement>();
            IAccount acc = _mockery.StrictMock<IAccount>();
            SetupResult.For(acc.Key).Return(1);
            SetupResult.For(disbursement.Account).Return(acc);

            IDisbursementTransactionType dTranType = _mockery.StrictMock<IDisbursementTransactionType>();

            if (isDisbursement)
                SetupResult.For(disbursement.Key).Return(1);
            else
                SetupResult.For(disbursement.Key).Return(0);

            SetupResult.For(disbursement.DisbursementTransactionType).Return(dTranType);
            SetupResult.For(dTranType.Key).Return((int)DisbursementTransactionTypes.QuickCash);

            IList<IDisbursement> dList = new List<IDisbursement>();
            dList.Add(disbursement);

            IReadOnlyEventList<IDisbursement> disburseList = new ReadOnlyEventList<IDisbursement>(dList);

            List<IApplicationInformationQuickCashDetail> qcDetails = new List<IApplicationInformationQuickCashDetail>();
            IApplicationInformationQuickCashDetail qcDetail = _mockery.StrictMock<IApplicationInformationQuickCashDetail>();

            //if (isValidDisbursement)
            //{
            SetupResult.For(qcDetail.Disbursed).Return(!isValidDisbursement);
            SetupResult.For(qcDetail.RequestedAmount).Return(1D);

            //}
            //else
            //{
            //    SetupResult.For(qcDetail.Disbursed).Return(true);
            //    SetupResult.For(qcDetail.RequestedAmount).Return(1D);
            //}
            qcDetails.Add(qcDetail);

            SetupResult.For(qcRepo.GetApplicationInformationQuickCashDetailFromDisbursementObj(disbursement)).Return(qcDetails);
            SetupResult.For(qcRepo.GetApplicationInformationQuickCashDetailByAccountKey(1)).IgnoreArguments().Return(qcDetails);

            ExecuteRule(rule, msgCount, disburseList);
        }

        #endregion CATSDisbursementQuickCashDisbursementValidate
    }
}