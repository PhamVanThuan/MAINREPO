using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.BusinessModel.Rules.UnsecuredLending.PersonalLoan;
using Machine.Specifications;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Specs.Rules.UnsecuredLending.ITCUnsecuredLendingSpecs
{
    [Subject(typeof(ITCUnsecuredLending))]
    public class When_legalentity_has_a_valid_ITC_enquiry : RulesBaseWithFakes<ITCUnsecuredLending>
    {
        Establish context = () =>
            {
                var castleTransactionsService = An<SAHL.Common.BusinessModel.Interfaces.Service.ICastleTransactionsService>();
                castleTransactionsService.WhenToldTo(x => x.ExecuteScalarOnCastleTran(Param.IsAny<string>(), Param.IsAny<Databases>(), Param.IsAny<ParameterCollection>())).Return(99);

                var uiStatementService = An<IUIStatementService>();
                uiStatementService.WhenToldTo(x => x.GetStatement(Param.IsAny<string>(), Param.IsAny<string>())).Return("USE [2am]; select * from ITCScore");

                IAccountSequence accountSequence = An<IAccountSequence>();
                accountSequence.WhenToldTo(x => x.Key).Return(1);

                IApplicationUnsecuredLending unsecuredLending = An<IApplicationUnsecuredLending>();
                unsecuredLending.WhenToldTo(x => x.Key).Return(1);
                unsecuredLending.WhenToldTo(x => x.ReservedAccount).Return(accountSequence);

                businessRule = new ITCUnsecuredLending(castleTransactionsService, uiStatementService);

                parameters = new object[]
			        {
				        unsecuredLending
			        };

                RulesBaseWithFakes<ITCUnsecuredLending>.startrule.Invoke();
            };

        Because of = () =>
            {
                businessRule.ExecuteRule(messages, parameters);
            };

        It should_not_return_domain_messages = () =>
            {
                messages.Count.ShouldEqual(0);
            };
    }
}
