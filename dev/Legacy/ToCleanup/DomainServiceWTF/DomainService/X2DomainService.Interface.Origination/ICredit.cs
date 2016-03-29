using System;
using System.Collections.Generic;
using System.Text;
using SAHL.X2.Common;
using SAHL.X2.Common.DataAccess;

namespace X2DomainService.Interface.Origination
{
    public interface ICredit
    {
        IX2ReturnData IsResub(Int64 InstanceID, int ApplicationKey, out bool b);
        IX2ReturnData WakeupPreviousMostSeniorPerson(IActiveDataTransaction Tran, int ApplicationKey, out string AssignedTo);
        IX2ReturnData PerformCreditMandateCheck(IActiveDataTransaction Tran, int ApplicationKey, out List<string> Users, out string AssignedTo);
        IX2ReturnData CreditResub(IActiveDataTransaction Tran, int ApplicationKey);
        IX2ReturnData CreditDecisionCheckMatrixRules(IActiveDataTransaction Tran, int ApplicationKey, bool IgnoreWarnings, out bool b);
        IX2ReturnData CreditDecisionCheckAuthorisationRules(IActiveDataTransaction Tran, int ApplicationKey, Int64 InstanceID, out bool b);
        
        IX2ReturnData MarkCreditUsersAsInactive(IActiveDataTransaction Tran, int ApplicationKey);
        IX2ReturnData UpdateConditions(IActiveDataTransaction Tran, int ApplicationKey);
        
        IX2ReturnData SendResubMail(int ApplicationKey);
        IX2ReturnData ReviewNotRequired(Int64 InstanceID, int ApplicationKey, string ActionSource, out bool b);
    }
}
