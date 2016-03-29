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
	/// SAHL.Common.BusinessModel.DAO.EmploymentSector_DAO
	/// </summary>
	public partial class EmploymentSector : BusinessModelBase<SAHL.Common.BusinessModel.DAO.EmploymentSector_DAO>, IEmploymentSector
	{
				public EmploymentSector(SAHL.Common.BusinessModel.DAO.EmploymentSector_DAO EmploymentSector) : base(EmploymentSector)
		{
			this._DAO = EmploymentSector;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.EmploymentSector_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.EmploymentSector_DAO.GeneralStatusKey
		/// </summary>
		public IGeneralStatus GeneralStatusKey 
		{
			get
			{
				if (null == _DAO.GeneralStatusKey) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IGeneralStatus, GeneralStatus_DAO>(_DAO.GeneralStatusKey);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.GeneralStatusKey = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.GeneralStatusKey = (GeneralStatus_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.EmploymentSector_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}


