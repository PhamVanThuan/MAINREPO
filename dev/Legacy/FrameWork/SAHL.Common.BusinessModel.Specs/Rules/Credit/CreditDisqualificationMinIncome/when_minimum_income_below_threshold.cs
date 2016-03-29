using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Rules.Calculator;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Specs.Rules.Application.Credit.CreditDisqualificationMinIncome
{
	[Subject(typeof(SAHL.Common.BusinessModel.Rules.Application.Credit.CreditDisqualificationMinIncome))]
	public class when_minimum_income_below_threshold : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.Credit.CreditDisqualificationMinIncome>
	{
		protected static ICreditMatrixRepository creditMatrixRepository;
		protected static ICreditMatrix creditMatrix;
		protected static double? minimumLoanAmount;
		protected static double? income;
		protected static IApplication application;
		protected static IApplicationProductVariableLoan applicationProduct;
		protected static IApplicationInformationVariableLoan applicationInformationVariableLoan;
		Establish context = () =>
		{
			minimumLoanAmount = 5000;
			income = 4999;

			application = An<IApplication>();
			applicationProduct = An<IApplicationProductVariableLoan>();
			applicationInformationVariableLoan = An<IApplicationInformationVariableLoan>();

			application.WhenToldTo(x => x.CurrentProduct).Return(applicationProduct);
			applicationProduct.WhenToldTo(x => x.Application).Return(application);
			applicationProduct.WhenToldTo(x => x.VariableLoanInformation).Return(applicationInformationVariableLoan);
			applicationInformationVariableLoan.WhenToldTo(x => x.HouseholdIncome).Return(income);
				
			var creditCriteria = An<ICreditCriteria>();
			creditCriteria.WhenToldTo(x => x.MinIncomeAmount).Return(minimumLoanAmount);
			creditCriteria.WhenToldTo(x => x.ExceptionCriteria).Return(false);

			creditMatrix = An<ICreditMatrix>();
			creditMatrix.WhenToldTo(x => x.CreditCriterias).Return(new EventList<ICreditCriteria>(new ICreditCriteria[] { creditCriteria }));

			creditMatrixRepository = An<ICreditMatrixRepository>();
			creditMatrixRepository.WhenToldTo(x => x.GetCreditMatrix(Param.IsAny<OriginationSources>())).Return(creditMatrix);

			businessRule = new SAHL.Common.BusinessModel.Rules.Application.Credit.CreditDisqualificationMinIncome(creditMatrixRepository);

			RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.Credit.CreditDisqualificationMinIncome>.startrule.Invoke();
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
