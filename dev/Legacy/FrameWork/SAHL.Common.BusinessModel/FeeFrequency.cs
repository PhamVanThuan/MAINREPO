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
	/// SAHL.Common.BusinessModel.DAO.FeeFrequency_DAO
	/// </summary>
	public partial class FeeFrequency : BusinessModelBase<SAHL.Common.BusinessModel.DAO.FeeFrequency_DAO>, IFeeFrequency
	{
				public FeeFrequency(SAHL.Common.BusinessModel.DAO.FeeFrequency_DAO FeeFrequency) : base(FeeFrequency)
		{
			this._DAO = FeeFrequency;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FeeFrequency_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FeeFrequency_DAO.Interval
		/// </summary>
		public Int32 Interval 
		{
			get { return _DAO.Interval; }
			set { _DAO.Interval = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FeeFrequency_DAO.FrequencyUnit
		/// </summary>
		public IFrequencyUnit FrequencyUnit 
		{
			get
			{
				if (null == _DAO.FrequencyUnit) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IFrequencyUnit, FrequencyUnit_DAO>(_DAO.FrequencyUnit);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.FrequencyUnit = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.FrequencyUnit = (FrequencyUnit_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


