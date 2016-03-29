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
    public class when_the_hoc_insurer_is_set_to_paid_up_with_no_hoc : RulesBaseWithFakes<ApplicationFurtherLendingPreventPayingwithHOCPaidUpwithNoHOC>
    {
        private static IHOCRepository commonRepo;
        private static IApplicationMortgageLoan application;

        Establish context = () =>
            {
                IEventList<IAccount> relatedAccounts = new EventList<IAccount>();
                IEventList<IFinancialService> financialServices = new EventList<IFinancialService>();
                IAccount hocAccount = An<IAccount>();
                IProduct hocProduct = An<IProduct>();
                IFinancialService hocFinancialService = An<IFinancialService>();
                IFinancialServiceType financialServiceType = An<IFinancialServiceType>();
                IHOC hoc = An<IHOC>();
                IHOCInsurer hocInsurer = An<IHOCInsurer>();
                IHOCStatus hocStatus = An<IHOCStatus>();

                hocProduct.WhenToldTo(x => x.Key)
                    .Return((int)SAHL.Common.Globals.Products.HomeOwnersCover);

                hocAccount.WhenToldTo(x => x.Product)
                    .Return(hocProduct);

                relatedAccounts.Add(messages, hocAccount);

                financialServices.Add(messages, hocFinancialService);

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

                IHOCRepository hocRepository = An<IHOCRepository>();
                hocRepository.WhenToldTo(x => x.GetHOCByKey(hocFinancialService.Key))
                    .Return(hoc);

                AddRepositoryToMockCache(typeof(IHOCRepository), hocRepository);

                // setup for this specific test
                hocInsurer.WhenToldTo(x => x.Key)
                    .Return((int)SAHL.Common.Globals.HOCInsurers.PaidupwithnoHOC);

                hocStatus.WhenToldTo(x => x.Key)
                    .Return((int)SAHL.Common.Globals.HocStatuses.Open);

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