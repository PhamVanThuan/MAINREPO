using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AccountDocument_DAO
    /// </summary>
    public partial interface IAccountDocument : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountDocument_DAO.LegalEntityKey
        /// </summary>
        Int32? LegalEntityKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountDocument_DAO.DocumentReceived
        /// </summary>
        Boolean? DocumentReceived
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountDocument_DAO.DocumentReceivedDate
        /// </summary>
        DateTime? DocumentReceivedDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountDocument_DAO.DocumentReceivedBy
        /// </summary>
        System.String DocumentReceivedBy
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountDocument_DAO.DocumentVersionNumber
        /// </summary>
        System.String DocumentVersionNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountDocument_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountDocument_DAO.Account
        /// </summary>
        IAccount Account
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountDocument_DAO.DocumentType
        /// </summary>
        IDocumentType DocumentType
        {
            get;
            set;
        }
    }
}