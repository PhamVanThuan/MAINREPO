using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.WorkflowRuleSet_DAO
	/// </summary>
	public partial class WorkflowRuleSet : BusinessModelBase<SAHL.Common.BusinessModel.DAO.WorkflowRuleSet_DAO>, IWorkflowRuleSet
	{
				public WorkflowRuleSet(SAHL.Common.BusinessModel.DAO.WorkflowRuleSet_DAO WorkflowRuleSet) : base(WorkflowRuleSet)
		{
			this._DAO = WorkflowRuleSet;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.WorkflowRuleSet_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.WorkflowRuleSet_DAO.Name
		/// </summary>
		public String Name 
		{
			get { return _DAO.Name; }
			set { _DAO.Name = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.WorkflowRuleSet_DAO.Rules
		/// </summary>
		private DAOEventList<RuleItem_DAO, IRuleItem, RuleItem> _Rules;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.WorkflowRuleSet_DAO.Rules
		/// </summary>
		public IEventList<IRuleItem> Rules
		{
			get
			{
				if (null == _Rules) 
				{
					if(null == _DAO.Rules)
						_DAO.Rules = new List<RuleItem_DAO>();
					_Rules = new DAOEventList<RuleItem_DAO, IRuleItem, RuleItem>(_DAO.Rules);
					_Rules.BeforeAdd += new EventListHandler(OnRules_BeforeAdd);					
					_Rules.BeforeRemove += new EventListHandler(OnRules_BeforeRemove);					
					_Rules.AfterAdd += new EventListHandler(OnRules_AfterAdd);					
					_Rules.AfterRemove += new EventListHandler(OnRules_AfterRemove);					
				}
				return _Rules;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_Rules = null;
			
		}
	}
}


