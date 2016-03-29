using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationDocumentReference_DAO
    /// </summary>
    public partial class ApplicationDocumentReference : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationDocumentReference_DAO>, IApplicationDocumentReference
    {
        public ApplicationDocumentReference(SAHL.Common.BusinessModel.DAO.ApplicationDocumentReference_DAO ApplicationDocumentReference)
            : base(ApplicationDocumentReference)
        {
            this._DAO = ApplicationDocumentReference;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDocumentReference_DAO.GenericKey
        /// </summary>
        public Int32 GenericKey
        {
            get { return _DAO.GenericKey; }
            set { _DAO.GenericKey = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDocumentReference_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDocumentReference_DAO.DocumentTypeReferenceObject
        /// </summary>
        public IDocumentTypeReferenceObject DocumentTypeReferenceObject
        {
            get
            {
                if (null == _DAO.DocumentTypeReferenceObject) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IDocumentTypeReferenceObject, DocumentTypeReferenceObject_DAO>(_DAO.DocumentTypeReferenceObject);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.DocumentTypeReferenceObject = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.DocumentTypeReferenceObject = (DocumentTypeReferenceObject_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDocumentReference_DAO.ApplicationDocument
        /// </summary>
        public IApplicationDocument ApplicationDocument
        {
            get
            {
                if (null == _DAO.ApplicationDocument) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplicationDocument, ApplicationDocument_DAO>(_DAO.ApplicationDocument);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ApplicationDocument = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ApplicationDocument = (ApplicationDocument_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}