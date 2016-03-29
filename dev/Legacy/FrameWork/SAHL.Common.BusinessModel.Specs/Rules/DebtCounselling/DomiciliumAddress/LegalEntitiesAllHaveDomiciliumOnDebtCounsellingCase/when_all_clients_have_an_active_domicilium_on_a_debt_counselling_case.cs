using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;


namespace SAHL.Common.BusinessModel.Specs.Rules.DebtCounselling.DomiciliumAddress.LegalEntitiesAllHaveDomiciliumOnDebtCounsellingCase
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.DebtCounselling.LegalEntitiesAllHaveDomiciliumOnDebtCounsellingCase))]
    public class when_all_clients_have_an_active_domicilium_on_a_debt_counselling_case : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.DebtCounselling.LegalEntitiesAllHaveDomiciliumOnDebtCounsellingCase>
    {
        protected static IDebtCounselling debtCounselling;
        protected static ILegalEntity legalEntity1;
        protected static ILegalEntity legalEntity2;
        protected static ILegalEntityDomicilium legalEntityDomicilium;
        protected static List<ILegalEntity> clients;

        Establish context = () =>
        {
            debtCounselling = An<IDebtCounselling>();
            legalEntityDomicilium = An<ILegalEntityDomicilium>();
            clients = new List<ILegalEntity>();

            // set up 2 clients both with an active domicilium
            legalEntity1 = An<ILegalEntity>();
            legalEntity1.WhenToldTo(x => x.ActiveDomicilium).Return(legalEntityDomicilium);
            clients.Add(legalEntity1);

            legalEntity2 = An<ILegalEntity>();
            legalEntity2.WhenToldTo(x => x.ActiveDomicilium).Return(legalEntityDomicilium);
            clients.Add(legalEntity2);          

            debtCounselling.WhenToldTo(x => x.Clients).Return(clients);

            businessRule = new BusinessModel.Rules.DebtCounselling.LegalEntitiesAllHaveDomiciliumOnDebtCounsellingCase();
            RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.DebtCounselling.LegalEntitiesAllHaveDomiciliumOnDebtCounsellingCase>.startrule.Invoke();
        };

        Because of = () =>
        {
            businessRule.ExecuteRule(messages, debtCounselling);
        };

        It rule_should_pass = () =>
        {
            messages.Count.ShouldEqual(0);
        };
    }
}
