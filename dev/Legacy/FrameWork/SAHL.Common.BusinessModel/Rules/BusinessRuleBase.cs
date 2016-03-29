using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework.Config;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel.Rules
{
    public abstract class BusinessRuleBase : IBusinessRule
    {
        static BusinessRuleBase()
        {
            return;
        }

        private IRuleItem _ruleItem;

        // protected TransactionScope _scope;

        /// <summary>
        /// Adds a domain message to the DomainMessageCollection for this RuleItem. If the rule is set to not be enforced it will
        /// be adding a warning message to the DomainMessageCollection, otherwise it will add an error message.
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Description"></param>
        /// <param name="Messages"></param>
        protected void AddMessage(string Message, string Description, IDomainMessageCollection Messages)
        {
            if (RuleItem.EnforceRule)
                Messages.Add(new Error(Message, Description));
            else
                Messages.Add(new Warning(Message, Description));
        }

        /// <summary>
        /// Gets the IRuleItem object associated with the rule.
        /// </summary>
        protected IRuleItem RuleItem
        {
            get
            {
                if (_ruleItem == null)
                    throw new NullReferenceException("RuleItem is null - ensure StartRule is being called.");

                return _ruleItem;
            }
        }

        #region IBusinessRule Members

        public void CompleteRule()
        {
            //_scope.VoteCommit();
        }

        public abstract int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters);

        public bool StartRule(IDomainMessageCollection Messages, IRuleItem ruleItem)
        {
            _ruleItem = ruleItem;
            return true;
        }

        #endregion IBusinessRule Members
    }
}