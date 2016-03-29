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
	/// SAHL.Common.BusinessModel.DAO.Reset_DAO
	/// </summary>
	public partial class Reset : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Reset_DAO>, IReset
	{
				public Reset(SAHL.Common.BusinessModel.DAO.Reset_DAO Reset) : base(Reset)
		{
			this._DAO = Reset;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Reset_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Reset_DAO.ResetDate
		/// </summary>
		public DateTime ResetDate 
		{
			get { return _DAO.ResetDate; }
			set { _DAO.ResetDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Reset_DAO.RunDate
		/// </summary>
		public DateTime RunDate 
		{
			get { return _DAO.RunDate; }
			set { _DAO.RunDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Reset_DAO.JIBARRate
		/// </summary>
		public Double JIBARRate 
		{
			get { return _DAO.JIBARRate; }
			set { _DAO.JIBARRate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Reset_DAO.JIBARDiscountRate
		/// </summary>
		public Double JIBARDiscountRate 
		{
			get { return _DAO.JIBARDiscountRate; }
			set { _DAO.JIBARDiscountRate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Reset_DAO.ResetConfiguration
		/// </summary>
		public IResetConfiguration ResetConfiguration 
		{
			get
			{
				if (null == _DAO.ResetConfiguration) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IResetConfiguration, ResetConfiguration_DAO>(_DAO.ResetConfiguration);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ResetConfiguration = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ResetConfiguration = (ResetConfiguration_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


