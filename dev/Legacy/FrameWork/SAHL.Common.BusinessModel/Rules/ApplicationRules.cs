using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Data;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel.Rules
{
  ///// <summary>
  ///// This rule checks that there is at least ONE Legal Entity linked to an account (Should we check that its a 
  ///// main applicant type)
  ///// </summary>
//  public class MinNumberApplicatsForAccount : IBusinessRule
//  {
//    IAccount Account;
//    public MinNumberApplicatsForAccount()
//    {
//      int DefaultConstructor = 1;
//    }

//    public MinNumberApplicatsForAccount(IAccount Account)
//    {
//      if (null == Account)
//      {
//        throw new ArgumentNullException("IAccount Account", "Expected an IAccount but got null");
//      }
//      this.Account = Account;
//    }

//    public bool ExecuteRule(IDomainMessageCollection Messages, IRuleItem RuleItem)
//    {
//      // find using the property on the rulesset (which is name)

//      IEventList<IRuleParameter> RuleParams = RuleItem.RuleParameters;
//      foreach (IRuleParameter Param in RuleParams)
//      {
//        if ("@NumApplicants" == Param.Name)
//        {
//          // check we have the minium number of applicants for an account
//          if (Account.Roles.Count >= Convert.ToInt32(Param.Value))
//          {
//            // now check we have at least ONE Main Aplicant
//            IEventList<IRole> roles = Account.Roles;
//            foreach (IRole role in roles)
//            {
//              if (role.RoleType.Key == 2)// main applicant
//              {
//                // we have a main applicant so all is good.
//                return true;
//              }
//            }
//            Messages.Add(new Error("Must be at least one Main Applicant Role", "Bla"));
//          }
//          else
//          {
//            Messages.Add(new Error("Must be at least one applicant per account", "Bla"));
//          }
//        }
//      }
//      return false;
//    }
   
//}
  //public IRuleFailure RapidLessThan80Percent(int OfferKey)
  //{
  //  TransactionContext ctx = null;
  //  IRuleFailure Retval = null;
  //  try
  //  {
  //    // Get the rule Params
  //    SAHL.Common.Datasets.Rules.RuleItemRow[] drc = RulesProcessor.dsRules.RuleItem.Select("Name='RapidLessThan80Percent'") as SAHL.Common.Datasets.Rules.RuleItemRow[];
  //    SAHL.Common.Datasets.Rules.RuleParameterRow[] drcParam = drc[0].GetRuleParameterRows();
  //    // Execute the rule
  //    ctx = TransactionController.GetContext(false, new Metrics());
  //    bool Ret = FurtherLoanWorker.RapidLessThen80Percent(ctx, OfferKey, drcParam);
  //    if (!Ret)
  //    {
  //      Retval = new ConcreteRuleFailure(0, "PTI > 80%");
  //    }
  //    TransactionController.Commit(ctx);
  //    return Retval;
  //  }
  //  catch (Exception ex)
  //  {
  //    return new ConcreteRuleFailure(-1, ex.ToString());
  //  }
  //  finally
  //  {
  //    if (null != ctx)
  //    {
  //      ctx.DisposeContext();
  //    }
  //  }
  //}
}
