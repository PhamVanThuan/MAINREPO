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
	/// SAHL.Common.BusinessModel.DAO.ParameterType_DAO
	/// </summary>
	public partial class ParameterType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ParameterType_DAO>, IParameterType
	{
				public ParameterType(SAHL.Common.BusinessModel.DAO.ParameterType_DAO ParameterType) : base(ParameterType)
		{
			this._DAO = ParameterType;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ParameterType_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ParameterType_DAO.SQLDataType
		/// </summary>
		public String SQLDataType 
		{
			get { return _DAO.SQLDataType; }
			set { _DAO.SQLDataType = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ParameterType_DAO.CSharpDataType
		/// </summary>
		public String CSharpDataType 
		{
			get { return _DAO.CSharpDataType; }
			set { _DAO.CSharpDataType = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ParameterType_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}


