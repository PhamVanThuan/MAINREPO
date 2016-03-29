using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace SAHL.Common.BusinessModel.Specs.Rules.Credit.CreditDisqualificationMinLoanAgreeAmount
{
	[Subject(typeof(SAHL.Common.BusinessModel.Rules.Application.Credit.CreditDisqualificationMinLoanAgreeAmount))]
	public class when_application_is_further_lending : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.Credit.CreditDisqualificationMinLoanAgreeAmount>
	{
		protected static ICreditMatrixRepository creditMatrixRepository;
		protected static IApplicationRepository applicationRepository;
		protected static ISupportsVariableLoanApplicationInformation variableLoanApplicationInformation;
		protected static IApplicationInformationVariableLoan applicationInformationVariableLoan;
		protected static IApplication application;
		protected static IApplicationProductVariableLoan applicationProduct;
		protected static IApplicationType applicationType;

		Establish context = () =>
		{
			creditMatrixRepository = An<ICreditMatrixRepository>();
			applicationRepository = An<IApplicationRepository>();

			application = An<IApplication>();
			applicationType = An<IApplicationType>();
			applicationProduct = An<IApplicationProductVariableLoan>();
			applicationInformationVariableLoan = An<IApplicationInformationVariableLoan>();

			applicationType.WhenToldTo(x => x.Key).Return((int)SAHL.Common.Globals.OfferTypes.FurtherAdvance);
			application.WhenToldTo(x => x.ApplicationType).Return(applicationType);
			application.WhenToldTo(x => x.CurrentProduct).Return(applicationProduct);
			applicationProduct.WhenToldTo(x => x.Application).Return(application);
			applicationProduct.WhenToldTo(x => x.VariableLoanInformation).Return(applicationInformationVariableLoan);

			businessRule = new SAHL.Common.BusinessModel.Rules.Application.Credit.CreditDisqualificationMinLoanAgreeAmount(creditMatrixRepository, applicationRepository);
			RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.Credit.CreditDisqualificationMinLoanAgreeAmount>.startrule.Invoke();
		};

		Because of = () =>
		{
			businessRule.ExecuteRule(messages, application);
		};

		It rule_should_pass = () =>
		{
			messages.Count.ShouldEqual(0);
		};
	}
}
