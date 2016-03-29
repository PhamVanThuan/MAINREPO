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
	/// Employment_DAO is instantiated in order to create an Employment record for a Legal Entity. It is discriminated based on the
		/// Employment Type.
	/// </summary>
	public partial class Employment : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Employment_DAO>, IEmployment
	{
				public Employment(SAHL.Common.BusinessModel.DAO.Employment_DAO Employment) : base(Employment)
		{
			this._DAO = Employment;
		}
		/// <summary>
		/// The date on which the Legal Entity was employed with the employer
		/// </summary>
		public DateTime? EmploymentStartDate
		{
			get { return _DAO.EmploymentStartDate; }
			set { _DAO.EmploymentStartDate = value;}
		}
		/// <summary>
		/// The date on which the employment was ended.
		/// </summary>
		public DateTime? EmploymentEndDate
		{
			get { return _DAO.EmploymentEndDate; }
			set { _DAO.EmploymentEndDate = value;}
		}
		/// <summary>
		/// A Reference for the Legal Entity's Employment.
		/// </summary>
		public String ContactPerson 
		{
			get { return _DAO.ContactPerson; }
			set { _DAO.ContactPerson = value;}
		}
		/// <summary>
		/// The Phone Number for the Contact Person.
		/// </summary>
		public String ContactPhoneNumber 
		{
			get { return _DAO.ContactPhoneNumber; }
			set { _DAO.ContactPhoneNumber = value;}
		}
		/// <summary>
		/// The Area Code for the Contact Person's Phone Number.
		/// </summary>
		public String ContactPhoneCode 
		{
			get { return _DAO.ContactPhoneCode; }
			set { _DAO.ContactPhoneCode = value;}
		}
		/// <summary>
		/// The SAHL Employee who confirmed the Legal Entity's Income.
		/// </summary>
		public String ConfirmedBy 
		{
			get { return _DAO.ConfirmedBy; }
			set { _DAO.ConfirmedBy = value;}
		}
		/// <summary>
		/// The date on which the Legal Entity's Income was confirmed.
		/// </summary>
		public DateTime? ConfirmedDate
		{
			get { return _DAO.ConfirmedDate; }
			set { _DAO.ConfirmedDate = value;}
		}
		/// <summary>
		/// The UserID of the user who last updated the Employment Record.
		/// </summary>
		public String UserID 
		{
			get { return _DAO.UserID; }
			set { _DAO.UserID = value;}
		}
		/// <summary>
		/// The date on which the Employment record was last changed.
		/// </summary>
		public DateTime? ChangeDate
		{
			get { return _DAO.ChangeDate; }
			set { _DAO.ChangeDate = value;}
		}
		/// <summary>
		/// The department in which the Legal Entity works.
		/// </summary>
		public String Department 
		{
			get { return _DAO.Department; }
			set { _DAO.Department = value;}
		}
		/// <summary>
		/// Primary Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// Each Employment record belongs to a single Employer. This is the foreign key reference to the Employer table.
		/// </summary>
		public IEmployer Employer 
		{
			get
			{
				if (null == _DAO.Employer) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IEmployer, Employer_DAO>(_DAO.Employer);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Employer = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Employer = (Employer_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// Each Employment record belongs to a LegalEntity. This is the foreign key reference to the LegalEntity table.
		/// </summary>
		public ILegalEntity LegalEntity 
		{
			get
			{
				if (null == _DAO.LegalEntity) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ILegalEntity, LegalEntity_DAO>(_DAO.LegalEntity);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.LegalEntity = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.LegalEntity = (LegalEntity_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Employment_DAO.ConfirmedEmploymentFlag
		/// </summary>
		public Boolean? ConfirmedEmploymentFlag
		{
			get { return _DAO.ConfirmedEmploymentFlag; }
			set { _DAO.ConfirmedEmploymentFlag = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Employment_DAO.ConfirmedIncomeFlag
		/// </summary>
		public Boolean? ConfirmedIncomeFlag
		{
			get { return _DAO.ConfirmedIncomeFlag; }
			set { _DAO.ConfirmedIncomeFlag = value;}
		}

        /// <summary>
        /// 
        /// </summary>
        public bool? UnionMember
        {
            get { return _DAO.UnionMember; }
            set { _DAO.UnionMember = value; }
        }
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Employment_DAO.SalaryPaymentDay
        /// </summary>
        public System.Int32? SalaryPaymentDay
        {
            get { return _DAO.SalaryPaymentDay; }
            set { _DAO.SalaryPaymentDay = value; }
        }
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Employment_DAO.EmploymentVerificationProcesses
		/// </summary>
		private DAOEventList<EmploymentVerificationProcess_DAO, IEmploymentVerificationProcess, EmploymentVerificationProcess> _EmploymentVerificationProcesses;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Employment_DAO.EmploymentVerificationProcesses
		/// </summary>
		public IEventList<IEmploymentVerificationProcess> EmploymentVerificationProcesses
		{
			get
			{
				if (null == _EmploymentVerificationProcesses) 
				{
					if(null == _DAO.EmploymentVerificationProcesses)
						_DAO.EmploymentVerificationProcesses = new List<EmploymentVerificationProcess_DAO>();
					_EmploymentVerificationProcesses = new DAOEventList<EmploymentVerificationProcess_DAO, IEmploymentVerificationProcess, EmploymentVerificationProcess>(_DAO.EmploymentVerificationProcesses);
					_EmploymentVerificationProcesses.BeforeAdd += new EventListHandler(OnEmploymentVerificationProcesses_BeforeAdd);					
					_EmploymentVerificationProcesses.BeforeRemove += new EventListHandler(OnEmploymentVerificationProcesses_BeforeRemove);					
					_EmploymentVerificationProcesses.AfterAdd += new EventListHandler(OnEmploymentVerificationProcesses_AfterAdd);					
					_EmploymentVerificationProcesses.AfterRemove += new EventListHandler(OnEmploymentVerificationProcesses_AfterRemove);					
				}
				return _EmploymentVerificationProcesses;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_EmploymentVerificationProcesses = null;
			
		}
	}
}


