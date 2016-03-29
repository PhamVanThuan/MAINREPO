using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;
using System;

namespace SAHL.Common.BusinessModel
{
    public partial class DisabilityClaimStatus : BusinessModelBase<SAHL.Common.BusinessModel.DAO.DisabilityClaimStatus_DAO>, IDisabilityClaimStatus
    {
        public DisabilityClaimStatus(SAHL.Common.BusinessModel.DAO.DisabilityClaimStatus_DAO DisabilityClaimStatus)
            : base(DisabilityClaimStatus)
		{
            this._DAO = DisabilityClaimStatus;
		}

		/// <summary>
        /// SAHL.Common.BusinessModel.DAO.DisabilityClaimStatus_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}

		/// <summary>
        /// SAHL.Common.BusinessModel.DAO.DisabilityClaimStatus_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
    }
}
