
using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.UiStatementType_DAO
	/// </summary>
    public partial class UiStatementType_WTF : BusinessModelBase<UiStatementType_WTF_DAO>, IUiStatementType_WTF
	{
        public UiStatementType_WTF(UiStatementType_WTF_DAO UiStatementType_WTF) : base(UiStatementType_WTF)
		{
            this._DAO = UiStatementType_WTF;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UiStatementType_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UiStatementType_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}



