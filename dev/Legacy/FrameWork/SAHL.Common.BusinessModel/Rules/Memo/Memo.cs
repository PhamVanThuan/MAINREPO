using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord.Queries;

namespace SAHL.Common.BusinessModel.Rules.Memo
{
    [RuleDBTag("MemoAddUpdateMemoReminderDateMandatory",
    "Reminder Date must be entered",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Memo.MemoAddUpdateMemoReminderDateMandatory")]
    [RuleInfo]
    public class MemoAddUpdateMemoReminderDateMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IMemo memo = (IMemo)Parameters[0];

            if (!memo.ReminderDate.HasValue)
            {
                string msg = "Memo must have a valid Reminder Date";
                AddMessage(msg, msg, Messages);
            }
            return 1;
        }
    }

    [RuleDBTag("MemoAddUpdateMemoExpiryDateMandatory",
    "Expiry Date must be entered",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Memo.MemoAddUpdateMemoExpiryDateMandatory")]
    [RuleInfo]
    public class MemoAddUpdateMemoExpiryDateMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IMemo memo = (IMemo)Parameters[0];

            if (!memo.ExpiryDate.HasValue)
            {
                string msg = "Memo must have a valid Expiry Date";
                AddMessage(msg, msg, Messages);
            }
            return 1;
        }
    }


    [RuleDBTag("MemoAddUpdateMemoReminderDate",
    "Reminder Date can not be greater than Expiry Date",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Memo.MemoAddUpdateMemoReminderDate")]
    [RuleInfo]
    public class MemoAddUpdateMemoReminderDate : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IMemo memo = (IMemo)Parameters[0];

            if ((memo.GeneralStatus.Key == (int)GeneralStatuses.Active) && 
                (memo.ReminderDate.HasValue && memo.ExpiryDate.HasValue))
            {
                if (memo.ReminderDate > memo.ExpiryDate)
                {
                    AddMessage("Memo Reminder Date can not be greater than Expiry Date", "", Messages);
                }
            }
            return 1;
        }
    }

    [RuleDBTag("MemoAddUpdateMemoExpiryDate",
    "Expiry Date must be greater than or equal to Today's Date",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Memo.MemoAddUpdateMemoExpiryDate")]
    [RuleInfo]
    public class MemoAddUpdateMemoExpiryDate : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IMemo memo = (IMemo)Parameters[0];

            if ((memo.GeneralStatus.Key == (int)GeneralStatuses.Active) && 
                memo.ExpiryDate.HasValue)
            {
                if (memo.ExpiryDate < DateTime.Now.Date)
                {
                    AddMessage("Expiry Date must be greater than or equal to Today's Date", "", Messages);
                }
            }
            return 1;
        }
    }

    [RuleDBTag("MemoAddUpdateReminderDate",
     "Reminder Date must be greater than or equal to Today's Date",
     "SAHL.Rules.DLL",
     "SAHL.Common.BusinessModel.Rules.Memo.MemoAddUpdateReminderDate")]
    [RuleInfo]
    public class MemoAddUpdateReminderDate : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IMemo memo = (IMemo)Parameters[0];

            if ((memo.GeneralStatus.Key == (int)GeneralStatuses.Active) && memo.ReminderDate.HasValue)
            {
                if (memo.ReminderDate < DateTime.Now.Date)
                {
                    AddMessage("Reminder Date must be greater than or equal to Today's Date", "", Messages);
                }

                if (memo.ReminderDate.Value.Date == DateTime.Now.Date)
                {
                    if (memo.ReminderDate.Value.TimeOfDay.TotalSeconds > 0 &&  memo.ReminderDate.Value.TimeOfDay < DateTime.Now.TimeOfDay)
                    {
                        AddMessage("Reminder Date is Today - Reminder Time must be greater than Now.", "", Messages);
                       
                    }
                }
            }
            return 1;
        }
    }

    [RuleDBTag("MemoAddUpdateDescription",
     "Description cannot be empty",
     "SAHL.Rules.DLL",
     "SAHL.Common.BusinessModel.Rules.Memo.MemoAddUpdateDescription")]
    [RuleInfo]
    public class MemoAddUpdateDescription : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IMemo memo = Parameters[0] as IMemo;

            if (memo != null)
            {
                if (string.IsNullOrEmpty(memo.Description) || memo.Description.Trim().Length == 0)
                {
                    string msg = "Description cannot be empty.";
                    AddMessage(msg,msg,Messages);
                    return 0;
                }
            }
            return 1;
        }
    }
}
