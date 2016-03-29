
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
	/// SAHL.Common.BusinessModel.DAO.GeneralStatus_DAO
	/// </summary>
    public partial class GeneralStatus_WTF : BusinessModelBase<GeneralStatus_WTF_DAO>, IGeneralStatus_WTF
	{
        public GeneralStatus_WTF(GeneralStatus_WTF_DAO GeneralStatus_WTF) : base(GeneralStatus_WTF)
		{
            this._DAO = GeneralStatus_WTF;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.GeneralStatus_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.GeneralStatus_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}



