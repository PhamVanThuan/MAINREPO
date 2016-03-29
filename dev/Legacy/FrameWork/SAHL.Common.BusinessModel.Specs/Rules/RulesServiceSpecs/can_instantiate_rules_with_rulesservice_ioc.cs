using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Fakes;
using Machine.Specifications;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces.Configuration;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Service;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel.Specs.RulesServiceSpecs
{
	[TestFixture]
	public class can_instantiate_rules_with_rulesservice_ioc : WithFakes
	{
		[Test, TestCaseSource(typeof(RulesSource), "GetRules")]
		public void when_given_rule(Type rule)
		{
			var rulesServiceConfigurationProvider = An<IRulesServiceConfigurationProvider>();
			var lookupRepository = An<ILookupRepository>();
			var ruleRepository = An<IRuleRepository>();
			var castleTransactionsService = An<ICastleTransactionsService>();
			var creditCriteriaUnsecuredLendingRepository = An<ICreditCriteriaUnsecuredLendingRepository>();
			var uiStatementService = An<UIStatementService>();
			var commonRepository = An<ICommonRepository>();
			var applicationRepository = An<IApplicationRepository>();
			var legalEntityRepository = An<ILegalEntityRepository>();
			var creditMatrixRepository = An<ICreditMatrixRepository>();
			var propertyRepository = An<IPropertyRepository>();
			var reasonRepository = An<IReasonRepository>();
			var stageDefinitionRepository = An<IStageDefinitionRepository>();
			var accountRepository = An<IAccountRepository>();
			var creditCriteriaRepository = An<ICreditCriteriaRepository>();
            var debtCounsellingRepository = An<IDebtCounsellingRepository>();
            var applicationUnsecuredLendingRepository = An<IApplicationUnsecuredLendingRepository>();
            var x2Repo = An<IX2Repository>();
            IRuleService ruleService = new RuleService(rulesServiceConfigurationProvider, lookupRepository, ruleRepository, castleTransactionsService, creditCriteriaUnsecuredLendingRepository, uiStatementService, commonRepository, applicationRepository, legalEntityRepository, creditMatrixRepository, reasonRepository, propertyRepository, stageDefinitionRepository, accountRepository, creditCriteriaRepository, debtCounsellingRepository, x2Repo);
			object createdRule = null;
			try
			{
				createdRule = ruleService.CreateRule(rule);
			}
			catch (Exception ex)
			{
				Assert.Fail(String.Format("Failed to Create Rule {0} with error : {1}", rule.Name, ex.Message));
			}
			if (createdRule == null)
			{
				Assert.Fail(String.Format("Could not Create Rule {0} from the Rules Service", rule.GetType().Name));
			}
		}

		public class RulesSource
		{
			public static IEnumerable GetRules()
			{
				//get rules
				var ruleAssembly = System.Reflection.Assembly.GetAssembly(typeof(SAHL.Common.BusinessModel.Rules.Application.Credit.CreditDisqualificationMaxLTV));
				var rules = GetAllRules(ruleAssembly, typeof(IBusinessRule)); ;
				return rules;
			}
		}

		private static IList<Type> GetAllRules(System.Reflection.Assembly assembly, Type interfaceType)
		{
			return assembly.GetTypes().Where(x =>
								!x.IsInterface &&
								!x.IsGenericType &&
								!x.IsAbstract &&
								x.GetInterfaces().Where(i => i.Name == interfaceType.Name).Count() > 0
							  ).ToList();
		}
	}
}