using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface IAttorney : IEntityValidation
    {
        /// <summary>
        /// Get a list of Attorney contacts
        /// </summary>
        /// <param name="ert"></param>
        /// <param name="gs"></param>
        /// <returns></returns>
        IList<ILegalEntity> GetContacts(ExternalRoleTypes ert, GeneralStatuses gs);
    }
}