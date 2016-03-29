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
	/// SAHL.Common.BusinessModel.DAO.FinancialAdjustmentType_DAO
	/// </summary>
	public partial class FinancialAdjustmentType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.FinancialAdjustmentType_DAO>, IFinancialAdjustmentType
	{
				public FinancialAdjustmentType(SAHL.Common.BusinessModel.DAO.FinancialAdjustmentType_DAO FinancialAdjustmentType) : base(FinancialAdjustmentType)
		{
			this._DAO = FinancialAdjustmentType;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialAdjustmentType_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialAdjustmentType_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
	}
}


