using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationDocument_DAO
    /// </summary>
    public partial class ApplicationDocument : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationDocument_DAO>, IApplicationDocument
    {
        public ApplicationDocument(SAHL.Common.BusinessModel.DAO.ApplicationDocument_DAO ApplicationDocument)
            : base(ApplicationDocument)
        {
            this._DAO = ApplicationDocument;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDocument_DAO.DocumentReceivedDate
        /// </summary>
        public DateTime? DocumentReceivedDate
        {
            get { return _DAO.DocumentReceivedDate; }
            set { _DAO.DocumentReceivedDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDocument_DAO.DocumentReceivedBy
        /// </summary>
        public String DocumentReceivedBy
        {
            get { return _DAO.DocumentReceivedBy; }
            set { _DAO.DocumentReceivedBy = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDocument_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDocument_DAO.GenericKey
        /// </summary>
        public Int32 GenericKey
        {
            get { return _DAO.GenericKey; }
            set { _DAO.GenericKey = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDocument_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDocument_DAO.ApplicationDocumentReferences
        /// </summary>
        private DAOEventList<ApplicationDocumentReference_DAO, IApplicationDocumentReference, ApplicationDocumentReference> _ApplicationDocumentReferences;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDocument_DAO.ApplicationDocumentReferences
        /// </summary>
        public IEventList<IApplicationDocumentReference> ApplicationDocumentReferences
        {
            get
            {
                if (null == _ApplicationDocumentReferences)
                {
                    if (null == _DAO.ApplicationDocumentReferences)
                        _DAO.ApplicationDocumentReferences = new List<ApplicationDocumentReference_DAO>();
                    _ApplicationDocumentReferences = new DAOEventList<ApplicationDocumentReference_DAO, IApplicationDocumentReference, ApplicationDocumentReference>(_DAO.ApplicationDocumentReferences);
                    _ApplicationDocumentReferences.BeforeAdd += new EventListHandler(OnApplicationDocumentReferences_BeforeAdd);
                    _ApplicationDocumentReferences.BeforeRemove += new EventListHandler(OnApplicationDocumentReferences_BeforeRemove);
                    _ApplicationDocumentReferences.AfterAdd += new EventListHandler(OnApplicationDocumentReferences_AfterAdd);
                    _ApplicationDocumentReferences.AfterRemove += new EventListHandler(OnApplicationDocumentReferences_AfterRemove);
                }
                return _ApplicationDocumentReferences;
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDocument_DAO.Application
        /// </summary>
        public IApplication Application
        {
            get
            {
                if (null == _DAO.Application) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplication, Application_DAO>(_DAO.Application);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Application = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Application = (Application_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDocument_DAO.DocumentType
        /// </summary>
        public IDocumentType DocumentType
        {
            get
            {
                if (null == _DAO.DocumentType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IDocumentType, DocumentType_DAO>(_DAO.DocumentType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.DocumentType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.DocumentType = (DocumentType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _ApplicationDocumentReferences = null;
        }
    }
}