using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Fakes;
using Machine.Specifications;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Service.Interfaces;
using SAHL.Test.Strategies;

namespace SAHL.Common.BusinessModel.Specs.Rules
{
    public class RulesBaseWithFakes<T> : WithFakes where T : IBusinessRule
    {
        protected static object[] parameters;
        public static IRuleItem ruleItem;
        protected static IDomainMessageCollection messages;
        protected static T businessRule;
        protected static int RuleResult;

        public RulesBaseWithFakes()
        {
            messages = new DomainMessageCollection();
            ruleItem = An<IRuleItem>();

            TypeFactory.SetStrategy(new MockStrategy());
        }

        public static Establish startrule = () =>
        {
            businessRule.StartRule(messages, ruleItem);
        };

        public static void AddRepositoryToMockCache(Type repositoryType, object repositoryObject)
        {
            CacheManager CM = CacheFactory.GetCacheManager("MOCK");
            CM.Add(repositoryType.ToString(), repositoryObject);
        }
    }
}