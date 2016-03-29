using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.DocumentType_DAO
    /// </summary>
    public partial interface IDocumentType : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DocumentType_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DocumentType_DAO.LegalEntity
        /// </summary>
        Boolean? LegalEntity
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DocumentType_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DocumentType_DAO.DocumentTypes
        /// </summary>
        IEventList<IDocumentType> DocumentTypes
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DocumentType_DAO.DocumentVersions
        /// </summary>
        IEventList<IDocumentVersion> DocumentVersions
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DocumentType_DAO.GenericKeyType
        /// </summary>
        IGenericKeyType GenericKeyType
        {
            get;
            set;
        }
    }
}