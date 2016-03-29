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

namespace SAHL.Common.BusinessModel.Specs.Rules.Calculator.CalcCreditDisqualificationMinIncome
{
	[Subject(typeof(SAHL.Common.BusinessModel.Rules.Calculator.CalcCreditDisqualificationMinIncome))]
	public class when_minimum_income_above_threshold : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Calculator.CalcCreditDisqualificationMinIncome>
	{
		protected static ICreditMatrixRepository creditMatrixRepository;
		protected static ICreditMatrix creditMatrix;
		protected static double? minimumLoanAmount;
		protected static double? income;
		Establish context = () =>
		{
			minimumLoanAmount = 5000;
			income = 5001;

			var creditCriteria = An<ICreditCriteria>();
			creditCriteria.WhenToldTo(x => x.MinIncomeAmount).Return(minimumLoanAmount);

			creditMatrix = An<ICreditMatrix>();
			creditMatrix.WhenToldTo(x => x.CreditCriterias).Return(new EventList<ICreditCriteria>(new ICreditCriteria[] { creditCriteria }));

			creditMatrixRepository = An<ICreditMatrixRepository>();
			creditMatrixRepository.WhenToldTo(x => x.GetCreditMatrix(Param.IsAny<OriginationSources>())).Return(creditMatrix);

			businessRule = new SAHL.Common.BusinessModel.Rules.Calculator.CalcCreditDisqualificationMinIncome(creditMatrixRepository);

			RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Calculator.CalcCreditDisqualificationMinIncome>.startrule.Invoke();
		};

		Because of = () =>
		{
			businessRule.ExecuteRule(messages, income);
		};

		It rule_should_pass = () =>
		{
			messages.Count.ShouldEqual(0);
		};
	}
}
