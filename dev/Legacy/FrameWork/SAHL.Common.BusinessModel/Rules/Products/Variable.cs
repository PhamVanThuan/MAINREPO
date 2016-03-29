using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Rules.Products
{
    //[RuleInfo]
    //public class VariableNoExtendedTerm : BusinessRuleBase
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="Messages"></param>
    //    /// <param name="RuleName"></param>
    //    /// <param name="IsError"></param>
    //    /// <param name="Parameters"></param>
    //    /// <returns></returns>
    //    public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
    //    {
    //        if (!(Parameters[0] is IMortgageLoan))
    //            throw new ArgumentException("Parameter[0] is not of type IMortgageLoan.");

    //        IMortgageLoan ml = (IMortgageLoan)Parameters[0];

    //        if (ml.Account.Product.Key == (int)SAHL.Common.Globals.Products.VariableLoan && ml.InitialInstallments > 240)
    //            AddMessage("The Variable product can not have a term over 20 years.", "The Variable product can not have a term over 20 years.", Messages);

    //        return 0;
    //    }
    //}
}
