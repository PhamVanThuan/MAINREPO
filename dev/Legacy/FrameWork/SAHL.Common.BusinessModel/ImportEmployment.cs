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
	/// 
	/// </summary>
	public partial class ImportEmployment : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ImportEmployment_DAO>, IImportEmployment
	{
				public ImportEmployment(SAHL.Common.BusinessModel.DAO.ImportEmployment_DAO ImportEmployment) : base(ImportEmployment)
		{
			this._DAO = ImportEmployment;
		}
		/// <summary>
		/// 
		/// </summary>
		public String EmploymentTypeKey 
		{
			get { return _DAO.EmploymentTypeKey; }
			set { _DAO.EmploymentTypeKey = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public String RemunerationTypeKey 
		{
			get { return _DAO.RemunerationTypeKey; }
			set { _DAO.RemunerationTypeKey = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public String EmploymentStatusKey 
		{
			get { return _DAO.EmploymentStatusKey; }
			set { _DAO.EmploymentStatusKey = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public String EmployerName 
		{
			get { return _DAO.EmployerName; }
			set { _DAO.EmployerName = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public String EmployerContactPerson 
		{
			get { return _DAO.EmployerContactPerson; }
			set { _DAO.EmployerContactPerson = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public String EmployerPhoneCode 
		{
			get { return _DAO.EmployerPhoneCode; }
			set { _DAO.EmployerPhoneCode = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public String EmployerPhoneNumber 
		{
			get { return _DAO.EmployerPhoneNumber; }
			set { _DAO.EmployerPhoneNumber = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? EmploymentStartDate
		{
			get { return _DAO.EmploymentStartDate; }
			set { _DAO.EmploymentStartDate = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? EmploymentEndDate
		{
			get { return _DAO.EmploymentEndDate; }
			set { _DAO.EmploymentEndDate = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Double MonthlyIncome 
		{
			get { return _DAO.MonthlyIncome; }
			set { _DAO.MonthlyIncome = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public IImportLegalEntity ImportLegalEntity 
		{
			get
			{
				if (null == _DAO.ImportLegalEntity) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IImportLegalEntity, ImportLegalEntity_DAO>(_DAO.ImportLegalEntity);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ImportLegalEntity = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ImportLegalEntity = (ImportLegalEntity_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


