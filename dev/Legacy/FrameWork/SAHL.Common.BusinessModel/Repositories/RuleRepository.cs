using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.DAO;

using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using System.Reflection;
using SAHL.Common.Configuration;
using System.Configuration;
using NHibernate;
using Castle.ActiveRecord;
using SAHL.Common.Security;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Exceptions;
using SAHL.Common.CacheData;

namespace SAHL.Common.BusinessModel.Repositories
{

	[FactoryType(typeof(IRuleRepository), LifeTime = FactoryTypeLifeTime.Singleton)]
	public class RuleRepository : AbstractRepositoryBase, IRuleRepository
	{
		#region IRuleRepository Members

		public IRuleItem FindRuleItemByName(string RuleName)
		{
			RuleItem_DAO[] item = RuleItem_DAO.FindAllByProperty("Name", RuleName);
			if (item == null || item.Length == 0)
			{
				throw new Exception(string.Format("Rule named \"{0}\" not found in the database.", RuleName));
			}
			IRuleItem ruleItem = new RuleItem(item[0]);
			return ruleItem;
		}
		public IRuleItem FindRuleItemByTypeName(string RuleName)
		{
			SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
			RuleItem_DAO[] item = RuleItem_DAO.FindAllByProperty("TypeName", RuleName);
			if (item == null || item.Length == 0)
			{
				throw new Exception(string.Format("Rule of type \"{0}\" not found in the database.", RuleName));
			}
			IRuleItem ruleItem = new RuleItem(item[0]);
			return ruleItem;
		}
		/// <summary>
		/// <see href="IRuleRepository.FindRulesByPartialName">IRuleRepository.FindRulesByPartialName</see>
		/// </summary>
		/// <param name="PartialRuleName"></param>
		/// <returns></returns>
		public IEventList<IRuleItem> FindRulesByPartialName(string PartialRuleName)
		{
			string hql = string.Format("select r from RuleItem_DAO r where r.Name like '{0}%' order by r.Name", PartialRuleName);
			SimpleQuery query = new SimpleQuery(typeof(RuleItem_DAO), hql);
			//object o = RuleItem_DAO.ExecuteQuery(query);
			//List<RuleItem_DAO> Rules = o as List<RuleItem_DAO>;
			//return new DAOEventList<RuleItem_DAO, IRuleItem, RuleItem>(Rules);
			//RuleNameQuery query = new RuleNameQuery(PartialRuleName);
			object o = RuleItem_DAO.ExecuteQuery(query);
			RuleItem_DAO[] Rules = o as RuleItem_DAO[];
			List<RuleItem_DAO> rules = new List<RuleItem_DAO>();
			for (int i = 0; i < Rules.Length; i++)
			{
				rules.Add(Rules[i]);
			}
			return new DAOEventList<RuleItem_DAO, IRuleItem, RuleItem>(rules);
		}

		/// <summary>
		/// <see href="IRuleRepository.GetRuleParameterByRuleKey">GetRuleParameterByRuleKey</see>
		/// </summary>
		/// <param name="RuleItemKey"></param>
		/// <returns></returns>
		public IEventList<IRuleParameter> GetRuleParameterByRuleKey(int RuleItemKey)
		{
			string hql = string.Format("select r from RuleParameter_DAO r where r.RuleItem.Key ={0}", RuleItemKey);
			SimpleQuery query = new SimpleQuery(typeof(RuleParameter_DAO), hql);
			object o = RuleParameter_DAO.ExecuteQuery(query);
			RuleParameter_DAO[] param = o as RuleParameter_DAO[];
			List<RuleParameter_DAO> Param = new List<RuleParameter_DAO>();
			for (int i = 0; i < param.Length; i++)
			{
				Param.Add(param[i]);
			}
			return new DAOEventList<RuleParameter_DAO, IRuleParameter, RuleParameter>(Param);
		}

		/// <summary>
		/// See <see href="IRuleRepository.FindRuleByKey">IRuleRepository.FindRuleByKey</see>
		/// </summary>
		/// <param name="Key"></param>
		/// <returns></returns>
		public IRuleItem FindRuleByKey(int Key)
		{
			return base.GetByKey<IRuleItem, RuleItem_DAO>(Key);
		}

