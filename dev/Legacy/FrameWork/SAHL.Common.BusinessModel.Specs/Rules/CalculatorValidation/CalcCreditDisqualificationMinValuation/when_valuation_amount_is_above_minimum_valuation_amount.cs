using System.Text;
using Machine.Specifications;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Specs.Rules.CalculatorValidation.CalcCreditDisqualificationMinValuation
{
	[Subject(typeof(SAHL.Common.BusinessModel.Rules.Application.Credit.CreditDisqualificationMinValuation))]
	public class when_valuation_amount_is_above_minimum_valuation_amount : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Calculator.CalcCreditDisqualificationMinValuation>
	{
		protected static ICreditMatrixRepository creditMatrixRepository;
		protected static IApplicationRepository applicationRepository;

		protected static ICreditMatrix creditMatrix;
		protected static double? minimumValuationAmount;
		protected static double valuationAmount;
		Establish context = () =>
		{
			minimumValuationAmount = 1000;
			valuationAmount = minimumValuationAmount.Value + 1;
			creditMatrixRepository = An<ICreditMatrixRepository>();
			applicationRepository = An<IApplicationRepository>();

			var creditCriteria = An<ICreditCriteria>();
			creditCriteria.WhenToldTo(x => x.MinPropertyValue).Return(minimumValuationAmount);
			creditCriteria.WhenToldTo(x => x.ExceptionCriteria).Return(true);

			creditMatrix = An<ICreditMatrix>();
			creditMatrix.WhenToldTo(x => x.CreditCriterias).Return(new EventList<ICreditCriteria>(new ICreditCriteria[] { creditCriteria }));

			creditMatrixRepository = An<ICreditMatrixRepository>();
			creditMatrixRepository.WhenToldTo(x => x.GetCreditMatrix(Param.IsAny<OriginationSources>())).Return(creditMatrix);

			businessRule = new SAHL.Common.BusinessModel.Rules.Calculator.CalcCreditDisqualificationMinValuation(creditMatrixRepository);
			RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Calculator.CalcCreditDisqualificationMinValuation>.startrule.Invoke();
		};

		Because of = () =>
		{
			businessRule.ExecuteRule(messages, valuationAmount);
		};

		It rule_should_pass = () =>
		{
			messages.Count.ShouldEqual(0);
		};
	}
}
