using System;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ITC.Models;
using SAHL.Services.ITC.Managers.Itc;
using SAHL.Services.ITC.TransUnion;

namespace SAHL.Services.ITC.Common
{
    public class ITCCommon : IITCCommon
    {
        public ItcResponse PerformITCRequest(IItcManager itcManager, ITransUnionService transUnionService, ItcRequest itcRequest, Core.SystemMessages.ISystemMessageCollection systemMessages)
        {
            var itcResponse = transUnionService.PerformRequest(itcRequest);
            if (itcResponse == null)
            {
                systemMessages.AddMessage(new SystemMessage("ITC Response object is empty", SystemMessageSeverityEnum.Error));
                itcManager.LogFailedITCRequestAndResponse(itcRequest, itcResponse, this.GetType().Name);
                return itcResponse;
            }

            if (!String.IsNullOrEmpty(itcResponse.ErrorCode))
            {
                itcManager.LogFailedITCRequestAndResponse(itcRequest, itcResponse, this.GetType().Name);
            }

            return itcResponse;
        }
    }
}