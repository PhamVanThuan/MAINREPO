
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
	/// SAHL.Common.BusinessModel.DAO.GenericKeyType_DAO
	/// </summary>
    public partial class GenericKeyType_WTF : BusinessModelBase<GenericKeyType_WTF_DAO>, IGenericKeyType_WTF
	{
        public GenericKeyType_WTF(GenericKeyType_WTF_DAO GenericKeyType_WTF)  : base(GenericKeyType_WTF)
		{
            this._DAO = GenericKeyType_WTF;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.GenericKeyType_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.GenericKeyType_DAO.TableName
		/// </summary>
		public String TableName 
		{
			get { return _DAO.TableName; }
			set { _DAO.TableName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.GenericKeyType_DAO.PrimaryKeyColumn
		/// </summary>
		public String PrimaryKeyColumn 
		{
			get { return _DAO.PrimaryKeyColumn; }
			set { _DAO.PrimaryKeyColumn = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.GenericKeyType_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}



