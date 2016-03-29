using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.Fees_DAO
	/// </summary>
	public partial class Fees : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Fees_DAO>, IFees
	{
				public Fees(SAHL.Common.BusinessModel.DAO.Fees_DAO Fees) : base(Fees)
		{
			this._DAO = Fees;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeNaturalTransferDuty
		/// </summary>
		public Double? FeeNaturalTransferDuty
		{
			get { return _DAO.FeeNaturalTransferDuty; }
			set { _DAO.FeeNaturalTransferDuty = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeNaturalConveyancing
		/// </summary>
		public Double? FeeNaturalConveyancing
		{
			get { return _DAO.FeeNaturalConveyancing; }
			set { _DAO.FeeNaturalConveyancing = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeNaturalVAT
		/// </summary>
		public Double? FeeNaturalVAT
		{
			get { return _DAO.FeeNaturalVAT; }
			set { _DAO.FeeNaturalVAT = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeLegalTransferDuty
		/// </summary>
		public Double? FeeLegalTransferDuty
		{
			get { return _DAO.FeeLegalTransferDuty; }
			set { _DAO.FeeLegalTransferDuty = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeLegalConveyancing
		/// </summary>
		public Double? FeeLegalConveyancing
		{
			get { return _DAO.FeeLegalConveyancing; }
			set { _DAO.FeeLegalConveyancing = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeLegalVAT
		/// </summary>
		public Double? FeeLegalVAT
		{
			get { return _DAO.FeeLegalVAT; }
			set { _DAO.FeeLegalVAT = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeBondStamps
		/// </summary>
		public Double? FeeBondStamps
		{
			get { return _DAO.FeeBondStamps; }
			set { _DAO.FeeBondStamps = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeBondConveyancing
		/// </summary>
		public Double? FeeBondConveyancing
		{
			get { return _DAO.FeeBondConveyancing; }
			set { _DAO.FeeBondConveyancing = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeBondVAT
		/// </summary>
		public Double? FeeBondVAT
		{
			get { return _DAO.FeeBondVAT; }
			set { _DAO.FeeBondVAT = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeAdmin
		/// </summary>
		public Double? FeeAdmin
		{
			get { return _DAO.FeeAdmin; }
			set { _DAO.FeeAdmin = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeValuation
		/// </summary>
		public Double? FeeValuation
		{
			get { return _DAO.FeeValuation; }
			set { _DAO.FeeValuation = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeCancelDuty
		/// </summary>
		public Double? FeeCancelDuty
		{
			get { return _DAO.FeeCancelDuty; }
			set { _DAO.FeeCancelDuty = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeCancelConveyancing
		/// </summary>
		public Double? FeeCancelConveyancing
		{
			get { return _DAO.FeeCancelConveyancing; }
			set { _DAO.FeeCancelConveyancing = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeCancelVAT
		/// </summary>
		public Double? FeeCancelVAT
		{
			get { return _DAO.FeeCancelVAT; }
			set { _DAO.FeeCancelVAT = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeFlexiSwitch
		/// </summary>
		public Double? FeeFlexiSwitch
		{
			get { return _DAO.FeeFlexiSwitch; }
			set { _DAO.FeeFlexiSwitch = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeRCSBondConveyancing
		/// </summary>
		public Double? FeeRCSBondConveyancing
		{
			get { return _DAO.FeeRCSBondConveyancing; }
			set { _DAO.FeeRCSBondConveyancing = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeRCSBondVAT
		/// </summary>
		public Double? FeeRCSBondVAT
		{
			get { return _DAO.FeeRCSBondVAT; }
			set { _DAO.FeeRCSBondVAT = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeDeedsOffice
		/// </summary>
		public Double? FeeDeedsOffice
		{
			get { return _DAO.FeeDeedsOffice; }
			set { _DAO.FeeDeedsOffice = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeRCSBondPreparation
		/// </summary>
		public Double? FeeRCSBondPreparation
		{
			get { return _DAO.FeeRCSBondPreparation; }
			set { _DAO.FeeRCSBondPreparation = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeBondConveyancing80Pct
		/// </summary>
		public Double? FeeBondConveyancing80Pct
		{
			get { return _DAO.FeeBondConveyancing80Pct; }
			set { _DAO.FeeBondConveyancing80Pct = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeBondVAT80Pct
		/// </summary>
		public Double? FeeBondVAT80Pct
		{
			get { return _DAO.FeeBondVAT80Pct; }
			set { _DAO.FeeBondVAT80Pct = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Fees_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeBondConveyancing
        /// </summary>
        public Double? FeeBondConveyancingNoFICA
        {
            get { return _DAO.FeeBondConveyancingNoFICA; }
            set { _DAO.FeeBondConveyancingNoFICA = value; }
        }
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeBondVAT
        /// </summary>
        public Double? FeeBondNoFICAVAT
        {
            get { return _DAO.FeeBondNoFICAVAT; }
            set { _DAO.FeeBondNoFICAVAT = value; }
        }
	}
}


