using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ContentType_DAO
    /// </summary>
    public partial class ContentType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ContentType_DAO>, IContentType
    {
        public ContentType(SAHL.Common.BusinessModel.DAO.ContentType_DAO ContentType)
            : base(ContentType)
        {
            this._DAO = ContentType;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ContentType_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ContentType_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }
    }
}