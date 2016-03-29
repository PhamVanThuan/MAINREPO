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
	/// The Employer_DAO class is used to instantiate a new Employer.
	/// </summary>
	public partial class Employer : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Employer_DAO>, IEmployer
	{
				public Employer(SAHL.Common.BusinessModel.DAO.Employer_DAO Employer) : base(Employer)
		{
			this._DAO = Employer;
		}
		/// <summary>
		/// The Employer's Name
		/// </summary>
		public String Name 
		{
			get { return _DAO.Name; }
			set { _DAO.Name = value;}
		}
		/// <summary>
		/// The Employers Telephone Number
		/// </summary>
		public String TelephoneNumber 
		{
			get { return _DAO.TelephoneNumber; }
			set { _DAO.TelephoneNumber = value;}
		}
		/// <summary>
		/// The Area Code of the Employer's Telephone Number
		/// </summary>
		public String TelephoneCode 
		{
			get { return _DAO.TelephoneCode; }
			set { _DAO.TelephoneCode = value;}
		}
		/// <summary>
		/// A Contact Person for the Employer
		/// </summary>
		public String ContactPerson 
		{
			get { return _DAO.ContactPerson; }
			set { _DAO.ContactPerson = value;}
		}
		/// <summary>
		/// An email address for the Employer
		/// </summary>
		public String ContactEmail 
		{
			get { return _DAO.ContactEmail; }
			set { _DAO.ContactEmail = value;}
		}
		/// <summary>
		/// The Employer's Accountant.
		/// </summary>
		public String AccountantName 
		{
			get { return _DAO.AccountantName; }
			set { _DAO.AccountantName = value;}
		}
		/// <summary>
		/// A Contact Person for the Accountant.
		/// </summary>
		public String AccountantContactPerson 
		{
			get { return _DAO.AccountantContactPerson; }
			set { _DAO.AccountantContactPerson = value;}
		}
		/// <summary>
		/// The Area Code for the Accountant's telephone number.
		/// </summary>
		public String AccountantTelephoneCode 
		{
			get { return _DAO.AccountantTelephoneCode; }
			set { _DAO.AccountantTelephoneCode = value;}
		}
		/// <summary>
		/// The Telephone Number for the Accountant.
		/// </summary>
		public String AccountantTelephoneNumber 
		{
			get { return _DAO.AccountantTelephoneNumber; }
			set { _DAO.AccountantTelephoneNumber = value;}
		}
		/// <summary>
		/// An email address for the Accountant.
		/// </summary>
		public String AccountantEmail 
		{
			get { return _DAO.AccountantEmail; }
			set { _DAO.AccountantEmail = value;}
		}
		/// <summary>
		/// Each Employer belongs to a specific business type. This is a foreign key reference to the EmployerBusinessType table.
		/// </summary>
		public IEmployerBusinessType EmployerBusinessType 
		{
			get
			{
				if (null == _DAO.EmployerBusinessType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IEmployerBusinessType, EmployerBusinessType_DAO>(_DAO.EmployerBusinessType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.EmployerBusinessType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.EmployerBusinessType = (EmployerBusinessType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// The UserID of the person who last updated the Employer record.
		/// </summary>
		public String UserID 
		{
			get { return _DAO.UserID; }
			set { _DAO.UserID = value;}
		}
		/// <summary>
		/// The date on which the Employer record was last changed.
		/// </summary>
		public DateTime ChangeDate 
		{
			get { return _DAO.ChangeDate; }
			set { _DAO.ChangeDate = value;}
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
		/// Each Employer belongs to a specific Employment Sector. This is a foreign key reference to the EmploymentSector table.
		/// </summary>
		public IEmploymentSector EmploymentSector 
		{
			get
			{
				if (null == _DAO.EmploymentSector) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IEmploymentSector, EmploymentSector_DAO>(_DAO.EmploymentSector);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.EmploymentSector = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.EmploymentSector = (EmploymentSector_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


