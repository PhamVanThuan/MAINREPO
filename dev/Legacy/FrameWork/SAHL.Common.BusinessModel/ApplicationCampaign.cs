using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationCampaign_DAO
    /// </summary>
    public partial class ApplicationCampaign : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationCampaign_DAO>, IApplicationCampaign
    {
        public ApplicationCampaign(SAHL.Common.BusinessModel.DAO.ApplicationCampaign_DAO ApplicationCampaign)
            : base(ApplicationCampaign)
        {
            this._DAO = ApplicationCampaign;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationCampaign_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationCampaign_DAO.StartDate
        /// </summary>
        public DateTime StartDate
        {
            get { return _DAO.StartDate; }
            set { _DAO.StartDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationCampaign_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }
    }
}