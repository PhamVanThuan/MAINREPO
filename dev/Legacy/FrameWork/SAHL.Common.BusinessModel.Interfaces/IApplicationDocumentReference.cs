using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationDocumentReference_DAO
    /// </summary>
    public partial interface IApplicationDocumentReference : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDocumentReference_DAO.GenericKey
        /// </summary>
        System.Int32 GenericKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDocumentReference_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDocumentReference_DAO.DocumentTypeReferenceObject
        /// </summary>
        IDocumentTypeReferenceObject DocumentTypeReferenceObject
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDocumentReference_DAO.ApplicationDocument
        /// </summary>
        IApplicationDocument ApplicationDocument
        {
            get;
            set;
        }
    }
}