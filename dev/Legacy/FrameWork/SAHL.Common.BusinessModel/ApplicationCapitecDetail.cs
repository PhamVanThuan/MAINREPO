using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationCapitecDetail_DAO
    /// </summary>
    public partial class ApplicationCapitecDetail : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationCapitecDetail_DAO>, IApplicationCapitecDetail
    {
        public ApplicationCapitecDetail(SAHL.Common.BusinessModel.DAO.ApplicationCapitecDetail_DAO ApplicationCapitecDetail)
            : base(ApplicationCapitecDetail)
        {
            this._DAO = ApplicationCapitecDetail;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationCapitecDetail_DAO.Key
        /// </summary>
        public virtual int Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationCapitecDetail_DAO.ApplicationKey
        /// </summary>
        public Int32 ApplicationKey
        {
            get { return _DAO.ApplicationKey; }
            set { _DAO.ApplicationKey = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationCapitecDetail_DAO.Branch
        /// </summary>
        public String Branch
        {
            get { return _DAO.Branch; }
            set { _DAO.Branch = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationCapitecDetail_DAO.Consultant
        /// </summary>
        public String Consultant
        {
            get { return _DAO.Consultant; }
            set { _DAO.Consultant = value; }
        }
    }
}