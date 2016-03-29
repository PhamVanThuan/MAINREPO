using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.ClaimStatus_DAO
	/// </summary>
	public partial class ClaimStatus : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ClaimStatus_DAO>, IClaimStatus
	{
        public ClaimStatus(SAHL.Common.BusinessModel.DAO.ClaimStatus_DAO ClaimStatus) 
            : base(ClaimStatus)
		{
			this._DAO = ClaimStatus;
		}

		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ClaimStatus_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}

		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifeClaimStatus_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}


