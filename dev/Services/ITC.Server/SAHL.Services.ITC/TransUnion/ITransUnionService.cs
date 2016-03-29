using SAHL.Services.Interfaces.ITC.Models;

namespace SAHL.Services.ITC.TransUnion
{
    public interface ITransUnionService
    {
        ItcResponse PerformRequest(ItcRequest request);
    }
}