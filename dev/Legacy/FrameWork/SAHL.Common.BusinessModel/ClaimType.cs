using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
	/// <summary>
    /// SAHL.Common.BusinessModel.DAO.ClaimType_DAO
	/// </summary>
	public partial class ClaimType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ClaimType_DAO>, IClaimType
	{
        public ClaimType(SAHL.Common.BusinessModel.DAO.ClaimType_DAO ClaimType)
            : base(ClaimType)
		{
            this._DAO = ClaimType;
		}

		/// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClaimType_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}

		/// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClaimType_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}


