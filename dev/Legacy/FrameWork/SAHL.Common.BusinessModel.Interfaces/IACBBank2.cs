using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public partial interface IACBBank : IEntityValidation
    {
        /// <summary>
        /// Retrieves all ACB branches starting with <c>Prefix</c>.
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="maxRowCount"></param>
        /// <returns></returns>
        IReadOnlyEventList<IACBBranch> GetACBBranchesByPrefix(string prefix, int maxRowCount);
    }
}