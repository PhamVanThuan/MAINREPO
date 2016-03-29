using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface IProvince : IEntityValidation
    {
        IReadOnlyEventList<ISuburb> SuburbsByPrefix(string prefix, int maxRowCount);
    }
}