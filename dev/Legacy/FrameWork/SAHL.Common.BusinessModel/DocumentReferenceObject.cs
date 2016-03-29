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
	/// 
	/// </summary>
	public partial class DocumentReferenceObject : BusinessModelBase<SAHL.Common.BusinessModel.DAO.DocumentReferenceObject_DAO>, IDocumentReferenceObject
	{
				public DocumentReferenceObject(SAHL.Common.BusinessModel.DAO.DocumentReferenceObject_DAO DocumentReferenceObject) : base(DocumentReferenceObject)
		{
			this._DAO = DocumentReferenceObject;
		}
		/// <summary>
		/// 
		/// </summary>
		public String TableName 
		{
			get { return _DAO.TableName; }
			set { _DAO.TableName = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public String ColumnName 
		{
			get { return _DAO.ColumnName; }
			set { _DAO.ColumnName = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}


