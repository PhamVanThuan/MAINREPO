using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Rules.Application.FurtherLending;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Specs.Rules.Application.FurtherLending.PreventPayingwithHOCPaidUpwithNoHOCSpecs
{
    [Subject(typeof(ApplicationFurtherLendingPreventPayingwithHOCPaidUpwithNoHOC))]
    public class when_their_is_a_paid_up_with_no_hoc_detail_type : RulesBaseWithFakes<ApplicationFurtherLendingPreventPayingwithHOCPaidUpwithNoHOC>
    {
        private static IHOCRepository commonRepo;
        private static IApplicationMortgageLoan application;

        Establish context = () =>
        {
            IEventList<IAccount> relatedAccounts = new EventList<IAccount>();
            IEventList<IFinancialService> financialServices = new EventList<IFinancialService>();
            IEventList<IDetail> details = new EventList<IDetail>();

            IAccount hocAccount = An<IAccount>();
            IProduct hocProduct = An<IProduct>();
            IFinancialService hocFinancialService = An<IFinancialService>();
            IFinancialServiceType financialServiceType = An<IFinancialServiceType>();
            IHOC hoc = An<IHOC>();
            IHOCInsurer hocInsurer = An<IHOCInsurer>();
            IHOCStatus hocStatus = An<IHOCStatus>();
            IDetail hocDetail = An<IDetail>();
            IDetailType hocDetailType = An<IDetailType>();

            hocProduct.WhenToldTo(x => x.Key)
                .Return((int)SAHL.Common.Globals.Products.HomeOwnersCover);

            hocAccount.WhenToldTo(x => x.Product)
                .Return(hocProduct);

            hocAccount.WhenToldTo(x => x.Details)
                .Return(details);

            relatedAccounts.Add(messages, hocAccount);

            financialServices.Add(messages, hocFinancialService);

            details.Add(messages, hocDetail);

            hocDetail.WhenToldTo(x => x.DetailType)
                .Return(hocDetailType);

            hocAccount.WhenToldTo(x => x.FinancialServices)
                .Return(financialServices);

            financialServiceType.WhenToldTo(x => x.Key)
                .Return((int)SAHL.Common.Globals.FinancialServiceTypes.HomeOwnersCover);

            hocFinancialService.WhenToldTo(x => x.FinancialServiceType)
                .Return(financialServiceType);
            hocFinancialService.WhenToldTo(x => x.Key)
                .Return(1111);

            application = An<IApplicationMortgageLoan>();

            application.WhenToldTo(x => x.RelatedAccounts)
                .Return(relatedAccounts);

            hoc.WhenToldTo(x => x.HOCInsurer)
                .Return(hocInsurer);

            hoc.WhenToldTo(x => x.HOCStatus)
                .Return(hocStatus);

            application.WhenToldTo(x => x.Account)
                .Return(hocAccount);

            IHOCRepository hocRepository = An<IHOCRepository>();
            hocRepository.WhenToldTo(x => x.GetHOCByKey(hocFinancialService.Key))
                .Return(hoc);

            AddRepositoryToMockCache(typeof(IHOCRepository), hocRepository);

            // setup for this specific test
            hocInsurer.WhenToldTo(x => x.Key)
                .Return((int)SAHL.Common.Globals.HOCInsurers.SAHLHOC);

            hocStatus.WhenToldTo(x => x.Key)
                .Return((int)SAHL.Common.Globals.HocStatuses.Open);

            hocDetailType.WhenToldTo(x => x.Key)
                .Return((int)SAHL.Common.Globals.DetailTypes.PaidUpWithNoHOC);

            businessRule = new ApplicationFurtherLendingPreventPayingwithHOCPaidUpwithNoHOC();
            RulesBaseWithFakes<ApplicationFurtherLendingPreventPayingwithHOCPaidUpwithNoHOC>.startrule.Invoke();
        };

        Because of = () =>
        {
            RuleResult = businessRule.ExecuteRule(messages, application);
        };

        It should_rule_should_fail = () =>
        {
            RuleResult.ShouldIndicateAFailedRule();
        };

        It should_report_an_error_message = () =>
        {
            messages.Count.ShouldEqual(1);
        };
    }
}