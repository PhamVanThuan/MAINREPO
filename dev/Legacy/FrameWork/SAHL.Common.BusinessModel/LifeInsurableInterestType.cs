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
	/// SAHL.Common.BusinessModel.DAO.LifeInsurableInterestType_DAO
	/// </summary>
	public partial class LifeInsurableInterestType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.LifeInsurableInterestType_DAO>, ILifeInsurableInterestType
	{
				public LifeInsurableInterestType(SAHL.Common.BusinessModel.DAO.LifeInsurableInterestType_DAO LifeInsurableInterestType) : base(LifeInsurableInterestType)
		{
			this._DAO = LifeInsurableInterestType;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifeInsurableInterestType_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifeInsurableInterestType_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}


