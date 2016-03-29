using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Test;
using System.Data;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord.Queries;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Collections;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class RuleRepositoryTest : TestBase
    {
        private IRuleRepository _repo = RepositoryFactory.GetRepository<IRuleRepository>();

        [NUnit.Framework.Test]
        public void FindRuleItemByNameTest()
        {
            try
            {
                IRuleItem rule = GetARule("select rule from RuleItem_DAO rule");
                IRuleItem returnedRule = _repo.FindRuleItemByName(rule.Name);
                Assert.AreEqual(returnedRule.Name, rule.Name);

                IRuleItem returnedRuleFail = _repo.FindRuleItemByName("NORULEBYTHISNAME");
            }
            catch (Exception ex)
            {
                Assert.IsNotEmpty(ex.Message);
            }
        }

        [NUnit.Framework.Test]
        public void FindRuleItemByTypeNameTest()
        {
            try
            {
                IRuleItem rule = GetARule("select rule from RuleItem_DAO rule");
                IRuleItem returnedRule = _repo.FindRuleItemByTypeName(rule.TypeName);
                Assert.AreEqual(returnedRule.TypeName, rule.TypeName);

                IRuleItem returnedRuleFail = _repo.FindRuleItemByTypeName("NORULEBYTHISNAME");
            }
            catch (Exception ex)
            {
                Assert.IsNotEmpty(ex.Message);
            }
        }

        [NUnit.Framework.Test]
        public void FindRulesByPartialNameTest()
        {
            CurrentPrincipalCache.DomainMessages.Clear();
            IDomainMessageCollection dmc = CurrentPrincipalCache.DomainMessages;

            IRuleItem getrule = GetARule("select rule from RuleItem_DAO rule");
            IEventList<IRuleItem> rules = new EventList<IRuleItem>();
            string subStringName = getrule.Name.Substring(0, 5);

            string sql = String.Format(@"select RuleItemKey from RuleItem where Name like '{0}%'", subStringName);

            DataTable dt = base.GetQueryResults(sql);
            if (dt.Rows.Count == 0)
                Assert.Ignore("No data found");
            foreach (DataRow dr in dt.Rows)
            {
                int RuleItemKey = Convert.ToInt32(dr[0]);
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IRuleItem rule = BMTM.GetMappedType<IRuleItem, RuleItem_DAO>(RuleItem_DAO.Find(RuleItemKey) as RuleItem_DAO);
                rules.Add(dmc, rule);
            }

            IEventList<IRuleItem> returnedRules = _repo.FindRulesByPartialName(subStringName);
            Assert.AreEqual(rules.Count, returnedRules.Count);

        }

        [NUnit.Framework.Test]
        public void GetRuleParameterByRuleKeyTest()
        {
            CurrentPrincipalCache.DomainMessages.Clear();
            IDomainMessageCollection dmc = CurrentPrincipalCache.DomainMessages;
            
            IRuleItem rule = GetARule("select rule from RuleItem_DAO rule where rule.RuleParameters.size > 0");
            IEventList<IRuleParameter> ruleParameters = new EventList<IRuleParameter>();
            
            string sql = String.Format(@"select RuleParameterKey from RuleParameter where RuleItemKey like '%{0}%'", rule.Key);

            DataTable dt = base.GetQueryResults(sql);
            if (dt.Rows.Count == 0)
                Assert.Ignore("No data found");
            foreach (DataRow dr in dt.Rows)
            {
                int RuleParameterKey = Convert.ToInt32(dr[0]);
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IRuleParameter ruleParameter = BMTM.GetMappedType<IRuleParameter, RuleParameter_DAO>(RuleParameter_DAO.Find(RuleParameterKey) as RuleParameter_DAO);
                ruleParameters.Add(dmc, ruleParameter);
            }


            IEventList<IRuleParameter> returnedRuleParameters = _repo.GetRuleParameterByRuleKey(rule.Key);

            Assert.AreEqual(ruleParameters.Count, returnedRuleParameters.Count);

        }

        [NUnit.Framework.Test]
        public void FindRuleByKeyTest()
        {
            IRuleItem rule = GetARule("select rule from RuleItem_DAO rule");
            IRuleItem returnedRule = _repo.FindRuleByKey(rule.Key);
            Assert.AreEqual(returnedRule.Key, rule.Key);

        }

        [Test]
        public void CreateEmptyRuleItemTest()
        {
            IRuleItem rule = _repo.CreateEmptyRuleItem();
            Assert.IsNotNull(rule);
        }

        [NUnit.Framework.Test]
        public void FindRuleParameterByKey()
        {
            IRuleItem rule = GetARule("select rule from RuleItem_DAO rule where rule.RuleParameters.size > 0");
            IRuleParameter returnedRuleParameter = _repo.FindParameterByKey(rule.RuleParameters[0].Key);
            Assert.AreEqual(returnedRuleParameter.Key, rule.RuleParameters[0].Key);

        }

        [Test]
        public void CreateEmptyRuleParameterTest()
        {
            IRuleParameter ruleParameter = _repo.CreateEmptyRuleParameter();
            Assert.IsNotNull(ruleParameter);
        }

        [NUnit.Framework.Test]
        public void GetAllRulesTest()
        {
            SimpleQuery<RuleItem_DAO> q = new SimpleQuery<RuleItem_DAO>("select rule from RuleItem_DAO rule");
            RuleItem_DAO[] res = q.Execute();

            IEventList<IRuleItem> rules = _repo.GetAllRules();

            Assert.AreEqual(rules.Count, res.Length);

        }

        [NUnit.Framework.Test]
        public void GetRuleSetByNameTest()
        {
            SimpleQuery<WorkflowRuleSet_DAO> q = new SimpleQuery<WorkflowRuleSet_DAO>("select ruleSet from WorkflowRuleSet_DAO ruleSet");
            q.SetQueryRange(1);
            WorkflowRuleSet_DAO[] res = q.Execute();
            
            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            IWorkflowRuleSet rule = BMTM.GetMappedType<IWorkflowRuleSet, WorkflowRuleSet_DAO>(WorkflowRuleSet_DAO.Find(res[0].Key) as WorkflowRuleSet_DAO);

            IWorkflowRuleSet ruleSet = _repo.GetRuleSetByName(rule.Name);

            Assert.AreEqual(ruleSet.Key, rule.Key);
        }

        [NUnit.Framework.Test]
        public void GetRuleSetForKeyTest()
        {
            SimpleQuery<WorkflowRuleSet_DAO> q = new SimpleQuery<WorkflowRuleSet_DAO>("select ruleSet from WorkflowRuleSet_DAO ruleSet");
            q.SetQueryRange(1);
            WorkflowRuleSet_DAO[] res = q.Execute();

            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            IWorkflowRuleSet rule = BMTM.GetMappedType<IWorkflowRuleSet, WorkflowRuleSet_DAO>(WorkflowRuleSet_DAO.Find(res[0].Key) as WorkflowRuleSet_DAO);

            IWorkflowRuleSet ruleSet = _repo.GetRuleSetForKey(rule.Key);

            Assert.AreEqual(ruleSet.Name, rule.Name);
        }

        [Test]
        public void GetAllocationMandatesForOrgStructureKeysTest()
        {

            List<int> keys = new List<int>();
            keys.Add(1007);
            IEventList<IAllocationMandateSetGroup> groups = _repo.GetAllocationMandatesForOrgStructureKeys(keys);
            Assert.IsNotNull(groups.Count);
        }

        internal IRuleItem GetARule(string HQL)
        {
            SimpleQuery<RuleItem_DAO> q = new SimpleQuery<RuleItem_DAO>(HQL);
            q.SetQueryRange(1);
            RuleItem_DAO[] res = q.Execute();

            //IApplication app = new SAHL.Common.BusinessModel.Application(res[0]); // there should always be at least one bank account

            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            IRuleItem rule = BMTM.GetMappedType<IRuleItem, RuleItem_DAO>(RuleItem_DAO.Find(res[0].Key) as RuleItem_DAO);
            return rule;
        }
    }
}
