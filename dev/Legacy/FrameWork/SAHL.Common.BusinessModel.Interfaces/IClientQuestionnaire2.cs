using System;
using SAHL.Common.BusinessModel.Validation;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface IClientQuestionnaire : IEntityValidation
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientQuestionnaire_DAO.GUID
        /// </summary>
        String GUID
        {
            get;
            set;
        }
    }
}