using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ExternalRoleDeclaration_DAO
    /// </summary>
    public partial interface IExternalRoleDeclaration : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ExternalRoleDeclaration_DAO.ExternalRole
        /// </summary>
        IExternalRole ExternalRole
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ExternalRoleDeclaration_DAO.ExternalRoleDeclarationDate
        /// </summary>
        DateTime? ExternalRoleDeclarationDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ExternalRoleDeclaration_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclaration_DAO.ApplicationDeclarationAnswer
        /// </summary>
        IApplicationDeclarationAnswer ApplicationDeclarationAnswer
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclaration_DAO.ApplicationDeclarationQuestion
        /// </summary>
        IApplicationDeclarationQuestion ApplicationDeclarationQuestion
        {
            get;
            set;
        }
    }
}