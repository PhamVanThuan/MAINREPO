using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationDeclaration_DAO
    /// </summary>
    public partial interface IApplicationDeclaration : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclaration_DAO.ApplicationRole
        /// </summary>
        IApplicationRole ApplicationRole
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclaration_DAO.ApplicationDeclarationDate
        /// </summary>
        DateTime? ApplicationDeclarationDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclaration_DAO.Key
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