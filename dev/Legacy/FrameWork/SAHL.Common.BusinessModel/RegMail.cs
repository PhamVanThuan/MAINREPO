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
	/// SAHL.Common.BusinessModel.DAO.RegMail_DAO
	/// </summary>
	public partial class RegMail : BusinessModelBase<SAHL.Common.BusinessModel.DAO.RegMail_DAO>, IRegMail
	{
				public RegMail(SAHL.Common.BusinessModel.DAO.RegMail_DAO RegMail) : base(RegMail)
		{
			this._DAO = RegMail;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RegMail_DAO.LoanNumber
		/// </summary>
		public Int32 LoanNumber 
		{
			get { return _DAO.LoanNumber; }
			set { _DAO.LoanNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RegMail_DAO.PurposeNumber
		/// </summary>
		public Decimal PurposeNumber 
		{
			get { return _DAO.PurposeNumber; }
			set { _DAO.PurposeNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RegMail_DAO.DetailTypeNumber
		/// </summary>
		public Decimal DetailTypeNumber 
		{
			get { return _DAO.DetailTypeNumber; }
			set { _DAO.DetailTypeNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RegMail_DAO.AttorneyNumber
		/// </summary>
		public Decimal AttorneyNumber 
		{
			get { return _DAO.AttorneyNumber; }
			set { _DAO.AttorneyNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailLoanStatus
		/// </summary>
		public Int16? RegMailLoanStatus
		{
			get { return _DAO.RegMailLoanStatus; }
			set { _DAO.RegMailLoanStatus = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailDateTime
		/// </summary>
		public DateTime? RegMailDateTime
		{
			get { return _DAO.RegMailDateTime; }
			set { _DAO.RegMailDateTime = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailBondAmount
		/// </summary>
		public Double? RegMailBondAmount
		{
			get { return _DAO.RegMailBondAmount; }
			set { _DAO.RegMailBondAmount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailBondDate
		/// </summary>
		public DateTime? RegMailBondDate
		{
			get { return _DAO.RegMailBondDate; }
			set { _DAO.RegMailBondDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailGuaranteeAmount
		/// </summary>
		public Double? RegMailGuaranteeAmount
		{
			get { return _DAO.RegMailGuaranteeAmount; }
			set { _DAO.RegMailGuaranteeAmount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailCashRequired
		/// </summary>
		public Double? RegMailCashRequired
		{
			get { return _DAO.RegMailCashRequired; }
			set { _DAO.RegMailCashRequired = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailCashDeposit
		/// </summary>
		public Double? RegMailCashDeposit
		{
			get { return _DAO.RegMailCashDeposit; }
			set { _DAO.RegMailCashDeposit = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailConveyancingFee
		/// </summary>
		public Double? RegMailConveyancingFee
		{
			get { return _DAO.RegMailConveyancingFee; }
			set { _DAO.RegMailConveyancingFee = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailVAT
		/// </summary>
		public Double? RegMailVAT
		{
			get { return _DAO.RegMailVAT; }
			set { _DAO.RegMailVAT = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailTransferDuty
		/// </summary>
		public Double? RegMailTransferDuty
		{
			get { return _DAO.RegMailTransferDuty; }
			set { _DAO.RegMailTransferDuty = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailDeedsFee
		/// </summary>
		public Double? RegMailDeedsFee
		{
			get { return _DAO.RegMailDeedsFee; }
			set { _DAO.RegMailDeedsFee = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailInstructions1
		/// </summary>
		public String RegMailInstructions1 
		{
			get { return _DAO.RegMailInstructions1; }
			set { _DAO.RegMailInstructions1 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailInstructions2
		/// </summary>
		public String RegMailInstructions2 
		{
			get { return _DAO.RegMailInstructions2; }
			set { _DAO.RegMailInstructions2 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailInstructions3
		/// </summary>
		public String RegMailInstructions3 
		{
			get { return _DAO.RegMailInstructions3; }
			set { _DAO.RegMailInstructions3 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailStampDuty
		/// </summary>
		public Double? RegMailStampDuty
		{
			get { return _DAO.RegMailStampDuty; }
			set { _DAO.RegMailStampDuty = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailCancelFee
		/// </summary>
		public Double? RegMailCancelFee
		{
			get { return _DAO.RegMailCancelFee; }
			set { _DAO.RegMailCancelFee = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailCATSFlag
		/// </summary>
		public Int16? RegMailCATSFlag
		{
			get { return _DAO.RegMailCATSFlag; }
			set { _DAO.RegMailCATSFlag = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailLoanAgreementAmount
		/// </summary>
		public Double? RegMailLoanAgreementAmount
		{
			get { return _DAO.RegMailLoanAgreementAmount; }
			set { _DAO.RegMailLoanAgreementAmount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailValuationFee
		/// </summary>
		public Double? RegMailValuationFee
		{
			get { return _DAO.RegMailValuationFee; }
			set { _DAO.RegMailValuationFee = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailAdminFee
		/// </summary>
		public Double? RegMailAdminFee
		{
			get { return _DAO.RegMailAdminFee; }
			set { _DAO.RegMailAdminFee = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RegMail_DAO.QuickCashInterest
		/// </summary>
		public Double? QuickCashInterest
		{
			get { return _DAO.QuickCashInterest; }
			set { _DAO.QuickCashInterest = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RegMail_DAO.QCAdminFee
		/// </summary>
		public Double? QCAdminFee
		{
			get { return _DAO.QCAdminFee; }
			set { _DAO.QCAdminFee = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RegMail_DAO.Key
		/// </summary>
		public Decimal Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}


