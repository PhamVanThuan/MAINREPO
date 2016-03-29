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
	public class when_minimum_income_above_threshold_with_exceptions : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Calculator.CalcCreditDisqualificationMinIncome>
	{
		protected static ICreditMatrixRepository creditMatrixRepository;
		protected static ICreditMatrix creditMatrix;
		protected static double? exceptionCriteriaMinimumIncomeAmount;
		protected static double? minimumIncomeAmount;
		protected static double? income;
		Establish context = () =>
		{
			exceptionCriteriaMinimumIncomeAmount = 3000;
			minimumIncomeAmount = 4000;
			income = 3001;

			var minimumIncomeCreditCriteria = An<ICreditCriteria>();
			var minimumIncomeExceptionsCreditCriteria = An<ICreditCriteria>();

			minimumIncomeCreditCriteria.WhenToldTo(x => x.MinIncomeAmount).Return(minimumIncomeAmount);
			minimumIncomeCreditCriteria.WhenToldTo(x => x.ExceptionCriteria).Return(false);

			minimumIncomeExceptionsCreditCriteria.WhenToldTo(x => x.MinIncomeAmount).Return(exceptionCriteriaMinimumIncomeAmount);
			minimumIncomeExceptionsCreditCriteria.WhenToldTo(x => x.ExceptionCriteria).Return(true);

			creditMatrix = An<ICreditMatrix>();
			creditMatrix.WhenToldTo(x => x.CreditCriterias).Return(new EventList<ICreditCriteria>(new ICreditCriteria[] { minimumIncomeCreditCriteria, minimumIncomeExceptionsCreditCriteria }));

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
