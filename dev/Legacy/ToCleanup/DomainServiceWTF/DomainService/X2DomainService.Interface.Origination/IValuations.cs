using System;
using System.Collections.Generic;
using System.Text;
using SAHL.X2.Common;
using SAHL.X2.Common.DataAccess;

namespace X2DomainService.Interface.Origination
{
    public interface IValuations
    {
        IX2ReturnData GetLastValuationKeyAndPropKey(IActiveDataTransaction Tran, Int64 InstanceID, int GenericKey, out int PropertyKey, out int ValuationKey);
        IX2ReturnData EmailOnCompleteValuation(int ValuationKey, int ApplicationKey);
        IX2ReturnData CleanupChildCases(IActiveDataTransaction Tran, Int64 ParentInstanceID);
        IX2ReturnData SetValuationStatusWithDrawn(IActiveDataTransaction Tran, int ValuationKey);
        IX2ReturnData SetValuationStatus(IActiveDataTransaction Tran, int ValuationKey, int Status);
        IX2ReturnData SetValuationIsActive(IActiveDataTransaction Tran, int ValuationKey);
        IX2ReturnData SetValuationActiveAndSave(IActiveDataTransaction Tran, int ValuationKey);

        IX2ReturnData RequestValuation(IActiveDataTransaction Tran, int ApplicationKey, int AdcheckPropertyID, int PropertyKey, out int AdCheckValuationID, object Data, out bool b);
        IX2ReturnData RaiseAdCheckValautionFailed(IActiveDataTransaction Tran, Int64 CurrentInstanceID);
        IX2ReturnData RaiseAdCheckValautionSucceeded(IActiveDataTransaction Tran, Int64 CurrentInstanceID);
        IX2ReturnData AdCheckValuationIDStatus(int AdCheckValuationID, out int ID);
        IX2ReturnData AssesmentReturnedNotCompleted(int AdCheckValuationID, out bool b);
        IX2ReturnData AssesmentComplete(IActiveDataTransaction Tran, int ApplicationKey, int AdCheckValuationID, int ValuationKey, string ADUserWhoRequestedValuation, out bool b);

        IX2ReturnData RecalcHOC(IActiveDataTransaction Tran, int ValuationKey);
        IX2ReturnData WithDraw(IActiveDataTransaction Tran, int AdcheckValuatinID, int ApplicationKey, Int64 InstanceID, out string Resp);
        IX2ReturnData DeletePendingValuationsForOffer(IActiveDataTransaction Tran, int ApplicationKey);
    }
}
