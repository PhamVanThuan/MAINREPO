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
	/// RateOverrideType_DAO is used in order to hold the descriptions of the types of Rate Overrides.
	/// </summary>
	public partial class RateOverrideType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.RateOverrideType_DAO>, IRateOverrideType
	{
				public RateOverrideType(SAHL.Common.BusinessModel.DAO.RateOverrideType_DAO RateOverrideType) : base(RateOverrideType)
		{
			this._DAO = RateOverrideType;
		}
		/// <summary>
		/// The description of the Rate Override. e.g. Interest Only/Super Lo/
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
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
		/// SAHL.Common.BusinessModel.DAO.RateOverrideType_DAO.RateOverrideTypeGroup
		/// </summary>
		public IRateOverrideTypeGroup RateOverrideTypeGroup 
		{
			get
			{
				if (null == _DAO.RateOverrideTypeGroup) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IRateOverrideTypeGroup, RateOverrideTypeGroup_DAO>(_DAO.RateOverrideTypeGroup);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.RateOverrideTypeGroup = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.RateOverrideTypeGroup = (RateOverrideTypeGroup_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


