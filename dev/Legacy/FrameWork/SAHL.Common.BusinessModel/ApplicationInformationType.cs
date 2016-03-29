using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationInformationType_DAO
    /// </summary>
    public partial class ApplicationInformationType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationInformationType_DAO>, IApplicationInformationType
    {
        public ApplicationInformationType(SAHL.Common.BusinessModel.DAO.ApplicationInformationType_DAO ApplicationInformationType)
            : base(ApplicationInformationType)
        {
            this._DAO = ApplicationInformationType;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationType_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationType_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }
    }
}