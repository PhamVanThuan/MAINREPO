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

namespace SAHL.Common.BusinessModel.Specs.Rules.CalculatorValidation.CalcCreditDisqualificationMinLAA
{
	[Subject(typeof(SAHL.Common.BusinessModel.Rules.Calculator.CalcCreditDisqualificationMinLAA))]
	public class when_loan_amount_is_above_minimum_loan_amount : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Calculator.CalcCreditDisqualificationMinLAA>
	{
		protected static ICreditMatrixRepository creditMatrixRepository;
		protected static IApplicationRepository applicationRepository;

		protected static ICreditMatrix creditMatrix;
		protected static double? minimumLoanAmount;
		protected static double amount;

		Establish context = () =>
		{
			minimumLoanAmount = 1000;
			amount = minimumLoanAmount.Value + 1;

			var creditCriteria = An<ICreditCriteria>();
			creditCriteria.WhenToldTo(x => x.MinLoanAmount).Return(minimumLoanAmount);
			creditCriteria.WhenToldTo(x => x.ExceptionCriteria).Return(true);

			creditMatrix = An<ICreditMatrix>();
			creditMatrix.WhenToldTo(x => x.CreditCriterias).Return(new EventList<ICreditCriteria>(new ICreditCriteria[] { creditCriteria }));

			creditMatrixRepository = An<ICreditMatrixRepository>();
			creditMatrixRepository.WhenToldTo(x => x.GetCreditMatrix(Param.IsAny<OriginationSources>())).Return(creditMatrix);

			businessRule = new SAHL.Common.BusinessModel.Rules.Calculator.CalcCreditDisqualificationMinLAA(creditMatrixRepository);
			RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Calculator.CalcCreditDisqualificationMinLAA>.startrule.Invoke();
		};

		Because of = () =>
		{
			businessRule.ExecuteRule(messages, amount);
		};

		It rule_should_pass = () =>
		{
			messages.Count.ShouldEqual(0);
		};
	}
}
