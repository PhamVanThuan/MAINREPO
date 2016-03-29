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


namespace SAHL.Common.BusinessModel.Specs.Rules.Credit.CreditDisqualificationMinLoanAgreeAmount
{
	[Subject(typeof(SAHL.Common.BusinessModel.Rules.Application.Credit.CreditDisqualificationMinLoanAgreeAmount))]
	public class when_application_loan_amount_is_above_minimum_loan_agreement_amount : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.Credit.CreditDisqualificationMinLoanAgreeAmount>
	{
		protected static ICreditMatrixRepository creditMatrixRepository;
		protected static IApplicationRepository applicationRepository;

		protected static ICreditMatrix creditMatrix;
		protected static double? minimumLoanAmount;

		protected static ISupportsVariableLoanApplicationInformation variableLoanApplicationInformation;
		protected static IApplicationInformationVariableLoan applicationInformationVariableLoan;
		protected static IApplication application;
		protected static IApplicationProductVariableLoan applicationProduct;
		protected static IApplicationType applicationType;

		Establish context = () =>
		{
			minimumLoanAmount = 1000;
			creditMatrixRepository = An<ICreditMatrixRepository>();
			applicationRepository = An<IApplicationRepository>();

			application = An<IApplication>();
			applicationType = An<IApplicationType>();
			applicationProduct = An<IApplicationProductVariableLoan>();
			applicationInformationVariableLoan = An<IApplicationInformationVariableLoan>();

			var creditCriteria = An<ICreditCriteria>();
			creditCriteria.WhenToldTo(x => x.MinLoanAmount).Return(minimumLoanAmount);
			creditCriteria.WhenToldTo(x => x.ExceptionCriteria).Return(true);

			creditMatrix = An<ICreditMatrix>();
			creditMatrix.WhenToldTo(x => x.CreditCriterias).Return(new EventList<ICreditCriteria>(new ICreditCriteria[] { creditCriteria }));

			creditMatrixRepository = An<ICreditMatrixRepository>();
			creditMatrixRepository.WhenToldTo(x => x.GetCreditMatrix(Param.IsAny<OriginationSources>())).Return(creditMatrix);

			applicationType.WhenToldTo(x => x.Key).Return((int)SAHL.Common.Globals.OfferTypes.NewPurchaseLoan);
			application.WhenToldTo(x => x.ApplicationType).Return(applicationType);
			application.WhenToldTo(x => x.CurrentProduct).Return(applicationProduct);
			applicationProduct.WhenToldTo(x => x.Application).Return(application);
			applicationProduct.WhenToldTo(x => x.VariableLoanInformation).Return(applicationInformationVariableLoan);
			applicationInformationVariableLoan.WhenToldTo(x => x.LoanAgreementAmount).Return(minimumLoanAmount + 1);

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
