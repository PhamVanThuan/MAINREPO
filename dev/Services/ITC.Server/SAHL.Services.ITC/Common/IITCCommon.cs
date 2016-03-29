using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ITC.Models;
using SAHL.Services.ITC.Managers.Itc;
using SAHL.Services.ITC.TransUnion;

namespace SAHL.Services.ITC.Common
{
    public interface IITCCommon
    {
        ItcResponse PerformITCRequest(IItcManager itcManager, ITransUnionService transUnionService, ItcRequest itcRequest, ISystemMessageCollection systemMessages);
    }
}