		public IRuleItem CreateEmptyRuleItem()
		{
			return base.CreateEmpty<IRuleItem, RuleItem_DAO>();
		}

		public void SaveRuleItem(IRuleItem RuleToSave)
		{
			base.Save<IRuleItem, RuleItem_DAO>(RuleToSave);
		}

		public IRuleParameter FindParameterByKey(int Key)
		{
			return base.GetByKey<IRuleParameter, RuleParameter_DAO>(Key);
		}

		public IRuleParameter CreateEmptyRuleParameter()
		{
			return base.CreateEmpty<IRuleParameter, RuleParameter_DAO>();
		}

		public void SaveRuleParameter(IRuleParameter Param)
		{
			base.Save<IRuleParameter, RuleParameter_DAO>(Param);
		}

		//internal class RuleNameQuery : ActiveRecordBaseQuery
		//{
		//    string _RuleName;
		//    public RuleNameQuery(string RuleName)
		//        : base(typeof(RuleItem_DAO))
		//    {
		//        _RuleName = RuleName;
		//    }

		//    protected override NHibernate.IQuery CreateQuery(NHibernate.ISession session)
		//    {
		//        string HQL = string.Format("from RuleItem_DAO obj where obj.Name like :ruleName");
		//        IQuery q = session.CreateQuery(HQL);
		//        q.SetString("ruleName", this._RuleName);
		//        return q;
		//    }

		//    protected override object InternalExecute(ISession session)
		//    {
		//        ICriteria Criteria = session.CreateCriteria(typeof(RuleItem_DAO));
		//        Criteria.SetMaxResults(25);
		//        IQuery query = CreateQuery(session);
		//        return query.List<RuleItem_DAO>();
		//    }
		//}

		public IEventList<IRuleItem> GetAllRules()
		{
			RuleItem_DAO[] o = RuleItem_DAO.FindAll() as RuleItem_DAO[];
			return new DAOEventList<RuleItem_DAO, IRuleItem, RuleItem>(o);
		}

		public IEventList<IAllocationMandateSetGroup> GetAllocationMandatesForOrgStructureKeys(List<int> Keys)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < Keys.Count; i++)
			{
				sb.AppendFormat(",{0}", Keys[i]);
			}
			sb.Remove(0, 1);
			string HQL = string.Format("from AllocationMandateSetGroup_DAO d where d.OrganisationStructure.Key in ({0})", sb.ToString());
			SimpleQuery<AllocationMandateSetGroup_DAO> query = new SimpleQuery<AllocationMandateSetGroup_DAO>(HQL);
			AllocationMandateSetGroup_DAO[] arr = query.Execute();
			return new DAOEventList<AllocationMandateSetGroup_DAO, IAllocationMandateSetGroup, AllocationMandateSetGroup>(arr);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Name"></param>
		/// <returns></returns>
		public IWorkflowRuleSet GetRuleSetByName(string Name)
		{
			WorkflowRuleSet_DAO[] item = WorkflowRuleSet_DAO.FindAllByProperty("Name", Name);
			if (item == null || item.Length == 0)
			{
				throw new Exception(string.Format("WorkflowRuleSet named \"{0}\" not found in the database.", Name));
			}
			IWorkflowRuleSet Item = new WorkflowRuleSet(item[0]);
			return Item;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="RuleSetKey"></param>
		/// <returns></returns>
		public IWorkflowRuleSet GetRuleSetForKey(int RuleSetKey)
		{
			WorkflowRuleSet_DAO dao = WorkflowRuleSet_DAO.Find(RuleSetKey);
			return new WorkflowRuleSet(dao);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="WFRuleSet"></param>
		public void SaveRuleSet(IWorkflowRuleSet WFRuleSet)
		{
			base.Save<IWorkflowRuleSet, WorkflowRuleSet_DAO>(WFRuleSet);
		}

		#endregion

	}
}
