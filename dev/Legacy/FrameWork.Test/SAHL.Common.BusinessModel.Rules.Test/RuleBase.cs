using Castle.ActiveRecord;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Rules;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Test;
using System;

namespace SAHL.Common.BusinessModel.Rules.Test
{
    public class RuleBase : TestBase
    {
        protected IRuleRepository RuleRepo = null;
        protected IDomainMessageCollection Messages = null;
        protected IRuleService _ruleService;

        /// <summary>
        /// Gets the RuleService to use for executing rules.
        /// </summary>
        protected IRuleService RuleService
        {
            get
            {
                return _ruleService;
            }
        }

        /// <summary>
        /// Executes a rule using <see cref="RuleService"/>, and asserts that the <c>expectedMessageCount</c> is
        /// matched in the domain message collection object.  This will clear the messages collection and mock
        /// repository after it is called.
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="expectedMessageCount"></param>
        /// <param name="ruleParams"></param>
        protected void ExecuteRule(BusinessRuleBase rule, int expectedMessageCount, params object[] ruleParams)
        {
            ExecuteRule(rule.GetType().Name, expectedMessageCount, ruleParams);
        }

        /// <summary>
        /// Executes a rule using <see cref="RuleService"/>, and asserts that the <c>expectedMessageCount</c> is
        /// matched in the domain message collection object.  This will clear the messages collection and mock
        /// repository after it is called.
        /// </summary>
        /// <param name="ruleName"></param>
        /// <param name="expectedMessageCount"></param>
        /// <param name="ruleParams"></param>
        protected void ExecuteRule(string ruleName, int expectedMessageCount, params object[] ruleParams)
        {
            if (RuleService == null)
                throw new NullReferenceException("RuleService is null - ensure base.Setup() has been called.");

            // make sure the rule hasn't been deactivated, otherwise we need to skip the test - note that if
            // a mock strategy is being used we can't use the lookUp repository
            IRuleItem rule = null;
            if (GetRepositoryStrategy() == TypeFactoryStrategy.Mock)
            {
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                RuleItem_DAO[] daoRules = RuleItem_DAO.FindAllByProperty("Name", ruleName);
                if (daoRules.Length > 0)
                    rule = BMTM.GetMappedType<RuleItem>(daoRules[0]);
            }
            else
            {
                if (LookupRepository.RuleItemsByName.ContainsKey(ruleName))
                    rule = LookupRepository.RuleItemsByName[ruleName];
            }

            if (rule == null)
                throw new Exception(String.Format("Rule {0} does not exist in the database", ruleName));

            if (rule.GeneralStatus.Key == (int)GeneralStatuses.Inactive)
            {
                Assert.Ignore("Rule {0} not run as inactive", ruleName);
            }
            else
            {
                _mockery.ReplayAll();
                using (new SessionScope())
                {
                    RuleService.ExecuteRule(Messages, ruleName, ruleParams);
                }
                //string prms = String.Empty;
                //foreach (var item in ruleParams)
                //{
                //    prms += item.ToString() + ", ";
                //}

                //Assert.AreEqual(expectedMessageCount, Messages.Count, String.Format(@"Rule {0} failed enabled = {1}, message count = {2}, parameters = {3}", ruleName, _ruleService.Enabled, Messages.Count, prms));
                Assert.AreEqual(expectedMessageCount, Messages.Count, String.Format(@"Rule {0} failed enabled = {1}, message count = {2}", ruleName, _ruleService.Enabled, Messages.Count));
            }
            Messages.Clear();
            _mockery.VerifyAll();
            _mockery.BackToRecordAll();
        }

        public virtual void Setup()
        {
            // set the strategy to default so we actually go to the database
            SetRepositoryStrategy(TypeFactoryStrategy.Default);
            Messages = new DomainMessageCollection();
            _ruleService = ServiceFactory.GetService<IRuleService>();
            _ruleService.Enabled = true;
            _mockery = new MockRepository();
        }

        public virtual void TearDown()
        {
            MockCache.Flush();
            // GC.Collect();
        }
    }
}