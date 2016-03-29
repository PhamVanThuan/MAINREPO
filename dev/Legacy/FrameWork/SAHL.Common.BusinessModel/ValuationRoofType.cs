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
	/// ValuationRoofType_DAO describes the different roof types available for SAHL Manual Valuations.
	/// </summary>
	public partial class ValuationRoofType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ValuationRoofType_DAO>, IValuationRoofType
	{
				public ValuationRoofType(SAHL.Common.BusinessModel.DAO.ValuationRoofType_DAO ValuationRoofType) : base(ValuationRoofType)
		{
			this._DAO = ValuationRoofType;
		}
		/// <summary>
		/// ValuationRoofType Description
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
	}
}


