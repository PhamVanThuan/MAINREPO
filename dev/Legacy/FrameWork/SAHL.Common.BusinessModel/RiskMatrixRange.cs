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
	/// SAHL.Common.BusinessModel.DAO.RiskMatrixRange_DAO
	/// </summary>
	public partial class RiskMatrixRange : BusinessModelBase<SAHL.Common.BusinessModel.DAO.RiskMatrixRange_DAO>, IRiskMatrixRange
	{
				public RiskMatrixRange(SAHL.Common.BusinessModel.DAO.RiskMatrixRange_DAO RiskMatrixRange) : base(RiskMatrixRange)
		{
			this._DAO = RiskMatrixRange;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RiskMatrixRange_DAO.Min
		/// </summary>
		public Double? Min
		{
			get { return _DAO.Min; }
			set { _DAO.Min = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RiskMatrixRange_DAO.Max
		/// </summary>
		public Double? Max
		{
			get { return _DAO.Max; }
			set { _DAO.Max = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RiskMatrixRange_DAO.Designation
		/// </summary>
		public String Designation 
		{
			get { return _DAO.Designation; }
			set { _DAO.Designation = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RiskMatrixRange_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}


