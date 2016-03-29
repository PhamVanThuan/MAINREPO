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

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.WorkflowRuleSet_DAO.Rules
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnRules_BeforeAdd(ICancelDomainArgs args, object Item)
        {

        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.WorkflowRuleSet_DAO.Rules
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnRules_BeforeRemove(ICancelDomainArgs args, object Item)
        {

        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.WorkflowRuleSet_DAO.Rules
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnRules_AfterAdd(ICancelDomainArgs args, object Item)
        {

        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.WorkflowRuleSet_DAO.Rules
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnRules_AfterRemove(ICancelDomainArgs args, object Item)
        {

        }

        public List<string> RulesToRun
        {
            get
            {
                List<string> R2R = new List<string>();
                foreach (IRuleItem r in this.Rules)
                {
                    R2R.Add(r.Name);
                }
                return R2R;
            }
        }

        public IList<int> RuleKeys
        {
            get
            {
                List<int> Keys = new List<int>();
                foreach (IRuleItem r in this.Rules)
                {
                    Keys.Add(r.Key);
                }
                return Keys;
            }
        }
    }
}


