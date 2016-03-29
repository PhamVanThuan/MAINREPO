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
	/// SAHL.Common.BusinessModel.DAO.EmploymentVerificationProcess_DAO
	/// </summary>
	public partial class EmploymentVerificationProcess : BusinessModelBase<SAHL.Common.BusinessModel.DAO.EmploymentVerificationProcess_DAO>, IEmploymentVerificationProcess
	{
				public EmploymentVerificationProcess(SAHL.Common.BusinessModel.DAO.EmploymentVerificationProcess_DAO EmploymentVerificationProcess) : base(EmploymentVerificationProcess)
		{
			this._DAO = EmploymentVerificationProcess;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.EmploymentVerificationProcess_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.EmploymentVerificationProcess_DAO.Employment
		/// </summary>
		public IEmployment Employment 
		{
			get
			{
				if (null == _DAO.Employment) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IEmployment, Employment_DAO>(_DAO.Employment);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Employment = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Employment = (Employment_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.EmploymentVerificationProcess_DAO.EmploymentVerificationProcessType
		/// </summary>
		public IEmploymentVerificationProcessType EmploymentVerificationProcessType 
		{
			get
			{
				if (null == _DAO.EmploymentVerificationProcessType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IEmploymentVerificationProcessType, EmploymentVerificationProcessType_DAO>(_DAO.EmploymentVerificationProcessType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.EmploymentVerificationProcessType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.EmploymentVerificationProcessType = (EmploymentVerificationProcessType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.EmploymentVerificationProcess_DAO.UserID
		/// </summary>
		public String UserID 
		{
			get { return _DAO.UserID; }
			set { _DAO.UserID = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.EmploymentVerificationProcess_DAO.ChangeDate
		/// </summary>
		public DateTime? ChangeDate
		{
			get { return _DAO.ChangeDate; }
			set { _DAO.ChangeDate = value;}
		}
	}
}


