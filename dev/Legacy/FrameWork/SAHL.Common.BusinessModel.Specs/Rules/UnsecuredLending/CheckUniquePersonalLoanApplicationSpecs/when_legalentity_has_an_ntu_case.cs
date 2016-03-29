using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Rules.UnsecuredLending.PersonalLoan;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel.Specs.Rules.UnsecuredLending.CheckUniquePersonalLoanApplicationSpecs
{
    [Subject(typeof(CheckUniquePersonalLoanApplication))]
    public class when_legalentity_has_an_ntu_case : RulesBaseWithFakes<CheckUniquePersonalLoanApplication>
    {
        Establish context = () =>
        {
            var castleTransactionsService = An<SAHL.Common.BusinessModel.Interfaces.Service.ICastleTransactionsService>();
            castleTransactionsService.WhenToldTo(x => x.Many<SAHL.Common.BusinessModel.Interfaces.IExternalRole>(Param.IsAny<Globals.QueryLanguages>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<Globals.Databases>(), Param.IsAny<object[]>())).Return(new List<SAHL.Common.BusinessModel.Interfaces.IExternalRole>() { });

            businessRule = new CheckUniquePersonalLoanApplication(castleTransactionsService);
            parameters = new object[] { Param.IsAny<int>() }; //legal entity key
            RulesBaseWithFakes<CheckUniquePersonalLoanApplication>.startrule.Invoke();
        };

        Because of = () =>
        {
            businessRule.ExecuteRule(messages, parameters);
        };

        It rule_should_not_fail = () =>
        {
            messages.Count.ShouldEqual(0);
        };
    }
}
