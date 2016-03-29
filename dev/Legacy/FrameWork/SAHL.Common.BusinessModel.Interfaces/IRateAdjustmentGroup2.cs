using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface IRateAdjustmentGroup
    {
        IEventList<IRateAdjustmentElement> ActiveRateAdjustmentElements { get; }
    }
}