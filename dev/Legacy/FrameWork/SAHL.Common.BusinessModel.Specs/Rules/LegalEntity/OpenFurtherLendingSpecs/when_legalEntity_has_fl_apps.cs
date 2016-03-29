using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Rules.Disbursement;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Rules.LegalEntity;


namespace SAHL.Common.BusinessModel.Specs.Rules.LegalEntity.OpenFurtherLendingSpecs
{
    [Subject(typeof(LegalEntityOpenFurtherLending))]
    public class when_legalEntity_has_fl_apps : RulesBaseWithFakes<LegalEntityOpenFurtherLending>
    {
        Establish context = () =>
        {
            ILegalEntity legalEntity = An<ILegalEntity>();
            ILegalEntityRepository legalEntityRepository = An<ILegalEntityRepository>();

            IList<IApplication> furtherLendingCases = new List<IApplication>();
            IList<IApplication> apps = new List<IApplication>();

            IAccount account = An<IAccount>();
            IApplication app = An<IApplication>();
            app.WhenToldTo(x => x.ApplicationType.Key).Return(4);
            app.WhenToldTo(x => x.Key).Return(104);

            account.WhenToldTo(x => x.Key).Return(1);
            app.WhenToldTo(x => x.Account).Return(account);
            apps.Add(app);

            IApplication app2 = An<IApplication>();
            app2.WhenToldTo(x => x.ApplicationType.Key).Return(4);
            app2.WhenToldTo(x => x.Key).Return(105);
            app2.WhenToldTo(x => x.Account).Return(account);           
            apps.Add(app2);

            furtherLendingCases.Add(app);
            furtherLendingCases.Add(app2);

            var readonlyFurtherLendingCases = new ReadOnlyEventList<IApplication>(furtherLendingCases);
            legalEntityRepository.WhenToldTo(x => x.GetOpenFurtherLendingApplicationsByLegalEntity(legalEntity)).Return(readonlyFurtherLendingCases);

            businessRule = new LegalEntityOpenFurtherLending(legalEntityRepository);
            parameters = new object[] { legalEntity };
            RulesBaseWithFakes<LegalEntityOpenFurtherLending>.startrule.Invoke();

        };
        Because of = () =>
        {
            businessRule.ExecuteRule(messages, parameters);
        };

        It rule_should_pass = () =>
        {
            messages.Count.ShouldEqual(2);
        };

        }
}
