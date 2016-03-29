using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationDocument_DAO
    /// </summary>
    public partial interface IApplicationDocument : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDocument_DAO.DocumentReceivedDate
        /// </summary>
        DateTime? DocumentReceivedDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDocument_DAO.DocumentReceivedBy
        /// </summary>
        System.String DocumentReceivedBy
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDocument_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDocument_DAO.GenericKey
        /// </summary>
        System.Int32 GenericKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDocument_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDocument_DAO.ApplicationDocumentReferences
        /// </summary>
        IEventList<IApplicationDocumentReference> ApplicationDocumentReferences
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDocument_DAO.Application
        /// </summary>
        IApplication Application
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDocument_DAO.DocumentType
        /// </summary>
        IDocumentType DocumentType
        {
            get;
            set;
        }
    }
}