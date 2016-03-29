using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System.Collections;
using System.Reflection;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework.Config;

namespace SAHL.Common.BusinessModel.Rules
{
    [RuleInfo]
    public class TestRule1 : BusinessRuleBase
    {
        #region IBusinessRule Members

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            int Param1 = (int)Parameters[0];
            IEventList<IRuleParameter> RuleParams = RuleItem.RuleParameters;
            foreach (IRuleParameter Param in RuleParams)
            {
                if ("@Param1" == Param.Name)
                {
                    if (Param1 > Convert.ToInt32(Param.Value))
                    {
                        return 1;
                    }
                    else
                    {
                        AddMessage("Error", string.Format("Param1 must be greater than {0} was {1}", Param.Value, Param1), Messages);

                    }
                }
            }
            AddMessage("Error", "Error Desc", Messages);
            return 1;
        }

        #endregion
    }

    [RuleInfo]
    public class TestRule2 : BusinessRuleBase
    {
        #region IBusinessRule Members

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            int Param1 = (int)Parameters[0];
            IEventList<IRuleParameter> RuleParams = RuleItem.RuleParameters;
            foreach (IRuleParameter Param in RuleParams)
            {
                if ("@Param1" == Param.Name)
                {
                    if (Param1 < Convert.ToInt32(Param.Value))
                    {
                        return 1;
                    }
                    else
                    {
                        AddMessage("Error", string.Format("Param1 must be less than {0} was {1}", Param.Value, Param1), Messages);

                    }
                }
            }
            AddMessage("Error", "Error Desc", Messages);
            return 1;
        }
        #endregion
    }
}