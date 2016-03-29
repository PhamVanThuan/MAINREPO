using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ExternalRoleDomicilium_DAO
    /// </summary>
    public partial interface IExternalRoleDomicilium : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ExternalRoleDomicilium_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ExternalRoleDomicilium_DAO.LegalEntityDomicilium
        /// </summary>
        ILegalEntityDomicilium LegalEntityDomicilium
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ExternalRoleDomicilium_DAO.ExternalRole
        /// </summary>
        IExternalRole ExternalRole
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ExternalRoleDomicilium_DAO.ADUser
        /// </summary>
        IADUser ADUser
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ExternalRoleDomicilium_DAO.ChangeDate
        /// </summary>
        DateTime? ChangeDate
        {
            get;
            set;
        }
    }
}