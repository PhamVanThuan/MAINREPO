using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Specs.Rules.Credit.CreditDisqualificationMinValuation
{
	[Subject(typeof(SAHL.Common.BusinessModel.Rules.Application.Credit.CreditDisqualificationMinValuation))]
	public class when_application_has_no_application_information : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.Credit.CreditDisqualificationMinValuation>
	{
		protected static ICreditCriteriaRepository creditCriteriaRepository;
		protected static IApplicationRepository applicationRepository;
		protected static ISupportsVariableLoanApplicationInformation variableLoanApplicationInformation;
		protected static IApplicationInformationVariableLoan applicationInformationVariableLoan;
		protected static IApplication application;
		protected static IApplicationProductVariableLoan applicationProduct;
		protected static IApplicationType applicationType;

		Establish context = () =>
		{
			creditCriteriaRepository = An<ICreditCriteriaRepository>();
			applicationRepository = An<IApplicationRepository>();

			application = An<IApplication>();
			applicationType = An<IApplicationType>();
			applicationProduct = An<IApplicationProductVariableLoan>();
			applicationInformationVariableLoan = An<IApplicationInformationVariableLoan>();

			applicationType.WhenToldTo(x => x.Key).Return((int)SAHL.Common.Globals.OfferTypes.NewPurchaseLoan);
			application.WhenToldTo(x => x.ApplicationType).Return(applicationType);
			application.WhenToldTo(x => x.CurrentProduct).Return(applicationProduct);

			businessRule = new SAHL.Common.BusinessModel.Rules.Application.Credit.CreditDisqualificationMinValuation(creditCriteriaRepository, applicationRepository);
			RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.Credit.CreditDisqualificationMinValuation>.startrule.Invoke();
		};

		Because of = () =>
		{
			businessRule.ExecuteRule(messages, application);
		};

		It rule_should_fail = () =>
		{
			messages.Count.ShouldEqual(1);
		};
	}
}
