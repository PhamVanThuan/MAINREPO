using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CorrespondenceTemplate_DAO
    /// </summary>
    public partial class CorrespondenceTemplate : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CorrespondenceTemplate_DAO>, ICorrespondenceTemplate
    {
        public CorrespondenceTemplate(SAHL.Common.BusinessModel.DAO.CorrespondenceTemplate_DAO CorrespondenceTemplate)
            : base(CorrespondenceTemplate)
        {
            this._DAO = CorrespondenceTemplate;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CorrespondenceTemplate_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CorrespondenceTemplate_DAO.Name
        /// </summary>
        public String Name
        {
            get { return _DAO.Name; }
            set { _DAO.Name = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CorrespondenceTemplate_DAO.Subject
        /// </summary>
        public String Subject
        {
            get { return _DAO.Subject; }
            set { _DAO.Subject = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CorrespondenceTemplate_DAO.Template
        /// </summary>
        public String Template
        {
            get { return _DAO.Template; }
            set { _DAO.Template = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CorrespondenceTemplate_DAO.DefaultEmail
        /// </summary>
        public String DefaultEmail
        {
            get { return _DAO.DefaultEmail; }
            set { _DAO.DefaultEmail = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CorrespondenceTemplate_DAO.ContentType
        /// </summary>
        public IContentType ContentType
        {
            get
            {
                if (null == _DAO.ContentType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IContentType, ContentType_DAO>(_DAO.ContentType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ContentType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ContentType = (ContentType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}