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
using SAHL.Common.Globals;
using System.Linq;

namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.Regent_DAO
	/// </summary>
	public partial class Regent : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Regent_DAO>, IRegent, IAccount
	{
				public Regent(SAHL.Common.BusinessModel.DAO.Regent_DAO Regent) : base(Regent)
		{
			this._DAO = Regent;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentClientSalutation
		/// </summary>
		public String RegentClientSalutation 
		{
			get { return _DAO.RegentClientSalutation; }
			set { _DAO.RegentClientSalutation = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentClientSurname
		/// </summary>
		public String RegentClientSurname 
		{
			get { return _DAO.RegentClientSurname; }
			set { _DAO.RegentClientSurname = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentClientFirstNames
		/// </summary>
		public String RegentClientFirstNames 
		{
			get { return _DAO.RegentClientFirstNames; }
			set { _DAO.RegentClientFirstNames = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentClientIDNumber
		/// </summary>
		public Decimal RegentClientIDNumber 
		{
			get { return _DAO.RegentClientIDNumber; }
			set { _DAO.RegentClientIDNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentClientGender
		/// </summary>
		public Int16 RegentClientGender 
		{
			get { return _DAO.RegentClientGender; }
			set { _DAO.RegentClientGender = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentClientDateBirth
		/// </summary>
		public DateTime? RegentClientDateBirth
		{
			get { return _DAO.RegentClientDateBirth; }
			set { _DAO.RegentClientDateBirth = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentStatus
		/// </summary>
		public IRegentStatus RegentStatus 
		{
			get
			{
				if (null == _DAO.RegentStatus) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IRegentStatus, RegentStatus_DAO>(_DAO.RegentStatus);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.RegentStatus = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.RegentStatus = (RegentStatus_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentApplicationDate
		/// </summary>
		public DateTime? RegentApplicationDate
		{
			get { return _DAO.RegentApplicationDate; }
			set { _DAO.RegentApplicationDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentInceptionDate
		/// </summary>
		public DateTime? RegentInceptionDate
		{
			get { return _DAO.RegentInceptionDate; }
			set { _DAO.RegentInceptionDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentExpiryDate
		/// </summary>
		public DateTime? RegentExpiryDate
		{
			get { return _DAO.RegentExpiryDate; }
			set { _DAO.RegentExpiryDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentLoanTerm
		/// </summary>
		public Decimal RegentLoanTerm 
		{
			get { return _DAO.RegentLoanTerm; }
			set { _DAO.RegentLoanTerm = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentSumInsured
		/// </summary>
		public Double RegentSumInsured 
		{
			get { return _DAO.RegentSumInsured; }
			set { _DAO.RegentSumInsured = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentPremium
		/// </summary>
		public Double RegentPremium 
		{
			get { return _DAO.RegentPremium; }
			set { _DAO.RegentPremium = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentClientSecSalutation
		/// </summary>
		public String RegentClientSecSalutation 
		{
			get { return _DAO.RegentClientSecSalutation; }
			set { _DAO.RegentClientSecSalutation = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentClientSecSurname
		/// </summary>
		public String RegentClientSecSurname 
		{
			get { return _DAO.RegentClientSecSurname; }
			set { _DAO.RegentClientSecSurname = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentClientSecFirstNames
		/// </summary>
		public String RegentClientSecFirstNames 
		{
			get { return _DAO.RegentClientSecFirstNames; }
			set { _DAO.RegentClientSecFirstNames = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentClientSecIDNumber
		/// </summary>
		public Decimal RegentClientSecIDNumber 
		{
			get { return _DAO.RegentClientSecIDNumber; }
			set { _DAO.RegentClientSecIDNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentClientSecGender
		/// </summary>
		public Int16 RegentClientSecGender 
		{
			get { return _DAO.RegentClientSecGender; }
			set { _DAO.RegentClientSecGender = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentClientSecDateBirth
		/// </summary>
		public DateTime? RegentClientSecDateBirth
		{
			get { return _DAO.RegentClientSecDateBirth; }
			set { _DAO.RegentClientSecDateBirth = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentJointIndicator
		/// </summary>
		public Int16 RegentJointIndicator 
		{
			get { return _DAO.RegentJointIndicator; }
			set { _DAO.RegentJointIndicator = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentReinstateDate
		/// </summary>
		public DateTime? RegentReinstateDate
		{
			get { return _DAO.RegentReinstateDate; }
			set { _DAO.RegentReinstateDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentLastUpdateDate
		/// </summary>
		public DateTime? RegentLastUpdateDate
		{
			get { return _DAO.RegentLastUpdateDate; }
			set { _DAO.RegentLastUpdateDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentCommision
		/// </summary>
		public Double RegentCommision 
		{
			get { return _DAO.RegentCommision; }
			set { _DAO.RegentCommision = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentUnderwritingFirst
		/// </summary>
		public Int32 RegentUnderwritingFirst 
		{
			get { return _DAO.RegentUnderwritingFirst; }
			set { _DAO.RegentUnderwritingFirst = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentUnderwritingSecond
		/// </summary>
		public Int32 RegentUnderwritingSecond 
		{
			get { return _DAO.RegentUnderwritingSecond; }
			set { _DAO.RegentUnderwritingSecond = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Regent_DAO.SAHLEmployeeNumber
		/// </summary>
		public Decimal SAHLEmployeeNumber 
		{
			get { return _DAO.SAHLEmployeeNumber; }
			set { _DAO.SAHLEmployeeNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentUpdatedStatus
		/// </summary>
		public Int16 RegentUpdatedStatus 
		{
			get { return _DAO.RegentUpdatedStatus; }
			set { _DAO.RegentUpdatedStatus = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentClientOccupation
		/// </summary>
		public Int16 RegentClientOccupation 
		{
			get { return _DAO.RegentClientOccupation; }
			set { _DAO.RegentClientOccupation = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentClientAge
		/// </summary>
		public Int16 RegentClientAge 
		{
			get { return _DAO.RegentClientAge; }
			set { _DAO.RegentClientAge = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Regent_DAO.ReplacementPolicy
		/// </summary>
		public String ReplacementPolicy 
		{
			get { return _DAO.ReplacementPolicy; }
			set { _DAO.ReplacementPolicy = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Regent_DAO.AdviceRequired
		/// </summary>
		public String AdviceRequired 
		{
			get { return _DAO.AdviceRequired; }
			set { _DAO.AdviceRequired = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Regent_DAO.LifeAssuredName
		/// </summary>
		public String LifeAssuredName 
		{
			get { return _DAO.LifeAssuredName; }
			set { _DAO.LifeAssuredName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Regent_DAO.OldInsurer
		/// </summary>
		public String OldInsurer 
		{
			get { return _DAO.OldInsurer; }
			set { _DAO.OldInsurer = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Regent_DAO.OldPolicyNo
		/// </summary>
		public String OldPolicyNo 
		{
			get { return _DAO.OldPolicyNo; }
			set { _DAO.OldPolicyNo = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentNewBusinessDate
		/// </summary>
		public DateTime? RegentNewBusinessDate
		{
			get { return _DAO.RegentNewBusinessDate; }
			set { _DAO.RegentNewBusinessDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Regent_DAO.CapitalizedMonthlyBalance
		/// </summary>
		public Double CapitalizedMonthlyBalance 
		{
			get { return _DAO.CapitalizedMonthlyBalance; }
			set { _DAO.CapitalizedMonthlyBalance = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Regent_DAO.Key
		/// </summary>
		public Decimal Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}

        public bool IsThirtyYearTerm
        {
            get { throw new NotImplementedException(); } 
        }

        public bool HasAccountInformationType(AccountInformationTypes accountInformationType)
        {
            return this.AccountInformations.Any(x => x.AccountInformationType.Key == (int)accountInformationType);
        }

    }
}


