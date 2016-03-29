using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.DataAccess;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.CacheData;
using System.Data;
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using System.Collections.Generic;

namespace SAHL.Common.BusinessModel.Specs.Rules.Application.FurtherLending.AccountDetailTypeVarifixOptOut90DayPendingHoldCheck
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.Application.FurtherLending.AccountDetailTypeVarifixOptOut90DayPendingHoldCheck))]
    public class when_the_account_has_total_app_amt_greater_than_laa_and_doesnt_contain_detail_type : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.FurtherLending.AccountDetailTypeVarifixOptOut90DayPendingHoldCheck>
    {
        protected static IMortgageLoanAccount account;
        protected static ICastleTransactionsService castleTransactionService;
        protected static IUIStatementService uistatementService;

        protected static IApplicationReAdvance reAdvanceApplication;
        protected static IApplicationType applicationType;
        protected static IApplicationProductVariableLoan applicationProduct;

        protected static ISupportsVariableLoanApplicationInformation variableLoanApplicationInformation;
        protected static IApplicationInformationVariableLoan applicationInformationVariableLoan;

        Establish context = () =>
        {
            var totalApplicationAmount = 2;


            IApplicationStatus appStatus = An<IApplicationStatus>();
            reAdvanceApplication = An<IApplicationReAdvance>();
            applicationType = An<IApplicationType>();
            applicationProduct = An<IApplicationProductVariableLoan>();
            applicationInformationVariableLoan = An<IApplicationInformationVariableLoan>();
            IMortgageLoan vML = An<IMortgageLoan>();
            IBond bond = An<IBond>();
            ILoanAgreement loanAgreement = An<ILoanAgreement>();

            account = An<IMortgageLoanAccount>();
            castleTransactionService = An<ICastleTransactionsService>();
            uistatementService = An<IUIStatementService>();

            IEventList<IBond> bonds = new EventList<IBond>(new List<IBond>() { bond });
            IEventList<ILoanAgreement> loanAgreements = new EventList<ILoanAgreement>(new List<ILoanAgreement>() { loanAgreement });

            bond.WhenToldTo(x => x.LoanAgreements).Return(loanAgreements);
            loanAgreement.WhenToldTo(x => x.Amount).Return(totalApplicationAmount-1);

            account.WhenToldTo(x => x.SecuredMortgageLoan).Return(vML);
            vML.WhenToldTo(x => x.CurrentBalance).Return(0);
            vML.WhenToldTo(x => x.Bonds).Return(bonds);

            appStatus.WhenToldTo(x => x.Key).Return((int)OfferStatuses.Open);
            reAdvanceApplication.WhenToldTo(x => x.ApplicationStatus).Return(appStatus);
            applicationType.WhenToldTo(x => x.Key).Return((int)SAHL.Common.Globals.OfferTypes.ReAdvance);
            reAdvanceApplication.WhenToldTo(x => x.ApplicationType).Return(applicationType);
            reAdvanceApplication.WhenToldTo(x => x.CurrentProduct).Return(applicationProduct);
            applicationProduct.WhenToldTo(x => x.Application).Return(reAdvanceApplication);
            applicationProduct.WhenToldTo(x => x.VariableLoanInformation).Return(applicationInformationVariableLoan);
            applicationInformationVariableLoan.WhenToldTo(x => x.LoanAmountNoFees).Return(totalApplicationAmount);

            DataSet dataSet = new DataSet();

            uistatementService.WhenToldTo(x => x.GetStatement(Param.IsAny<string>(), Param.IsAny<string>())).Return("some sql");

            account.WhenToldTo(x => x.Key).Return(1);
            castleTransactionService.WhenToldTo(x => x.ExecuteQueryOnCastleTran(Param.IsAny<string>(), Param.IsAny<SAHL.Common.Globals.Databases>(), Param.IsAny<DataAccess.ParameterCollection>())).Return(dataSet);

            businessRule = new SAHL.Common.BusinessModel.Rules.Application.FurtherLending.AccountDetailTypeVarifixOptOut90DayPendingHoldCheck(castleTransactionService, uistatementService);
            RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.FurtherLending.AccountDetailTypeVarifixOptOut90DayPendingHoldCheck>.startrule.Invoke();
        };

        Because of = () =>
        {
            var parameters = new object[] { account, reAdvanceApplication, null, null };
            RuleResult = businessRule.ExecuteRule(messages, parameters);
        };

        It should_fail_the_rule = () =>
        {
            messages.Count.ShouldEqual(0);
        };
    }
}