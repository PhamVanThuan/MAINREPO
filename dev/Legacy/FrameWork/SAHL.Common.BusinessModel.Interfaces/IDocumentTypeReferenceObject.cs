using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.DocumentTypeReferenceObject_DAO
    /// </summary>
    public partial interface IDocumentTypeReferenceObject : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DocumentTypeReferenceObject_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DocumentTypeReferenceObject_DAO.ApplicationDocumentReferences
        /// </summary>
        IEventList<IApplicationDocumentReference> ApplicationDocumentReferences
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DocumentTypeReferenceObject_DAO.DocumentType
        /// </summary>
        IDocumentType DocumentType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DocumentTypeReferenceObject_DAO.GenericKeyType
        /// </summary>
        IGenericKeyType GenericKeyType
        {
            get;
            set;
        }
    }
}